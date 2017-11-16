using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
    public static class PointFExtension
    {
        public static double DistanceTo(this PointF start, PointF end)
        {
            return Math.Sqrt(Math.Pow((double)end.X - start.X, 2) +
                             Math.Pow((double)end.Y - start.Y, 2));
        }

        public static double AngleTo(this PointF start, PointF end)
        {
            return Math.Atan((end.Y - start.Y) / (end.X - start.X));
        }

        public static PointF Subtract(this PointF a, PointF b)
        {
            return new PointF(a.X - b.X, a.Y - b.Y);
        }

        public static PointF Add(this PointF a, PointF b)
        {
            return new PointF(a.X + b.X, a.Y + b.Y);
        }

        public static PointF Mult(this PointF a, float v)
        {
            return new PointF(a.X * v, a.Y * v);
        }
        public static float Dot(this PointF a, PointF b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static PointF Rotate(this PointF a, float theta)
        {
            return new PointF((float)(a.X * Math.Cos(theta) - a.Y * Math.Sin(theta)),
                (float)(a.X * Math.Sin(theta) + a.Y * Math.Cos(theta)));
        }

        public static PointF Perpendicular(this PointF a)
        {
            return new PointF(a.Y, -a.X);
        }

        public static bool IsZero(this PointF a)
        {
            return a.X == 0 && a.Y == 0;
        }

        public static float Length(this PointF a)
        {
            return (float)Math.Sqrt(a.X * a.X + a.Y * a.Y);
        }

        public static PointF Normalized(this PointF a)
        {
            return a.Mult(1 / a.Length());
        }
    }
}
