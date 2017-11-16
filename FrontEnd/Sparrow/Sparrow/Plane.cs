using Sparrow.Representation;
using Sparrow.Road;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Sparrow
{
    public partial class Plane : UserControl
    {
        private List<RoadModel> roadSegments = new List<RoadModel>();
        private List<VehicleModel> vehicles = new List<VehicleModel>();

        private System.Threading.Timer vehicleUpdater;

        private float zoom = 1f;
        private Point2D offset = new Point2D(0, 0);//add offset after adding zoom so you can still move around at expected speeds while zoomed in

        private const int timerPeriodMilliseconds = 30;
        private RoadSegments _roadSegments;

        public Plane()
        {
            RoadModel rs = new RoadModel(new Point2D(0, 0), new Point2D(175, 75));
            RoadModel rs2 = new RoadModel(new Point2D(175, 75), new Point2D(100, 150));

            rs.endPointRoadChoices.Add(rs2);
            rs2.endPointRoadChoices.Add(rs);

            VehicleModel vr = new VehicleModel();
            vr.currentPosition = rs.startPoint;
            vr.currentRoadSegment = rs;
            vr.speed = 30;
            roadSegments.Add(rs);
            roadSegments.Add(rs2);
            vehicles.Add(vr);

            vehicleUpdater = new System.Threading.Timer(UpdateVehicles, null, 0, timerPeriodMilliseconds);

            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            Click += DrawingSpace_Click;
            //Scroll += DrawingSpace_Scroll;
        }

        internal void BuildRoads(RoadSegments _roadSegments)
        {
            this._roadSegments = _roadSegments;
        }

        private void DrawRoadSegment(PaintEventArgs e, Datum roadSegment)
        {
            System.Drawing.Pen myPen = new System.Drawing.Pen((System.Drawing.Color.Blue), 10);

            e.Graphics.DrawLine(myPen, roadSegment.start_location.x, roadSegment.start_location.y, roadSegment.end_location.x, roadSegment.end_location.y);
        }

        private void DrawingSpace_Scroll(object sender, ScrollEventArgs e)
        {
            ZoomIn();
        }

        private void DrawingSpace_Click(object sender, EventArgs e)
        {
            //Someday will want to discern between left and right clicks for zoom in and out
            ZoomIn();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_roadSegments == null)
            {
                return;
            }
            foreach (var roadSegment in _roadSegments.roads[0].data)
            {
                DrawRoadSegment(e, roadSegment);
            }
            //            int segmentEndpointWidth = 4;
            //            int segWidthDiv2 = segmentEndpointWidth / 2;
            //
            //            Pen segmentEndpointDrawer = new Pen(Color.BurlyWood, segmentEndpointWidth);
            //            Pen roadDrawer = new Pen(Color.Purple, 2);
            //            Pen vehicleDrawer = new Pen(Color.DarkOliveGreen, segmentEndpointWidth);
            //
            //            foreach (var roadSegment in roadSegments)
            //            {
            //                e.Graphics.DrawEllipse(segmentEndpointDrawer, roadSegment.startPoint.x * zoom, roadSegment.startPoint.y * zoom, segmentEndpointWidth * zoom, segmentEndpointWidth * zoom);
            //                e.Graphics.DrawEllipse(segmentEndpointDrawer, roadSegment.endPoint.x * zoom, roadSegment.endPoint.y * zoom, segmentEndpointWidth * zoom, segmentEndpointWidth * zoom);
            //
            //                e.Graphics.DrawLine(roadDrawer, (roadSegment.startPoint.x + segWidthDiv2) * zoom, (roadSegment.startPoint.y + segWidthDiv2) * zoom, (roadSegment.endPoint.x + segWidthDiv2) * zoom, (roadSegment.endPoint.y + segWidthDiv2) * zoom);
            //            }
            ////            foreach (var vehicle in vehicles)
            ////            {
            ////                e.Graphics.DrawEllipse(vehicleDrawer, vehicle.currentPosition.x * zoom, vehicle.currentPosition.y * zoom, vehicleDrawer.Width * zoom, vehicleDrawer.Width * zoom);
            ////            }
        }

        private void UpdateVehicles(object state)
        {
            float deltaTime = timerPeriodMilliseconds / ((float)1000);
            foreach (var vehicle in vehicles)
            {
                vehicle.Update(deltaTime);
            }
            Invalidate(); //redraw the form.
        }

        private void ZoomIn()
        {
            zoom += 0.1f;
        }

        private void DrawingSpace_Load(object sender, EventArgs e)
        {
        }

        private void Plane_Load(object sender, EventArgs e)
        {
        }
    }
}