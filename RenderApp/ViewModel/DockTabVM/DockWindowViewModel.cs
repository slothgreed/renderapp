using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp;
using System.Windows.Input;
using KI.Foundation.ViewModel;

namespace RenderApp.ViewModel.DockTabVM
{
    public abstract class DockWindowViewModel : ViewModelBase
    {
        //protected string _title;
        //public virtual string Title
        //{
        //    get
        //    {
        //        return _title;
        //    }
        //    set
        //    {
        //        SetValue<string>(ref _title, value);
        //    }
        //}
        //private ICommand _CloseTabItem;
        //public ICommand CloseTabItem
        //{
        //    get
        //    {
        //        if (_CloseTabItem == null)
        //        {
        //            return _CloseTabItem = CreateCommand(CloseTabItemCommand);
        //        }
        //        return _CloseTabItem;
        //    }
        //}

        //private void CloseTabItemCommand()
        //{
        //    MainWindowViewModel.Instance.CloseTabWindow(this);
        //}
    }
}
