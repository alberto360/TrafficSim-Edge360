using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace TrafficSim
{
    public partial class SimMap : UserControl
    {
        public SimManager simulation;
        private Timer timer;
        private SimGraphics simGraphics;
        private RoadSegments _roadSegments;
        public SimMap()
        {
            MouseMove += SimMap_MouseMove;
            MouseDown += SimMap_MouseDown;
            MouseUp += SimMap_MouseUp;

            InitializeComponent();
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
        }

        internal void BuildRoads(RoadSegments roadSegments)
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

        private void UIThread(Action t)
        {
            Invoke(t);
        }

        private void SimMap_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            simGraphics = new SimGraphics();

            using (StreamReader r = new StreamReader(@"../../../../../Schemas/Roads.json"))
            {
                string json = r.ReadToEnd();
                _roadSegments = JsonConvert.DeserializeObject<RoadSegments>(json);
                BuildRoads(_roadSegments);
            }

            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void SimMap_MouseDown(object sender, MouseEventArgs e)
        {
            simGraphics.RefreshLineSelection(e.Location, simulation.Roads, this);

            if (simGraphics.SelectedLine != null)
            {
                Capture = true;
                //Route has been selected
                var parent = Parent as Form1;
                parent.DisplayRoadInformation(simGraphics.SelectedLine);
            }
        }

        private void SimMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (simulation == null)
            {
                return;
            }
            simGraphics.RefreshLineSelection(e.Location, simulation.Roads, this);
        }

        private void SimMap_MouseUp(object sender, MouseEventArgs e)
        {
            //            throw new NotImplementedException();
        }

        private void SimMap_Paint(object sender, PaintEventArgs e)
        {
            if (simulation == null)
            {
                return;
            }
            foreach (var road in simulation.Roads)
            {
                simGraphics.DrawRoads(e, road);
            }

            foreach (var car in simulation.Cars)
            {
                simGraphics.DrawVehicle(e, car);
            }

            foreach (var intersection in simulation.Intersections)
            {
                simGraphics.DrawIntersection(e, intersection);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (simulation == null)
            {
                return;
            }
            simulation.Update();
            UIThread(Invalidate);
        }
    }
}