using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;
using KI.Foundation.Core;
using KI.Foundation.Tree;

namespace RenderApp.ViewModel
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

        public RootNodeViewModel(KINode rootNode, string title)
        {
            Title = title;
            Initialize(new List<KINode>() { rootNode });

        }
        public RootNodeViewModel(List<KINode> rootNodes, string title)
        {
            Title = title;
            Initialize(rootNodes);

        }
        public void Initialize(List<KINode> rootNodes)
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
        private void InitAddNode(KINode parent, NodeItemViewModel parentVM)
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
            if (nodeList.SelectedItems.Count > 0)
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
            if (ActiveNode == null)
            {
                return;
            }
            if (ActiveNode.Model is KINode)
            {
                var node = ActiveNode.Model as KINode;
                if (node.KIObject is KIFile)
                {
                    var asset = node.KIObject as KIFile;
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
