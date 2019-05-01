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
            InfoServer.Instance.PropertyChanged += Vm_PropertyChanged;
        }

        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Added"))
            {
                string[] data = InfoServer.Instance.getData();
                double newLon = Double.Parse(data[0]);
                double newLat = Double.Parse(data[1]);
                Boolean changed = false;
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
                if(changed) NotifyPropertyChanged("new point");
            }
        }

        private double _Lon;
        public double Lon
        {
            get { return _Lon; }
            set
            {
                _Lon = value;
            }
        }

        private double _Lat;
        public double Lat
        {
            get { return _Lat; }
            set
            {
                _Lat = value;
            }
        }
    }
}