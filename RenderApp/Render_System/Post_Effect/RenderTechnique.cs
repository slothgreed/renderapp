using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil.ShaderModel;
using System.Runtime.CompilerServices;
using KI.Foundation.Utility;
using RenderApp.GLUtil;
using KI.Gfx.KIAsset;
using RenderApp.AssetModel;
using KI.Gfx.Render;
using KI.Foundation.Core;
namespace RenderApp.Render_System
{
    public abstract class RenderTechnique : KIObject
    {
        public Shader Shader
        {
            get;
            protected set;
        }
        public Material Material
        {
            get;
            protected set;
        }

        public RenderTarget RenderTarget
        {
            get;
            protected set;
        }

        public RenderTechnique(string name)
            :base(name)
        {
            Init();
        }
        public RenderTechnique(string name, string vertexShader, string fragShader)
            :base(name)
        {
            Init(vertexShader, fragShader);
        }

        public RenderTechnique(string name, Material material, string vertexShader, string fragShader)
            :base(name)
        {
            Init(vertexShader,fragShader,material); 
        }

        protected void SetValue<T>(ref T member, T value, [CallerMemberName]string memberName = "")
        {
            if (Shader.SetValue(memberName, value))
            {
                member = value;
            }
            else
            {
                Logger.Log(Logger.LogLevel.Error, "Set Shader Error " + memberName);
            }
        }
        private void Init(string vertexShader = null,string fragShader = null,Material materail = null)
        {
            if(vertexShader != null && fragShader != null)
            {
                CreateShader(vertexShader, fragShader);
            }
            CreateRenderTarget(KI.Gfx.GLUtil.DeviceContext.Instance.Width, KI.Gfx.GLUtil.DeviceContext.Instance.Height);
            CreateMaterial();
            Material.CurrentShader = Shader;
            Initialize();
        }
        public void ClearBuffer()
        {
            RenderTarget.ClearBuffer();
        }

        public virtual void SizeChanged(int width, int height)
        {
            RenderTarget.SizeChanged(width, height);
        }

        public abstract void Initialize();

        public virtual void CreateShader(string vertexShader, string fragShader)
        {
           Shader = ShaderFactory.Instance.CreateShaderVF(vertexShader, fragShader);
        }
        public virtual void CreateRenderTarget(int width,int height)
        {
            Texture texture = TextureFactory.Instance.CreateTexture("Texture:" + Name, width, height);
            RenderTarget = new RenderTarget("RenderTarget:" + Name, width, height, texture);
        }
        public virtual void CreateMaterial()
        {
            Material = AssetFactory.Instance.CreateMaterial("Material : " + Name);
        }
    }
}
