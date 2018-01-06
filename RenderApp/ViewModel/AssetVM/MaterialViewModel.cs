using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Renderer;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public class MaterialViewModel : DockWindowViewModel
    {
        private MaterialBase Model;

        public MaterialViewModel(ViewModelBase parent)
            : base(parent, "No Geometry", Place.RightUp)
        {
        }

        public MaterialViewModel(ViewModelBase parent, MaterialBase model)
            : base(parent, "No Geometry", Place.RightUp)
        {
            Model = model;
        }
    }
}
