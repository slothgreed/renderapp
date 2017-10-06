using System.Collections.Generic;
using KI.Gfx;
using KI.Gfx.KIShader;
using KI.Gfx.KITexture;
using OpenTK;

namespace RenderApp.ViewModel
{
    class ShaderViewModel : TabItemViewModel
    {
        public ShaderViewModel(Shader shader)
        {
        }

        public override string Title
        {
            get
            {
                return "Shader";
            }
        }

        public Dictionary<string, object> PropertyItem
        {
            get;
            set;
        }

        public override void UpdateProperty()
        {
        }
    }
}
