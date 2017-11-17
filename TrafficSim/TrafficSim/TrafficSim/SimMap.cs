using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TrafficSim
{
    public partial class SimMap : UserControl
    {
        public SimManager simulation;
        private Timer timer;
        private SimGraphics simGraphics;

        public SimMap()
        {
            MouseMove += SimMap_MouseMove;
            MouseDown += SimMap_MouseDown; ;
            MouseUp += SimMap_MouseUp;

            InitializeComponent();
        }

        private void SimMap_MouseUp(object sender, MouseEventArgs e)
        {
            //            throw new NotImplementedException();
        }

        private void SimMap_MouseDown(object sender, MouseEventArgs e)
        {
            simGraphics.RefreshLineSelection(e.Location, simulation.Roads, this);
 
            if (simGraphics.SelectedLine != null )
            {
                Capture = true;
                //Route has been selected
                var parent = this.Parent as Form1;
                parent.DisplayRoadInformation(simGraphics.SelectedLine);
            }
        }

        internal void BuildRoads(RoadSegments roadSegments)
        {
            CarManager cm = new CarManager();
            Road[] roadList = new Road[roadSegments.roads[0].data.Count];
            int i = 0;
            foreach (var roadSegment in roadSegments.roads[0].data)
            {
                roadList[i] = new Road(roadSegment, 2.2f, cm);
                i++;

            }
            simulation = new SimManager(roadList, cm);
//            simulation = new SimManager(new Road[]
//                {
//                    new Road(new List<PointF>()
//                    {
//                        new PointF(30, 30),
//                        new PointF(100, 100),
//                        new PointF(100, 300),
//                        new PointF(300, 500),
//                        new PointF(600, 500)
//                    }, 2.2f)
//                    ,
//                    new Road(new List<PointF>()
//                    {
//                        new PointF(600, 30),
//                        new PointF(30, 600),
//                    }, 2.2f)
//                    ,
//                    new Road(new List<PointF>()
//                    {
//                        new PointF(500, 320),
//                        new PointF(10, 90),
//                    }, 2.2f)
//                    ,
//                    new Road(new List<PointF>()
//                    {
//                        new PointF(600, 320),
//                        new PointF(400, 50),
//                    }, 2.2f)
//                });
        }

        private void SimMap_MouseMove(object sender, MouseEventArgs e)
        {
//            simGraphics.RefreshLineSelection(e.Location, simulation.Roads, this);

            if (simulation == null)
            {
                return;
            }
            simGraphics.RefreshLineSelection(e.Location, simulation.Roads, this);
        }

        private void SimMap_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            simGraphics = new SimGraphics();

            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
            timer.Start();
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

        private void UIThread(Action t)
        {
            Invoke(t);
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
    }
}