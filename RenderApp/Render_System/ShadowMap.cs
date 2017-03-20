using KI.Gfx.Render;
using RenderApp.AssetModel;
namespace RenderApp.Render_System
{
    public class ShadowMap : RenderTechnique
    {
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\shadow.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\shadow.frag";

        public ShadowMap(RenderTechniqueType tech)
            : base("ShadowMap", vertexShader, fragShader, tech, RenderType.Original)
        {

        }
        public override void Initialize()
        {
        }

        public override void Render()
        {
            ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
            foreach (var asset in SceneManager.Instance.ActiveScene.RootNode.AllChildren())
            {
                if(asset.KIObject is Geometry)
                {
                    var geometry = asset.KIObject as Geometry;
                    var old = geometry.Shader;
                    geometry.Shader = ShaderItem;
                    geometry.Render();
                    geometry.Shader = old;
                }
            }
            RenderTarget.UnBindRenderTarget();
        }
    }
}
