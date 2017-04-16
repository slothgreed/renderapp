﻿using KI.Gfx.KIAsset;

namespace RenderApp.KIRenderSystem
{

    public class DeferredBuffer : RenderTechnique
    {
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\Lighthing\Defferd.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\Lighthing\Defferd.frag";

        public DeferredBuffer(RenderTechniqueType tech)
            : base("Deferred", vertexShader, fragShader, tech, RenderType.Original)
        {

        }

        public override void Render()
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
            foreach (var light in SceneManager.Instance.ActiveScene.RootNode.AllChildren())
            {
                if (light.KIObject is Light)
                {
                    Plane.Shader = ShaderItem;
                    Plane.Render();
                }
            }
            RenderTarget.UnBindRenderTarget();
        }

        public override void Initialize()
        {
            Plane.AddTexture(TextureKind.Albedo, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[2]);
            Plane.AddTexture(TextureKind.Normal, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[1]);
            Plane.AddTexture(TextureKind.World, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[0]);
            Plane.AddTexture(TextureKind.Lighting, SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[3]);
        }
    }
}