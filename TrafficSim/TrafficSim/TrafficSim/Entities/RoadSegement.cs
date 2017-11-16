using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
    public class RoadSegement
    {
        //Having road segments as a seperate entity will allow cars to more easily eventually pick where they will go when they reach an endpoint, as an endpoint will naturally contain a list of choices
        public RoadSegmentEndpoint endpointA;
        public RoadSegmentEndpoint endpointB;
    }
}
