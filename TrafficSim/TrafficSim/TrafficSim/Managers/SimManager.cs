using System;
using System.Collections.Generic;
using System.Linq;

namespace TrafficSim
{
    public class SimManager : ASimBase
    {
        private DateTime _lastTick = DateTime.Now;

        public SimManager(Road[] roads, CarManager cm)
        {
            RoadManager = new RoadManager(this);
            RoadManager.Roads = roads.ToList();

            CarManager = cm;

            IntersectionManager = new IntersectionManager(this);
        }

        public CarManager CarManager { get; set; }

        public List<Car> Cars => CarManager.Cars;

        public IntersectionManager IntersectionManager { get; set; }
        public List<Intersection> Intersections => IntersectionManager.Intersections;
        public RoadManager RoadManager { get; set; }

        public List<Road> Roads => RoadManager.Roads;

        public override void Initialize()
        {
        }

        public void Update()
        {
            //This is a real time simulation, but all other components using DateTime have been changed, 
            //so we can now Tick at arbitrary rates and update with arbitrary deltas - MR
            Update((float) (DateTime.Now - _lastTick).TotalSeconds); 
            _lastTick = DateTime.Now;
        }

        public override void Update(float delta)
        {
            RoadManager.Update(delta);
            CarManager.Update(delta);
            IntersectionManager.Update(delta);
        }
    }
}