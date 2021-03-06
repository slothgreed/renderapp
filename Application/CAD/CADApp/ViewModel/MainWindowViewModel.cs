﻿using System;
using System.Collections.ObjectModel;
using CADApp.Model;
using CADApp.Tool.Controller;
using KI.Renderer;
using KI.Presenter.ViewModel;
using CADApp.Tool.Command;
using KI.Asset.Loader.Model;
using KI.Presenter.KIEvent;

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
            Workspace.Instance.CommandManager.OnCommandExecuted += CommandManager_OnCommandExecuted;
            Workspace.Instance.CommandManager.OnCommandUndoPerformed += CommandManager_OnCommandUndoPerformed;
            Workspace.Instance.CommandManager.OnCommandRedoPerformed += CommandManager_OnCommandRedoPerformed;
            RootNode.Add(Workspace.Instance.MainScene.RootNode);
            var loader = new IGESLoader(@"E:\cgModel\model\Chair\Chair_igs\Chair.igs");
            //loader = new IGESLoader(@"E:\cgModel\model\VoronoiRing\VoronoiRing_igs\VoronoiRing.igs");
            //loader = new IGESLoader(@"E:\cgModel\model\ToyLoco\ToyLoco_igs\ToyLoco.igs");
            //loader = new IGESLoader(@"E:\cgModel\model\Table\Table_igs\Table.igs");
            //loader = new IGESLoader(@"E:\cgModel\model\SmartPhone\SmartPhone_igs\SmartPhone.igs");
            //loader = new IGESLoader(@"E:\cgModel\model\Plate\Plate_igs\Plate.igs");
            //loader = new IGESLoader(@"E:\cgModel\model\PencilCase\PencilCase_igs\PencilCase.igs");
            //loader = new IGESLoader(@"E:\cgModel\model\monument\monument_igs\monument.igs");
            //loader = new IGESLoader(@"E:\cgModel\model\Maureen\Maureen_igs\Maureen.igs");
            //loader = new IGESLoader(@"E:\cgModel\model\iPhoneCase\iPhoneCase_igs\iPhoneCase.igs");
            //loader = new IGESLoader(@"E:\cgModel\model\Highheal\Highheal_igs\Highheal.igs");
            //loader = new IGESLoader(@"E:\cgModel\model\Dollhouse\Dollhouse_igs\Dollhouse.igs");
            //loader = new IGESLoader(@"E:\cgModel\model\Chair\Chair_igs\Chair.igs");

        }



        private void UndoCommand()
        {
            Workspace.Instance.CommandManager.Undo();
            ViewportViewModel.Controller[ViewportViewModel.CurrentControllerType].Reset();
        }

        private void RedoCommand()
        {
            Workspace.Instance.CommandManager.Redo();
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

        private void DeleteNodeCommand()
        {
            var commandArgs = new DeleteAssemblyNodeCommandArgs(null);
            Workspace.Instance.CommandManager.Execute(new DeleteAssemblyNodeCommand(commandArgs));
        }

        private void CommandManager_OnCommandExecuted(object sender, EventArgs e)
        {
            UpdateViewModel(e);
            ViewportViewModel.Viewport.GLControl_Paint(null, null);
        }

        private void CommandManager_OnCommandRedoPerformed(object sender, EventArgs e)
        {
            UpdateViewModel(e);
            ViewportViewModel.Viewport.GLControl_Paint(null, null);
        }

        private void CommandManager_OnCommandUndoPerformed(object sender, EventArgs e)
        {
            UpdateViewModel(e);
            ViewportViewModel.Viewport.GLControl_Paint(null, null);
        }


        private void UpdateViewModel(EventArgs e)
        {
            //ViewportViewModel.UpdateViewModel();
        }
    }
}
