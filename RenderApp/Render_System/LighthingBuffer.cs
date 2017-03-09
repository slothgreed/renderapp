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

        public LighthingBuffer()
            : base("LighthingBuffer", vertexShader, fragShader, RenderType.Original)
        {
            Plane.AddTexture(TextureKind.Albedo, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[0]);
            Plane.AddTexture(TextureKind.Normal, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[1]);
            Plane.AddTexture(TextureKind.World, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[2]);
            Plane.AddTexture(TextureKind.Lighting, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[3]);
        }

        public override void Render()
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
            //foreach(var light in SceneManager.Instance.ActiveScene.RootNode.AllChildren())
            //{
            //    if(light.KIObject is Light)
            //    {
                    Plane.Shader = ShaderItem;
                    Plane.Render();
            //    }
            //}
            RenderTarget.UnBindRenderTarget();
        }

        public override void Initialize()
        {
            //throw new NotImplementedException();
        }
    }
}
