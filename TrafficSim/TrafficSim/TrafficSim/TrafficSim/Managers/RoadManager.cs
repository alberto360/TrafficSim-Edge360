using System.Collections.Generic;

namespace TrafficSim
{
    public class RoadManager : ASimBase
    {
        public RoadManager(SimManager manager)
        {
            Roads = new List<Road>();
            SimManager = manager;
        }

        public List<Road> Roads { get; set; }
        public SimManager SimManager { get; set; }

        public override void Initialize()
        {
        }

        public override void Update(float delta)
        {
            foreach (var road in Roads)
            {
                road.Update(delta);
            }
        }
    }
}