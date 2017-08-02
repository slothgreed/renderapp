using KI.Gfx.Render;

namespace KI.Renderer
{
    /// <summary>
    /// シャドウマップ
    /// </summary>
    public class ShadowMap : RenderTechnique
    {
        /// <summary>
        /// 頂点シェーダ
        /// </summary>
        private static string vertexShader = Global.ShaderDirectory + @"\shadow.vert";

        /// <summary>
        /// フラグシェーダ
        /// </summary>
        private static string fragShader = Global.ShaderDirectory + @"\shadow.frag";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ShadowMap()
            : base("ShadowMap", vertexShader, fragShader, RenderTechniqueType.Shadow, RenderType.Original)
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
        public override void Render()
        {
            ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
            foreach (var asset in Global.Scene.RootNode.AllChildren())
            {
                if (asset.KIObject is RenderObject)
                {
                    var geometry = asset.KIObject as RenderObject;
                    var old = geometry.Shader;
                    geometry.Shader = ShaderItem;
                    geometry.Render(Global.Scene);
                    geometry.Shader = old;
                }
            }

            RenderTarget.UnBindRenderTarget();
        }
    }
}
