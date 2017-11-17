using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
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
    public class Segments
    {
        public int lat { get; set; }
        public int lng { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Datum
    {
        public Distance distance { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public List<Lane> lanes { get; set; }
        public Speedlimit speedlimit { get; set; }
        public List<Segments> segments { get; set; }
    }

    public class RoadData
    {
        public List<Datum> data { get; set; }
    }

    public class RoadSegments
    {
        public List<RoadData> roads { get; set; }
    }
    internal class RoadDisplay
    {
        public RoadDisplay()
        {
        }

        public string Name { get; set; }
        public string Number { get; set; }
        public bool OneWay { get; set; }
        public string Speedlimit { get; set; }
    }
}
