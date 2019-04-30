using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class ManualControlsModel
    {

        public double Rudder { get; set; }
        public double Throttle { get; set; }
        public double Aileron { get; set; }
        public double Elevator { get; set; }

        public ManualControlsModel()
        {
            Rudder = 0;
            Throttle = 0;
            Aileron = 0;
            Elevator = 0;
        }

    }
}
