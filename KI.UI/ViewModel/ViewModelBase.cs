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

        protected ICommand CreateCommand(Action command, Func<bool> canExecute)
        {
            return new DelegateCommand(command, canExecute);
        }

        protected ICommand CreateCommand(Action command)
        {
            return new DelegateCommand(command);
        }

        protected ICommand CreateCommand(Action<object> command)
        {
            return new DelegateCommand(command);
        }
    }
}
