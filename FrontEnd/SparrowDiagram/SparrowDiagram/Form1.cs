using Newtonsoft.Json;
using SparrowDiagram.Diagram;
using SparrowDiagram.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace SparrowDiagram
{
    public partial class SparrowDiagram : Form
    {
        private SparrowRoads _roads;
        private IntersectionRootModel _intersections;
        private RoadSegments _roadSegments;

        public SparrowDiagram()
        {
            DoubleBuffered = true;

//            Paint += SparrowDiagram_Paint;
//            MouseMove += SparrowDiagram_MouseMove;
//            MouseDown += SparrowDiagram_MouseDown; ;
//            MouseUp += SparrowDiagram_MouseUp;

            //            var Lines = new List<DiagramRoad>
            //            {
            //                new DiagramRoad(10, 10, 100, 200),
            //                new DiagramRoad(10, 150, 120, 40)
            //            };
            //
            //            _roads = new SparrowRoads(Lines);

            InitializeComponent();
        }

        internal void displayRoadInformation(DiagramRoad selectedLine)
        {

            var displayRoad = new RoadDisplay()
            {
                Name = selectedLine.roadSegment.name,
                Number = selectedLine.roadSegment.number,
                Speedlimit = selectedLine.roadSegment.speedlimit.speedlimit.ToString(),
                OneWay = selectedLine.roadSegment.lanes.Count > 1,

            };

            infoPropertyGrid.SelectedObject = displayRoad;
        }

        private void SparrowDiagram_Load(object sender, EventArgs e)
        {
            using (StreamReader r = new StreamReader(@"D:\temp\TrafficControl\Schemas\Roads.json"))
            {
                string json = r.ReadToEnd();
                _roadSegments = JsonConvert.DeserializeObject<RoadSegments>(json);

                //                Console.WriteLine(items.ToString());

                sparrowPlane1.BuildRoads(_roadSegments);
            }

            using (StreamReader r = new StreamReader(@"D:\temp\TrafficControl\Schemas\Intersections.json"))
            {
                string json = r.ReadToEnd();
                _intersections = JsonConvert.DeserializeObject<IntersectionRootModel>(json);

                //                Console.WriteLine(items.ToString());

                Console.WriteLine(_intersections);
                sparrowPlane1.BuildIntersections(_intersections);
            }
        }
    }
}