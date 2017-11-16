using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparrow.Road
{
    class Intersection
    {
        public class Road
        {
            public string id { get; set; }
            public int distance_from_start { get; set; }
        }
        public class RootObject
        {
            public string id { get; set; }
            public string title { get; set; }
            public List<Road> roads { get; set; }
        }
    }
}
