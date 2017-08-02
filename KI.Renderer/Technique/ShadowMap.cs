using KI.Asset;
using KI.Gfx.Render;

namespace KI.Renderer
{
    public class ShadowMap : RenderTechnique
    {
        private static string vertexShader = Global.ShaderDirectory + @"\shadow.vert";
        private static string fragShader = Global.ShaderDirectory + @"\shadow.frag";

        public ShadowMap(RenderTechniqueType tech)
            : base("ShadowMap", vertexShader, fragShader, tech, RenderType.Original)
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
