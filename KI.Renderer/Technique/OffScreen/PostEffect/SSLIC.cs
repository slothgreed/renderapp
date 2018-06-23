using System;
using KI.Asset;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KITexture;
using KI.Gfx.Render;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset.Technique
{
    /// <summary>
    /// スクリーンスペースLIC
    /// </summary>
    public partial class SSLIC : RenderTechnique
    {
        private Texture _uNoize;
        public Texture uNoize
        {
            get
            {
                return _uNoize;
            }

            set
            {
                SetValue<Texture>(ref _uNoize, value);
            }
        }

        RenderObject preRectangle;
        RenderObject postRectangle;
        Texture ssLicTex;

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
            CreateNoizeTexture(64, 64);
            CreateFrameBuffer(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public override void SizeChanged(int width, int height)
        {
            base.SizeChanged(width, height);

            CreateNoizeTexture(width, height);
            CreateFrameBuffer(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void Render(Scene scene)
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture);

            preRectangle.Render(scene);

            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //postRectangle.Render(scene);
            //GL.Disable(EnableCap.Blend);

            RenderTarget.UnBindRenderTarget();

            var pixelData = RenderTarget.FrameBuffer.GetPixelData(DeviceContext.Instance.Width, DeviceContext.Instance.Height, PixelFormat.Rgba);
            ssLicTex.GenTexture(pixelData);
        }

        /// <summary>
        /// ノイズの生成
        /// </summary>
        /// <param name="width">ノイズテクスチャ横</param>
        /// <param name="height">ノイズテクスチャ縦</param>
        private void CreateNoizeTexture(int width, int height)
        {
            if (uNoize == null)
            {
                uNoize = TextureFactory.Instance.CreateTexture("Noize", 64, 64);
                uNoize.WrapMode = TextureWrapMode.Clamp;
            }

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

        /// <summary>
        /// レンダーターゲットの生成
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        private void CreateFrameBuffer(int width, int height)
        {
            var textures = Global.Renderer.RenderQueue.OutputTexture(RenderTechniqueType.GBuffer);
            var colorTexture = textures[(int)GBuffer.OutputTextureType.Light];

            if (ssLicTex == null)
            {
                ssLicTex = TextureFactory.Instance.CreateTexture("SSLIC Texture", width, height);

                preRectangle = RenderObjectFactory.Instance.CreateRenderObject("SSLIC Rectangle", AssetFactory.Instance.CreateRectangle("SSLIC Rectangle"));
                preRectangle.Shader = ShaderCreater.Instance.CreateShader(ShaderType.Output);
                preRectangle.Shader.SetValue("uTarget", colorTexture);

                postRectangle = RenderObjectFactory.Instance.CreateRenderObject("SSLIC PostRectangle", AssetFactory.Instance.CreateRectangle("SSLIC PostRectangle"));
                postRectangle.Shader = ShaderCreater.Instance.CreateShader(ShaderType.SSLIC);
                postRectangle.Shader.SetValue("uVector", OutputTexture[0]);
                postRectangle.Shader.SetValue("uNoize", uNoize);
            }

            float[,,] rgba = new float[DeviceContext.Instance.Width, DeviceContext.Instance.Height, 4];
            for (int i = 0; i < rgba.GetLength(0); i++)
            {
                for (int j = 0; j < rgba.GetLength(1); j++)
                {
                    rgba[i, j, 0] = 255.0f;
                    rgba[i, j, 1] = 0;
                    rgba[i, j, 2] = 0;
                    rgba[i, j, 3] = 255.0f;
                }
            }

            ssLicTex.GenTexture(rgba);
        }
    }
}
