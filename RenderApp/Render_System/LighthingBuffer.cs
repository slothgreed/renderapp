using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.KIAsset;
using RenderApp.AssetModel;
namespace RenderApp.Render_System
{

    public class LighthingBuffer : RenderTechnique
    {
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\Defferd.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\Defferd.frag";

        public LighthingBuffer(Geometry plane)
            : base("LighthingBuffer", vertexShader, fragShader)
        {
            Plane = plane;
        }

        public void Render()
        {
            RenderTarget.BindRenderTarget();
            foreach(var light in SceneManager.Instance.ActiveScene.RootNode.AllChildren())
            {
                if(light.KIObject is Light)
                {
                    Plane.Shader = Shader;
                    Plane.Render();
                }
            }
            RenderTarget.UnBindRenderTarget();
        }

        public override void Initialize()
        {
            //throw new NotImplementedException();
        }
    }
}
