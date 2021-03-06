﻿using System.Drawing;

namespace TrafficSim
{
    public class Car : ASimBase
    {
        private Road _currentRoad;

        public Car(Road road, PointF position, float direction)
        {
            CurrentRoad = road;
            Position = position;
            Direction = direction;
            IsForward = true;

            VisibilityRangeScalar = Util.GetRandomNumber(40, 50);
        }

        public string AngleDiff { get; set; }

        public Intersection ClosestIntersection { get; set; }

        public PointF CurrentDirection { get; set; }

        public Road CurrentRoad
        {
            get => _currentRoad;
            set
            {
                _currentRoad?.CarExited(this);
                _currentRoad = value;
                _currentRoad.CarEntered(this);
            }
        }

        public Line CurrentSegment { get; set; }

        public float Direction { get; set; }

        public bool IsForward { get; set; }

        public PointF Position { get; set; }
        public int VisibilityRangeScalar { get; set; }

        public const int RED_LIGHT_PASS_THRESHOLD = 6;
        public const float FRONT_ANGLE_THRESHOLD = 0.8f;

        public float ApplyObstructionModifier(float units, PointF tangent)
        {
            var forward = CurrentRoad.IsForwardDirection(Direction);
            var carListCurrentRoad = forward ? CurrentRoad.ForwardCars : CurrentRoad.AlternateCars; // cars on same road
            var directionTangent = forward ? tangent : tangent.Mult(-1);

            CurrentDirection = directionTangent;

            var nextEdge = CurrentRoad.GetNextEdge(Direction, CurrentSegment, out PointF start);

            foreach (var car in carListCurrentRoad)
            {
                if (car == this)
                {
                    continue;
                }

                // only care about vehicles on our current or next edges
                if (car.CurrentSegment != CurrentSegment && car.CurrentSegment != nextEdge)
                {
                    continue;
                }

                var heading = car.Position.Subtract(Position).Normalized();
                var dot = heading.Dot(directionTangent);

                // its not infront so we ignore this car
                if (dot < FRONT_ANGLE_THRESHOLD)
                {
                    continue;
                }

                if (car.IsForward != IsForward)
                {
                    continue;
                }

                var dist = car.Position.DistanceTo(Position);
                if (dist <= units * VisibilityRangeScalar)
                {
                    return units / VisibilityRangeScalar / (float) dist;
                }
            }

            foreach (var intersection in CurrentRoad.Intersections)
            {
                var dist = intersection.Position.DistanceTo(Position);
                var forwardDirection = intersection.Position.Subtract(Position).Normalized();
                var dot = forwardDirection.Dot(directionTangent);

                var state = intersection.GetLightState(CurrentRoad);

                //if (state == TrafficLight.ETrafficLightStatus.Red && dist < RED_LIGHT_PASS_THRESHOLD) //eventually, make cars leave intersection even if red
                //{
                //    return units * VisibilityRangeScalar * (float) dist;
                //}

                if (state != TrafficLight.ETrafficLightStatus.Red)
                {
                    continue;
                }

                // verify this intersection is infront of the car
                if (dot < FRONT_ANGLE_THRESHOLD)
                {
                    continue;
                }

                // verify the intersection is within visibility distance
                if (dist <= units * VisibilityRangeScalar)
                {
                    return (units / VisibilityRangeScalar) / (float) dist;
                }
            }

            return units;
        }

        public void GoalReached()
        {
        }

        public override void Initialize()
        {
        }

        public void Move(float delta)
        {
            if (CurrentRoad == null)// currently never expected to be null
            {
                GoalReached();
                return;
            }

            if (CurrentSegment == null) // may be null when no longer on line segment
            {
                return;
            }

            var units = CurrentRoad.GetUnitsPerHour(CurrentRoad.SpeedLimit); //pixel distance

            var forwardVector = CurrentSegment.Normalized();

            units = units / 60 / 60 / (delta * 1000);
            units = ApplyObstructionModifier(units, forwardVector);

            if (!forwardVector.IsZero())
            {
                if (!CurrentRoad.IsForwardDirection(Direction))
                {
                    forwardVector = forwardVector.Mult(-1);
                    IsForward = false;
                }
                Position = Position.Add(forwardVector.Mult(units));
            }

            if (!Line.PointOnLineSegment(CurrentSegment, Position))
            {
                CurrentSegment = CurrentRoad.GetNextEdge(Direction, CurrentSegment, out PointF start);
                Position = start;
            }
        }

        public override void Update(float delta)
        {
            Move(delta);
        }
    }
}