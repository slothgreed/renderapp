﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.ViewModel
{
    class ComboItemViewModel : ViewModelBase
    {
        Action<int> action;
        private object Owner;
        public string Name
        {
            get;
            set;
        }
        private List<RAObject> _items;
        public List<RAObject> Items
        {
            get
            {
                return _items;
            }
            set
            {
                SetValue(ref _items, value);
            }
        }
        private int _selectIndex = 0;
        public int SelectedIndex
        {
            get
            {
                return _selectIndex;
            }
            set
            {
                SetValue(action, value);
            }
        }
        //public ComboItemViewModel(object owner, string name, List<string> value, int selectedIndex = 0)
        //{
        //    action = new Action<int>(UpdateProperty);
        //    Name = name;
        //    SelectedIndex = selectedIndex;
        //}

        public ComboItemViewModel(object owner, string name, IEnumerable<RAObject> value, int selectedIndex = 0)
        {
            Owner = owner;
            Name = name;
            _items = new List<RAObject>();
            foreach(var item in value)
            {
                Items.Add(item);
            }
            SelectedIndex = selectedIndex;
            action = new Action<int>(UpdateProperty);
        }
        public void UpdateProperty(int selectedIndex)
        {
            if (Owner == null)
                return;

            var property = Owner.GetType().GetProperty(Name);
            if (property == null)
                return;

            _selectIndex = selectedIndex;
            property.SetValue(Owner, Items[SelectedIndex]);

            return;
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
