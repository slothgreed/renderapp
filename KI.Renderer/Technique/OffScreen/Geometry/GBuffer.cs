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
        public GBuffer()
            : base("GBuffer", RenderTechniqueType.GBuffer, RenderType.Original)
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
            return OutputTexture[(int)target];
        }

        /// <summary>
        /// レンダーターゲットの生成
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        protected override void CreateRenderTarget(int width, int height)
        {
            OutputTexture = new Texture[4]
            {
                TextureFactory.Instance.CreateTexture("GPosit", width, height),
                TextureFactory.Instance.CreateTexture("GNormal", width, height),
                TextureFactory.Instance.CreateTexture("GColor", width, height),
                TextureFactory.Instance.CreateTexture("GLight", width, height)
            };

            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget(Name, width, height, OutputTexture.Length);
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
            RenderTarget.BindRenderTarget(OutputTexture);
            foreach (var asset in scene.RootNode.AllChildren())
            {
                if (asset.KIObject is RenderObject)
                {
                    var polygon = asset.KIObject as RenderObject;
                    polygon.Render(scene);
                }
            }

            RenderTarget.UnBindRenderTarget();
        }
    }
}
