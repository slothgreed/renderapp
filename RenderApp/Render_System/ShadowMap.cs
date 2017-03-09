using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.Render;
using KI.Gfx.KIAsset;
using RenderApp.AssetModel;
namespace RenderApp.Render_System
{
    public class ShadowMap : RenderTechnique
    {
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\shadow.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\shadow.frag";

        public ShadowMap()
            : base("ShadowMap", vertexShader, fragShader, RenderType.Original)
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
