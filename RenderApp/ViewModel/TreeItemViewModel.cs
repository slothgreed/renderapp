using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp;
using System.Collections.ObjectModel;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
namespace RenderApp.ViewModel
{
    public class TreeItemViewModel : ViewModelBase
    {
        public EAssetType AssetType
        {
            get;
            private set;
        }
        public Asset Model
        {
            get;
            private set;
        }
        public static TreeItemViewModel ActiveItem
        {
            get;
            set;
        }
        private ObservableCollection<TreeItemViewModel> _children;
        public ObservableCollection<TreeItemViewModel> Children
        {
            get
            {
                return _children;
            }
        }
        public bool IsExpanded
        {
            get
            {
                if (Children != null)
                {
                    if (Children.Count != 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            
        }
        private string _displayName;
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                SetValue<string>(ref _displayName, value);
                if(Model != null)
                {
                    Model.Key = _displayName;
                }
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    SetValue<bool>(ref _isSelected, value);

                    if(_isSelected == true)
                    {
                        ActiveItem = this;
                        Console.WriteLine(this.DisplayName);
                    }
                }
            }
        }
        public TreeItemViewModel()
        {
            _children = new ObservableCollection<TreeItemViewModel>();
        }
        public TreeItemViewModel(string displayName)
            : this()
        {
            DisplayName = displayName;
            AssetType = EAssetType.Unknown;

        }
        public TreeItemViewModel(Asset asset,EAssetType type)
            : this()
        {
            DisplayName = asset.ToString();
            AssetType = type;
            Model = asset;
        }
        public override void UpdateProperty()
        {

        }
    }
}
