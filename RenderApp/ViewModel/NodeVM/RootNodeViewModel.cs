using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.IO;
using KI.Foundation.Core;
using KI.Foundation.Tree;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public partial class RootNodeViewModel : DockWindowViewModel
    {
        private NodeItemViewModel activeNode;

        public RootNodeViewModel(ViewModelBase parent, KINode rootNode, string title)
            : base(parent, null, title, Place.LeftUp)
        {
            Initialize(parent, new List<KINode>() { rootNode });
        }

        public RootNodeViewModel(ViewModelBase parent, List<KINode> rootNodes, string title)
            : base(parent, null, title, Place.LeftUp)
        {
            Initialize(parent, rootNodes);
        }

        public ObservableCollection<NodeItemViewModel> RootNode
        {
            get;
            set;
        }

        public NodeItemViewModel ActiveNode
        {
            get
            {
                return activeNode;
            }

            private set
            {
                activeNode = value;
                OnPropertyChanged(nameof(ActiveNode));
            }
        }

        public void Initialize(ViewModelBase parent, List<KINode> rootNodes)
        {
            if (rootNodes != null)
            {
                RootNode = new ObservableCollection<NodeItemViewModel>();
                foreach (var root in rootNodes)
                {
                    var rootVM = new NodeItemViewModel(parent, root);
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
                var nodeVM = new NodeItemViewModel(parentVM, node);
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
            }
        }

        public void DeleteCommand()
        {
            var parent = ActiveNode.Parent as NodeItemViewModel;
            ActiveNode.Model.Dispose();
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
    }
}
