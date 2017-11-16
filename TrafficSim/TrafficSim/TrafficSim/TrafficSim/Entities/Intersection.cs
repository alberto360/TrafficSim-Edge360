using System.Collections.Generic;
using System.Drawing;

namespace TrafficSim
{
    public class Intersection : ASimBase
    {
        private readonly Dictionary<Road, TrafficLight> _lightCache = new Dictionary<Road, TrafficLight>();
        private int _currentLightIndex;

        public Intersection(SimManager simManager, IntersectionManager manager, Road[] roads, PointF position)
        {
            Roads = roads;
            Position = position;

            SimManager = simManager;

            IntersectionManager = manager;

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

        public SimManager SimManager { get; set; }

        public PointF Position { get; set; }
        public Road[] Roads { get; set; }

        private int CurrentLightIndex
        {
            get => _currentLightIndex;
            set => _currentLightIndex = value % 2;
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

        public override void Initialize()
        {
        }


        public override void Update(float delta)
        {
            foreach (var light in Lights)
            {
                light.Update(delta*SimManager.Rate);

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