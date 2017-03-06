using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using RenderApp.GLUtil.ShaderModel;
using KI.Gfx.KIAsset;
using KI.Gfx.Render;
namespace RenderApp.Render_System
{
    class PostEffectManager
    {
        public List<RenderTechnique> PostEffects = new List<RenderTechnique>();

        public PostEffectManager()
        {
            CreatePostProcessFlow();
        }

        private void CreatePostProcessFlow()
        {
            RenderTechnique bloom = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Bloom);
            RenderTechnique sobel = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Sobel);

            Bloom b = bloom as Bloom;
            b.uTarget = SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[2];
            Sobel s = sobel as Sobel;
            s.uTarget = SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[3];
            PostEffects.Add(bloom);
            PostEffects.Add(sobel);
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
