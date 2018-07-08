using System;
using System.Linq;
using KI.Asset;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KITexture;
using KI.Gfx.Render;
using OpenTK;
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

        RenderObject postRectangle;
        Texture ssLicTex;
        Texture uFrame;
        float[,,] frame;
        ImageInfo imageInfo = new ImageInfo("SSLIC : PreRendering");
        Vector3[] lookUpTable;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSLIC(string vertexShader, string fragShader)
            : base("SSLIC", vertexShader, fragShader, RenderType.Original)
        {
            lookUpTable = new Vector3[]
                {
                    new Vector3(0.1f,0.1f,0.1f),
                    new Vector3(0.1f,0.1f,0.1f),
                    new Vector3(0.1f,0.1f,0.1f),
                    new Vector3(0.1f,0.1f,0.1f),
                    new Vector3(0.1f,0.1f,0.1f),
                    new Vector3(0.1f,0.1f,0.1f),
                    new Vector3(0.1f,0.1f,0.1f),
                    new Vector3(0.1f,0.1f,0.1f),
                    new Vector3(0.1f,0.1f,0.1f),
                    new Vector3(0.1f,0.1f,0.1f),
                };
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            CreateFrameBuffer(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
            CreateNoizeTexture(64, 64);
            CreateFrameTexture(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public override void SizeChanged(int width, int height)
        {
            base.SizeChanged(width, height);

            CreateFrameBuffer(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
            CreateNoizeTexture(64, 64);
            CreateFrameTexture(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }


        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void Render(Scene scene)
        {
            // alpha blend が分からない
            var gBuffer = Global.Renderer.RenderQueue.Items.OfType<GBuffer>().First();
            gBuffer.RenderTarget.GetPixelData(imageInfo, DeviceContext.Instance.Width, DeviceContext.Instance.Height, PixelFormat.Bgr, (int)GBuffer.OutputTextureType.Light);
            ssLicTex.GenTexture(imageInfo);
            uFrame.GenTexture(frame);

            RenderTarget.ClearColor(1, 1, 1, 1);
            int i = 0;
            for (i = 0; i < 10; i++)
            {
                RenderTarget.ClearBuffer();
                RenderTarget.BindRenderTarget();

                postRectangle.Render(scene);

                RenderTarget.UnBindRenderTarget();

                RenderTarget.GetPixelData(imageInfo, DeviceContext.Instance.Width, DeviceContext.Instance.Height,PixelFormat.Rgb, 0);
                uFrame.SetupTexImage2D(TextureTarget.Texture2D, imageInfo, PixelInternalFormat.Rgb, PixelFormat.Rgb);
                postRectangle.Shader.SetValue("uFrame", uFrame);
                postRectangle.Shader.SetValue("uScale", lookUpTable[i]);
                imageInfo.BmpImage.Save("VectorField" + i.ToString() + ".jpg");
            }

            RenderTarget.ClearColor(0, 0, 0, 1);
        }

        /// <summary>
        /// フレームレンダリング用のバッファ生成
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        private void CreateFrameTexture(int width, int height)
        {
            frame = new float[width, height, 4];
            for (int i = 0; i < frame.GetLength(0); i++)
            {
                for (int j = 0; j < frame.GetLength(1); j++)
                {
                    frame[i, j, 0] = 0.3f;
                    frame[i, j, 1] = 0.3f;
                    frame[i, j, 2] = 0.3f;
                    frame[i, j, 3] = 1.0f;
                }
            }

            uFrame.GenTexture(frame);
        }
        /// <summary>
        /// ノイズの生成
        /// </summary>
        /// <param name="width">ノイズテクスチャ横</param>
        /// <param name="height">ノイズテクスチャ縦</param>
        private void CreateNoizeTexture(int width, int height)
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

        /// <summary>
        /// レンダーターゲットの生成
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        private void CreateFrameBuffer(int width, int height)
        {
            var textures = Global.Renderer.RenderQueue.OutputTexture<GBuffer>();
            var vectorTexture = textures[(int)GBuffer.OutputTextureType.Light];

            if (ssLicTex == null)
            {
                ssLicTex = TextureFactory.Instance.CreateTexture("SSLIC Texture", width, height);
                uNoize = TextureFactory.Instance.CreateTexture("SSLIC Texture", width, height);
                uFrame = TextureFactory.Instance.CreateTexture("SSLIC Frame", width, height);

                postRectangle = RenderObjectFactory.Instance.CreateRenderObject("SSLIC PostRectangle", AssetFactory.Instance.CreateRectangle("SSLIC PostRectangle"));
                postRectangle.Shader = ShaderCreater.Instance.CreateShader(ShaderType.SSLIC);
                postRectangle.Shader.SetValue("uVector", ssLicTex);
                postRectangle.Shader.SetValue("uNoize", uNoize);
                postRectangle.Shader.SetValue("uFrame", uFrame);
            }
        }

        public override void Dispose()
        {
            imageInfo.Dispose();
        }
    }
}
