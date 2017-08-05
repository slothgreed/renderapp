using KI.Asset;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    /// <summary>
    /// 最終出力用のバッファ
    /// </summary>
    public partial class OutputBuffer : RenderTechnique
    {
        /// <summary>
        /// 頂点シェーダ
        /// </summary>
        private static string vertexShader = Global.ShaderDirectory + @"\PostEffect\Output.vert";

        /// <summary>
        /// フラグシェーダ
        /// </summary>
        private static string fragShader = Global.ShaderDirectory + @"\PostEffect\Output.frag";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OutputBuffer()
            : base("OutputBuffer", vertexShader, fragShader, RenderTechniqueType.Output, RenderType.Original)
        {
            Plane = RenderObjectFactory.Instance.CreateRenderObject("OutputBuffer", AssetFactory.Instance.CreatePlane("OutputPlane"));
            Plane.AddTexture(TextureKind.Normal, Global.RenderSystem.GBufferStage.OutputTexture[2]);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uSelectMap = null;
        }

        /// <summary>
        /// 描画テクスチャの設定
        /// </summary>
        /// <param name="textureKind">テクスチャ種類</param>
        /// <param name="outputTexture">出力テクスチャ</param>
        public void SetOutputTarget(TextureKind textureKind, Texture outputTexture)
        {
            Plane.AddTexture(textureKind, outputTexture);
        }

        /// <summary>
        /// 描画
        /// </summary>
        public override void Render()
        {
            //最終出力フレームバッファのバインドの必要なし
            Plane.Shader = ShaderItem;
            Plane.Render(Global.Scene);
        }
    }
}
