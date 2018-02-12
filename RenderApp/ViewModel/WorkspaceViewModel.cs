using System;
using System.Collections.ObjectModel;
using System.Linq;
using KI.Foundation.Tree;
using KI.Gfx.GLUtil;
using KI.Asset;
using KI.Asset.Attribute;
using KI.UI.ViewModel;
using RenderApp.Globals;

namespace RenderApp.ViewModel
{
    public partial class WorkspaceViewModel : ViewModelBase
    {
        public Workspace workspace;
        public RootNodeViewModel SceneNodeViewModel;
        public RendererViewModel RendererViewModel;
        public ViewportViewModel ViewportViewModel;

        public WorkspaceViewModel(ViewModelBase parent, Workspace workspace)
            : base(parent, workspace)
        {
            SceneNodeViewModel = new RootNodeViewModel(this, workspace.MainScene.RootNode, "Scene");
            ViewportViewModel = new ViewportViewModel(this);
            RendererViewModel = new RendererViewModel(this);

            RendererViewModel.PropertyChanged += RendererViewModel_PropertyChanged;
            SceneNodeViewModel.PropertyChanged += SceneNodeViewModel_PropertyChanged;
            ViewportViewModel.PropertyChanged += ViewportViewModel_PropertyChanged;

            AnchorablesSources = new ObservableCollection<ViewModelBase>();
            DocumentsSources = new ObservableCollection<ViewModelBase>();
            AnchorablesSources.Add(SceneNodeViewModel);
            AnchorablesSources.Add(RendererViewModel);
            DocumentsSources.Add(ViewportViewModel);
            this.workspace = workspace;
        }

        private void ViewportViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Loaded")
            {
                workspace.InitializeRenderer(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
                workspace.InitializeScene();
                RendererViewModel.Model = workspace.Renderer;
            }
            else if (e.PropertyName == "Resize")
            {
                workspace.MainScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
                workspace.Renderer.SizeChanged(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
            }
            else if (e.PropertyName == "Renderer")
            {
                workspace.Renderer.Render();
            }
        }

        private void SceneNodeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SceneNodeViewModel.ActiveNode))
            {
                UpdateSelectNode(SceneNodeViewModel.ActiveNode.Model);
            }

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
            if (node.KIObject is RenderObject)
            {
                vm = new RenderObjectViewModel(this, node.KIObject as RenderObject);
                vm.PropertyChanged += RenderObjectViewModel_PropertyChanged;
                workspace.MainScene.SelectNode = (SceneNode)node.KIObject;
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
