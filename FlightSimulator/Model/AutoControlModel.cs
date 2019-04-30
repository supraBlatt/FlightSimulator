using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlightSimulator.ViewModels;

namespace FlightSimulator.Model
{
    class AutoControlModel : BaseNotify
    {
        string _commandString;
        public string CommandsString
        {
            get { return _commandString; }
            set
            {
                if (value == _commandString)
                    return;
                _commandString = value;
                NotifyPropertyChanged("CommandsString");
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
        }
    }
}
