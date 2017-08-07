using KI.Asset;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    /// <summary>
    /// Deffered Rendering
    /// </summary>
    public class DeferredBuffer : RenderTechnique
    {
        /// <summary>
        /// 頂点シェーダ
        /// </summary>
        private static string vertexShader = Global.ShaderDirectory + @"\Lighthing\Defferd.vert";

        /// <summary>
        /// フラグシェーダ
        /// </summary>
        private static string fragShader = Global.ShaderDirectory + @"\Lighthing\Defferd.frag";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DeferredBuffer()
            : base("Deferred", vertexShader, fragShader, RenderTechniqueType.Deferred, RenderType.Original)
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void Render(Scene scene)
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
            foreach (var light in scene.RootNode.AllChildren())
            {
                if (light.KIObject is Light)
                {
                    Plane.Shader = ShaderItem;
                    Plane.Render(scene);
                }
            }

            RenderTarget.UnBindRenderTarget();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            Plane.Geometry.AddTexture(TextureKind.Albedo, Global.RenderSystem.GBufferStage.OutputTexture[2]);
            Plane.Geometry.AddTexture(TextureKind.Normal, Global.RenderSystem.GBufferStage.OutputTexture[1]);
            Plane.Geometry.AddTexture(TextureKind.World, Global.RenderSystem.GBufferStage.OutputTexture[0]);
            Plane.Geometry.AddTexture(TextureKind.Lighting, Global.RenderSystem.GBufferStage.OutputTexture[3]);
        }
    }
}
