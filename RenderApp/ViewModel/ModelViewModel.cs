using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.ViewModel
{
    public class ModelViewModel : AvalonWindowViewModel
    {
        private string _title;
        public override string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetValue<string>(ref _title, value);
            }
        }
        public ModelViewModel()
        {
            Title = "Material";
        }


        public override void SizeChanged()
        {
        }
        public override void UpdateProperty()
        {

        }
    }
}
