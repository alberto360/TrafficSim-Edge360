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
        public Form1()
        {
            InitializeComponent();
        }

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
                }, 2.2f, cm)
                ,
                new Road(new List<PointF>()
                {
                    new PointF(600, 30),
                    new PointF(30, 600), 
                }, 2.2f, cm) 
                ,
                new Road(new List<PointF>()
                {
                    new PointF(500, 320),
                    new PointF(10, 90),
                }, 2.2f, cm)
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
                Color fast = Color.Blue;
                Color slow = Color.Red;

                var colorLerpVal = car.GetSpeedPercentage();
                if (colorLerpVal < 0)
                    colorLerpVal = 0;
                else if (colorLerpVal > 1)
                    colorLerpVal = 1f;

                //If we can get the cars to not almost full stop then go full speed every other update, this interpolation will look much nicer
                Color finalColor = Color.FromArgb(
                    (int)(slow.A + (colorLerpVal * (fast.A - slow.A))),
                    (int)(slow.R + (colorLerpVal * (fast.R - slow.R))),
                    (int)(slow.G + (colorLerpVal * (fast.G - slow.G))),
                    (int)(slow.B + (colorLerpVal * (fast.B - slow.B))));

                //var color = car.IsForward ? Pens.Red : Pens.Blue;
                //var brush = car.IsForward ? Brushes.Red : Brushes.Blue;
                //var p = new Pen(car.IsForward ? Brushes.Red : Brushes.Blue, 3);
                Pen ellipsePen = new Pen(finalColor, 1);
                Pen linePen = new Pen(finalColor, 3);

                var offset = car.Position.Add(car.CurrentDirection.Perpendicular().Mult(5));
                 
                e.Graphics.DrawEllipse(ellipsePen, new RectangleF(offset.Subtract(new PointF(r, r)), new SizeF(r * 2, r * 2)));
                e.Graphics.DrawLine(linePen, offset, offset.Add(car.CurrentDirection.Mult(7))); 
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

                    var offset = light.Segment.Normalized().Mult(15);
                    e.Graphics.DrawLine(new Pen(color, 2), intersection.Position.Add(offset),
                        intersection.Position.Subtract(offset));
                }
                
            }
        }
    }
}
