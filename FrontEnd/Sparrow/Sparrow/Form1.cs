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
using Sparrow.Road;

namespace Sparrow
{
    public partial class SparrowForm : Form
    {
        public SparrowForm()
        {
            InitializeComponent();
        }


        private RoadSegments _roadSegments;
        private void SparrowForm_Load(object sender, EventArgs e)
        {
            using (StreamReader r = new StreamReader(@"D:\temp\TrafficControl\Schemas\Roads.json"))
            {
                string json = r.ReadToEnd();
                _roadSegments = JsonConvert.DeserializeObject<RoadSegments> (json);

//                Console.WriteLine(items.ToString());

                plane1.BuildRoads(_roadSegments);
            }
        }
    }
}
