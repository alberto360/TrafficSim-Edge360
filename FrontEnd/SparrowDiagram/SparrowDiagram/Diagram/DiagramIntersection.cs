using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparrowDiagram.Models;

namespace SparrowDiagram.Diagram
{
    public class DiagramIntersection
    {

        public PointF locationPoint;
        public IntersectionData interSegment;

        public DiagramIntersection(IntersectionData interSegment, PointF location)
        {
            this.locationPoint = location;
            this.interSegment = interSegment;
        }
    }
    
}
