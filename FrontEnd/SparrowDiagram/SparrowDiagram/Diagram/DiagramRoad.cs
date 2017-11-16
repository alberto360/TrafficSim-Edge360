using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparrowDiagram.Models;

namespace SparrowDiagram.Diagram
{
    public class DiagramRoad
    {
        public PointF StartPoint;
        public PointF EndPoint;
        public Datum roadSegment;

        public DiagramRoad(Datum roadSegment)
        {
            this.StartPoint = new PointF(roadSegment.start_location.x, roadSegment.start_location.y);
            this.EndPoint = new PointF(roadSegment.end_location.x, roadSegment.end_location.y);
            this.roadSegment = roadSegment;
        }
    }

    public class DiagramRoadMoveInfo
    {
        public DiagramRoad Line;
        public PointF StartLinePoint;
        public PointF EndLinePoint;
        public Point StartMoveMousePoint;
    }
}
