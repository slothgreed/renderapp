using CADApp.Tool.Control;
using KI.UI.ViewModel;

namespace CADApp.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private ViewportViewModel _ViewportViewModel;
        public ViewportViewModel ViewportViewModel
        {
            get
            {
                return _ViewportViewModel;
            }

            set
            {
                SetValue(ref _ViewportViewModel, value);
            }
        }

        public MainWindowViewModel()
            : base(null)
        {
            ViewportViewModel = new ViewportViewModel(this);
        }

        private void UndoCommand()
        {
            
        }

        private void RedoCommand()
        {

        }

        private void ControllerCommand(object parameter)
        {
            if (parameter is ControllerType)
            {
                var type = (ControllerType)parameter;
                ViewportViewModel.CurrentController = type;
            }
        }
    }
}
