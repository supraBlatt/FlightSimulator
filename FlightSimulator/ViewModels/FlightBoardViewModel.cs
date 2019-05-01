using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    /*the ViewModel for the flight board*/
    public class FlightBoardViewModel : BaseNotify
    {
        /*set the viewModel up*/
        public FlightBoardViewModel()
        {
            //subscribe our "property changed" delegate to the InfoServer property changed event
            InfoServer.Instance.PropertyChanged += Vm_PropertyChanged;
        }
        /*what to do upon the InfoServer property change - update the points*/
        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //execute only if something was added to the queue
            if (e.PropertyName.Equals("Added"))
            {
                //get the data from the queue using the server
                string[] data = InfoServer.Instance.getData();
                //get the new values
                double newLon = Double.Parse(data[0]);
                double newLat = Double.Parse(data[1]);
                //boolean to see if any of the values changed
                Boolean changed = false;
                //change the values, if they indeed need a change
                if (newLon != Lon)
                {
                    Lon = newLon;
                    changed = true;
                }
                if (newLat != Lat)
                {
                    Lat = newLat;
                    changed = true;
                }
                //if any of them changed, notify the flight board that something changed
                if(changed) NotifyPropertyChanged("new point");
            }
        }

        //property for the lon of the plane
        public double Lon
        {
            get;
            set;
        }

        //property for the lat of the plane
        public double Lat
        {
            get;
            set;
        }
    }
}