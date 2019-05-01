using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.Model
{
    /*basic command handler class*/
    public class CommandHandler : ICommand
    {
        private Action _action;
        /*set up the command handeler*/
        public CommandHandler(Action action)
        {
            _action = action;
        }

        /*check if the action is executable, which is always true*/
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        /*exeucte the action*/
        public void Execute(object parameter)
        {
            _action();
        }
    }
}
