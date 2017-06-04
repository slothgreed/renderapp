using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KI.Foundation.ViewModel
{
    public class CommandBase : ICommand
    {
        #region [Command]
        private Action _Command;
        private Action<object> _CommandParam;
        private Func<bool> _CanExecute;

        private CommandBase(Action command, Func<bool> canExecute)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }
            _Command = command;
            _CanExecute = canExecute;
        }
        private CommandBase(Action command)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }
            _Command = command;
        }
        private CommandBase(Action<object> command)
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
        public event EventHandler CanExecuteChanged;

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
        public static ICommand CreateCommand(Action command, Func<bool> canExecute)
        {
            return new CommandBase(command, canExecute);
        }
        public static ICommand CreateCommand(Action command)
        {
            return new CommandBase(command);
        }
        public static ICommand CreateCommand(Action<object> command)
        {
            return new CommandBase(command);
        }
        #endregion
    }
}
