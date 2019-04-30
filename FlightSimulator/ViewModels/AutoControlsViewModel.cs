using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;
using System.Windows.Input;

namespace FlightSimulator.ViewModels
{

    class AutoControlsViewModel
    {
        AutoControlModel model = new AutoControlModel();
        public string CommandsString
        {
            get { return model.CommandsString; }
            set { model.CommandsString = value; }
        }

        public ICommand ClearBtnCommand { get { return model.ClearBtnCommand; } }
        public ICommand OKBtnCommand { get { return model.OKBtnCommand; } }

        public AutoControlsViewModel() { }
    }
}
