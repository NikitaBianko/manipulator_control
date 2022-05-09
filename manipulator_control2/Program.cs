using ManipulatorControl.Core;

var manipulator = new Manipulator(10, 8, 2);

// just moving to a given point
manipulator.MoveInPoint(new Point(5, 5, 5));
Console.WriteLine(manipulator.Angle.ToString() + ' ' + manipulator.ShoulderAngle.ToString() + ' ' + manipulator.ElbowAngle.ToString() + ' ' + manipulator.WristAngle.ToString());
Console.WriteLine(manipulator.Position.X.ToString() + ' ' + manipulator.Position.Y.ToString() + ' ' + manipulator.Position.Z.ToString() + '\n');

// moving to a given point with a fixed wrist
manipulator.SetFixedWrist(90.0);
manipulator.MoveInPoint(new Point(5, 5, 5));
Console.WriteLine(manipulator.Angle.ToString() + ' ' + manipulator.ShoulderAngle.ToString() + ' ' + manipulator.ElbowAngle.ToString() + ' ' + manipulator.WristAngle.ToString());
Console.WriteLine(manipulator.Position.X.ToString() + ' ' + manipulator.Position.Y.ToString() + ' ' + manipulator.Position.Z.ToString() + '\n');

// moving to a given point with a brush that maintains an angle to the horizon
manipulator.SetFixedWrist(false);
manipulator.SetAngleToHorizon(0);
manipulator.MoveInPoint(new Point(5, 5, 5));
Console.WriteLine(manipulator.Angle.ToString() + ' ' + manipulator.ShoulderAngle.ToString() + ' ' + manipulator.ElbowAngle.ToString() + ' ' + manipulator.WristAngle.ToString());
Console.WriteLine(manipulator.Position.X.ToString() + ' ' + manipulator.Position.Y.ToString() + ' ' + manipulator.Position.Z.ToString() + '\n');

// changing the angles of inclination of the parts of the manipulator
manipulator.SetAngles(90, 90, 90, 90);
Console.WriteLine(manipulator.Angle.ToString() + ' ' + manipulator.ShoulderAngle.ToString() + ' ' + manipulator.ElbowAngle.ToString() + ' ' + manipulator.WristAngle.ToString());
Console.WriteLine(manipulator.Position.X.ToString() + ' ' + manipulator.Position.Y.ToString() + ' ' + manipulator.Position.Z.ToString() + '\n');

//check  
manipulator.SetFixedWrist(false);
manipulator.MoveInPoint(new Point(-10, 10, 7));
Console.WriteLine(manipulator.Angle.ToString() + ' ' + manipulator.ShoulderAngle.ToString() + ' ' + manipulator.ElbowAngle.ToString() + ' ' + manipulator.WristAngle.ToString());
manipulator.SetAngles(manipulator.Angle, manipulator.ShoulderAngle, manipulator.ElbowAngle, manipulator.WristAngle);
Console.WriteLine(manipulator.Position.X.ToString() + ' ' + manipulator.Position.Y.ToString() + ' ' + manipulator.Position.Z.ToString());

manipulator.MoveInPoint(new Point(0, 10, 0));
