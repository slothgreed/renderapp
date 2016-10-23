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
namespace RenderApp.ViewModel
{
   
    public partial class RootNodeViewModel : DockWindowViewModel
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
                MainWindowViewModel.Instance.UpdateSelectNode(ActiveNode);
            }
        }
        public void DeleteCommand()
        {
            bool exist = false;
            foreach(var root in RootNode)
            {
                foreach (var folder in root.Children)
                {
                    if (folder.Children.Contains(ActiveNode))
                    {
                        folder.Children.Remove(ActiveNode);
                        exist = true;
                    }
                }
                if (exist)
                {
                    Scene.ActiveScene.RootNode.RemoveRecursiveChild(ActiveNode.Model.Key);
                    break;
                }

            }

        }
        public void OpenExplolerCommand()
        {
            if(ActiveNode == null)
            {
                return;
            }
            if (ActiveNode.Model is Asset)
            {
                var asset = ActiveNode.Model as Asset;
                if (File.Exists(asset.FilePath))
                {
                    Process.Start("EXPLORER.exe",@"/select," + asset.FilePath);
                }
            }
        }
        public override void UpdateProperty()
        {

        }

    }
}
