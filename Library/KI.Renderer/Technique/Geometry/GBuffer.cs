﻿using System.Linq;
using KI.Asset;
using KI.Gfx.Render;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.Buffer;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// GBuffer
    /// </summary>
    public class GBuffer : RenderTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GBuffer(RenderSystem renderer)
            : base("GBuffer", renderer, false)
        {
        }

        /// <summary>
        /// GBufferの入力値
        /// </summary>
        public enum OutputTextureType
        {
            Posit = 0,
            Normal,
            Color,
            Light
        }

        /// <summary>
        /// 出力テクスチャの取得
        /// </summary>
        /// <param name="target">GBufferのタイプ</param>
        /// <returns>テクスチャ</returns>
        public TextureBuffer GetOutputTexture(OutputTextureType target)
        {
            return RenderTarget.RenderTexture[(int)target];
        }

        /// <summary>
        /// レンダーターゲットの生成
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        /// <param name="useDepthTexture">デプステクスチャを使うかどうか</param>
        protected override void CreateRenderTarget(int width, int height, bool useDepthTexture)
        {
            var textures = new RenderTexture[4]
            {
                TextureFactory.Instance.CreateRenderTexture("GPosit", width, height, PixelFormat.Rgba),
                TextureFactory.Instance.CreateRenderTexture("GNormal", width, height, PixelFormat.Rgba),
                TextureFactory.Instance.CreateRenderTexture("GColor", width, height, PixelFormat.Rgba),
                TextureFactory.Instance.CreateRenderTexture("GLight", width, height, PixelFormat.Rgba)
            };

            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget(Name, width, height, useDepthTexture);
            RenderTarget.SetRenderTexture(textures);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="renderInfo">レンダリング情報</param>
        public override void Render(Scene scene, RenderInfo renderInfo)
        {
            // デプステクスチャはZPrepassで書いたものを利用する。
            //ClearBuffer(ClearBufferMask.ColorBufferBit);
            //GL.DepthFunc(DepthFunction.Lequal);
            ClearBuffer();
            RenderTarget.BindRenderTarget();
            foreach (SceneNode asset in scene.RootNode.AllChildren().OfType<SceneNode>())
            {
                asset.Render(scene, renderInfo);
            }

            RenderTarget.UnBindRenderTarget();
        }
    }
}
