using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KI.Foundation.Utility;
using KI.Gfx.Render;
using KI.Foundation.Core;
using KI.Gfx.KIShader;
using KI.Gfx.KITexture;
using KI.Asset;

namespace KI.Renderer
{
    public abstract class RenderTechnique : KIObject
    {
        public RenderTechnique(string name, RenderTechniqueType tech, RenderType type)
            : base(name)
        {
            renderType = type;
            Technique = tech;
            Init();
        }

        public RenderTechnique(string name, string vertexShader, string fragShader, RenderTechniqueType tech, RenderType type)
            : base(name)
        {
            renderType = type;
            Init(vertexShader, fragShader);
            Technique = tech;
        }

        public enum RenderType
        {
            Original,
            OffScreen
        }

        protected RenderType renderType { get; private set; }

        public RenderTechniqueType Technique { get; private set; }

        public Shader ShaderItem { get; set; }

        protected RenderObject Plane { get; set; }

        public RenderTarget RenderTarget { get; set; }

        public List<Texture> OutputTexture { get; set; }

        /// <summary>
        /// シェーダへ値のセット
        /// </summary>
        protected void SetValue<T>(ref T member, T value, [CallerMemberName]string memberName = "")
        {
            if (ShaderItem.SetValue(memberName, value))
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

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public virtual void SizeChanged(int width, int height)
        {
            RenderTarget.SizeChanged(width, height);
        }

        /// <summary>
        /// 描画
        /// </summary>
        public virtual void Render()
        {
            if (renderType == RenderType.Original)
            {
                Logger.Log(Logger.LogLevel.Error, "RenderTechnique : Not Defined Original Render");
            }
            if (renderType == RenderType.OffScreen)
            {
                if (Plane != null)
                {
                    RenderTarget.ClearBuffer();
                    RenderTarget.BindRenderTarget(OutputTexture.ToArray());
                    Plane.Shader = ShaderItem;
                    Plane.Render(Global.Scene);
                    RenderTarget.UnBindRenderTarget();
                }
            }
        }

        #region [initalize event]

        /// <summary>
        /// 初期化
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// シェーダの生成
        /// </summary>
        /// <param name="vertexShader">頂点シェーダ</param>
        /// <param name="fragShader">フラグメントシェーダ</param>
        public virtual void CreateShader(string vertexShader, string fragShader)
        {
            ShaderItem = ShaderFactory.Instance.CreateShaderVF(vertexShader, fragShader);
        }

        public virtual void CreateRenderTarget(int width, int height)
        {
            OutputTexture.Add(TextureFactory.Instance.CreateTexture("Texture:" + Name, width, height));
            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget("RenderTarget:" + Name, width, height, OutputTexture.Count);
            //RenderTarget = RenderTargetFactory.Instance.Default;
            RenderTarget.SizeChanged(width, height);
        }

        private void Init(string vertexShader = null, string fragShader = null)
        {
            if (vertexShader != null && fragShader != null)
            {
                CreateShader(vertexShader, fragShader);
            }

            OutputTexture = new List<Texture>();
            Plane = RenderObjectFactory.Instance.CreateRenderObject(Name, AssetFactory.Instance.CreatePlane(Name));
            CreateRenderTarget(KI.Gfx.GLUtil.DeviceContext.Instance.Width, KI.Gfx.GLUtil.DeviceContext.Instance.Height);
            Initialize();
        }
        #endregion
    }
}
