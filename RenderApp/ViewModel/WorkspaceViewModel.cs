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
        public WorkspaceViewModel(ViewModelBase parent)
            : base(parent)
        {
            LeftUpDockPanel = new TabControlViewModel(this);
            LeftDownDockPanel = new TabControlViewModel(this);
            RightUpDockPanel = new TabControlViewModel(this);
            RightDownDockPanel = new TabControlViewModel(this);
            CenterDockPanel = new TabControlViewModel(this);

            LeftUpDockPanel.Add(new RootNodeViewModel(LeftUpDockPanel, Project.ActiveProject.RootNode, "Project"));
            CenterDockPanel.Add(new ViewportViewModel(CenterDockPanel));
            RightDownDockPanel = new TabControlViewModel(RightDownDockPanel);
            RightDownDockPanel.Add(new VoxelViewModel(RightDownDockPanel));

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
                vm = new RenderObjectViewModel(LeftDownDockPanel, node.KIObject as RenderObject);
                Workspace.MainScene.SelectNode = (SceneNode)node.KIObject;
            }

            ReplaceTabWindow(vm);
        }

        public void ReplaceTabWindow(TabItemViewModel window)
        {
            if (window is RenderObjectViewModel)
            {
                var oldItem = LeftDownDockPanel.FindVM<RenderObjectViewModel>();
                LeftDownDockPanel.ReplaceVM(oldItem, window);
            }
        }

        public void OpenWindow(AppWindow windowType)
        {

        }
    }
}
