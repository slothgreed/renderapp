using KI.Asset;
using KI.Gfx.KITexture;
using KI.Gfx.Render;

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
            : base("GBuffer", renderer, RenderType.Original)
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
        public Texture GetOutputTexture(OutputTextureType target)
        {
            return RenderTarget.RenderTexture[(int)target];
        }

        /// <summary>
        /// レンダーターゲットの生成
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        protected override void CreateRenderTarget(int width, int height)
        {
            var textures = new RenderTexture[4]
            {
                TextureFactory.Instance.CreateRenderTexture("GPosit", width, height),
                TextureFactory.Instance.CreateRenderTexture("GNormal", width, height),
                TextureFactory.Instance.CreateRenderTexture("GColor", width, height),
                TextureFactory.Instance.CreateRenderTexture("GLight", width, height)
            };

            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget(Name, width, height, textures.Length);
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
        public override void Render(Scene scene)
        {
            ClearBuffer();
            RenderTarget.BindRenderTarget();
            foreach (var asset in scene.RootNode.AllChildren())
            {
                if (asset is Light)
                {
                    var light = asset as Light;
                    if (light.Model != null)
                    {
                        light.Model.Render(scene);
                    }
                }
                else if (asset is PolygonNode)
                {
                    var polygonNode = asset as PolygonNode;
                    polygonNode.Render(scene);
                }
            }

            RenderTarget.UnBindRenderTarget();
        }
    }
}
