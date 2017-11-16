using System;
using System.Collections.Generic;
using System.Drawing;

namespace TrafficSim
{
    public class IntersectionManager : ASimBase
    {
        public IntersectionManager(SimManager manager, List<Intersection> intersections)
        {
            Intersections = intersections;
            SimManager = manager;
        }

        public List<Intersection> Intersections { get; set; }
        public SimManager SimManager { get; set; }

        public void AddIntersection(Intersection intersection)
        {
            Intersections.Add(intersection);
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