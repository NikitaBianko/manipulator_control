namespace ManipulatorControl.Core
{
    public class Manipulator
    {
        public double Angle { get; private set; } // rotation angle of the whole manipulator

        public double ShoulderLen { get; private set; }
        public double ShoulderAngle { get; private set; }

        public double ElbowLen { get; private set; }
        public double ElbowAngle { get; private set; }

        public double WristLen { get; private set; }
        public double WristAngle { get; private set; }
        public bool WristIsFixed { get; private set; } // fixed relative to the elbow
        public double WristAngleToHorizon { get; private set; }
        public double WristCompression { get; private set; }

        public Point Position { get; private set; }

        public Manipulator(double lenShoulder, double lenElbow, double lenWrist, bool setToInitialPosition = false)
        {
            this.ShoulderLen = lenShoulder;

            this.ElbowLen = lenElbow;

            this.WristLen = lenWrist;
            this.WristAngle = 90;
            this.WristAngleToHorizon = 90;
            this.WristCompression = 0;
            this.WristIsFixed = false;

            if (setToInitialPosition)
            {
                this.Position = new Point(0, lenElbow + lenWrist, lenShoulder);
                this.MoveInPoint(Position);
            }
        }

        public bool CheckCoordinates(Point coordinates)
        {

            double len = Math.Pow(coordinates.X, 2) + Math.Pow(coordinates.Y, 2) + Math.Pow(coordinates.Z, 2);

            if (len > Math.Pow(ShoulderLen + ElbowLen + WristLen, 2))
            {
                return false;
            }
            if (coordinates.Y < 0)
            {
                return false;
            }

            return true;
        }

        public bool CheckAngles(double manipulatorAngle, double shoulderAngle, double elbowAngle, double wristAngle)
        {
            if (manipulatorAngle < 0 || manipulatorAngle > 180)
            {
                return false;
            }
            if (shoulderAngle < 0 || shoulderAngle > 180)
            {
                return false;
            }
            if (elbowAngle < 0 || elbowAngle > 180)
            {
                return false;
            }
            if (wristAngle < 0 || wristAngle > 180)
            {
                return false;
            }

            return true;
        }

        public bool MoveInPoint(Point coordinates)
        {

            if (!CheckCoordinates(coordinates))
            {
                return false;
            }

            this.Position = coordinates;
            this.Angle = Math.Atan(coordinates.Y / coordinates.X) * 180 / Math.PI;

            if (coordinates.X < 0)
            {
                this.Angle *= -1;
            }
            else if (coordinates.X > 0)
            {
                this.Angle = -this.Angle + 180;
            }

            double h = coordinates.Z;
            double r = Math.Sqrt(Math.Pow(coordinates.X, 2) + Math.Pow(coordinates.Y, 2));

            if (this.WristIsFixed)
            {
                double cc = r * r + Math.Pow(h, 2);
                double dd = Math.Pow(this.WristLen, 2) + Math.Pow(this.ElbowLen, 2) - 2 * this.WristLen * this.ElbowLen * Math.Cos((90 + this.WristAngle) * Math.PI / 180);
                this.ShoulderAngle = (Math.Atan(h / r) + Math.Acos((Math.Pow(this.ShoulderLen, 2) + cc - dd) / (2 * this.ShoulderLen * Math.Sqrt(cc)))) * 180 / Math.PI;
                this.ElbowAngle = (Math.Acos((Math.Pow(this.ShoulderLen, 2) + dd - cc) / (2 * this.ShoulderLen * Math.Sqrt(dd))) +
                    Math.Acos((Math.Pow(this.ElbowLen, 2) + dd - Math.Pow(this.WristLen, 2)) / (2 * this.ElbowLen * Math.Sqrt(dd)))) * 180 / Math.PI;
                //this->wrist.angleToHorizon
            }
            else
            {
                h += this.WristLen * Math.Cos(this.WristAngleToHorizon * Math.PI / 180);
                r -= this.WristLen * Math.Sin(this.WristAngleToHorizon * Math.PI / 180);
                double cc = r * r + Math.Pow(h, 2);
                this.ShoulderAngle = (Math.Atan(h / r) +
                    Math.Acos((Math.Pow(this.ShoulderLen, 2) + cc - Math.Pow(this.ElbowLen, 2)) / (2 * this.ShoulderLen * Math.Sqrt(cc)))) * 180 / Math.PI;
                this.ElbowAngle = Math.Acos((Math.Pow(this.ShoulderLen, 2) + Math.Pow(this.ElbowLen, 2) - cc) / (2 * this.ShoulderLen * this.ElbowLen)) * 180 / Math.PI;
                this.WristAngle = 180 - this.ShoulderAngle - this.ElbowAngle + this.WristAngleToHorizon;
            }

            return true;
        }
        public void SetFixedWrist(bool isFixed)
        {
            this.WristIsFixed = isFixed;
        }

        public void SetFixedWrist(double angle)
        {
            this.WristIsFixed = true;
            this.WristAngle = angle;
            MoveInPoint(this.Position);
        }

        public void SetAngleToHorizon(double angle)
        {
            this.WristAngleToHorizon = angle + 90;
            MoveInPoint(this.Position);
        }

        public bool SetAngles(double manipulatorAngle, double shoulderAngle, double elbowAngle, double wristAngle)
        {

            if (!CheckAngles(manipulatorAngle, shoulderAngle, elbowAngle, wristAngle))
            {
                return false;
            }

            this.WristAngle = wristAngle;
            //this->wrist.angle_to_horizon
            this.Angle = manipulatorAngle;
            this.ShoulderAngle = shoulderAngle;
            this.ElbowAngle = elbowAngle;


            this.Position.Y = Math.Cos(shoulderAngle * Math.PI / 180) * this.ShoulderLen +
                Math.Sin((elbowAngle + shoulderAngle - 90) * Math.PI / 180) * this.ElbowLen -
                Math.Cos((elbowAngle + shoulderAngle + wristAngle - 90) * Math.PI / 180) * this.WristLen;

            this.Position.X = this.Position.Y * Math.Cos((180 - manipulatorAngle) * Math.PI / 180);
            this.Position.Y *= Math.Sin((180 - manipulatorAngle) * Math.PI / 180);

            this.Position.Z = Math.Sin(shoulderAngle * Math.PI / 180) * this.ShoulderLen -
                Math.Cos((elbowAngle + shoulderAngle - 90) * Math.PI / 180) * this.ElbowLen +
                Math.Sin((elbowAngle + shoulderAngle + wristAngle - 90) * Math.PI / 180) * this.WristLen;

            return true;

        }
    }
}