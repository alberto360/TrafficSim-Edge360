using System;
using System.Drawing;
using System.Linq;

namespace TrafficSim
{
    public class Car : ASimBase
    {
        private Road _currentRoad;
        private float _remainingMovement;
        public Car(SimManager simManager, Road road, PointF position, float direction)
        {
            SimManager = simManager;
            CurrentRoad = road;
            Position = position;
            Direction = direction;
            IsForward = true;
            Disabled = false;
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
        public SimManager SimManager { get; set; }

        public float Direction { get; set; }

        public bool IsForward { get; set; }
        public bool Disabled { get; set; }

        public PointF Position { get; set; }
        public int VisibilityRangeScalar { get; set; }

        public const int RED_LIGHT_PASS_THRESHOLD = 6;
        public const float FRONT_ANGLE_THRESHOLD = 0.8f;
        public const float STOPLIGHTOFFSET = 10f;

        public float ApplyObstructionModifier(float units, PointF tangent)
        {
            var forward = CurrentRoad.IsForwardDirection(Direction);
            var list = forward ? CurrentRoad.ForwardCars : CurrentRoad.AlternateCars;
            var directionTangent = forward ? tangent : tangent.Mult(-1);

            CurrentDirection = directionTangent;

            var nextEdge = CurrentRoad.GetNextEdge(Direction, CurrentSegment, out PointF start);

          
//
//            if (nextEdge == null)
//            {
//                var pt = IsForward ? CurrentRoad.Vertices.Last() : CurrentRoad.Vertices.First();
//               
//                if (pt.X <= 40 && pt.Y <= 60)
//                {
//                    return 0;
//                }
//            }

            foreach (var k in list)
            {
                if (k == this)
                {
                    continue;
                }

                // only care about vehicles on our current or next edges
                if (k.CurrentSegment != CurrentSegment && k.CurrentSegment != nextEdge)
                {
                    continue;
                }

                var heading = k.Position.Subtract(Position).Normalized();
                var dot = heading.Dot(directionTangent);

                // its not infront so we ignore this car
                if (dot < FRONT_ANGLE_THRESHOLD)
                {
                    continue;
                }

                if (k.IsForward != IsForward)
                {
                    continue;
                }
                
                var dist = k.Position.DistanceTo(Position);

                if (dist < VisibilityRangeScalar/8)
                {
                    return 0;
                }

                if (dist <= units * VisibilityRangeScalar/SimManager.Rate)
                {
                    return units / VisibilityRangeScalar / (float) dist;
                }
            }

            foreach (var intersection in CurrentRoad.Intersections)
            {
                var dist = intersection.Position.DistanceTo(Position)- STOPLIGHTOFFSET;
                var heading = intersection.Position.Subtract(Position).Normalized();
                var dot = heading.Dot(directionTangent);

                var state = intersection.GetLightState(CurrentRoad);

//                if (state == TrafficLight.ETrafficLightStatus.Red && dist < RED_LIGHT_PASS_THRESHOLD)
//                {
//                    return units * VisibilityRangeScalar * (float) dist;
//                }

                if (state != TrafficLight.ETrafficLightStatus.Red)
                {
                    continue;
                }

                // verify this intersection is infront of the car
                if (dot < FRONT_ANGLE_THRESHOLD)
                {
                    continue;
                }

                if (dist < VisibilityRangeScalar/ SimManager.Rate)
                {
                    return 0;
                }

                // verify the intersection is within visibility distance
                if (dist <= units * VisibilityRangeScalar/ SimManager.Rate)
                {
                    return units / VisibilityRangeScalar / (float) dist;
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
            if (CurrentRoad == null)
            {
                GoalReached();
                return;
            }

            if (CurrentSegment == null)
            {
                this.Dispose();
                return;
            }



            var units = CurrentRoad.GetUnitsPerHour(CurrentRoad.SpeedLimit);

            var forward = CurrentSegment.Normalized();

            units = units / 60 / 60 / (delta * 1000);

            if (_remainingMovement >0)
            {
                 _remainingMovement -= units;
     

            }
            units = ApplyObstructionModifier(units, forward);

            if (!forward.IsZero())
            {
                if (!CurrentRoad.IsForwardDirection(Direction))
                {
                    forward = forward.Mult(-1);
                    IsForward = false;
                }
                Position = Position.Add(forward.Mult(units));
            }

            if (!Line.PointOnLineSegment(CurrentSegment, Position))
            {
                CurrentSegment = CurrentRoad.GetNextEdge(Direction, CurrentSegment, out PointF start);
                Position = start;

                var pt = IsForward ? CurrentRoad.Vertices.Last() : CurrentRoad.Vertices.First();
                _remainingMovement = (float)Position.DistanceTo(pt);

                Move(delta);
            }
        }

        private void Dispose()
        {
            Disabled = true;
        }

        public override void Update(float delta)
        {
            Move(delta);
        }
    }
}