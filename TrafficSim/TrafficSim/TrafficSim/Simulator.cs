using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace TrafficSim
{
    public class Simulator:IDisposable
    {
        public SimManager simulation;

        private readonly int TIME_INTERVAL_IN_MILLISECONDS;
        private readonly Timer _timer;
        private SimGraphics simGraphics;
        private readonly RoadSegments _roadSegments;


        public Simulator()
        {
            TIME_INTERVAL_IN_MILLISECONDS = 10;
            simGraphics = new SimGraphics();

            using (var r = new StreamReader(@"D:/git/TrafficSim-Edge360/Schemas/Roads.json"))
            {
                var json = r.ReadToEnd();
                _roadSegments = JsonConvert.DeserializeObject<RoadSegments>(json);
                BuildRoads(_roadSegments);
            }
            _timer = new Timer(Timer_Tick, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        ///     Get configuration values,
        ///     will return each light's green duration value.
        /// </summary>
        /// <returns></returns>
        public Dictionary<Guid, float> GetDefaultValues()
        {
            var lightConfigs = new Dictionary<Guid, float>();
            foreach (var intersection in simulation.Intersections)
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
        ///     TODO: clean up logic.
        ///     sets the configuration for each light.
        ///     pass in value is a dictionary composed of the key
        /// </summary>
        /// <param name="newStates"></param>
        public void SetConfigStates(Dictionary<Guid, float> newStates)
        {
            foreach (var newState in newStates)
            {
                var foundIntersection = simulation.Intersections.First(
                    intersection => intersection.Lights
                                        .FirstOrDefault(light => light.Id == newState.Key) != null
                );
                var foundLight = foundIntersection.Lights.FirstOrDefault(light => light.Id == newState.Key);
                foundLight.GreenDuration = newState.Value;
            }
            _timer.Change(TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);

        }

        private void BuildRoads(RoadSegments roadSegments)
        {
            var cm = new CarManager();
            var roadList = new Road[roadSegments.roads[0].data.Count];
            var i = 0;
            foreach (var roadSegment in roadSegments.roads[0].data)
            {
                roadList[i] = new Road(roadSegment, 2.2f, cm);
                i++;
            }
            simulation = new SimManager(roadList, cm);
        }

        private void Timer_Tick(object state)
        {
            if (simulation == null)
            {
                return;
            }
            simulation.Update();
            _timer.Change(TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }
        }
    }
}