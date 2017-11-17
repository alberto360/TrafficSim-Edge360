using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace TrafficSim
{
    public partial class Form1 : Form
    {
        private RoadSegments _roadSegments;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (StreamReader r = new StreamReader(@"D:/git/TrafficSim-Edge360/Schemas/Roads.json"))
            {
                string json = r.ReadToEnd();
                _roadSegments = JsonConvert.DeserializeObject<RoadSegments>(json);

                //                Console.WriteLine(items.ToString());

                simMap1.BuildRoads(_roadSegments);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
        }

        private void UIThread(Action t)
        {
            Invoke(t);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void DisplayRoadInformation(Road selectedLine)
        {
            var displayRoad = new RoadDisplay()
            {
                Name = selectedLine.RoadSegment.name,
                Number = selectedLine.RoadSegment.number,
                Speedlimit = selectedLine.RoadSegment.speedlimit.speedlimit.ToString(),


            };

            infoPropertyGrid.SelectedObject = displayRoad;
        }
    }
}

