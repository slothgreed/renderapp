using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Tree;
using KI.Renderer;
using KI.UI.ViewModel;
using RenderApp.Globals;

namespace RenderApp.ViewModel
{
    public partial class WorkspaceViewModel : ViewModelBase
    {
        public RootNodeViewModel SceneNodeViewModel;
        public RootNodeViewModel ProjectNodeViewModel;
        public RendererViewModel RendererViewModel;
        public ViewportViewModel ViewportViewModel;

        public WorkspaceViewModel(ViewModelBase parent)
            : base(parent)
        {
            LeftUpDockPanel = new TabControlViewModel(this);
            LeftDownDockPanel = new TabControlViewModel(this);
            RightDockPanel = new TabControlViewModel(this);
            CenterDockPanel = new TabControlViewModel(this);

            ProjectNodeViewModel = new RootNodeViewModel(LeftUpDockPanel, Project.ActiveProject.RootNode, "Project");
            SceneNodeViewModel = new RootNodeViewModel(LeftUpDockPanel, Workspace.MainScene.RootNode, "Scene");
            ViewportViewModel = new ViewportViewModel(CenterDockPanel);
            RendererViewModel = new RendererViewModel(LeftDownDockPanel);

            RendererViewModel.PropertyChanged += RendererViewModel_PropertyChanged;
            SceneNodeViewModel.PropertyChanged += SceneNodeViewModel_PropertyChanged;

            LeftUpDockPanel.Add(ProjectNodeViewModel);
            CenterDockPanel.Add(ViewportViewModel);
            LeftDownDockPanel.Add(RendererViewModel);
            LeftUpDockPanel.Add(SceneNodeViewModel);
        }

        private void SceneNodeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewportViewModel.Invalidate();
        }

        private void RendererViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewportViewModel.Invalidate();
        }

        private void RenderObjectViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ViewportViewModel.Invalidate();
        }

        public void UpdateSelectNode(KINode node)
        {
            if (node.KIObject == null)
            {
                return;
            }

            TabItemViewModel vm = null;
            if (node.KIObject is SceneNode)
            {
                vm = new RenderObjectViewModel(RightDockPanel, node.KIObject as RenderObject);
                vm.PropertyChanged += RenderObjectViewModel_PropertyChanged;
                Workspace.MainScene.SelectNode = (SceneNode)node.KIObject;
            }

            ReplaceTabWindow(vm);
        }

        public void ReplaceTabWindow(TabItemViewModel window)
        {
            if (window is RenderObjectViewModel)
            {
                var oldItem = RightDockPanel.FindVM<RenderObjectViewModel>();
                RightDockPanel.ReplaceVM(oldItem, window);
            }
        }

        public void OpenWindow(AppWindow windowType)
        {
        }
    }
}
