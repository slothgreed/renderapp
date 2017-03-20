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
            Bloom bloom = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Bloom) as Bloom;
            bloom.uTarget = SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[3];

            Sobel sobel = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Sobel) as Sobel;
            sobel.uTarget = SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[3];

            //SSAO ssao = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSAO) as SSAO;
            //ssao.uPosition = SceneManager.Instance.RenderSystem.GBufferStage.OutputTexture[0];
            //ssao.uTarget = SceneManager.Instance.RenderSystem.LightingStage.OutputTexture[0];

            PostEffects.Add(bloom);
            PostEffects.Add(sobel);
            //PostEffects.Add(ssao);

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
