using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.ViewModel;

namespace RenderApp.ViewModel
{
    class DefaultViewModel : ViewModelBase
    {

        public string Name
        {
            get;
            set;
        }
        private object _value;
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                SetValue<object>(ref _value, value);
            }
        }
        public DefaultViewModel(string name,string value)
        {
            Name = name;
            Value = value;
            
        }
        public override void UpdateProperty()
        {

        }

    }
}
