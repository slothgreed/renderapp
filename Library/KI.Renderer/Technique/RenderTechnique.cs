using System;
using KI.Foundation.Core;
using KI.Gfx.Render;
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

        protected bool UseDepthTexture
        {
            get;
            private set;
        }

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
            UseDepthTexture = _useDepthTexture;
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
        protected abstract void CreateRenderTarget();


        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="vertexShader">頂点シェーダ</param>
        /// <param name="fragShader">フラグシェーダ</param>
        public void InitializeTechnique()
        {
            CreateRenderTarget();
            Initialize();
        }
        #endregion
    }
}
