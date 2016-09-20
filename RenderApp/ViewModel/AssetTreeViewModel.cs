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
namespace RenderApp.ViewModel
{
   
    public partial class AssetTreeViewModel : AvalonWindowViewModel
    {
        
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetValue<string>(ref _title, value);
            }
        }
        public TreeItemViewModel Root
        {
            get;
            private set;
        }
        public TreeItemViewModel Textures
        {
            get;
            private set;
        }
        public TreeItemViewModel Materials
        {
            get;
            private set;
        }
        public TreeItemViewModel Geometry
        {
            get;
            private set;
        }
        public TreeItemViewModel Light
        {
            get;
            private set;
        }
        public TreeItemViewModel Camera
        {
            get;
            private set;
        }
        public TreeItemViewModel EnvProbe
        {
            get;
            private set;
        }
        private TreeItemViewModel _activeNode;
        public TreeItemViewModel ActiveNode
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
        public void AddAssetTree(TreeItemViewModel value)
        {

            switch (value.AssetType)
            {
                case EAssetType.Geometry:
                    Geometry.Children.Add(value);
                    break;
                case EAssetType.Light:
                    Light.Children.Add(value);
                    break;
                case EAssetType.Camera:
                    Camera.Children.Add(value);
                    break;
                case EAssetType.Materials:
                    Materials.Children.Add(value);
                    break;
                case EAssetType.Textures:
                    Textures.Children.Add(value);
                    break;
                case EAssetType.EnvProbe:
                    EnvProbe.Children.Add(value);
                    break;
            }
        }
        public AssetTreeViewModel(TreeItemViewModel parent,string title)
        {
            WindowPosition = AvalonWindow.LeftUp;
            Title = title;
            Geometry = new TreeItemViewModel("Models");
            Textures = new TreeItemViewModel("Textures");
            Materials = new TreeItemViewModel("Materials");
            Light = new TreeItemViewModel("Light");
            Camera = new TreeItemViewModel("Camera");
            EnvProbe = new TreeItemViewModel("EnvProbe");
            Root = new TreeItemViewModel("Root");

            Root.Children.Add(Geometry);
            Root.Children.Add(Camera);
            Root.Children.Add(Light);
            Root.Children.Add(Textures);
            Root.Children.Add(Materials);
            Root.Children.Add(EnvProbe);
        }

        public override void SizeChanged()
        {

        }

        public void SelectionChangedCommand(object sender, EventArgs e)
        {
            var nodeList = sender as MultiSelectTreeView;
            if(nodeList.SelectedItems.Count > 0)
            {
                ActiveNode = nodeList.SelectedItems[0] as TreeItemViewModel;
                MainWindowViewModel.Instance.UpdateSelectNode(ActiveNode);
            }
        }
        public void DeleteCommand()
        {
            bool exist = false;
            foreach(var folder in Root.Children)
            {
                if(folder.Children.Contains(ActiveNode))
                {
                    folder.Children.Remove(ActiveNode);
                    exist = true;
                }
            }
            if(exist)
            {
                Scene.ActiveScene.DeleteAsset(ActiveNode.Model.Key, ActiveNode.AssetType);
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
