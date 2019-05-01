using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlightSimulator.Views.Windows;
using FlightSimulator.Model.Interface;

namespace FlightSimulator.Model
{
    
    public class PlaneViewWithButtonsModel
    {
        private CommandClient commandsSender = CommandClient.Instance;
        private InfoServer infoReceiver = InfoServer.Instance;
        private ISettingsModel settings = ApplicationSettingsModel.Instance;

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
            commandsSender.connect(settings.FlightServerIP, settings.FlightCommandPort);
            infoReceiver.connect(settings.FlightServerIP, settings.FlightInfoPort);
        }
    }
}
