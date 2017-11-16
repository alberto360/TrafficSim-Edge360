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
        public PointF _position; //Use this to position the roads: the road segments are to be connected by their endpoints. 
        //At first I considered having an intersection be a type of RoadSegmentEndpoint, but the more I thought about it, it should just contain a set of RoadSegmentEndpoints
        //An intersection is like a web of endpoints. In a 4 way intersection, the cars reach one enpoint and can choose to go towards any 3 others all while within the intersection
        public List<RoadSegment> _connectedSegments = new List<RoadSegment>();

        public RoadSegmentEndpoint(PointF position)
        {
            _position = position;
        }
    }
}
