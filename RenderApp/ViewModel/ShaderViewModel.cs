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
    class ShaderViewModel : AvalonWindowViewModel
    {
        public override string Title
        {
            get
            {
                return "Shader";
            }
        }
        public Dictionary<string,object> Items
        {
            get;
            private set;
        }
        public PropertyGridViewModel Property
        {
            get;
            private set;
        }
        public ShaderViewModel(Material material)
        {
            WindowPosition = AvalonWindow.RightDown;

            Items = new Dictionary<string,object>();
            foreach (ShaderProgramInfo loop in material.CurrentShader.GetShaderVariable())
            {
                if (loop.variable is Vector2)
                {
                    Items.Add(loop.Name, (Vector2)loop.variable);
                }
                if (loop.variable is Vector3)
                {
                    Items.Add(loop.Name, (Vector3)loop.variable);
                }
                if (loop.variable is Vector4)
                {
                    Items.Add(loop.Name, (Vector4)loop.variable);
                }
                if (loop.variable is Matrix3)
                {
                    Items.Add(loop.Name, (Matrix3)loop.variable);
                }
                if (loop.variable is Matrix4)
                {
                    Items.Add(loop.Name, (Matrix4)loop.variable);
                }
                if (loop.variable is Texture)
                {
                    Items.Add(loop.Name, (Texture)loop.variable);
                }
            }
            Property = new PropertyGridViewModel(Items);
        }
        public override void SizeChanged()
        {

        }
        public override void UpdateProperty()
        {

        }
    }
}
