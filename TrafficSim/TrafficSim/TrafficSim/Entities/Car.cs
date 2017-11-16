using System.Drawing;

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

        public RoadSegment CurrentSegment { get; set; }

        public float Direction { get; set; }

        public bool IsForward { get; set; }

        public PointF Position { get; set; }
        public int VisibilityRangeScalar { get; set; }

        public const int RED_LIGHT_PASS_THRESHOLD = 6;
        public const float FRONT_ANGLE_THRESHOLD = 0.8f;

        public float ApplyObstructionModifier(float pixelUnits, PointF forwardVector)
        {
            var isForward = CurrentRoad.IsForwardDirection(Direction);
            var carListCurrentRoad = isForward ? CurrentRoad.ForwardCars : CurrentRoad.AlternateCars; // cars on same road
            var directionTangent = isForward ? forwardVector : forwardVector.Mult(-1);

            CurrentDirection = directionTangent;

            var nextEdge = CurrentRoad.GetNextEdge(Direction, CurrentSegment, out RoadSegmentEndpoint start);

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
                    return (pixelUnits / VisibilityRangeScalar) / (float) dist;
                }
            }

            return pixelUnits;
        }

        public void GoalReached()
        {
        }

        public void Initialize()
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

            var expectedTravelDistance = ((CurrentRoad.SpeedLimit / 60f) / 60f) * delta; //Speed limit is in MPH and delta is expected to be in seconds, so apply a conversion so we can get miles/second
            var forwardVector = CurrentSegment.GetDirection();

            expectedTravelDistance = ApplyObstructionModifier(expectedTravelDistance, forwardVector);

            if (!forwardVector.IsZero())
            {
                if (!CurrentRoad.IsForwardDirection(Direction))
                {
                    forwardVector = forwardVector.Mult(-1);
                    IsForward = false;
                }
                Position = Position.Add(forwardVector.Mult(expectedTravelDistance));

                //if (currentPosition.DistanceSquared(destination) < Math.Pow(speed * deltaTime, 2))
                //{
                //    //we've reached the end of the road, pick a new one and move the remainder
                //    //for now, just set current position to start
                //    //currentPosition = travelingTowardsRoadEnd ? currentRoadSegment.startPoint : currentRoadSegment.endPoint;

                //    List<RoadSegment> destinationRoadChoices = travelingTowardsRoadEnd ? currentRoadSegment.endPointRoadChoices : currentRoadSegment.startPointRoadChoices;
                //    if (destinationRoadChoices.Count > 0)
                //    {
                //        //eventually pick according to an algorithm, for now pick option 0
                //        float magnitude = destination.Distance(currentPosition);
                //        float remainderDistance = (speed * deltaTime) - magnitude;
                //        currentRoadSegment = destinationRoadChoices[0];
                //        currentPosition = travelingTowardsRoadEnd ? currentRoadSegment.startPoint : currentRoadSegment.endPoint;
                //        destination = travelingTowardsRoadEnd ? currentRoadSegment.endPoint : currentRoadSegment.startPoint;

                //        magnitude = destination.Distance(currentPosition);
                //        currentPosition.x += remainderDistance * (destination.x - currentPosition.x) / magnitude;
                //        currentPosition.y += remainderDistance * (destination.y - currentPosition.y) / magnitude;
                //    }
                //    else
                //    {
                //        currentPosition = destination;
                //    }
                //}
                //else
                //{
                //    //Find our direction vector (normalize) and apply speed * time
                //    float magnitude = destination.Distance(currentPosition);
                //    currentPosition.x += speed * deltaTime * (destination.x - currentPosition.x) / magnitude;
                //    currentPosition.y += speed * deltaTime * (destination.y - currentPosition.y) / magnitude;
                //    //Console.WriteLine(currentPosition.x + " " + currentPosition.y + " " + (speed * deltaTime * (destination.x - currentPosition.x) / magnitude));
                //}
            }

            if (!Line.PointOnLineSegment(CurrentSegment, Position))
            {
                CurrentSegment = CurrentRoad.GetNextEdge(Direction, CurrentSegment, out RoadSegmentEndpoint start);
                Position = start._position;
            }
        }

        public void Update(float delta)
        {
            Move(delta);
        }
    }
}