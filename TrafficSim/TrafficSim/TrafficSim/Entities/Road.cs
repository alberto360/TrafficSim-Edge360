using System;
using System.Collections.Generic;
using System.Drawing;

namespace TrafficSim
{
    public class Road : ASimBase
    {  

        public Road(Datum roadSegment, float lengthInMiles)
        {
            RoadSegment = roadSegment;
            Vertices = roadSegment.segments.ToVertices();
            Segments = new List<Line>();
            Intersections = new List<Intersection>();
            ForwardCars = new List<Car>();
            AlternateCars = new List<Car>();

            LengthInMiles = lengthInMiles;
            ComputeCartesianLength();

            CalculateDirectionTrend(out float dir, out float coDir);

            SpeedLimit = 60;

            Direction = dir;
            CoDirection = coDir;
        }

        //        public Road(List<PointF> vertices, float lengthInMiles)
        //        {
        //            Vertices = vertices;
        //            Segments = new List<Line>();
        //            Intersections = new List<Intersection>();
        //            ForwardCars = new List<Car>();
        //            AlternateCars = new List<Car>();
        //
        //            LengthInMiles = lengthInMiles;
        //            ComputeCartesianLength();
        //
        //            CalculateDirectionTrend(out float dir, out float coDir);
        //
        //            SpeedLimit = 60;
        //
        //            Direction = dir;
        //            CoDirection = coDir;
        //        }
        public Datum RoadSegment { get; set; }
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

        public override void Initialize()
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

        public override void Update(float delta)
        {
        }

        private void ComputeCartesianLength()
        {
            var dist = 0.0;
            for (var i = 0; i < Vertices.Count - 1; i++)
            {
                dist += Vertices[i].DistanceTo(Vertices[i + 1]);

                // populate edge list
                Segments.Add(new Line(Vertices[i], Vertices[i + 1]));
            }
            CartesianLength = (float) dist;
        }
    }
}