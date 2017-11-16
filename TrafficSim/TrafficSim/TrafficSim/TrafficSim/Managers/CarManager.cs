using System;
using System.Collections.Generic;
using System.Linq;

namespace TrafficSim
{
    public class CarManager : ASimBase
    {
        private DateTime? _lastCarAdded;

        public CarManager(SimManager manager, List<Road> roads)
        {
            Cars = new List<Car>();
            Roads = roads;
            SimManager = manager;
        }

        public List<Car> Cars { get; set; }
        public List<Road> Roads { get; set; }
        public SimManager SimManager { get; set; }

        public void AddCar(Car c)
        {
            Cars.Add(c);
        }

        public override void Initialize()
        {
        }

        public void RemoveCar(Car c)
        {
            Cars.Remove(c);
        }

        public override void Update(float delta)
        {
            foreach (var car in Cars)
            {
                if (!car.Disabled)
                {
                    car.Update(delta);

                }
                
            }

            if (_lastCarAdded == null || (DateTime.Now - _lastCarAdded).Value.TotalSeconds > 100)
            {
                _lastCarAdded = DateTime.Now;
                foreach (var road in Roads)
                {
                    SpawnCar(road);
                }
            }
        }

        private void SpawnCar(Road road)
        {
            if (road.Intersections.Count == 0)
            {
                return;
            }
            var end = Util.GetRandomNumber(0, 2) == 1;
            var spawnPoint = end ? road.Vertices.Last() : road.Vertices.First();
            var direction = end ? road.CoDirection : road.Direction;

            var segment = end ? road.Segments.Last() : road.Segments.First();

            var car = new Car(SimManager, road, spawnPoint, direction)
            {
                CurrentSegment = segment,
                CurrentRoad = road
            };

            AddCar(car);
        }
    }
}