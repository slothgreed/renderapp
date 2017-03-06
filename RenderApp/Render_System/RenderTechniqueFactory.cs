using KI.Gfx.KIAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.Render_System
{
    enum RenderTechniqueType
    {
        GBuffer,
        Lighting,
        Bloom,
        Dof,
        Sobel,
        SSAO,
        Selection,
        Output
    }

    class RenderTechniqueFactory
    {
        List<KeyValuePair<RenderTechniqueType, RenderTechnique>> RenderTechniques = new List<KeyValuePair<RenderTechniqueType, RenderTechnique>>();
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
                case RenderTechniqueType.GBuffer:
                    technique = new GBuffer();
                    break;
                case RenderTechniqueType.Lighting:
                    technique = new LighthingBuffer();
                    break;
                case RenderTechniqueType.Selection:
                    technique = new Selection();
                    break;
                case RenderTechniqueType.Sobel:
                    technique = new Sobel();
                    break;
                case RenderTechniqueType.Bloom:
                    technique = new Bloom();
                    break;
                case RenderTechniqueType.Output:
                    technique = new OutputBuffer();
                    break;
                default:
                    technique = null;
                    break;
            }
            RenderTechniques.Add(new KeyValuePair<RenderTechniqueType, RenderTechnique>(type, technique));
            return technique;
        }
        public List<Texture> OutputTexture(RenderTechniqueType type)
        {
            foreach(var technique in RenderTechniques)
            {
                if(technique.Key == type)
                {
                    return technique.Value.OutputTexture;
                }
            }
            return null;
        }
        public IEnumerable<Texture> OutputTextures()
        {
            foreach(var technique in RenderTechniques)
            {
                //出力用のは返さない
                if(technique.Value is OutputBuffer)
                {
                    continue;
                }
                foreach(var texture in technique.Value.OutputTexture)
                {
                    yield return texture;
                }
            }
        }
        public void SizeChanged(int width, int height)
        {
            foreach (var technique in RenderTechniques)
            {
                technique.Value.SizeChanged(width, height);
            }
        }
        public void Dispose()
        {
            foreach(var technique in RenderTechniques)
            {
                technique.Value.Dispose();
            }
            RenderTechniques.Clear();
        }
    }
}
