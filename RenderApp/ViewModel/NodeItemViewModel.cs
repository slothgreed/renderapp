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
namespace RenderApp.ViewModel
{
    public class NodeItemViewModel : ViewModelBase
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
        private NodeItemViewModel Parent
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
        public NodeItemViewModel()
        {
            _children = new ObservableCollection<NodeItemViewModel>();
        }

        public NodeItemViewModel(Node node,NodeItemViewModel parent)
            : this()
        {
            if (parent != null)
            {
                Parent = parent;
                Parent.Children.Add(this);
            }
            node.InsertNodeEvent += InsertNodeEvent;
            node.RemoveNodeEvent += RemoveNodeEvent;
            DisplayName = node.Name;
            
        }
        private void InsertNodeEvent(object sender, NotifyNodeChangedEventArgs e)
        {
            if (sender is Node)
            {
                Node node = sender as Node;
                if(Children.Count < e.NewIndex)
                {
                    Children.Insert(e.NewIndex, new NodeItemViewModel(node, this));
                }
                else
                {
                    Children.Add(new NodeItemViewModel(node, this));
                }
            }
        }
        private void RemoveNodeEvent(object sender, NotifyNodeChangedEventArgs e)
        {
            if (sender is Node)
            {
                Node node = sender as Node;
                node.InsertNodeEvent -= InsertNodeEvent;
                node.RemoveNodeEvent -= RemoveNodeEvent;
                if(Parent != null)
                {
                    Parent.Children.Remove(this);
                }
            }
        }


        public override void UpdateProperty()
        {

        }
    }
}
