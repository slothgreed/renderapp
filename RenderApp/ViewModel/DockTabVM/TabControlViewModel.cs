using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RenderApp.ViewModel
{
    public enum DockPosition
    {
        LeftUp,
        LeftDown,
        RightUp,
        RightDown,
        Center,
    }

    public class TabControlViewModel : DockWindowViewModel
    {
        private TabItemViewModel _activeItem;

        private ObservableCollection<TabItemViewModel> _ItemsSource = new ObservableCollection<TabItemViewModel>();

        public TabItemViewModel ActiveItem
        {
            get
            {
                return _activeItem;
            }

            set
            {
                SetValue(ref _activeItem, value);
            }
        }

        public ObservableCollection<TabItemViewModel> ItemsSource
        {
            get
            {
                return _ItemsSource;
            }

            set
            {
                SetValue(ref _ItemsSource, value);
            }
        }

        public TabControlViewModel()
        {
        }

        public void Add(TabItemViewModel addItem)
        {
            addItem.Parent = this;
            ItemsSource.Add(addItem);
            ActiveItem = addItem;
        }

        public void Replace(TabItemViewModel oldItem, TabItemViewModel newItem)
        {
            Remove(oldItem);
            Add(newItem);
        }

        public TabItemViewModel FindVM<T>() where T : TabItemViewModel
        {
            return ItemsSource.Where(p => p is T).FirstOrDefault();
        }

        public void ReplaceVM(TabItemViewModel oldItem, TabItemViewModel newItem)
        {
            if (oldItem != null)
            {
                Remove(oldItem);
                Add(newItem);
            }
            else
            {
                Add(newItem);
            }
        }

        public void Remove(TabItemViewModel removeItem)
        {
            bool delActive = false;
            if (removeItem == ActiveItem)
            {
                delActive = true;
            }

            ItemsSource.Remove(removeItem);
            if (delActive)
            {
                ActiveItem = ItemsSource.FirstOrDefault();
            }
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
