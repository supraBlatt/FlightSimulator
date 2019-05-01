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
            new Task(() => SendCommands()).Start();
        }

        private void SendCommands()
        {
            Object Lock = new Object();
            lock (Lock)
            {
                string[] subCommands = CommandsString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                foreach (string singleCommand in subCommands)
                {
                    commandSender.SendData(singleCommand);
                    Thread.Sleep(2000);
                    if (CommandsString.Length > singleCommand.Length+2)
                    {
                        CommandsString = CommandsString.Remove(0, singleCommand.Length + 2);
                    }
                }
            }
            ClearCommandsFunc();
        }
    }
}
