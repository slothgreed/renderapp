using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    class DefaultViewModel : ViewModelBase
    {
        private object value;

        public string Name { get; set; }

        public object Value
        {
            get
            {
                return value;
            }

            set
            {
                SetValue(ref this.value, value);
            }
        }

        public DefaultViewModel(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override void UpdateProperty()
        {
        }
    }
}
