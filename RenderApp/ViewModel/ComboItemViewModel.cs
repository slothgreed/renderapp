﻿using System;
using System.Collections.Generic;
using KI.Foundation.Core;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    class ComboItemViewModel : ViewModelBase
    {
        private Action<int> action;
        private object Owner;
        private List<object> items;
        private int selectIndex = 0;

        public ComboItemViewModel(object owner, string name, IEnumerable<object> value, int selectedIndex = 0)
        {
            Owner = owner;
            Name = name;
            items = new List<object>();
            if (value != null)
            {
                foreach (var item in value)
                {
                    Items.Add(item);
                }
            }

            SelectedIndex = selectedIndex;
            action = new Action<int>(UpdateProperty);
        }

        public string Name
        {
            get;
            set;
        }

        public List<object> Items
        {
            get
            {
                return items;
            }

            set
            {
                SetValue(ref items, value);
            }
        }

        public int SelectedIndex
        {
            get
            {
                return selectIndex;
            }

            set
            {
                SetValue(action, value);
            }
        }

        public void UpdateProperty(int selectedIndex)
        {
            if (Owner == null)
                return;

            var property = Owner.GetType().GetProperty(Name);
            if (property == null)
                return;

            selectIndex = selectedIndex;
            property.SetValue(Owner, Items[SelectedIndex]);

            return;
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
