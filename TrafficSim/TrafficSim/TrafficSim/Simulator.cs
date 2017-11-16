using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace TrafficSim
{
    public class Simulator
    {
        public SimManager _simulation;
        private Timer _timer;
        private float _interval;
        public Simulator()
        {
//            _simulation = new SimManager(null);
        }

        public void Run()
        {
            Console.WriteLine("Task has been init");
            Task responseTask = Task.Run(() => {
                Thread.Sleep(1000);
                Console.WriteLine("In task");
            });

            Console.WriteLine("Task has been completed");

//            _timer = new Timer();
//            _timer.Interval = 10;
//            _timer.Tick += Update_simulation;
//            _timer.Start();
        }

        private void Update_simulation(object sender, EventArgs e)
        {
            if (_simulation == null)
            {
                return;
            }
            _simulation.Update();
        }

        public void Stop()
        {
//            _simulation.Stop();
        }

    }
}
