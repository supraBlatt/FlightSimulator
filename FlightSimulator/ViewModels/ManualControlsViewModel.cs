using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    /*the View Model for the manual controls*/
    class ManualControlsViewModel
    {
        ManualControlsModel model = new ManualControlsModel();
        //property for the Rudder of the plane
        public double VM_Rudder
        {
            get { return model.Rudder; }
            set { model.Rudder = value;}
        }
        //property for the Throttle of the plane
        public double VM_Throttle
        {
            get { return model.Throttle; }
            set { model.Throttle = value; }
        }
        //property for the Aileron of the plane
        public double VM_Aileron
        {
            get { return Math.Truncate(model.Aileron * 100) / 100; }
            set { model.Aileron = value; }
        }
        //property for the Elevator of the plane
        public double VM_Elevator
        {
            get { return Math.Truncate(model.Elevator * 100) / 100; }
            set { model.Elevator = value; }
        }
    }
}
