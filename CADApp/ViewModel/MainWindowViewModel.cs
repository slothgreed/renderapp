using CADApp.Model;
using CADApp.Tool.Controller;
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
            Workspace.Instance.CommandManager.Undo();
            ViewportViewModel.Controller[ViewportViewModel.CurrentController].Reset();
            ViewportViewModel.Viewport.GLControl_Paint(null, null);
        }

        private void RedoCommand()
        {
            Workspace.Instance.CommandManager.Redo();
            ViewportViewModel.Viewport.GLControl_Paint(null, null);
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
