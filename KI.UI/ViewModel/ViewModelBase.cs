using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace KI.UI.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region [PropertyChange view vm]

        public ViewModelBase Parent
        {
            get;
            set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        public ViewModelBase(ViewModelBase parent)
        {
            Parent = parent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// WPF変更前に使用
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
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

        /// <summary>
        /// UI変更後に使用
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
