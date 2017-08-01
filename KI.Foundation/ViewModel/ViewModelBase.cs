using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace KI.Foundation.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region [PropertyChange view vm]
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        /// <summary>
        /// WPF変更前に使用
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        protected void SetValue<T>(ref T member, T value, [CallerMemberName]string memberName = "")
        {
            OnPropertyChanging(memberName);
            member = value;
            OnPropertyChanged(memberName);
        }
        protected void SetValue<T>(Func<T, bool> updateFunc, T value, [CallerMemberName]string memberName = "")
        {
            OnPropertyChanging(memberName);
            if (updateFunc != null)
            {
                updateFunc(value);
            }
            OnPropertyChanged(memberName);
        }
        protected void SetValue<T>(Action<T> action, T value, [CallerMemberName]string memberName = "")
        {
            OnPropertyChanging(memberName);
            if (action != null)
            {
                action(value);
            }
            OnPropertyChanged(memberName);

        }

        protected void OnPropertyChange([CallerMemberName]string memberName = "")
        {
            OnPropertyChanging(memberName);
            OnPropertyChanged(memberName);
        }
        /// <summary>
        /// WPF変更後に使用
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public virtual void UpdateProperty()
        {

        }


        #endregion

        #region [Command]
        private class _DelegateCommand : ICommand
        {
            private Action _Command;
            private Action<object> _CommandParam;
            private Func<bool> _CanExecute;

            public _DelegateCommand(Action command, Func<bool> canExecute)
            {
                if (command == null)
                {
                    throw new ArgumentNullException();
                }
                _Command = command;
                _CanExecute = canExecute;
            }
            public _DelegateCommand(Action command)
            {
                if (command == null)
                {
                    throw new ArgumentNullException();
                }
                _Command = command;
            }
            public _DelegateCommand(Action<object> command)
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
        }
        protected ICommand CreateCommand(Action command, Func<bool> canExecute)
        {
            return new _DelegateCommand(command, canExecute);
        }
        protected ICommand CreateCommand(Action command)
        {
            return new _DelegateCommand(command);
        }
        protected ICommand CreateCommand(Action<object> command)
        {
            return new _DelegateCommand(command);
        }
        #endregion

    }
}
