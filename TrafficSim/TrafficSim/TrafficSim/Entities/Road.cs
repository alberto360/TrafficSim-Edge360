using System;
using System.Collections.Generic;
using System.Drawing;

namespace TrafficSim
{
    public class Road : ASimBase
    {
        public float _carSpawnRate = 1f; //in seconds, via simulation time
        private float _carSpawnTimeAccumulator = 0;

        private CarManager _carManager;

        public Road(List<PointF> vertices, float lengthInMiles, CarManager carManager, int speedLimit = 60)
        {
            Vertices = vertices;
            Segments = new List<Line>();
            Intersections = new List<Intersection>();
            ForwardCars = new List<Car>();
            AlternateCars = new List<Car>();

            LengthInMiles = lengthInMiles;
            ComputeCartesianLength();
            //GenerateSegments(); //Previously, Computing the length was also generating the Segments, so now we need a sperate method - MR

            CalculateDirectionTrend(out float dir, out float coDir);

            SpeedLimit = speedLimit;

            Direction = dir;
            CoDirection = coDir;

            _carManager = carManager;
        }

        public List<Car> AlternateCars { get; set; }

        public float CartesianLength { get; set; }

        public float CoDirection { get; set; }

        public float Direction { get; set; }

        public List<Car> ForwardCars { get; set; }

        public List<Intersection> Intersections { get; set; }

        public float LengthInMiles { get; set; }

        public List<Line> Segments { get; set; }
        public int SpeedLimit { get; set; }

        public List<PointF> Vertices { get; set; }

        public void CalculateDirectionTrend(out float direction, out float coDirection)
        {
            var angle = 0.0f;
            for (var i = 0; i < Vertices.Count - 1; i++)
            {
                angle += (float) Vertices[i].AngleTo(Vertices[i + 1]);
            }
            angle /= Vertices.Count;
            angle *= MathUtil.RadToDegree;

            direction = 360 - angle;
            coDirection = angle;
        }

        public void CarEntered(Car car)
        {
            var list = IsForwardDirection(car.Direction) ? ForwardCars : AlternateCars;
            list?.Add(car);
        }

        public void CarExited(Car car)
        {
            var list = IsForwardDirection(car.Direction) ? ForwardCars : AlternateCars;
            list?.Remove(car);
        }

        public Line GetNextEdge(float direction, Line currentEdge, out PointF start)
        {
            var forward = IsForwardDirection(direction);

            var index = Segments.IndexOf(currentEdge);
            index = forward ? index + 1 : index - 1;
            if (index >= Segments.Count || index < 0)
            {
                start = new PointF();
                return null;
            }
            start = forward ? Segments[index].Start : Segments[index].End;
            return Segments[index];
        }

        public Line GetSegment(PointF position)
        {
            foreach (var k in Segments)
            {
                if (Line.PointOnLineSegment(k, position))
                {
                    return k;
                }
            }
            return null;
        }

        /// <summary>
        ///     Calculate how much a car should move along the road based on its cartesian length
        ///     with respect to its length in miles
        /// </summary>
        /// <param name="milesPerHour"></param>
        /// <returns></returns>
        public float GetUnitsPerHour(float milesPerHour)
        {
            return CartesianLength * milesPerHour / LengthInMiles;
        }

        public void Initialize()
        {
        }

        public bool Intersects(Road road, out PointF intersectionPoint)
        {
            foreach (var segment in Segments)
            {
                foreach (var altSegment in road.Segments)
                {
                    if (Line.Intersects(segment, altSegment, out PointF hit))
                    {
                        intersectionPoint = hit;
                        return true;
                    }
                }
            }
            intersectionPoint = new PointF();
            return false;
        }

        public bool IsForwardDirection(float direction)
        {
            return Math.Abs(Direction - direction) < Math.Abs(CoDirection - direction);
        }

        public void Update(float delta)
        {
            //Keep track of the passed time: if it exceeds our spawn rate, spawn cars until it doesn't. - MR
            _carSpawnTimeAccumulator += delta;
            while(_carSpawnTimeAccumulator > _carSpawnRate)
            {
                _carManager.SpawnCar(this);
                _carSpawnTimeAccumulator -= _carSpawnRate;
            }
        }
        //private void GenerateSegments()
        //{
        //    for (var i = 0; i < Vertices.Count - 1; i++)
        //    {
        //        Segments.Add(new Line(Vertices[i], Vertices[i + 1]));
        //    }
        //}

        private void ComputeCartesianLength()
        {
            var dist = 0.0;
            for (var i = 0; i < Vertices.Count - 1; i++)
            {
                dist += Vertices[i].DistanceTo(Vertices[i + 1]);

                // populate edge list
                Segments.Add(new Line(Vertices[i], Vertices[i + 1]));
            }
            CartesianLength = (float)dist;
        }
    }
}