using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
    public class RoadSegmentEndpoint
    {
        public PointF position; //Use this to position the roads: the road segments are to be connected by their endpoints. Intersections can have a special condition draw a box and not necessarily need all the road enpoints to have the same positions
        protected List<RoadSegement> connectedSegments;
    }
}
