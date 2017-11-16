using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparrowDiagram
{
    public static class Utils
    {
            public static PointF MidPoint(PointF pt1, PointF pt2)
            {
                var midX = (pt1.X + pt2.X) / 2;
                var midY = (pt1.Y + pt2.Y) / 2;
                return new PointF(midX, midY);
            }

        public static SizeF GetLineSize(PointF pt1, PointF pt2)
        {
            var midX = Math.Abs(pt1.X - pt2.X) ;
            var midY = Math.Abs(pt1.Y - pt2.Y) ;
            return new SizeF(midX, midY);
        }

        // Find the point of intersection between
        // the lines p1 --> p2 and p3 --> p4.
        public static void FindIntersection(
            PointF p1, PointF p2, PointF p3, PointF p4,
            out bool lines_intersect, out bool segments_intersect,
            out PointF intersection,
            out PointF close_p1, out PointF close_p2)
        {
            // Get the segments' parameters.
            float dx12 = p2.X - p1.X;
            float dy12 = p2.Y - p1.Y;
            float dx34 = p4.X - p3.X;
            float dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            float denominator = (dy12 * dx34 - dx12 * dy34);

            float t1 =
                ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34)
                    / denominator;
            if (float.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                lines_intersect = false;
                segments_intersect = false;
                intersection = new PointF(float.NaN, float.NaN);
                close_p1 = new PointF(float.NaN, float.NaN);
                close_p2 = new PointF(float.NaN, float.NaN);
                return;
            }
            lines_intersect = true;

            float t2 =
                ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12)
                    / -denominator;

            // Find the point of intersection.
            intersection = new PointF(p1.X + dx12 * t1, p1.Y + dy12 * t1);

            // The segments intersect if t1 and t2 are between 0 and 1.
            segments_intersect =
                ((t1 >= 0) && (t1 <= 1) &&
                 (t2 >= 0) && (t2 <= 1));

            // Find the closest points on the segments.
            if (t1 < 0)
            {
                t1 = 0;
            }
            else if (t1 > 1)
            {
                t1 = 1;
            }

            if (t2 < 0)
            {
                t2 = 0;
            }
            else if (t2 > 1)
            {
                t2 = 1;
            }

            close_p1 = new PointF(p1.X + dx12 * t1, p1.Y + dy12 * t1);
            close_p2 = new PointF(p3.X + dx34 * t2, p3.Y + dy34 * t2);
        }
        public static void FindIntersection(
           PointF p1, PointF p2, PointF p3, PointF p4,
           out bool lines_intersect, 
            out PointF intersection )
        {
            // Get the segments' parameters.
            float dx12 = p2.X - p1.X;
            float dy12 = p2.Y - p1.Y;
            float dx34 = p4.X - p3.X;
            float dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            float denominator = (dy12 * dx34 - dx12 * dy34);

            float t1 =
           ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34)
               / denominator;
            if (float.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                lines_intersect = false;
                intersection = new PointF(float.NaN, float.NaN);
                return;
            }
            lines_intersect = true;

            float t2 =
                ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12)
                    / -denominator;

            // Find the point of intersection.
            intersection = new PointF(p1.X + dx12 * t1, p1.Y + dy12 * t1);

        }

        public static PointF OffsetToCenter(this PointF location, SizeF objectSize)
        {
            if (objectSize == null)
            {
                return location;
            }
            PointF newLocation = new PointF();
            newLocation.X = location.X - objectSize.Width / 2;
            newLocation.Y = location.Y - objectSize.Width / 2;
            return newLocation;
        }

        public static PointF OffsetToCenter(this PointF location, int x, int y)
        {
            PointF newLocation = new PointF();
            newLocation.X = location.X + x;
            newLocation.Y = location.Y + y;
            return newLocation;
        }

    }
}
