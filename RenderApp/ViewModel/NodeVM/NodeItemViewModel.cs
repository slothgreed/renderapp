using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using KI.Foundation.Core;
using KI.Foundation.Tree;
using KI.UI.ViewModel;
using RenderApp.Model;

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
            : base(parent)
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

        private void RemoveNodeEvent(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is KINode)
            {
                // 前まではこちらでいけてた。（何故）
                // KINode node = e.NewItems[0] as KINode;
                KINode node = e.OldItems[0] as KINode;
                NodeItemViewModel nodeVM = Children.Where(p => p.DisplayName == node.Name).FirstOrDefault();
                if (nodeVM != null)
                {
                    Children.Remove(nodeVM);
                }
            }
        }

        private void InsertNodeEvent(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is KINode)
            {
                KINode node = sender as KINode;
                if (Children.Count > e.NewStartingIndex)
                {
                    Children.Insert(e.NewStartingIndex, new NodeItemViewModel(this, node));
                }
                else
                {
                    Children.Add(new NodeItemViewModel(this, node));
                }
            }
        }
    }
}
