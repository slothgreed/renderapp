using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.ViewModel.MathVM
{
    public class NumericViewModel : MathViewModel
    {
        public float Model;
        public NumericViewModel(string name, float value)
        {
            this.Name = name;
            Model = value;
        }
        private float _value;
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
