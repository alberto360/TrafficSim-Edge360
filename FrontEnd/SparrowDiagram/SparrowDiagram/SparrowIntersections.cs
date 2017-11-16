using System;
using System.Collections.Generic;
using System.Drawing;
using SparrowDiagram.Diagram;
using SparrowDiagram.Models;
using System.Linq;
using System.Windows.Forms;

namespace SparrowDiagram
{
    internal class SparrowIntersections
    {
        private IntersectionRootModel _intersections;
        public List<DiagramIntersection> Intersections = new List<DiagramIntersection>();

        public List<DiagramRoad> _roads;
        private bool v;

        //        public SparrowIntersections(RoadSegments roadSegments) 
        //        {
        //            var lines = new List<DiagramRoad>();
        //            foreach (var roadSegment in roadSegments.roads[0].data)
        //            {
        //                lines.Add(new DiagramRoad(roadSegment));
        //            }
        ////            Lines = lines;
        //        }

        public SparrowIntersections(SparrowRoads roads, IntersectionRootModel _intersections)
        {
            this._roads = roads.Lines;
            this._intersections = _intersections;

            foreach (var intersection in _intersections.intersections[0].data)
            {
                var roadA = _roads.FirstOrDefault(s => s.roadSegment.id== intersection.roads[0].id);
                var roadB = _roads.FirstOrDefault(s => s.roadSegment.id== intersection.roads[1].id);
                PointF interesction ;
                bool  intersected = false;
                Utils.FindIntersection(roadA.StartPoint, roadA.EndPoint, roadB.StartPoint, roadB.EndPoint, out intersected ,
                    out interesction);
                if (intersected)
                {
                    Console.WriteLine("----");
                    Intersections.Add(new DiagramIntersection( intersection, interesction));
                }
            }
        }

        internal void PaintIntersections(PaintEventArgs e)
        {
            foreach (var intersection in Intersections)
            {
                DrawIntersection(e, intersection);
            }
        }

        private void DrawIntersection(PaintEventArgs e, DiagramIntersection intersection)
        {
            var pen = new Pen(Color.Red, 2);
            SizeF textSize = new SizeF(50, 50);
            RectangleF rectf = new RectangleF(intersection.locationPoint.OffsetToCenter(textSize), textSize);

            e.Graphics.DrawEllipse(pen, rectf);
        }
    }
}