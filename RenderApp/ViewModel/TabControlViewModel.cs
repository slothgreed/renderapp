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
        private ObservableCollection<TabItemViewModel> _ItemsSource = new ObservableCollection<TabItemViewModel>();
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
        public void Replace(TabItemViewModel oldItem,TabItemViewModel newItem)
        {
            Remove(oldItem);
            Add(newItem);
        }
        public void ReplaceVM(TabItemViewModel newItem)
        {
            var vm = ItemsSource.Where(p => p is ShaderProgramViewModel).FirstOrDefault();
            if(vm != null)
            {
                Remove(vm);
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
