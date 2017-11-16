using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TrafficSim
{
    public class SimGraphics
    {
        public Color _pavementColor = Color.Gray;
        public Color _pavementHighlightColor = Color.LightGray;
        public Color _noCrossColor = Color.Yellow;
        public Color _multilaneColor = Color.White;
        public float _carRadius = 3;
        public Road SelectedLine;

        public SimGraphics()
        {
        }

        public void DrawIntersection()
        {
        }

        private Road FindLineByPoint(List<Road> roads, Point p)
        {
            var size = 10;
            var highLightSize = 10;
            var buffer = new Bitmap(size * 2, size * 2);
            foreach (var road in roads)
            {
                //draw each line on small region around current point p and check pixel in point p
                using (var g = Graphics.FromImage(buffer))
                {//verts[i], verts[i + 1]
                    var verts = road.Vertices;
                    g.Clear(Color.Black);
                    for (var i = 0; i < verts.Count - 1; i++)
                    {
                       
                        g.DrawLine(new Pen(Color.Green, highLightSize), verts[i].X - p.X + size, verts[i].Y - p.Y + size,
                        verts[i + 1].X - p.X + size, verts[i + 1].Y - p.Y + size);
                    }
                }
                if (buffer.GetPixel(size, size).ToArgb() != Color.Black.ToArgb())
                {
                    return road;
                }
            }
            return null;
        }

        /// <summary>
        /// Check to see if line is being highlighted
        /// </summary>
        /// <param name="point"></param>
        /// <param name="sparrowDiagram"></param>
        internal void RefreshLineSelection(Point point, List<Road> roads, SimMap simMap)
        {
            var selectedLine = FindLineByPoint(roads, point);
            if (selectedLine != SelectedLine)
            {
                SelectedLine = selectedLine;
                simMap.Invalidate();
            }

            simMap.Cursor =
                 SelectedLine != null
                        ? Cursors.Hand
                        : Cursors.Default;
        }

        public void DrawIntersection(PaintEventArgs e, Intersection intersection)
        {
            float ellipseRadius = 5;
            e.Graphics.DrawEllipse(Pens.Purple, new RectangleF(intersection.Position.Subtract(new PointF(ellipseRadius, ellipseRadius)), new SizeF(ellipseRadius * 2, ellipseRadius * 2)));

            foreach (var road in intersection.Roads)
            {
                var light = intersection.GetLight(road);
                var status = intersection.GetLightState(road);

                var color = Brushes.Purple;

                switch (status)
                {
                    case TrafficLight.ETrafficLightStatus.Yellow:
                        color = Brushes.Yellow;
                        break;

                    case TrafficLight.ETrafficLightStatus.Green:
                        color = Brushes.Green;
                        break;

                    case TrafficLight.ETrafficLightStatus.Red:
                        color = Brushes.Red;
                        break;
                }

                var offset = light.Segment.Normalized().Mult(15);
                e.Graphics.DrawLine(new Pen(color, 4), intersection.Position.Add(offset),
                    intersection.Position.Subtract(offset));
            }
            //            var signalSize = new SizeF(ellipseRadius * 2, ellipseRadius * 2);
            //            e.Graphics.DrawEllipse(Pens.White, new RectangleF(intersection.Position.OffsetToCenter(signalSize), signalSize));
            DrawLineInteresction(e, intersection.Position, Color.Black, (int)ellipseRadius);
        }

        public void DrawVehicle(PaintEventArgs e, Car car)
        {
            var color = car.IsForward ? Pens.Red : Pens.Blue;
            var brush = car.IsForward ? Brushes.Red : Brushes.Blue;

            var p = new Pen(car.IsForward ? Brushes.Red : Brushes.Blue, 3);

            var offset = car.Position.Add(car.CurrentDirection.Rotate(90).Mult(5));

            e.Graphics.DrawEllipse(color, new RectangleF(offset.Subtract(new PointF(_carRadius, _carRadius)), new SizeF(_carRadius * 2, _carRadius * 2)));
            e.Graphics.DrawLine(p, offset, offset.Add(car.CurrentDirection.Mult(7)));
        }

        public void DrawRoads(PaintEventArgs e, Road road)
        {
            var pavementColor = road == SelectedLine ? _pavementHighlightColor : _pavementColor;
            var pavementSize = 18;
            var verts = road.Vertices;
            for (var i = 0; i < verts.Count - 1; i++)
            {
                if (i + 1 != 0 || i + 1 != verts.Count - 1)
                {
                    DrawLineInteresction(e, verts[i + 1], pavementColor, pavementSize);
                }
                DrawLine(e, verts[i], verts[i + 1], pavementColor, pavementSize, false);
                DrawLine(e, verts[i], verts[i + 1], _noCrossColor, 1, false);
            }
        }

        private void DrawLine(PaintEventArgs e, PointF startPoint, PointF endpoint, Color color, int size, bool dashed)
        {
            if (size < 0)
            {
                size = 1;
            }
            var pen = new Pen(color, size);
            float[] dashValues = { 2, 2 };
            if (dashed)
            {
                pen.DashPattern = dashValues;
            }
            e.Graphics.DrawLine(pen, startPoint, endpoint);
        }

        private void DrawLineInteresction(PaintEventArgs e, PointF pointF, Color color, int size)
        {
            SizeF textSize = new SizeF(size, size);
            RectangleF rectf = new RectangleF(pointF.OffsetToCenter(textSize), textSize);
            SolidBrush redBrush = new SolidBrush(color);

            e.Graphics.FillEllipse(redBrush, rectf);
        }

        //        private void DrawStreetName(PaintEventArgs e, Road road)
        //        {
        //            var textDisplayPoint = Utils.MidPoint(road., road.EndPoint);
        //            SizeF textSize = new SizeF(90, 50);
        //            RectangleF rectf = new RectangleF(textDisplayPoint.OffsetToCenter(20, 10), textSize);
        //            e.Graphics.DrawString(road.roadSegment.name, new Font("Tahoma", 8), Brushes.Black, rectf);
        //        }
    }
}