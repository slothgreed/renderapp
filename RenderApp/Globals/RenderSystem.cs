using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Renderer;

namespace RenderApp.Globals
{
    /// <summary>
    /// レンダリングシステム
    /// </summary>
    public class RenderSystem : IRenderer
    {
        public override void Initialize(int width, int height)
        {
            //RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Shadow));
            RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer));
            //RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.IBL));
            //RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred));
            //RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Selection));
            //RenderQueue.AddTechnique((OutputBuffer)RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output));
            OutputBuffer = new OutputBuffer();
            OutputTexture = RenderQueue.OutputTexture(RenderTechniqueType.GBuffer).ToArray()[(int)GBuffer.GBufferOutputType.Color];

            //var textures = RenderQueue.OutputTexture(RenderTechniqueType.GBuffer);
            //Bloom bloom = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Bloom) as Bloom;
            //bloom.uTarget = textures[3];
            //PostEffect.AddTechnique(bloom);

            //Sobel sobel = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Sobel) as Sobel;
            //sobel.uTarget = textures[3];
            //PostEffect.AddTechnique(sobel);

            //SSAO ssao = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSAO) as SSAO;
            //ssao.uPosition = textures[0];
            //ssao.uTarget = textures[0];
            //PostEffect.AddTechnique(ssao);

            //SSLIC sslic = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSLIC) as SSLIC;
            //sslic.uVector = textures[2];
            //PostEffect.AddTechnique(sslic);
        }
    }
}
