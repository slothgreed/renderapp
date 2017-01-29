using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp;
using System.Collections.ObjectModel;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using RenderApp.Utility;
using KI.Foundation.ViewModel;
using KI.Foundation.Utility;

namespace RenderApp.ViewModel.NodeVM
{
    public class NodeItemViewModel : ViewModelBase
    {
        public EAssetType AssetType
        {
            get;
            private set;
        }
        public RANode Model
        {
            get;
            private set;
        }
        public static NodeItemViewModel ActiveItem
        {
            get;
            set;
        }
        private ObservableCollection<NodeItemViewModel> _children;
        public ObservableCollection<NodeItemViewModel> Children
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
        public NodeItemViewModel Parent
        {
            get;
            set;
        }

        private bool _hoverHighlighting = true;
        public bool HoverHighlighting
        {
            get
            {
                return _hoverHighlighting;
            }
            set
            {
                SetValue<bool>(ref _hoverHighlighting,value);
            }
        }
        private int _itemIndent = 0;
        public int ItemIndent
        {
            get
            {
                return _itemIndent;
            }
            set
            {
                SetValue<int>(ref _itemIndent,value);
            }
        }
        private bool _isKeyboardMode = true;
        public bool IsKeyboardMode
        {
            get
            {
                return _isKeyboardMode;
            }
            set
            {
                SetValue<bool>(ref _isKeyboardMode,value);
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
                    Model.RAObject.Name = _displayName;
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
                        Logger.Log(Logger.LogLevel.Debug, this.DisplayName);
                    }
                }
            }
        }
        public NodeItemViewModel()
        {
            _children = new ObservableCollection<NodeItemViewModel>();
        }

        public NodeItemViewModel(RANode node,NodeItemViewModel parent)
            : this()
        {
            if (parent != null)
            {
                Parent = parent;
            }
            node.InsertNodeEvent += InsertNodeEvent;
            node.RemoveNodeEvent += RemoveNodeEvent;
            DisplayName = node.Name;
            Model = node;
            
        }

        private void RemoveNodeEvent(object sender, NotifyNodeChangedEventArgs e)
        {
            if (sender is RANode)
            {
                RANode node = sender as RANode;
                NodeItemViewModel nodeVM = Children.Where(p => p.DisplayName == node.Name).FirstOrDefault();
                if(nodeVM != null)
                {
                    Children.Remove(nodeVM);
                }
            }
        }
        private void InsertNodeEvent(object sender, NotifyNodeChangedEventArgs e)
        {
            if (sender is RANode)
            {
                RANode node = sender as RANode;
                if(Children.Count > e.NewIndex)
                {
                    Children.Insert(e.NewIndex, new NodeItemViewModel(node, this));
                }
                else
                {
                    Children.Add(new NodeItemViewModel(node, this));
                }
            }
        }

        public override void UpdateProperty()
        {

        }
    }
}
