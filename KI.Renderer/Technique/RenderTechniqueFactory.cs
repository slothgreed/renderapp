using System.Collections.Generic;
using System.IO;
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
    public class RenderTechniqueFactory : KIFactoryBase<OffScreenTechnique>
    {
        /// <summary>
        /// シェーダファイルクラス
        /// </summary>
        private class ShaderFile
        {
            public string Vertex;
            public string Frag;
            //public string Geom;
            //public string Tes;
            //public string Tcs;
        }

        /// <summary>
        /// デフォルトシェーダ
        /// </summary>
        private Dictionary<RenderTechniqueType, ShaderFile> DefaultShader;

        /// <summary>
        /// シングルトン
        /// </summary>
        public static RenderTechniqueFactory Instance { get; } = new RenderTechniqueFactory();

        public RenderTechniqueFactory()
        {
            var directory = Global.ShaderDirectory;
            var postEffect = Global.ShaderDirectory + @"\PostEffect";
            var lighthing = Global.ShaderDirectory + @"\Lighthing";
            DefaultShader = new Dictionary<RenderTechniqueType, ShaderFile>();
            DefaultShader.Add(RenderTechniqueType.Shadow,    new ShaderFile() { Vertex = directory  + @"\shadow.vert",      Frag = directory  + @"\shadow.frag" });
            DefaultShader.Add(RenderTechniqueType.IBL,       new ShaderFile() { Vertex = lighthing  + @"\ibl.vert",         Frag = lighthing  + @"\ibl.frag" });
            DefaultShader.Add(RenderTechniqueType.Deferred,  new ShaderFile() { Vertex = lighthing  + @"\defferd.vert",     Frag = lighthing  + @"\defferd.frag" });
            DefaultShader.Add(RenderTechniqueType.Dof,       new ShaderFile() { Vertex = postEffect + @"\dof.vert",         Frag = postEffect + @"\dof.frag" });
            DefaultShader.Add(RenderTechniqueType.Sobel,     new ShaderFile() { Vertex = postEffect + @"\sobel.vert",       Frag = postEffect + @"\sobel.frag" });
            DefaultShader.Add(RenderTechniqueType.SSAO,      new ShaderFile() { Vertex = postEffect + @"\ssao.vert",        Frag = postEffect + @"\ssao.frag" });
            DefaultShader.Add(RenderTechniqueType.Bloom,     new ShaderFile() { Vertex = postEffect + @"\bloom.vert",       Frag = postEffect + @"\bloom.frag" });
            DefaultShader.Add(RenderTechniqueType.SSLIC,     new ShaderFile() { Vertex = postEffect + @"\sslic.vert",       Frag = postEffect + @"\sslic.frag" });
            DefaultShader.Add(RenderTechniqueType.Selection, new ShaderFile() { Vertex = postEffect + @"\selection.vert",   Frag = postEffect + @"\selection.frag" });
            DefaultShader.Add(RenderTechniqueType.Output,    new ShaderFile() { Vertex = postEffect + @"\output.vert",      Frag = postEffect + @"\output.frag" });
        }

        /// <summary>
        /// レンダーテクニックの作成
        /// </summary>
        /// <param name="type">テクニックのタイプ</param>
        /// <returns>レンダーテクニック</returns>
        public OffScreenTechnique CreateRenderTechnique(RenderTechniqueType type)
        {
            OffScreenTechnique technique = null;
            switch (type)
            {
                case RenderTechniqueType.Shadow:
                    technique = new ShadowMap(DefaultShader[RenderTechniqueType.Shadow].Vertex, DefaultShader[RenderTechniqueType.Shadow].Frag);
                    break;
                case RenderTechniqueType.GBuffer:
                    technique = new GBuffer();
                    break;
                case RenderTechniqueType.Deferred:
                    technique = new DeferredRendering(DefaultShader[RenderTechniqueType.Deferred].Vertex, DefaultShader[RenderTechniqueType.Deferred].Frag);
                    break;
                case RenderTechniqueType.IBL:
                    technique = new ImageBasedLighting(DefaultShader[RenderTechniqueType.IBL].Vertex, DefaultShader[RenderTechniqueType.IBL].Frag);
                    break;
                case RenderTechniqueType.Selection:
                    technique = new Selection(DefaultShader[RenderTechniqueType.Selection].Vertex, DefaultShader[RenderTechniqueType.Selection].Frag);
                    break;
                case RenderTechniqueType.Sobel:
                    technique = new Sobel(DefaultShader[RenderTechniqueType.Sobel].Vertex, DefaultShader[RenderTechniqueType.Sobel].Frag);
                    break;
                case RenderTechniqueType.Bloom:
                    technique = new Bloom(DefaultShader[RenderTechniqueType.Bloom].Vertex, DefaultShader[RenderTechniqueType.Bloom].Frag);
                    break;
                case RenderTechniqueType.Output:
                    technique = new OutputBuffer(DefaultShader[RenderTechniqueType.Output].Vertex, DefaultShader[RenderTechniqueType.Output].Frag);
                    break;
                case RenderTechniqueType.SSAO:
                    technique = new SSAO(DefaultShader[RenderTechniqueType.SSAO].Vertex, DefaultShader[RenderTechniqueType.SSAO].Frag);
                    break;
                case RenderTechniqueType.SSLIC:
                    technique = new SSLIC(DefaultShader[RenderTechniqueType.SSLIC].Vertex, DefaultShader[RenderTechniqueType.SSLIC].Frag);
                    break;
                default:
                    break;
            }

            if (technique != null)
            {
                AddItem(technique);
            }

            return technique;
        }
    }
}
