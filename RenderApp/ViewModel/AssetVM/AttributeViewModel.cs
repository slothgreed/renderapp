using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Renderer;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public class AttributeViewModel : DockWindowViewModel
    {
        private AttributeBase Model;

        public AttributeViewModel(ViewModelBase parent, AttributeBase model)
            : base(parent, model, model?.Name, Place.RightUp)
        {
            Model = model;
        }

        public string Name
        {
            get
            {
                return Model.Name;
            }
        }
    }
}
