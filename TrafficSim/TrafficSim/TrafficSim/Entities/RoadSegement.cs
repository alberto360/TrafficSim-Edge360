using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
    public class RoadSegment
    {
        //Having road segments as a seperate entity will allow cars to more easily eventually pick where they will go when they reach an endpoint, 
        //as an endpoint will naturally contain a list of choices
        public RoadSegmentEndpoint _endpointA;
        public RoadSegmentEndpoint _endpointB;

        public RoadSegment(PointF a, PointF b)
        {

        }
        public RoadSegment(RoadSegmentEndpoint a, RoadSegmentEndpoint b)
        {

        }

        public PointF GetDirection()
        {
            return _endpointB._position.Subtract(_endpointA._position).Normalized();
        }

        //In the end, the drawing functions should draw lines from endpoint to endpoint for each road segment
    }
}
