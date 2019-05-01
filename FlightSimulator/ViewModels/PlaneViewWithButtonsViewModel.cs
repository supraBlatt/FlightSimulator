using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;
using System.Windows.Input;

namespace FlightSimulator.ViewModels
{
    /*the View Model for the buttons of the plane*/
    public class PlaneViewWithButtonsViewModel
    {
        PlaneViewWithButtonsModel model = new PlaneViewWithButtonsModel();
        //Command property for what to do when the "Settings" button is pressed
        public ICommand SettingsBtnOpenCommand {
            get { return model.SettingsBtnOpenCommand; }
        }

        //Command property for what to do when the "Connect" button is pressed
        public ICommand ConnectBtnOpenCommand
        {
            get { return model.ConnectBtnOpenCommand; }
        }

    }
}
