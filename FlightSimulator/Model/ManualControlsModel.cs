using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class ManualControlsModel
    {
        private CommandClient commandSender;
        public double Rudder
        {
            get { return Rudder; }
            set
            {
                Rudder = value;
                string toSend = "set controls/flight/rudder " + value.ToString();
                commandSender.sendData(toSend);
            }
        }
        public double Throttle
        {
            get { return Throttle; }
            set
            {
                Throttle = value;
                string toSend = "set controls/flight/throttle " + value.ToString();
                commandSender.sendData(toSend);
            }
        }
        public double Aileron
        {
            get { return Aileron; }
            set
            {
                Aileron = value;
                string toSend = "set controls/flight/aileron " + value.ToString();
                commandSender.sendData(toSend);
            }
        }
        public double Elevator
        {
            get { return Elevator; }
            set
            {
                Elevator = value;
                string toSend = "set controls/flight/elevator " + value.ToString();
                commandSender.sendData(toSend);
            }
        }

        public ManualControlsModel()
        {
            Rudder = 0;
            Throttle = 0;
            Aileron = 0;
            Elevator = 0;
        }

    }
}
