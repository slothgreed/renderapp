using KI.UI.ViewModel;

namespace RenderApp.ViewModel.Dialog
{
    public abstract class DialogViewModelBase : ViewModelBase
    {
        public DialogViewModelBase(ViewModelBase parent)
            : base(parent)
        {
        }

        public virtual void Close(bool boolean)
        {
        }
    }
}
