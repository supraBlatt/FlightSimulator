using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    class ManualControlsViewModel
    {
        ManualControlsModel model = new ManualControlsModel();
        public double VM_Rudder
        {
            get { return model.Rudder; }
            set { model.Rudder = value;}
        }

        public double VM_Throttle
        {
            get { return model.Throttle; }
            set { model.Throttle = value; }
        }

        public double VM_Aileron
        {
            get {return Math.Truncate(model.Aileron * 100) / 100; }
            set { model.Aileron = value; }
        }

        public double VM_Elevator
        {
            get { return Math.Truncate(model.Elevator * 100) / 100; }
            set { model.Elevator = value; }
        }

        public ManualControlsViewModel() {
            
        }
    }
}
