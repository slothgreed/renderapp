using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KI.UI.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private Action _Command;
        private Action<object> _CommandParam;
        private Func<bool> _CanExecute;

        public DelegateCommand(Action command, Func<bool> canExecute)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }

            _Command = command;
            _CanExecute = canExecute;
        }

        public DelegateCommand(Action command)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }

            _Command = command;
        }

        public DelegateCommand(Action<object> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }

            _CommandParam = command;
        }

        bool ICommand.CanExecute(object parameter)
        {
            if (_CanExecute != null)
            {
                return _CanExecute();
            }
            else
            {
                return true;
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (_Command != null)
            {
                _Command();
            }

            if (_CommandParam != null)
            {
                _CommandParam(parameter);
            }
        }
    }
}
