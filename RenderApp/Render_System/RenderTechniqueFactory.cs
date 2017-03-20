using KI.Gfx.KIAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Core;
namespace RenderApp.Render_System
{
    public enum RenderTechniqueType
    {
        Shadow,
        GBuffer,
        Lighting,
        Bloom,
        Dof,
        Sobel,
        SSAO,
        Selection,
        Output
    }

    class RenderTechniqueFactory : KIFactoryBase<RenderTechnique>
    {
        private static RenderTechniqueFactory _instance = null;

        public static RenderTechniqueFactory Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new RenderTechniqueFactory();
                }
                return _instance;
            }
        }

        public RenderTechnique CreateRenderTechnique(RenderTechniqueType type)
        {
            RenderTechnique technique = null;
            switch (type)
            {
                case RenderTechniqueType.Shadow:
                    technique = new ShadowMap(RenderTechniqueType.Shadow);
                    break;
                case RenderTechniqueType.GBuffer:
                    technique = new GBuffer(RenderTechniqueType.GBuffer);
                    break;
                case RenderTechniqueType.Lighting:
                    technique = new LighthingBuffer(RenderTechniqueType.Lighting);
                    break;
                case RenderTechniqueType.Selection:
                    technique = new Selection(RenderTechniqueType.Selection);
                    break;
                case RenderTechniqueType.Sobel:
                    technique = new Sobel(RenderTechniqueType.Sobel);
                    break;
                case RenderTechniqueType.Bloom:
                    technique = new Bloom(RenderTechniqueType.Bloom);
                    break;
                case RenderTechniqueType.Output:
                    technique = new OutputBuffer(RenderTechniqueType.Output);
                    break;
                case RenderTechniqueType.SSAO:
                    technique = new SSAO(RenderTechniqueType.SSAO);
                    break;
                default:
                    technique = null;
                    break;
            }
            AddItem(technique);
            return technique;
        }
        public List<Texture> OutputTexture(RenderTechniqueType type)
        {
            foreach(var technique in AllItem)
            {
                if(technique.Technique == type)
                {
                    return technique.OutputTexture;
                }
            }
            return null;
        }
        public IEnumerable<Texture> OutputTextures()
        {
            foreach (var technique in AllItem)
            {
                //出力用のは返さない
                if(technique is OutputBuffer)
                {
                    continue;
                }
                foreach(var texture in technique.OutputTexture)
                {
                    yield return texture;
                }
            }
        }
        public void SizeChanged(int width, int height)
        {
            foreach (var technique in AllItem)
            {
                technique.SizeChanged(width, height);
            }
        }
    }
}
