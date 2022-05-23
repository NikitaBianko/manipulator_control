using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulatorControl.Core
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Point(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Point MiddlePoint(Point a, Point b)
        {
            var c = new Point((a.X + b.X) / 2.0, (a.Y + b.Y) / 2.0, (a.Z + b.Z) / 2.0);
            return c;
        }

        //public Point[] getPointsStraight(Point a, Point b, int stepLength)
        //{
        //    double len = Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.X - b.X, 2) + Math.Pow(a.X - b.X, 2));
        //    int numberSteps = (int)(len / stepLength);

        //    return getPointsStraightRecursion(a, b, numberSteps);
        //}

        //private Point[] getPointsStraightRecursion(Point a, Point b, int numberSteps)
        //{
        //    if (numberSteps <= 0)
        //    {
        //        return new Point[] { };
        //    }

        //    Point middle = Point.MiddlePoint(a, b);


        //}
    }
}
