using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
                        AddIntersection(new Intersection(this, new[] { road, otherRoad }, hit));
                    }
                }
            }
        }

        /// <summary>
        /// Get configuration values, 
        /// will return each light's green duration value. 
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<Guid, float> GetDefaultValues()
        {
            Dictionary<Guid, float> lightConfigs = new Dictionary<Guid, float>();
            foreach (var intersection in Intersections)
            {
                foreach (var light in intersection.Lights)
                {
                    if (!lightConfigs.ContainsKey(light.Id))
                    {
                        lightConfigs.Add(light.Id, light.GreenDuration);
                    }
                }
            }
            return lightConfigs;
        }


        /// <summary>
        /// TODO: clean up logic.
        /// sets the configuration for each light. 
        /// pass in value is a dictionary composed of the key 
        /// </summary>
        /// <param name="newStates"></param>
        public void SetConfigStates(Dictionary<Guid, float> newStates)
        {
            foreach (var newState in newStates)
            {
                var foundIntersection = Intersections.First(
                    intersection => intersection.Lights
                                        .FirstOrDefault(light => light.Id == newState.Key) != null
                );
                var foundLight = foundIntersection.Lights.FirstOrDefault(light => light.Id == newState.Key);
                foundLight.GreenDuration = newState.Value;

            }
        }

        public override void Initialize()
        {
            //            throw new NotImplementedException();
        }

        public override void Update(float delta)
        {
//            var results = GetDefaultValues();
//            Console.WriteLine(results);
//
//            var temp  = results.First();
//            results[temp.Key] = 1111111111111111;
//            SetConfigStates(results);
//            var results2 = GetDefaultValues();
//            Console.WriteLine(results2);
//
//            var results3 = GetDefaultValues();
//            Console.WriteLine(results3);
//
//            Console.WriteLine(Intersections.Count);


            foreach (var intersection in Intersections)
            {
                intersection.Update(delta);
            }
        }
    }
}