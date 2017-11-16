using System.Collections.Generic;
using System.Drawing;

namespace TrafficSim
{
    public class Intersection : ASimBase
    {
        private readonly Dictionary<RoadSegmentEndpoint, TrafficLight> _lightCache = new Dictionary<RoadSegmentEndpoint, TrafficLight>();
        private int _currentLightIndex;
        private int _numberOfLightsAtIntersection = 2; //changed this from a magic number to something we can manipulate on a per intersection basis later. -MR

        //Eventually this may be combined with or replace the "_lightCache" field above
        private List<RoadSegmentEndpoint> intersectionEndpoints; //the intersection enpoints should all be "around" the intersection itself - moving through the intersection should consist of moving from endpoint to endpoint, so in a way this class is like a special road segment

        public Intersection(IntersectionManager manager, List<RoadSegmentEndpoint> connectedSegmentEndpoints, PointF position, int numberOfDistinctLightSets = 2)
        {
            Position = position;

            IntersectionManager = manager;

            _numberOfLightsAtIntersection = numberOfDistinctLightSets;

            Lights = new List<TrafficLight>();
            intersectionEndpoints = connectedSegmentEndpoints;

            TrafficLight firstLight = null;

            foreach (var road in roads)
            {
                var light = new TrafficLight(this, firstLight);
                if (firstLight == null)
                {
                    firstLight = light;
                }
                Lights.Add(light);
                //light.Road = road;
                light.Segment = road.GetSegment(Position); //These should be modified to get / use the RoadSegment objects rather than just "Lines"
                road.Intersections.Add(this);
                _lightCache.Add(road, light);
            }

            IntersectionManager.AddIntersection(this);
        }

        public IntersectionManager IntersectionManager { get; set; }

        public List<TrafficLight> Lights { get; set; }

        public PointF Position { get; set; }
       
        private int CurrentLightIndex
        {
            get => _currentLightIndex;
            set => _currentLightIndex = value % _numberOfLightsAtIntersection;
        }

        public TrafficLight GetLight(RoadSegmentEndpoint roadEnpoint)
        {
            if (_lightCache.TryGetValue(roadEnpoint, out TrafficLight v))
            {
                return v;
            }
            return null;
        }

        public TrafficLight.ETrafficLightStatus GetLightState(RoadSegmentEndpoint roadEnpoint)
        {
            if (_lightCache.TryGetValue(roadEnpoint, out TrafficLight v))
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