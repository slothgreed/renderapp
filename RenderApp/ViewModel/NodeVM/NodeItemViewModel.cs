using System.Collections.ObjectModel;
using System.Linq;
using KI.Foundation.Tree;
using KI.Foundation.Utility;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public class NodeItemViewModel : ViewModelBase
    {
        private ObservableCollection<NodeItemViewModel> children;

        private bool hoverHighlighting = true;

        private int itemIndent = 0;

        private bool isKeyboardMode = true;

        private bool isSelected;

        private string displayName;

        public EAssetType AssetType
        {
            get;
            private set;
        }

        public KINode Model
        {
            get;
            private set;
        }

        public static NodeItemViewModel ActiveItem
        {
            get;
            set;
        }

        public ObservableCollection<NodeItemViewModel> Children
        {
            get
            {
                return children;
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

        public bool HoverHighlighting
        {
            get
            {
                return hoverHighlighting;
            }

            set
            {
                SetValue<bool>(ref hoverHighlighting, value);
            }
        }

        public int ItemIndent
        {
            get
            {
                return itemIndent;
            }

            set
            {
                SetValue<int>(ref itemIndent, value);
            }
        }

        public bool IsKeyboardMode
        {
            get
            {
                return isKeyboardMode;
            }

            set
            {
                SetValue<bool>(ref isKeyboardMode, value);
            }
        }

        public string DisplayName
        {
            get
            {
                return displayName;
            }

            set
            {
                SetValue<string>(ref displayName, value);
                if (Model != null)
                {
                    Model.KIObject.Name = displayName;
                }
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    SetValue<bool>(ref isSelected, value);

                    if (isSelected == true)
                    {
                        ActiveItem = this;
                        Logger.Log(Logger.LogLevel.Debug, this.DisplayName);
                    }
                }
            }
        }

        public NodeItemViewModel(ViewModelBase parent)
            :base(parent)
        {
            children = new ObservableCollection<NodeItemViewModel>();
        }

        public NodeItemViewModel(ViewModelBase parent, KINode node)
            : this(parent)
        {
            node.NodeInserted += InsertNodeEvent;
            node.NodeRemoved += RemoveNodeEvent;
            DisplayName = node.Name;
            Model = node;
        }

        private void RemoveNodeEvent(object sender, NotifyNodeChangedEventArgs e)
        {
            if (sender is KINode)
            {
                NodeItemViewModel nodeVM = Children.Where(p => p.DisplayName == e.NewItems.Name).FirstOrDefault();
                if (nodeVM != null)
                {
                    Children.Remove(nodeVM);
                }
            }
        }

        private void InsertNodeEvent(object sender, NotifyNodeChangedEventArgs e)
        {
            if (sender is KINode)
            {
                KINode node = sender as KINode;
                if (Children.Count > e.NewIndex)
                {
                    Children.Insert(e.NewIndex, new NodeItemViewModel(this, node));
                }
                else
                {
                    Children.Add(new NodeItemViewModel(this, node));
                }
            }
        }

        public override void UpdateProperty()
        {
        }
    }
}
