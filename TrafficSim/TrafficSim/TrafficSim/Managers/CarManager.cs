using System;
using System.Collections.Generic;
using System.Linq;

namespace TrafficSim
{
    public class CarManager : ASimBase
    {
        public CarManager()
        {
            Cars = new List<Car>();
        }

        public List<Car> Cars { get; set; }

        public void AddCar(Car c)
        {
            Cars.Add(c);
        }

        public void Initialize()
        {
        }

        public void RemoveCar(Car c)
        {
            Cars.Remove(c);
        }

        public void Update(float delta)
        {
            foreach (var car in Cars)
            {
                car.Update(delta);
            }
        }

        internal void SpawnCar(Road road)
        {
            if (road.Intersections.Count == 0)
            {
                return;
            }
            var end = Util.GetRandomNumber(0, 2) == 1;
            var spawnPoint = end ? road.Vertices.Last() : road.Vertices.First();
            var direction = end ? road.CoDirection : road.Direction;

            var segment = end ? road.Segments.Last() : road.Segments.First();

            var car = new Car(road, spawnPoint, direction)
            {
                CurrentSegment = segment,
                CurrentRoad = road
            };

            AddCar(car);
        }
    }
}