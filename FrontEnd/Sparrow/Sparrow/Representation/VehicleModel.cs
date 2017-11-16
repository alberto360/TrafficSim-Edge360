using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparrow.Representation
{
    class VehicleModel
    {
        public RoadModel currentRoadSegment;
        public Point2D currentPosition;
        public float speed; //in units per second. Make sure it's always positive!
        public bool travelingTowardsRoadEnd = true; //false is traveling towards road start

        //Call this from a central timer callback that updates all the vehicle positions at once, rather than keeping a timer per vehicle
        public void Update(float deltaTime)
        {
            Point2D destination = travelingTowardsRoadEnd ? currentRoadSegment.endPoint : currentRoadSegment.startPoint;
            //MoveTowards
            if (currentPosition.DistanceSquared(destination) < Math.Pow(speed * deltaTime, 2))
            {
                //we've reached the end of the road, pick a new one and move the remainder
                //for now, just set current position to start
                //currentPosition = travelingTowardsRoadEnd ? currentRoadSegment.startPoint : currentRoadSegment.endPoint;

                List<RoadModel> destinationRoadChoices = travelingTowardsRoadEnd ? currentRoadSegment.endPointRoadChoices : currentRoadSegment.startPointRoadChoices;
                if (destinationRoadChoices.Count > 0)
                {
                    //eventually pick according to an algorithm, for now pick option 0
                    float magnitude = destination.Distance(currentPosition);
                    float remainderDistance = (speed * deltaTime) - magnitude;
                    currentRoadSegment = destinationRoadChoices[0];
                    currentPosition = travelingTowardsRoadEnd ? currentRoadSegment.startPoint : currentRoadSegment.endPoint;
                    destination = travelingTowardsRoadEnd ? currentRoadSegment.endPoint : currentRoadSegment.startPoint;

                    magnitude = destination.Distance(currentPosition);
                    currentPosition.x += remainderDistance * (destination.x - currentPosition.x) / magnitude;
                    currentPosition.y += remainderDistance * (destination.y - currentPosition.y) / magnitude;
                }
                else
                {
                    currentPosition = destination;
                }
            }
            else
            {
                //Find our direction vector (normalize) and apply speed * time
                float magnitude = destination.Distance(currentPosition);
                currentPosition.x += speed * deltaTime * (destination.x - currentPosition.x) / magnitude;
                currentPosition.y += speed * deltaTime * (destination.y - currentPosition.y) / magnitude;
                //Console.WriteLine(currentPosition.x + " " + currentPosition.y + " " + (speed * deltaTime * (destination.x - currentPosition.x) / magnitude));
            }
        }
    }
}
