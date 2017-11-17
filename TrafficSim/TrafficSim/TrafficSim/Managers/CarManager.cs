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
            Scores = new List<float>();
        }

        public List<Car> Cars { get; set; }
        public List<float> Scores { get; set; }

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
            Console.WriteLine(GetSimulationScore());
            foreach (var car in Cars)
            {
                if (!car.Disabled)
                {
                    car.Update(delta);

                }
            }
        }

        public void PushPartialScoresToScoreList()
        {
            //Call this when the simulation ends to take into account all the cars that are still active
            foreach(Car car in Cars)
            {
                Scores.Add(car.GetScore());
            }
        }

        public float GetSimulationScore()
        {
            float totalScore = 0f;
            for(int i = 0; i < Scores.Count; ++i)
            {
                totalScore += Scores[i];
            }
            return Scores.Count > 0 ? totalScore / Scores.Count : 0;
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

            var car = new Car(this, road, spawnPoint, direction)
            {
                CurrentSegment = segment,
                CurrentRoad = road
            };

            AddCar(car);
        }
    }
}