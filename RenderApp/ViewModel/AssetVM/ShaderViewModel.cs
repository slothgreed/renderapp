using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using System.Collections.ObjectModel;
using RenderApp.GLUtil;
using OpenTK;
using RenderApp.GLUtil.ShaderModel;
namespace RenderApp.ViewModel
{
    class ShaderViewModel : TabItemViewModel,IPropertyGridViewModel
    {
        public override string Title
        {
            get
            {
                return "Shader";
            }
        }
        public Dictionary<string,object> PropertyItem
        {
            get;
            set;
        }
        public ShaderViewModel(Material material)
        {
            PropertyItem = new Dictionary<string, object>();
            foreach (ShaderProgramInfo loop in material.CurrentShader.GetShaderVariable())
            {
                if (loop.variable is Vector2)
                {
                    PropertyItem.Add(loop.Name, (Vector2)loop.variable);
                }
                if (loop.variable is Vector3)
                {
                    PropertyItem.Add(loop.Name, (Vector3)loop.variable);
                }
                if (loop.variable is Vector4)
                {
                    PropertyItem.Add(loop.Name, (Vector4)loop.variable);
                }
                if (loop.variable is Matrix3)
                {
                    PropertyItem.Add(loop.Name, (Matrix3)loop.variable);
                }
                if (loop.variable is Matrix4)
                {
                    PropertyItem.Add(loop.Name, (Matrix4)loop.variable);
                }
                if (loop.variable is Texture)
                {
                    PropertyItem.Add(loop.Name, (Texture)loop.variable);
                }
            }
        }
        public override void UpdateProperty()
        {

        }
    }
}
