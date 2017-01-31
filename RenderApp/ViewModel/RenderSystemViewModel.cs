using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.Render_System;
using System.Collections.ObjectModel;

namespace RenderApp.ViewModel
{
    public class RenderSystemViewModel : TabItemViewModel
    {
        public RenderSystem Model
        {
            get;
            set;
        }
        private int _textureIndex;
        public int TextureIndex
        {
            get
            {
                return _textureIndex;
            }
            private set
            {
                SetValue<int>(ref _textureIndex, value);
                Model.OutputTexture = Model.ProcessingTexture[value];
            }
        }
        public RenderSystemViewModel(RenderSystem _model)
        {
            Model = _model;
        }

        public override string Title
        {
            get
            {
                return "RenderSystem";
            }
        }



        public override void UpdateProperty()
        {
        }
    }
}
