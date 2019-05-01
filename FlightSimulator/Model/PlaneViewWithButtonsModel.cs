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
    /*The Model for the Buttons of the Plane View*/
    public class PlaneViewWithButtonsModel
    {
        private InfoServer infoReceiver = InfoServer.Instance;
        private CommandClient commandsSender = CommandClient.Instance;
        private ISettingsModel settings = ApplicationSettingsModel.Instance;

        public ICommand SettingsBtnOpenCommand { get; }

        public ICommand ConnectBtnOpenCommand { get; }

        /*set up the Model*/
        public PlaneViewWithButtonsModel()
        {
            SettingsBtnOpenCommand = new CommandHandler(OpenSettings);
            ConnectBtnOpenCommand = new CommandHandler(OpenConnection);
        }

        /*Opens the settings window when the "settings" button is pressed*/
        void OpenSettings()
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Show();
        }

        /*opens the server connections when the "connect" button is pressed*/
        void OpenConnection()
        {
            infoReceiver.Connect(settings.FlightServerIP, settings.FlightInfoPort, settings.FlightCommandPort);
        }
    }
}
