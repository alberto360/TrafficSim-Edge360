using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim
{
    public interface ASimBase
    {
        void Initialize();
        void Update(float delta); 
    }
}
