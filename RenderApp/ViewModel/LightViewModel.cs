using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Renderer;
using KI.UI.ViewModel;
using OpenTK;

namespace RenderApp.ViewModel
{
    /// <summary>
    /// ライトのビューモデル
    /// </summary>
    public class LightViewModel : DockWindowViewModel
    {
        public LightViewModel(ViewModelBase parent, Light light)
            : base(parent, "LightProperty", Place.RightUp)
        {
            Model = light;
        }

        public Vector3 Rotate
        {
            get
            {
                return Model.Rotate;
            }

            set
            {
                Model.Rotate = value;
                OnPropertyChanged(nameof(Rotate));
            }
        }

        public Vector3 Scale
        {
            get
            {
                return Model.Scale;
            }

            set
            {
                Model.Scale = value;
                OnPropertyChanged(nameof(Scale));
            }
        }

        public Vector3 Translate
        {
            get
            {
                return Model.Translate;
            }

            set
            {
                Model.Translate = value;
                OnPropertyChanged(nameof(Translate));
            }
        }


        private Light model;

        public Light Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
            }
        }
    }
}
