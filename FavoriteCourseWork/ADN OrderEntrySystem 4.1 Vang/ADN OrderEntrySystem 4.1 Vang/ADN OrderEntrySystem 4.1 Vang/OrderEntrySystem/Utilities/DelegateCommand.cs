using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderEntrySystem
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> command;

        private readonly Predicate<object> canExecute;

        public DelegateCommand(Action<object> command)
            : this(command, null)
        {
        }

        public DelegateCommand(Action<object> command, Predicate<object> canExecute)
        {
            if (command == null)
            {
                throw new Exception("Command was null.");
            }

            this.command = command;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.command(parameter);
        }
    }
}