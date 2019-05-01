using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    /*Model for the manual controls*/
    class ManualControlsModel
    {
        private CommandClient commandSender = CommandClient.Instance;

        //property for the Rudder of the plane
        private double _rudder;
        public double Rudder
        {
            get { return _rudder; }
            set
            {
                _rudder = value;
                //convert the Rudder in to the specific command to change it in the Flight Simulator
                string toSend = "set controls/flight/rudder " + value.ToString();
                //send the command through the client for the commands
                commandSender.SendData(toSend);
            }
        }
        private double _throttle;
        private double _realThrottle;
        public double Throttle
        {
            get { return _throttle; }
            set
            {
                _throttle = value;
                if (value >= 0)
                {
                    _realThrottle = value;
                    //convert the Throttle in to the specific command to change it in the Flight Simulator
                    string toSend = "set controls/engines/current-engine/throttle " + value.ToString();
                    //send the command through the client for the commands
                    commandSender.SendData(toSend);
                }
            }
        }
        //property for the Aileron of the plane
        private double _Aileron;
        public double Aileron
        {
            get { return _Aileron; }
            set
            {
                _Aileron = value;
                //convert the Aileron in to the specific command to change it in the Flight Simulator
                string toSend = "set controls/flight/aileron " + value.ToString();
                //send the command through the client for the commands
                commandSender.SendData(toSend);
            }
        }
        //property for the Elevator of the plane
        private double _Elevator;
        public double Elevator
        {
            get { return _Elevator; }
            set
            {
                _Elevator = value;
                //convert the Elevator in to the specific command to change it in the Flight Simulator
                string toSend = "set controls/flight/elevator " + value.ToString();
                //send the command through the client for the commands
                commandSender.SendData(toSend);
            }
        }

        /*set up the manual controls model*/
        public ManualControlsModel()
        {
            Rudder = 0;
            Throttle = 0;
            Aileron = 0;
            Elevator = 0;
        }

    }
}
