using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SparrowDiagram.Models
{
    public class Stoplight
    {
        public bool enabled { get; set; }
        public int cycle { get; set; }
    }

    public class IntersectionRoad
    {
        public string id { get; set; }
        public int distance_from_start { get; set; }
    }

    public class IntersectionData
    {
        public string id { get; set; }
        public string title { get; set; }
        public Stoplight stoplight { get; set; }
        [JsonProperty("roads")]

        public List<IntersectionRoad> roads { get; set; }
    }

    public class Intersection
    {
        [JsonProperty("data")]

        public List<IntersectionData> data { get; set; }
    }

    public class IntersectionRootModel
    {
        [JsonProperty("intersections")]
        public List<Intersection> intersections { get; set; }
    }
}
