using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KI.UI.ViewModel
{
    public class CommandBase : ICommand
    {
        #region [Command]
        private Action command;
        private Action<object> commandParam;
        private Func<bool> canExecute;

        private CommandBase(Action command, Func<bool> canExecute)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }

            this.command = command;
            this.canExecute = canExecute;
        }

        private CommandBase(Action command)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }

            this.command = command;
        }

        private CommandBase(Action<object> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }

            commandParam = command;
        }

        bool ICommand.CanExecute(object parameter)
        {
            if (canExecute != null)
            {
                return canExecute();
            }
            else
            {
                return true;
            }
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (command != null)
            {
                command();
            }

            if (commandParam != null)
            {
                commandParam(parameter);
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
