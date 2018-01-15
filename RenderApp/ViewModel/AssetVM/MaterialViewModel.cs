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

        public MaterialViewModel(ViewModelBase parent, MaterialBase model)
            : base(parent, model, model?.Name, Place.RightUp)
        {
            Model = model;
        }
    }
}
