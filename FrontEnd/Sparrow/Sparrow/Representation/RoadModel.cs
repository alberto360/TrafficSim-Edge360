using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparrow.Representation
{
    class RoadModel
    {
        public List<RoadModel> endPointRoadChoices = new List<RoadModel>();
        public List<RoadModel> startPointRoadChoices = new List<RoadModel>();
        public Point2D endPoint;
        public Point2D startPoint;

        public RoadModel(Point2D start, Point2D end)
        {
            startPoint = start;
            endPoint = end;
        }

        //Inlcude whatever other data a road should have

        //Create "Vehicles" class to display a point moving along the road segment?
    }

    struct Point2D
    {
        public float x;
        public float y;

        public Point2D(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float DistanceSquared(Point2D other)
        {
            return (float)(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2));
        }

        public float Distance(Point2D other)
        {
            return (float)Math.Sqrt(DistanceSquared(other));
        }
    }
}
