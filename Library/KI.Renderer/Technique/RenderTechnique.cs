using System;
using System.Runtime.CompilerServices;
using KI.Asset;
using KI.Foundation.Core;
using KI.Gfx.KIShader;
using KI.Gfx.Render;
using KI.Asset.Primitive;
using OpenTK.Graphics.OpenGL;
using KI.Gfx;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// レンダーテクニック
    /// </summary>
    public abstract class RenderTechnique : KIObject
    {
        /// <summary>
        /// レンダラ
        /// </summary>
        public RenderSystem RenderSystem
        {
            get;
            private set;
        }

        bool useDepthTexture;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="renderer">レンダラ</param>
        /// <param name="_useDepthTexture">デプステクスチャを使うかどうか</param>
        public RenderTechnique(string name, RenderSystem renderer, bool _useDepthTexture)
            : base(name)
        {
            RenderSystem = renderer;
            useDepthTexture = _useDepthTexture;
        }
        

        /// <summary>
        /// レンダリングターゲット
        /// </summary>
        public RenderTarget RenderTarget { get; set; }


        /// <summary>
        /// バッファのクリア
        /// </summary>
        public void ClearBuffer(ClearBufferMask mask = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit)
        {
            RenderTarget.ClearBuffer(mask);
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
        /// <param name="scene">シーン</param>
        /// <param name="renderInfo">レンダリング情報</param>
        public virtual void Render(Scene scene, RenderInfo renderInfo)
        {
            throw new NotImplementedException();
        }

        #region [initalize event]

        /// <summary>
        /// 初期化
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// レンダーターゲットの作成
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        /// <param name="_useDepthTexture">デプステクスチャを使うかどうか</param>
        protected virtual void CreateRenderTarget(int width, int height, bool useDepthTexture)
        {
            var texture = new RenderTexture[] { TextureFactory.Instance.CreateRenderTexture("Texture:" + Name, width, height, PixelFormat.Rgba) };
            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget("RenderTarget:" + Name, width, height, useDepthTexture);
            RenderTarget.SetRenderTexture(texture);
        }


        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="vertexShader">頂点シェーダ</param>
        /// <param name="fragShader">フラグシェーダ</param>
        public void InitializeTechnique()
        {
            CreateRenderTarget(DeviceContext.Instance.Width, DeviceContext.Instance.Height, useDepthTexture);
            Initialize();
        }
        #endregion
    }
}
