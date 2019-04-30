﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.Model
{
    class AutoControlModel
    {
        public string CommandsString { get; set; }

        public ICommand ClearBtnCommand { get; set; }
        public ICommand OKBtnCommand { get; set; }
        public AutoControlModel() {
            ClearBtnCommand = new CommandHandler(clearCommands);
            OKBtnCommand = new CommandHandler(okCommand);
        }

        void clearCommands()
        {
            CommandsString = "";
            System.Diagnostics.Debug.WriteLine("clearing ");
        }

        void okCommand()
        {

        }
    }
}
