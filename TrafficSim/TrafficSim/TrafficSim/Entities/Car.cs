using System.Collections.Generic;
using System.Drawing;

namespace TrafficSim
{
    public class Car : ASimBase
    {
        public const int RED_LIGHT_PASS_THRESHOLD = 6;
        public const float FRONT_ANGLE_THRESHOLD = 0.8f;
        private Road _currentRoad;

        private float _desiredSpeedPercentage = 1f;
        //it should be a value typically between 0 and 1, with 1 being the speed limit

        private List<float> _speedPercentages = new List<float>();

        private CarManager _myManager;

        private float _turnableDistanceFromIntersection = 1f; //if we are less than this distance from the intersection, allow the car to turn when the light is green

        public Car(CarManager myManager, Road road, PointF position, float direction)
        {
            CurrentRoad = road;
            Position = position;
            Direction = direction;
            IsForward = true;
            Disabled = false;
            VisibilityRangeScalar = Util.GetRandomNumber(40, 50);
            _myManager = myManager;

        }

        public void Initialize()
        {
        }

        public void Update(float delta)
        {
            Move(delta);
        }

        public string AngleDiff { get; set; }

        public Intersection ClosestIntersection { get; set; }

        public PointF CurrentDirection { get; set; }

        public Road CurrentRoad
        {
            get  =>
            _currentRoad;
            set
            {
                _currentRoad?.CarExited(this);
                _currentRoad = value;
                _currentRoad.CarEntered(this);
            }
        }

        public Line CurrentSegment { get; set; }

        public float Direction { get; set; }

        public bool Disabled { get; set; }

        public bool IsForward { get; set; }

        public PointF Position { get; set; }
        public int VisibilityRangeScalar { get; set; }

        public float ApplyObstructionModifier(float pixelUnits, PointF forwardVector)
        {
            var isForward = CurrentRoad.IsForwardDirection(Direction);
            var carListCurrentRoad = isForward ? CurrentRoad.ForwardCars : CurrentRoad.AlternateCars;
                // cars on same road
            var directionTangent = isForward ? forwardVector : forwardVector.Mult(-1);

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
                if (dist <= pixelUnits * VisibilityRangeScalar)
                {
                    return pixelUnits / VisibilityRangeScalar / (float) dist;
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
                    if(dist < _turnableDistanceFromIntersection)
                    {
                        var turningDecision = Util.GetRandomNumber(0, 100);
                        if (turningDecision == 0)
                        {
                            //turn one direction
                            for(int i = 0; i < intersection.Roads.Length; ++i)
                            {
                                if(CurrentRoad != intersection.Roads[i])
                                {
                                    CurrentRoad = intersection.Roads[i];
                                    CurrentSegment = CurrentRoad.GetSegment(intersection.Position);
                                    Position = intersection.Position;
                                    IsForward = !IsForward;
                                    break;
                                }
                            }
                        }
                        else if(turningDecision == 1)
                        {
                            //turn other direction
                            for (int i = 0; i < intersection.Roads.Length; ++i)
                            {
                                if (CurrentRoad != intersection.Roads[i])
                                {
                                    CurrentRoad = intersection.Roads[i];
                                    CurrentSegment = CurrentRoad.GetSegment(intersection.Position);
                                    Position = intersection.Position;

                                    break;
                                }
                            }
                        }
                    }
                    continue;
                }

                // verify this intersection is infront of the car
                if (dot < FRONT_ANGLE_THRESHOLD)
                {
                    continue;
                }

                // verify the intersection is within visibility distance
                if (dist <= pixelUnits * VisibilityRangeScalar)
                {
                    return pixelUnits / VisibilityRangeScalar / (float) dist;
                }
            }

            return pixelUnits;
        }

        public float GetSpeedPercentage()
        {
            return _desiredSpeedPercentage;
        }

        public void GoalReached()
        {
        }

        public void Move(float delta)
        {
            if (CurrentRoad == null) // currently never expected to be null
            {
                GoalReached();
                return;
            }

            if (CurrentSegment == null)// may be null when no longer on line segment
            {
                //local car has ended: push final score to Score manager
                _myManager.Scores.Add(GetScore());
                Dispose();
                return;
            }

            var pixelDistance = CurrentRoad.GetUnitsPerHour(CurrentRoad.SpeedLimit);

            var forwardVector = CurrentSegment.Normalized();

            pixelDistance = pixelDistance / 60 / 60 / (delta * 1000);
            var whole = pixelDistance;
            pixelDistance = ApplyObstructionModifier(pixelDistance, forwardVector);
            forwardVector = CurrentSegment.Normalized(); //it may have changed after the method call
            var truePercentage = pixelDistance / whole;
                //because this flickers between full speed and almost nothing every other update, we'll smooth out the interpolation value to make color look nicer
            _desiredSpeedPercentage = _desiredSpeedPercentage + 0.1f * (truePercentage - _desiredSpeedPercentage);
            _speedPercentages.Add(_desiredSpeedPercentage);

            if (!forwardVector.IsZero())
            {
                if (!CurrentRoad.IsForwardDirection(Direction))
                {
                    forwardVector = forwardVector.Mult(-1);
                    IsForward = false;
                }
                Position = Position.Add(forwardVector.Mult(pixelDistance));
            }

            if (!Line.PointOnLineSegment(CurrentSegment, Position))
            {
                CurrentSegment = CurrentRoad.GetNextEdge(Direction, CurrentSegment, out PointF start);
                Position = start;
            }
        }

        private void Dispose()
        {
            Disabled = true;
        }

        //Partial scores work fine here, and works independent of turns
        public float GetScore()
        {
            float totalScore = 0f;
            for (int i = 0; i < _speedPercentages.Count; ++i)
            {
                totalScore += _speedPercentages[i];
            }
            return _speedPercentages.Count > 0 ? totalScore / _speedPercentages.Count : 0;
        }
    }
}