using KI.Gfx.Render;
using System.Linq;
using KI.Asset;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// ZPrepassRender
    /// </summary>
    public class ZPrepassRender : GBufferTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ZPrepassRender(RenderSystem renderer)
            : base("ZPrepassRender", renderer, 1, true)
        {
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
        /// <param name="renderInfo">レンダリング情報</param>
        public override void Render(Scene scene, RenderInfo renderInfo)
        {
            ClearBuffer();
            RenderTarget.BindRenderTarget();
            foreach (SceneNode asset in scene.RootNode.AllChildren().OfType<SceneNode>())
            {
                asset.Render(scene, renderInfo);
            }

            RenderTarget.UnBindRenderTarget();
        }

    }
}
