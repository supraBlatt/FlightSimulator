using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlightSimulator.ViewModels;
using System.Threading;

namespace FlightSimulator.Model
{
    class AutoControlModel : BaseNotify
    {
        private CommandClient commandSender = CommandClient.Instance;
        private string _commandString;
        public string CommandsString
        {
            get { return _commandString; }
            set
            {
                if (!value.Equals(_commandString))
                {
                    _commandString = value;
                    NotifyPropertyChanged("CommandsString");
                }
            }
        }

        public ICommand ClearBtnCommand { get; set; }
        public ICommand OKBtnCommand { get; set; }
        public AutoControlModel() {
            ClearBtnCommand = new CommandHandler(ClearCommandsFunc);
            OKBtnCommand = new CommandHandler(OkCommandFunc);
        }

        void ClearCommandsFunc()
        {
            CommandsString = "";
        }

        void OkCommandFunc()
        {
            new Task(() => SendCommands(CommandsString)).Start();
        }

        private void SendCommands(string commands)
        {
            foreach (string singleCommand in commands.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
            {
                commandSender.sendData(singleCommand);
                Thread.Sleep(2000);
            }
            ClearCommandsFunc();
        }
    }
}
