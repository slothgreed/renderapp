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
        public enum RenderType
        {
            Original,
            OffScreen
        }
        protected RenderType renderType
        {
            get;
            set;
        }
        public Shader Shader
        {
            get;
            set;
        }

        protected Geometry Plane
        {
            get;
            set;
        }

        public RenderTarget RenderTarget
        {
            get;
            set;
        }

        public RenderTechnique(string name, RenderType type)
            :base(name)
        {
            renderType = type;
            Init();
        }
        public RenderTechnique(string name, string vertexShader, string fragShader, RenderType type)
            :base(name)
        {
            renderType = type; 
            Init(vertexShader, fragShader);
        }

        /// <summary>
        /// シェーダへ値のセット
        /// </summary>
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

        public void ClearBuffer()
        {
            RenderTarget.ClearBuffer();
        }

        public virtual void SizeChanged(int width, int height)
        {
            RenderTarget.SizeChanged(width, height);
        }
        public virtual void Render()
        {
            if (renderType == RenderType.Original)
            {
                Logger.Log(Logger.LogLevel.Error, "RenderTechnique : Not Defined Original Render");
            }
            if(renderType == RenderType.OffScreen)
            {
                if(Plane != null)
                {
                    RenderTarget.BindRenderTarget();
                    Plane.Render();
                    RenderTarget.UnBindRenderTarget();
                }
            }
        }

        #region [initalize event]
        public abstract void Initialize();

        public virtual void CreateShader(string vertexShader, string fragShader)
        {
            Shader = ShaderFactory.Instance.CreateShaderVF(vertexShader, fragShader);
        }
        public virtual void CreateRenderTarget(int width, int height)
        {
            Texture texture = TextureFactory.Instance.CreateTexture("Texture:" + Name, width, height);
            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget("RenderTarget:" + Name, width, height, texture);
        }
        private void Init(string vertexShader = null, string fragShader = null)
        {
            if (vertexShader != null && fragShader != null)
            {
                CreateShader(vertexShader, fragShader);
            }
            Plane = AssetFactory.Instance.CreatePostProcessPlane(Name);
            CreateRenderTarget(KI.Gfx.GLUtil.DeviceContext.Instance.Width, KI.Gfx.GLUtil.DeviceContext.Instance.Height);
            Initialize();
        }
        #endregion
    }
}
