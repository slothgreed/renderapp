using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Foundation.ViewModel
{
    public class NumericViewModel : PropertyViewModelBase
    {
        public float Model;
        public NumericViewModel(string name, float value)
        {
            this.Name = name;
            Model = value;
        }

        public float Value
        {
            get { return Model; }
            set { SetValue<float>(ref Model, value); }
        }

        public override void UpdateProperty()
        {

        }
    }
}
