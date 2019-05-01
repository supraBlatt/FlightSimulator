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
    public class FlightBoardViewModel : BaseNotify
    {

        public FlightBoardViewModel()
        {

        }

        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("Added"))
            {
                string dataBeforeConversion = (sender as DataQueue).RemoveElement();
                // so you add it to the queue and immediately remove it to here?

                string[] splitData = dataBeforeConversion.Split(',');
                double newLon = Double.Parse(splitData[0]);
                double newLat = Double.Parse(splitData[1]);
                if(newLon != Lon)
                {
                    Lon = newLon;
                }
                if(newLat != Lat)
                {
                    Lat = newLat;
                }
            }
        }

        private double _Lon;
        public double Lon
        {
            get { return _Lon; }
            set
            {
                _Lon = value;
                NotifyPropertyChanged("Lon");
            }
        }

        private double _Lat;
        public double Lat
        {
            get { return _Lat; }
            set
            {
                _Lat = value;
                NotifyPropertyChanged("Lat");
            }
        }
    }
}
