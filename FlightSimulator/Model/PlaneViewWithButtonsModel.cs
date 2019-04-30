using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlightSimulator.Views.Windows;

namespace FlightSimulator.Model
{
    
    public class PlaneViewWithButtonsModel
    {
        public ICommand SettingsBtnOpenCommand { get; }

        public ICommand ConnectBtnOpenCommand { get; }

        public PlaneViewWithButtonsModel()
        {
            SettingsBtnOpenCommand = new CommandHandler(OpenSettings);
            ConnectBtnOpenCommand = new CommandHandler(OpenConnection);
        }

        void OpenSettings()
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Show();
        }

        void OpenConnection()
        {
        }
    }
}
