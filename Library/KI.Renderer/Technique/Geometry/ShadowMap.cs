using KI.Asset;
using KI.Gfx.KIShader;
using KI.Gfx.Render;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// シャドウマップ
    /// </summary>
    public class ShadowMap : GBufferTechnique
    {
        private Shader ShadowMapShader;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vertexShader">頂点シェーダファイル</param>
        /// <param name="fragShader">フラグメントシェーダファイル</param>
        public ShadowMap(RenderSystem renderer, string vertexShader, string fragShader)
            : base("ShadowMap", renderer, 1, false)
        {
            ShadowMapShader = ShaderFactory.Instance.CreateShaderVF(vertexShader, fragShader);
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
            foreach (var node in scene.RootNode.AllChildren())
            {
                if (node is PolygonNode)
                {
                    var polygon = node as PolygonNode;
                    var old = polygon.Polygon.Material.Shader;
                    polygon.Polygon.Material.Shader = ShadowMapShader;
                    polygon.Render(scene, renderInfo);
                    polygon.Polygon.Material.Shader = old;
                }
            }

            RenderTarget.UnBindRenderTarget();
        }
    }
}
