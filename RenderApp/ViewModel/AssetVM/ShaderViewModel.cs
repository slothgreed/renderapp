using System.Collections.Generic;
using KI.Gfx;
using KI.Gfx.KIShader;
using KI.Gfx.KITexture;
using OpenTK;

namespace RenderApp.ViewModel
{
    class ShaderViewModel : TabItemViewModel, IPropertyGridViewModel
    {
        public ShaderViewModel(Shader shader)
        {
            PropertyItem = new Dictionary<string, object>();
            foreach (ShaderProgramInfo loop in shader.GetShaderVariable())
            {
                if (loop.Variable is Vector2)
                {
                    PropertyItem.Add(loop.Name, (Vector2)loop.Variable);
                }

                if (loop.Variable is Vector3)
                {
                    PropertyItem.Add(loop.Name, (Vector3)loop.Variable);
                }

                if (loop.Variable is Vector4)
                {
                    PropertyItem.Add(loop.Name, (Vector4)loop.Variable);
                }

                if (loop.Variable is Matrix3)
                {
                    PropertyItem.Add(loop.Name, (Matrix3)loop.Variable);
                }

                if (loop.Variable is Matrix4)
                {
                    PropertyItem.Add(loop.Name, (Matrix4)loop.Variable);
                }

                if (loop.Variable is Texture)
                {
                    PropertyItem.Add(loop.Name, (Texture)loop.Variable);
                }
            }
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
