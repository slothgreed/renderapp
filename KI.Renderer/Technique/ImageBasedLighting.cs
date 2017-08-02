using KI.Asset;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    /// <summary>
    /// IBL
    /// </summary>
    public class ImageBasedLighting : RenderTechnique
    {
        /// <summary>
        /// 頂点シェーダ
        /// </summary>
        private static string vertexShader = Global.ShaderDirectory + @"\Lighthing\ibl.vert";

        /// <summary>
        /// フラグシェーダ
        /// </summary>
        private static string fragShader = Global.ShaderDirectory + @"\Lighthing\ibl.frag";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageBasedLighting()
            : base("IBL", vertexShader, fragShader, RenderTechniqueType.IBL, RenderType.Original)
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        public override void Render()
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
            foreach (var probe in Global.Scene.RootNode.AllChildren())
            {
                if (probe.KIObject is EnvironmentProbe)
                {
                    EnvironmentProbe env = probe.KIObject as EnvironmentProbe;
                    Plane.AddTexture(TextureKind.Cubemap, env.Cubemap);
                    Plane.Shader = ShaderItem;
                    Plane.Render(Global.Scene);
                }
            }

            RenderTarget.UnBindRenderTarget();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            Plane.AddTexture(TextureKind.Albedo, Global.RenderSystem.GBufferStage.OutputTexture[2]);
            Plane.AddTexture(TextureKind.Normal, Global.RenderSystem.GBufferStage.OutputTexture[1]);
            Plane.AddTexture(TextureKind.World, Global.RenderSystem.GBufferStage.OutputTexture[0]);
            Plane.AddTexture(TextureKind.Lighting, Global.RenderSystem.GBufferStage.OutputTexture[3]);
        }
    }
}
