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
        Selection,
        PostEffect,
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
                case RenderTechniqueType.PostEffect:
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
