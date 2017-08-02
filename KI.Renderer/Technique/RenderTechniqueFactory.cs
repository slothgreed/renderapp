using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    /// <summary>
    /// レンダーテクニック種類
    /// </summary>
    public enum RenderTechniqueType
    {
        Shadow,
        GBuffer,
        IBL,
        Deferred,
        Bloom,
        Dof,
        Sobel,
        SSAO,
        SSLIC,
        Selection,
        Output
    }

    /// <summary>
    /// レンダーテクニックのファクトリークラス
    /// </summary>
    class RenderTechniqueFactory : KIFactoryBase<RenderTechnique>
    {
        /// <summary>
        /// シングルトン
        /// </summary>
        public static RenderTechniqueFactory Instance { get; } =  new RenderTechniqueFactory();

        public RenderTechnique CreateRenderTechnique(RenderTechniqueType type)
        {
            RenderTechnique technique = null;
            switch (type)
            {
                case RenderTechniqueType.Shadow:
                    technique = new ShadowMap(type);
                    break;
                case RenderTechniqueType.GBuffer:
                    technique = new GBuffer(type);
                    break;
                case RenderTechniqueType.Deferred:
                    technique = new DeferredBuffer(type);
                    break;
                case RenderTechniqueType.IBL:
                    technique = new ImageBasedLighting(type);
                    break;
                case RenderTechniqueType.Selection:
                    technique = new Selection(type);
                    break;
                case RenderTechniqueType.Sobel:
                    technique = new Sobel(type);
                    break;
                case RenderTechniqueType.Bloom:
                    technique = new Bloom(type);
                    break;
                case RenderTechniqueType.Output:
                    technique = new OutputBuffer(type);
                    break;
                case RenderTechniqueType.SSAO:
                    technique = new SSAO(type);
                    break;
                case RenderTechniqueType.SSLIC:
                    technique = new SSLIC(type);
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
            foreach (var technique in AllItem)
            {
                if (technique.Technique == type)
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
                if (technique is OutputBuffer)
                {
                    continue;
                }

                foreach (var texture in technique.OutputTexture)
                {
                    yield return texture;
                }
            }
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SizeChanged(int width, int height)
        {
            foreach (var technique in AllItem)
            {
                technique.SizeChanged(width, height);
            }
        }
    }
}
