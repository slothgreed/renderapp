using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.ViewModel;
using System.Windows.Input;
using KI.Foundation.ViewModel;

namespace RenderApp.ViewModel.Dialog
{
    public abstract class DialogViewModelBase : ViewModelBase
    {
        public virtual void Close(bool boolean)
        {
        }


        public override void UpdateProperty()
        {

        }
    }
}
