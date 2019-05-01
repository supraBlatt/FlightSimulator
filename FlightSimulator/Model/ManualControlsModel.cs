using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class ManualControlsModel
    {
        private CommandClient commandSender = CommandClient.Instance;
        private double _Rudder;
        public double Rudder
        {
            get { return _Rudder; }
            set
            {
                _Rudder = value;
                string toSend = "set controls/flight/rudder " + value.ToString();
                commandSender.SendData(toSend);
            }
        }
        private double _Throttle;
        public double Throttle
        {
            get { return _Throttle; }
            set
            {
                _Throttle = value;
                string toSend = "set controls/flight/throttle " + value.ToString();
                commandSender.SendData(toSend);
            }
        }
        private double _Aileron;
        public double Aileron
        {
            get { return _Aileron; }
            set
            {
                _Aileron = value;
                string toSend = "set controls/flight/aileron " + value.ToString();
                commandSender.SendData(toSend);
            }
        }
        private double _Elevator;
        public double Elevator
        {
            get { return _Elevator; }
            set
            {
                _Elevator = value;
                string toSend = "set controls/flight/elevator " + value.ToString();
                commandSender.SendData(toSend);
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
