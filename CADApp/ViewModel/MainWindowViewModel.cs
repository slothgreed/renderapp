using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CADApp.Model;
using CADApp.Tool.Controller;
using KI.Renderer;
using KI.UI.ViewModel;
using System.Windows.Controls;

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

        private ObservableCollection<SceneNode> _RootNode = new ObservableCollection<SceneNode>();
        public ObservableCollection<SceneNode> RootNode
        {
            get
            {
                return _RootNode;
            }

            set
            {
                SetValue(ref _RootNode, value);
            }

        }


        public MainWindowViewModel()
            : base(null)
        {
            ViewportViewModel = new ViewportViewModel(this);
            ViewportViewModel.OnInitialized += ViewportViewModel_Initialized;
        }

        private void ViewportViewModel_Initialized(object sender, EventArgs e)
        {
            RootNode.Add(Workspace.Instance.MainScene.RootNode);
        }

        private void UndoCommand()
        {
            Workspace.Instance.CommandManager.Undo();
            ViewportViewModel.Controller[ViewportViewModel.CurrentControllerType].Reset();
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
                ViewportViewModel.ChangeController(type, null);
            }

            if (parameter is ControllerCommandParameter)
            {
                var param = (ControllerCommandParameter)parameter;
                ViewportViewModel.ChangeController(param.ControllerType, param.ControllerArgs);
            }
        }
    }
}
