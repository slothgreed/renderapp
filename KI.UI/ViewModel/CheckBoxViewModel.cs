using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.UI.ViewModel
{
    public class CheckBoxViewModel : PropertyViewModelBase
    {
        Func<bool, bool> updateFunc;
        private bool Model;
        public CheckBoxViewModel(object owner, string name, bool value)
        {
            Owner = owner;
            Name = name;
            Model = value;
            updateFunc = new Func<bool, bool>(UpdateProperty);
        }

        public bool Check
        {
            get { return Model; }
            set
            {
                SetValue<bool>(updateFunc, value);
            }
        }

        public bool UpdateProperty(bool value)
        {
            Model = value;
            if (Owner == null)
                return false;

            var property = Owner.GetType().GetProperty(Name);

            if (property == null)
                return false;

            property.SetValue(Owner, value);

            return true;
        }
    }
}
