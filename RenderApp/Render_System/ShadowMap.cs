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
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\ShadowMap.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\ShadowMap.frag";

        public ShadowMap()
            : base("ShadowMap",RenderType.Original)
        {

        }
        public override void Initialize()
        {
        }

        public override void Render()
        {
            RenderTarget.BindRenderTarget();
            foreach (var asset in SceneManager.Instance.ActiveScene.RootNode.AllChildren())
            {
                if (asset.KIObject is Light)
                {
                    var light = asset.KIObject as Light;
                    Plane.Shader = Shader;
                    Plane.Render();
                }
            }
            RenderTarget.UnBindRenderTarget();
        }
    }
}
