using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KI.UI.ViewModel;

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
        private TabItemViewModel activeItem;

        private ObservableCollection<TabItemViewModel> itemsSource = new ObservableCollection<TabItemViewModel>();

        public TabItemViewModel ActiveItem
        {
            get
            {
                return activeItem;
            }

            set
            {
                SetValue(ref activeItem, value);
            }
        }

        public ObservableCollection<TabItemViewModel> ItemsSource
        {
            get
            {
                return itemsSource;
            }

            set
            {
                SetValue(ref itemsSource, value);
            }
        }

        public TabControlViewModel(ViewModelBase parent)
            : base(parent)
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
