using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using SparrowDiagram.Models;

namespace SparrowDiagram
{
    public partial class SparrowPlane : UserControl
    {

        private SparrowRoads _roads;

        private RoadSegments _roadSegments;
        private SparrowIntersections _intersections;
        public SparrowPlane()
        {
            DoubleBuffered = true;

            Paint += SparrowDiagram_Paint;
            MouseMove += SparrowDiagram_MouseMove;
            MouseDown += SparrowDiagram_MouseDown; ;
            MouseUp += SparrowDiagram_MouseUp;

            InitializeComponent();
        }

        private void SparrowDiagram_MouseUp(object sender, MouseEventArgs e)
        {
            LineMover_MouseUp(sender, e);
        }

        private void SparrowDiagram_MouseDown(object sender, MouseEventArgs e)
        {
            LineMover_MouseDown(sender, e);
        }

        private void SparrowDiagram_MouseMove(object sender, MouseEventArgs e)
        {
            LineMover_MouseMove(sender, e);
        }

        private void SparrowDiagram_Paint(object sender, PaintEventArgs e)
        {
            if (_roads != null)
            {
                _roads.PaintRoads(e);
            }
            if (_intersections != null)
            {
                _intersections.PaintIntersections(e);
            }
            

        }

        private void LineMover_MouseDown(object sender, MouseEventArgs e)
        {
            if (_roads == null)
            {
                return;
            }
            _roads.RefreshLineSelection(e.Location, this);
            if (_roads.SelectedLine != null && _roads.RoadMoving == null)
            {
                Capture = true;
                //Route has been selected
                var parent = this.Parent as SparrowDiagram;
                parent.displayRoadInformation(_roads.SelectedLine);
            }
            _roads.RefreshLineSelection(e.Location, this);
        }

        private void LineMover_MouseMove(object sender, MouseEventArgs e)
        {
            if (_roads == null)
            {
                return;
            }
            if (_roads.RoadMoving != null)
            {
                _roads.RoadMoving.Line.StartPoint = new PointF(_roads.RoadMoving.StartLinePoint.X + e.X - _roads.RoadMoving.StartMoveMousePoint.X,
                    _roads.RoadMoving.StartLinePoint.Y + e.Y - _roads.RoadMoving.StartMoveMousePoint.Y);
                _roads.RoadMoving.Line.EndPoint = new PointF(_roads.RoadMoving.EndLinePoint.X + e.X - _roads.RoadMoving.StartMoveMousePoint.X,
                    _roads.RoadMoving.EndLinePoint.Y + e.Y - _roads.RoadMoving.StartMoveMousePoint.Y);
            }
            _roads.RefreshLineSelection(e.Location, this);
        }



        private void LineMover_MouseUp(object sender, MouseEventArgs e)
        {
            if (_roads == null)
            {
                return;
            }
            //no route has been selected

            _roads.RefreshLineSelection(e.Location, this);
        }



        public void BuildRoads(RoadSegments _roadSegments)
        {
            _roads = new SparrowRoads(_roadSegments);
            
        }

        internal void BuildIntersections(IntersectionRootModel intersections)
        {
            _intersections = new SparrowIntersections(_roads, intersections);
        }

        private void SparrowPlane_Load(object sender, EventArgs e)
        {

        }
    }
}
