using System;
using System.Drawing;

namespace TrafficSim
{
    public class Line
    {
        public Line(PointF start, PointF end)
        {
            Start = start;
            End = end;
        }

        public PointF End { get; set; }
        public PointF Start { get; set; }

        public void GetInfiniteLine(out float A, out float B, out float C)
        {
            A = End.Y - Start.Y;
            B = End.X - Start.X;
            C = A * Start.X + B * Start.Y;
        }

        public static bool Intersects(Line a, Line b, out PointF intersection)
        {
            if (IntersectionTest(a.Start.X, a.Start.Y, a.End.X, a.End.Y, b.Start.X, b.Start.Y, b.End.X,
                b.End.Y, out float x, out float y))
            {
                intersection = new PointF(x, y);
                return true;
            }

            intersection = new PointF();
            return false;
        }

        public PointF Max()
        {
            return new PointF(Math.Max(Start.X, End.X), Math.Max(Start.Y, End.Y));
        }

        public static PointF Max(Line a, Line b)
        {
            var aMax = a.Max();
            var bMax = b.Max();
            return new PointF(Math.Max(aMax.X, bMax.X), Math.Max(aMax.Y, bMax.Y));
        }

        public PointF Min()
        {
            return new PointF(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
        }

        public static PointF Min(Line a, Line b)
        {
            var aMin = a.Min();
            var bMin = b.Min();
            return new PointF(Math.Min(aMin.X, bMin.X), Math.Min(aMin.Y, bMin.Y));
        }

        public Line Multiply(float val)
        {
            return new Line(Start.Mult(val), End.Mult(val));
        }

        public PointF Normalized()
        {
            return End.Subtract(Start).Normalized();
        }

        public static bool PointOnLineSegment(Line line, PointF point, double epsilon = 1)
        {
            var a = line.Start;
            var b = line.End;
            var p = point;
            var t = epsilon;

            // ensure points are collinear
            var zero = (b.X - a.X) * (p.Y - a.Y) - (p.X - a.X) * (b.Y - a.Y);
            if (zero > t || zero < -t)
            {
                return false;
            }

            // check if X-coordinates are not equal
            if (a.X - b.X > t || b.X - a.X > t)
                // ensure X is between a.X & b.X (use tolerance)
            {
                return a.X > b.X
                    ? p.X + t > b.X && p.X - t < a.X
                    : p.X + t > a.X && p.X - t < b.X;
            }

            // ensure Y is between a.Y & b.Y (use tolerance)
            return a.Y > b.Y
                ? p.Y + t > b.Y && p.Y - t < a.Y
                : p.Y + t > a.Y && p.Y - t < b.Y;
        }

        private static bool IntersectionTest(float p0_x, float p0_y, float p1_x, float p1_y,
            float p2_x, float p2_y, float p3_x, float p3_y, out float i_x, out float i_y)
        {
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x;
            s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x;
            s2_y = p3_y - p2_y;

            float s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                // Collision detected 
                i_x = p0_x + t * s1_x;
                i_y = p0_y + t * s1_y;
                return true;
            }

            i_x = 0;
            i_y = 0;

            return false; // No collision
        }
    }
}