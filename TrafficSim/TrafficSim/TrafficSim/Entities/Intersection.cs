using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace TrafficSim
{
    public class Intersection : ASimBase
    {
        private readonly Dictionary<Road, TrafficLight> _lightCache = new Dictionary<Road, TrafficLight>();
        private int _currentLightIndex;
        private int _numberOfLightsAtIntersection = 2; //changed this from a magic number to something we can manipulate on a per intersection basis later. -MR



        public Intersection(IntersectionManager manager, Road[] roads, PointF position, int numberOfDistinctLightSets = 2)
        {

            Roads = roads;
            Position = position;

            IntersectionManager = manager;

            _numberOfLightsAtIntersection = numberOfDistinctLightSets;

            Lights = new List<TrafficLight>();

            TrafficLight firstLight = null;

            foreach (var road in roads)
            {
                var light = new TrafficLight(this, firstLight);
                if (firstLight == null)
                {
                    firstLight = light;
                }
                Lights.Add(light);
                light.Road = road;
                light.Segment = road.GetSegment(Position);
                road.Intersections.Add(this);
                _lightCache.Add(road, light);
            }

            IntersectionManager.AddIntersection(this);
        }

        public IntersectionManager IntersectionManager { get; set; }

        public List<TrafficLight> Lights { get; set; }

        public PointF Position { get; set; }
        public Road[] Roads { get; set; }

        private int CurrentLightIndex
        {
            get => _currentLightIndex;
            set => _currentLightIndex = value % _numberOfLightsAtIntersection;
        }

        public TrafficLight GetLight(Road road)
        {
            if (_lightCache.TryGetValue(road, out TrafficLight v))
            {
                return v;
            }
            return null;
        }

        public TrafficLight.ETrafficLightStatus GetLightState(Road road)
        {
            if (_lightCache.TryGetValue(road, out TrafficLight v))
            {
                return v.Status;
            }
            return TrafficLight.ETrafficLightStatus.Red;
        }

        public void Initialize()
        {
        }


        public void Update(float delta)
        {


            foreach (var light in Lights)
            {
                light.Update(delta);

                if (light.Status != TrafficLight.ETrafficLightStatus.Red)
                {
                    return;
                }
            }

            CurrentLightIndex++;

            var trafficLight = Lights[CurrentLightIndex];
            if (trafficLight != null)
            {
                trafficLight.Status = TrafficLight.ETrafficLightStatus.Green;
            }
        }
    }
}