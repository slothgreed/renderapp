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
    public class RenderTechniqueFactory : KIFactoryBase<RenderTechnique>
    {
        /// <summary>
        /// シングルトン
        /// </summary>
        public static RenderTechniqueFactory Instance { get; } = new RenderTechniqueFactory();

        /// <summary>
        /// レンダーテクニックの作成
        /// </summary>
        /// <param name="type">テクニックのタイプ</param>
        /// <returns>レンダーテクニック</returns>
        public RenderTechnique CreateRenderTechnique(RenderTechniqueType type)
        {
            RenderTechnique technique = null;
            switch (type)
            {
                case RenderTechniqueType.Shadow:
                    technique = new ShadowMap();
                    break;
                case RenderTechniqueType.GBuffer:
                    technique = new GBuffer();
                    break;
                case RenderTechniqueType.Deferred:
                    technique = new DeferredBuffer();
                    break;
                case RenderTechniqueType.IBL:
                    technique = new ImageBasedLighting();
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
                case RenderTechniqueType.SSAO:
                    technique = new SSAO();
                    break;
                case RenderTechniqueType.SSLIC:
                    technique = new SSLIC();
                    break;
                default:
                    technique = null;
                    break;
            }

            AddItem(technique);
            return technique;
        }
    }
}
