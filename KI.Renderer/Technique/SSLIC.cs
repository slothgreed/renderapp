using System;
using KI.Asset;
using KI.Gfx.GLUtil;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer
{
    /// <summary>
    /// スクリーンスペースLIC
    /// </summary>
    public partial class SSLIC : OffScreenTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSLIC(string vertexShader, string fragShader)
            : base("SSLIC", vertexShader, fragShader, RenderTechniqueType.SSLIC, RenderType.Original)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uNoize = TextureFactory.Instance.CreateTexture("Noize", DeviceContext.Instance.Width, DeviceContext.Instance.Height);

            CreateNoize(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public override void SizeChanged(int width, int height)
        {
            base.SizeChanged(width, height);

            CreateNoize(width, height);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void Render(Scene scene)
        {
            if (Plane != null)
            {
                var textures = Global.Renderer.RenderQueue.OutputTexture(RenderTechniqueType.GBuffer);
                var vector = textures[(int)GBuffer.OutputTextureType.Color];
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                RenderTarget.ClearBuffer();
                RenderTarget.BindRenderTarget(OutputTexture);
                Plane.Shader = ShaderItem;
                Plane.Render(scene);
                RenderTarget.UnBindRenderTarget();
                GL.Disable(EnableCap.Blend);
            }
        }

        /// <summary>
        /// ノイズの生成
        /// </summary>
        /// <param name="width">ノイズテクスチャ横</param>
        /// <param name="height">ノイズテクスチャ縦</param>
        private void CreateNoize(int width, int height)
        {
            float[,,] rgba = new float[width, height, 4];
            Random rand = new Random();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float color = rand.Next(255) / 255.0f;

                    rgba[i, j, 0] = color;
                    rgba[i, j, 1] = color;
                    rgba[i, j, 2] = color;
                    rgba[i, j, 3] = 15 / 255.0f;
                }
            }

            uNoize.GenTexture(rgba);
        }
    }
}
