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
        public override void Render(IScene scene)
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
            var textures = Global.RenderSystem.RenderQueue.OutputTexture(RenderTechniqueType.GBuffer);
            Plane.Polygon.AddTexture(TextureKind.Albedo, textures[(int)GBuffer.GBufferOutputType.Posit]);
            Plane.Polygon.AddTexture(TextureKind.Normal, textures[(int)GBuffer.GBufferOutputType.Normal]);
            Plane.Polygon.AddTexture(TextureKind.World, textures[(int)GBuffer.GBufferOutputType.Color]);
            Plane.Polygon.AddTexture(TextureKind.Lighting, textures[(int)GBuffer.GBufferOutputType.Light]);
        }
    }
}
