using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace TrafficSim
{
   public class SimManagerSession
    {
        private static readonly SimManagerSession _instance = new SimManagerSession();
    

        internal static SimManagerSession Instance => _instance;

        private SimManager _simManager;
   

        internal SimManager CurrentSimManager => _simManager;

        internal void SetSimManager(SimManager simManager)
        {
            if (simManager == null)
            {
                throw new ArgumentException("dispatcher cannot be null in SetDispatcher()");
            }

            if (_simManager != null)
            {
                return;
            }

            _simManager = simManager;
        }
        private SimManagerSession()
        {
        }
    }
}
