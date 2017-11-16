using System;
using System.Collections.Generic;
using System.Linq;

namespace TrafficSim
{
    public class SimManager : ASimBase
    {
      
        private DateTime _lastTick = DateTime.Now;

        public SimManager(Road[] roads)
        {
            Rate = 8;
            RoadManager = new RoadManager(this);
            RoadManager.Roads = roads.ToList();

            CarManager = new CarManager(this, RoadManager.Roads);

            IntersectionManager = new IntersectionManager(this);
        }

        public CarManager CarManager { get; set; }
        public float Rate { get; set; }
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
            Update((float)(DateTime.Now - _lastTick).TotalSeconds / Rate);
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