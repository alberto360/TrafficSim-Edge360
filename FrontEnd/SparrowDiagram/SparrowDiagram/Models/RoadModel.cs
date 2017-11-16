using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparrowDiagram.Models
{
    public class Speedlimit
    {
        public int speedlimit { get; set; }
        public string units { get; set; }
    }

    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class StartLocation
    {
        public int lat { get; set; }
        public int lng { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
    public class Lane
    {
        public int laneNumber { get; set; }
        public string direction { get; set; }
    }
    public class EndLocation
    {
        public int lat { get; set; }
        public int lng { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Datum
    {
        public Distance distance { get; set; }
        public EndLocation end_location { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public List<Lane> lanes { get; set; }
        public Speedlimit speedlimit { get; set; }
        public StartLocation start_location { get; set; }
    }

    public class Road
    {
        public List<Datum> data { get; set; }
    }

    public class RoadSegments
    {
        public List<Road> roads { get; set; }
    }
}
