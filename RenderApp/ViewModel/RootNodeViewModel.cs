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
   
    public partial class RootNodeViewModel : AvalonWindowViewModel
    {
        
        public NodeItemViewModel RootNode
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

        public RootNodeViewModel(Node rootNode, string title)
        {
            WindowPosition = AvalonWindow.LeftUp;
            Title = title;
            if (rootNode != null)
            {
                RootNode = new NodeItemViewModel(rootNode, null);
                InitAddNode(rootNode,RootNode);
            }
        }
        /// <summary>
        /// 再帰関数
        /// </summary>
        /// <param name="node"></param>
        private void InitAddNode(Node parent,NodeItemViewModel parentVM)
        {
            foreach (var node in parent.Children)
            {
                var nodeVM = new NodeItemViewModel(node, parentVM);
                InitAddNode(node, nodeVM);
            }
        }
        public override void SizeChanged()
        {

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
            foreach(var folder in RootNode.Children)
            {
                if(folder.Children.Contains(ActiveNode))
                {
                    folder.Children.Remove(ActiveNode);
                    exist = true;
                }
            }
            if(exist)
            {
                Scene.ActiveScene.RootNode.RemoveRecursiveChild(ActiveNode.Model.Key);
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
