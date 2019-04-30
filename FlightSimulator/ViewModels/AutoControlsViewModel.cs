using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;
using System.Windows.Input;
using System.ComponentModel;

namespace FlightSimulator.ViewModels
{

    class AutoControlsViewModel : BaseNotify
    {


        AutoControlModel model = new AutoControlModel();
        public string CommandsString
        {
            get { return model.CommandsString; }
            set { model.CommandsString = value; }
        }

        public ICommand ClearBtnCommand { get { return model.ClearBtnCommand; } }
        public ICommand OKBtnCommand { get { return model.OKBtnCommand; } }

        public AutoControlsViewModel()
        {
            model.PropertyChanged += OnModelPropertyChanged;
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            NotifyPropertyChanged(args.PropertyName);
        }
    }
}
