using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.ViewModel
{
    class ComboItemViewModel : ViewModelBase
    {
        public string Name
        {
            get;
            set;
        }
        private List<string> _items;
        public List<string> Items
        {
            get
            {
                return _items;
            }
        }
        public int SelectIndex
        {
            get;
            set;
        }
        public ComboItemViewModel(string name, List<string> value,int selectedIndex = 0)
        {
            Name = name;
            _items = value;
            SelectIndex = selectedIndex;
        }
        public override void UpdateProperty()
        {

        }
    }
}
