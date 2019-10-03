using System;
using System.Linq;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;
using KI.Gfx.Render;
using OpenTK.Graphics.OpenGL;
using KI.Asset.Primitive;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// スクリーンスペースLIC
    /// </summary>
    public partial class SSLIC : DefferedTechnique
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

        PolygonNode postRectangle;
        Texture ssLicTex;
        ImageInfo imageInfo = new ImageInfo("SSLIC : PreRendering");

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSLIC(RenderSystem renderer, string vertexShader, string fragShader)
            : base("SSLIC", renderer, vertexShader, fragShader)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            CreateFrameBuffer(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
            CreateNoizeTexture(64, 64);
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
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="renderInfo">レンダリング情報</param>
        public override void Render(Scene scene, RenderInfo renderInfo)
        {
            var gBuffer = RenderSystem.RenderQueue.Items.OfType<GBuffer>().First();
            gBuffer.RenderTarget.GetPixelData(imageInfo, DeviceContext.Instance.Width, DeviceContext.Instance.Height, (int)GBuffer.OutputTextureType.Light);
            ssLicTex.SetTextureFromImage(imageInfo);

            for (int i = 0; i < 10; i++)
            {
                RenderTarget.ClearBuffer();
                RenderTarget.BindRenderTarget();

                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                postRectangle.Render(scene, renderInfo);
                GL.Disable(EnableCap.Blend);

                RenderTarget.UnBindRenderTarget();

                RenderTarget.GetPixelData(imageInfo, DeviceContext.Instance.Width, DeviceContext.Instance.Height, 0);
                uNoize.SetTextureFromImage(imageInfo);
            }
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

            uNoize.SetTextureFromArray(rgba);
        }

        /// <summary>
        /// レンダーターゲットの生成
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        private void CreateFrameBuffer(int width, int height)
        {
            var textures = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            var vectorTexture = textures[(int)GBuffer.OutputTextureType.Light];

            if (ssLicTex == null)
            {
                ssLicTex = TextureFactory.Instance.CreateTexture("SSLIC Texture", width, height);
                uNoize = TextureFactory.Instance.CreateTexture("SSLIC Texture", width, height);
                var polygon = PolygonUtility.CreatePolygon("SSLIC PostRectangle", new Rectangle());
                postRectangle = new PolygonNode(polygon);
                postRectangle.Polygon.Material.Shader = ShaderCreater.Instance.CreateShader(SHADER_TYPE.SSLIC);
                postRectangle.Polygon.Material.Shader.SetValue("uVector", ssLicTex);
                postRectangle.Polygon.Material.Shader.SetValue("uNoize", uNoize);
            }
        }
    }
}
