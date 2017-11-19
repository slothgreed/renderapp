using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
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
            ProjectNodeViewModel = new RootNodeViewModel(this, Project.ActiveProject.RootNode, "Project");
            SceneNodeViewModel = new RootNodeViewModel(this, Workspace.MainScene.RootNode, "Scene");
            ViewportViewModel = new ViewportViewModel(this);
            RendererViewModel = new RendererViewModel(this);

            RendererViewModel.PropertyChanged += RendererViewModel_PropertyChanged;
            SceneNodeViewModel.PropertyChanged += SceneNodeViewModel_PropertyChanged;

            AnchorablesSources = new ObservableCollection<ViewModelBase>();
            DocumentsSources = new ObservableCollection<ViewModelBase>();
            AnchorablesSources.Add(ProjectNodeViewModel);
            AnchorablesSources.Add(SceneNodeViewModel);
            AnchorablesSources.Add(RendererViewModel);
            DocumentsSources.Add(ViewportViewModel);
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

            DockWindowViewModel vm = null;
            if (node.KIObject is SceneNode)
            {
                vm = new RenderObjectViewModel(this, node.KIObject as RenderObject);
                vm.PropertyChanged += RenderObjectViewModel_PropertyChanged;
                Workspace.MainScene.SelectNode = (SceneNode)node.KIObject;
            }

            ReplaceTabWindow(vm);
        }

        public void ReplaceTabWindow(DockWindowViewModel window)
        {
            if (window is RenderObjectViewModel)
            {
                var oldItem = AnchorablesSources.FirstOrDefault(p => p is RenderObjectViewModel);
                AnchorablesSources.Add(window);
                AnchorablesSources.Remove(oldItem);
            }
        }

        public void OpenWindow(AppWindow windowType)
        {
        }
    }
}
