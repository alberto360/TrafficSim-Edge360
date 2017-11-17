using System;
using System.Collections.Generic;
using System.Drawing;

namespace TrafficSim
{
    public class IntersectionManager : ASimBase
    {
        public IntersectionManager(SimManager manager)
        {
            Intersections = new List<Intersection>();
            SimManager = manager;

            CalculateIntersections();
        }

        public List<Intersection> Intersections { get; set; }
        public SimManager SimManager { get; set; }

        public void AddIntersection(Intersection intersection)
        {
            Intersections.Add(intersection);
        }

        public void CalculateIntersections()
        {
            foreach (var road in SimManager.Roads)
            {
                foreach (var otherRoad in SimManager.Roads)
                {
                    if (road == otherRoad)
                    {
                        continue;
                    }

                    if (road.Intersects(otherRoad, out PointF hit))
                    {
                        var exists = false;
                        foreach (var existing in Intersections)
                        {
                            if (hit.DistanceTo(existing.Position) < 5)
                            {
                                exists = true;
                            }
                        }
                        if (exists)
                        {
                            continue;
                        }
                        AddIntersection(new Intersection(this, new[] {road, otherRoad}, hit));
                    }
                }
            }
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Update(float delta)
        {
            foreach (var intersection in Intersections)
            {
                intersection.Update(delta);
            }
        }
    }
}