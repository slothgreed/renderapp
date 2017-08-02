using KI.Gfx.KITexture;
using KI.Asset;

namespace KI.Renderer
{
    public partial class OutputBuffer : RenderTechnique
    {
        private static string vertexShader = Global.ShaderDirectory + @"\PostEffect\Output.vert";
        private static string fragShader = Global.ShaderDirectory + @"\PostEffect\Output.frag";

        public OutputBuffer(RenderTechniqueType tech)
            : base("OutputBuffer", vertexShader, fragShader, tech, RenderType.Original)
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

        internal void SetOutputTarget(TextureKind textureKind, Texture OutputTexture)
        {
            Plane.AddTexture(textureKind, OutputTexture);
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
