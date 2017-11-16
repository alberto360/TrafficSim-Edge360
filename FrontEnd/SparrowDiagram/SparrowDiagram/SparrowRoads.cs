using SparrowDiagram.Diagram;
using SparrowDiagram.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SparrowDiagram
{
    public class SparrowRoads
    {
        public List<DiagramRoad> Lines = new List<DiagramRoad>();
        public DiagramRoad SelectedLine;
        public DiagramRoadMoveInfo RoadMoving;// in case we need to be able to drag a new line
        public Color _pavementColor = Color.Gray; 
        public Color _pavementHighlightColor = Color.LightGray;
        public Color _noCrossColor = Color.Yellow;
        public Color _multilaneColor = Color.White;
        public SparrowRoads(RoadSegments roadSegments)
        {
            var lines = new List<DiagramRoad>();
            foreach (var roadSegment in roadSegments.roads[0].data)
            {
                lines.Add(new DiagramRoad(roadSegment));
            }
            Lines = lines;
        }

        public static DiagramRoad FindLineByPoint(List<DiagramRoad> lines, Point p)
        {
            var size = 10;
            var highLightSize = 10;
            var buffer = new Bitmap(size * 2, size * 2);
            foreach (var line in lines)
            {
                //draw each line on small region around current point p and check pixel in point p

                using (var g = Graphics.FromImage(buffer))
                {
                    g.Clear(Color.Black);
                    g.DrawLine(new Pen(Color.Green, highLightSize), line.StartPoint.X - p.X + size, line.StartPoint.Y - p.Y + size,
                        line.EndPoint.X - p.X + size, line.EndPoint.Y - p.Y + size);
                }

                if (buffer.GetPixel(size, size).ToArgb() != Color.Black.ToArgb())
                {
                    return line;
                }
            }
            return null;
        }

        /// <summary>
        /// Check to see if line is being highlighted
        /// </summary>
        /// <param name="point"></param>
        /// <param name="sparrowDiagram"></param>
        public void RefreshLineSelection(Point point, SparrowPlane sparrowDiagram)
        {
            var selectedLine = FindLineByPoint(Lines, point);
            if (selectedLine != SelectedLine)
            {
                SelectedLine = selectedLine;
                sparrowDiagram.Invalidate();
            }
            if (RoadMoving != null)
            {
                sparrowDiagram.Invalidate();
            }

            sparrowDiagram.Cursor =
                RoadMoving != null
                    ? Cursors.Hand
                    : SelectedLine != null
                        ? Cursors.Hand
                        : Cursors.Default;
        }

        public void PaintRoads(PaintEventArgs e)
        {
            if (Lines == null)
            {
                return;
            }
            try
            {
                e.Graphics.InterpolationMode = InterpolationMode.High;
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                foreach (var line in Lines)
                {
                    DrawRoad(e, line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void DrawRoad(PaintEventArgs e, DiagramRoad road)
        {
            var pavementColor = road == SelectedLine ?  _pavementHighlightColor: _pavementColor;
            var pavementSize = road.roadSegment.lanes.Count * 5;

            DrawLine(e, road.StartPoint, road.EndPoint, pavementColor, pavementSize, false);
            DrawLine(e, road.StartPoint, road.EndPoint, _noCrossColor, 1, false);

            DrawStreetName(e, road);
        }

        private void DrawLine(PaintEventArgs e, PointF startPoint, PointF  endpoint,  Color color, int size, bool dashed )
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

        private void DrawStreetName(PaintEventArgs e, DiagramRoad road)
        {
            var textDisplayPoint = Utils.MidPoint(road.StartPoint, road.EndPoint);
            SizeF textSize = new SizeF(90, 50);
            RectangleF rectf = new RectangleF(textDisplayPoint.OffsetToCenter(20,10), textSize);
            e.Graphics.DrawString(road.roadSegment.name, new Font("Tahoma", 8), Brushes.Black, rectf);
        }


    }
}