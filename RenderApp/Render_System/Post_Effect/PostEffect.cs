using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil.ShaderModel;
using System.Runtime.CompilerServices;
namespace RenderApp.Render_System.Post_Effect
{
    abstract class PostEffect
    {
        public Shader PostShader
        {
            get;
            private set;
        }
        public PostEffect(Shader shader)
        {
            PostShader = shader;
        }

        protected void SetValue<T>(ref T member, T value, [CallerMemberName]string memberName = "")
        {
            if (PostShader.SetValue(memberName, value))
            {
                member = value;
            }
            else
            {
                Utility.Output.Error("Set Shader Error " + memberName);
            }
         
        }
        public abstract void Initialize();
    }
}
