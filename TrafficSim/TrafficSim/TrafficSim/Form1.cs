using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficSim
{
    public partial class Form1 : Form
    {
        //All units are now assumed to be in miles or miles per second unless stated otherwise 
        //(this means that until the roads are updated with the new system, 100 pixels is actually going to be 100 miles and
        public Form1()
        {
            InitializeComponent();
        }
        private int zoomFactor = 100;
        private SimManager simulation;
        private Timer timer;
        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            CarManager cm = new CarManager();
            simulation = new SimManager(new Road[]
            {
                new Road(new List<PointF>()
                {
                    new PointF(30, 30),
                    new PointF(100, 100),
                    new PointF(100, 300),
                    new PointF(300, 500),
                    new PointF(600, 500)
                }, cm)
                ,
                new Road(new List<PointF>()
                {
                    new PointF(600, 30),
                    new PointF(30, 600), 
                }, cm) 
                ,
                new Road(new List<PointF>()
                {
                    new PointF(500, 320),
                    new PointF(10, 90),
                }, cm)
            }, cm);

            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
            timer.Start();
        } 

        private void Timer_Tick(object sender, EventArgs e)
        {
            simulation.Update();
            UIThread(Invalidate);
        }

        private void UIThread(Action t)
        {
            Invoke(t);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var road in simulation.Roads)
            {
                var verts = road.Vertices;
                for (var i = 0; i < verts.Count - 1; i++)
                {
                    e.Graphics.DrawLine(Pens.Black, verts[i], verts[i + 1]);
                }
            }

            float r = 3;
            foreach (var car in simulation.Cars)
            {
                var color = car.IsForward ? Pens.Red : Pens.Blue;
                var brush = car.IsForward ? Brushes.Red : Brushes.Blue;

                var p = new Pen(car.IsForward ? Brushes.Red : Brushes.Blue, 3);

                var offset = car.Position.Add(car.CurrentDirection.Perpendicular().Mult(5));
                 
                e.Graphics.DrawEllipse(color, new RectangleF(offset.Subtract(new PointF(r, r)), new SizeF(r * 2, r * 2)));
                e.Graphics.DrawLine(p, offset, offset.Add(car.CurrentDirection.Mult(7))); 
            }

            foreach (var intersection in simulation.Intersections)
            {
                r = 5;
                e.Graphics.DrawEllipse(Pens.Purple, new RectangleF(intersection.Position.Subtract(new PointF(r, r)), new SizeF(r * 2, r * 2)));

                r = 3; 
                foreach (var road in intersection.Roads)
                {
                    var light = intersection.GetLight(road);
                    var status = intersection.GetLightState(road);

                    var color = Brushes.Purple;
                     
                    switch (status)
                    {
                        case TrafficLight.ETrafficLightStatus.Yellow:
                            color = Brushes.Yellow;
                            break;
                        case TrafficLight.ETrafficLightStatus.Green:
                            color = Brushes.Green;
                            break;
                        case TrafficLight.ETrafficLightStatus.Red:
                            color = Brushes.Red;
                            break;
                    }

                    var offset = light.Segment.GetDirection().Mult(15);
                    e.Graphics.DrawLine(new Pen(color, 2), intersection.Position.Add(offset),
                        intersection.Position.Subtract(offset));
                }
                
            }
        }
    }
}
