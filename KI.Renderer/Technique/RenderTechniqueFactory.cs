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
        public static RenderTechniqueFactory Instance { get; } =  new RenderTechniqueFactory();

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

        /// <summary>
        /// 指定したレンダーテクニックのテクスチャを取得
        /// </summary>
        /// <param name="type">テクニックのタイプ</param>
        /// <returns>出力テクスチャ</returns>
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

        /// <summary>
        /// 全ての出力テクスチャの取得
        /// </summary>
        /// <returns>出力テクスチャ</returns>
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
