using System.Collections.Generic;

namespace KI.Renderer
{
    public class PostEffectManager
    {
        public List<RenderTechnique> PostEffects = new List<RenderTechnique>();

        public PostEffectManager()
        {
            CreatePostProcessFlow();
        }

        private void CreatePostProcessFlow()
        {
            //Bloom bloom = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Bloom) as Bloom;
            //bloom.uTarget = Workspace.SceneManager.RenderSystem.GBufferStage.OutputTexture[3];

            //Sobel sobel = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Sobel) as Sobel;
            //sobel.uTarget = Workspace.SceneManager.RenderSystem.GBufferStage.OutputTexture[3];

            //SSAO ssao = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSAO) as SSAO;
            //ssao.uPosition = Workspace.SceneManager.RenderSystem.GBufferStage.OutputTexture[0];
            //ssao.uTarget = Workspace.SceneManager.RenderSystem.LightingStage.OutputTexture[0];

            SSLIC sslic = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSLIC) as SSLIC;
            sslic.uVector = Global.RenderSystem.GBufferStage.OutputTexture[2];

            //PostEffects.Add(bloom);
            //PostEffects.Add(sobel);
            //PostEffects.Add(ssao);
            PostEffects.Add(sslic);

        }

        private void ClearBuffer()
        {
            foreach(var post in PostEffects)
            {
                post.ClearBuffer();
            }
        }

        public void SizeChanged(int width, int height)
        {
            foreach (var post in PostEffects)
            {
                post.SizeChanged(width, height);
            }
        }
        public void Render()
        {
           foreach(var post in PostEffects)
           {
               post.Render();
           }
        }

        public void Dispose()
        {

        }
    }
}
