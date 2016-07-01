using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.ViewModel
{
    class DefaultViewModel : ViewModelBase
    {
        public string Name
        {
            get;
            set;
        }
        public object Value
        {
            get;
            set;
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
