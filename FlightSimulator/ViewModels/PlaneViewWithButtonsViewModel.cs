using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;
using System.Windows.Input;

namespace FlightSimulator.ViewModels
{
    public class PlaneViewWithButtonsViewModel
    {
        PlaneViewWithButtonsModel model = new PlaneViewWithButtonsModel();
        public ICommand SettingsBtnOpenCommand {
            get { return model.SettingsBtnOpenCommand; }
        }

        public ICommand ConnectBtnOpenCommand
        {
            get { return model.ConnectBtnOpenCommand; }
        }

        public PlaneViewWithButtonsViewModel() { }
    }
}
