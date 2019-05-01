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
        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("added"))
            {
                string dataBeforeConversion = ((DataQueue)sender).RemoveElement();
                string[] splitData = dataBeforeConversion.Split(',');
                double[] lonLat = { Double.Parse(splitData[0]), Double.Parse(splitData[1]) };
                if(Double.Parse(splitData[0]) != Lon)
                {
                    Lon = Double.Parse(splitData[0]);
                }
                if(Double.Parse(splitData[1]) != Lat)
                {
                    Lat = Double.Parse(splitData[1]);
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
