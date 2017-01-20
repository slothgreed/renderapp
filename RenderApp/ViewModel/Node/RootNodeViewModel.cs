using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using RenderApp.AssetModel;
using System.Diagnostics;
using System.IO;
using RenderApp.Utility;
using RenderApp.ViewModel.DockTabVM;
namespace RenderApp.ViewModel.NodeVM
{
   
    public partial class RootNodeViewModel : TabItemViewModel
    {

        public ObservableCollection<NodeItemViewModel> RootNode
        {
            get;
            private set;
        }
        private NodeItemViewModel _activeNode;
        public NodeItemViewModel ActiveNode
        {
            get
            {
                return _activeNode;
            }
            private set
            {
                _activeNode = value;
            }
        }

        public RootNodeViewModel(RANode rootNode, string title)
        {
            Title = title;
            Initialize(new List<RANode>() { rootNode });
            
        }
        public RootNodeViewModel(List<RANode> rootNodes,string title)
        {
            Title = title;
            Initialize(rootNodes);
            
        }
        public void Initialize(List<RANode> rootNodes)
        {
            if (rootNodes != null)
            {
                RootNode = new ObservableCollection<NodeItemViewModel>();
                foreach (var root in rootNodes)
                {
                    var rootVM = new NodeItemViewModel(root, null);
                    RootNode.Add(rootVM);
                    InitAddNode(root, rootVM);
                }
            }
        }

        /// <summary>
        /// 再帰関数
        /// </summary>
        /// <param name="node"></param>
        private void InitAddNode(RANode parent,NodeItemViewModel parentVM)
        {
            foreach (var node in parent.Children)
            {
                var nodeVM = new NodeItemViewModel(node, parentVM);
                parentVM.Children.Add(nodeVM);
                InitAddNode(node, nodeVM);
            }
        }

        public void SelectionChangedCommand(object sender, EventArgs e)
        {
            var nodeList = sender as MultiSelectTreeView;
            if(nodeList.SelectedItems.Count > 0)
            {
                ActiveNode = nodeList.SelectedItems[0] as NodeItemViewModel;
                MainWindowViewModel.Instance.UpdateSelectNode(ActiveNode.Model);
            }
        }
        public void DeleteCommand()
        {
            var parent = ActiveNode.Parent;
            parent.Model.RemoveChild(ActiveNode.Model);
            parent.Children.Remove(ActiveNode);

        }
        public void OpenExplolerCommand()
        {
            if(ActiveNode == null)
            {
                return;
            }
            if (ActiveNode.Model is RANode)
            {
                var node = ActiveNode.Model as RANode;
                if (node.RAObject is RAFile)
                {
                    var asset = node.RAObject as RAFile;
                    if (File.Exists(asset.FilePath))
                    {
                        Process.Start("EXPLORER.exe", @"/select," + asset.FilePath);
                    }
                }
            }
        }
        public override void UpdateProperty()
        {

        }

    }
}
