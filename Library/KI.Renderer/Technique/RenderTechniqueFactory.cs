using System.Collections.Generic;
using KI.Asset;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Renderer.Technique;
using KI.Gfx.Render;

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
        GrayScale,
        SSAO,
        SSLIC,
        HUD,
        Selection,
        Output,
        ZPrePass
    }

    /// <summary>
    /// レンダーテクニックのファクトリークラス
    /// </summary>
    public class RenderTechniqueFactory : KIFactoryBase<RenderTechnique>
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
            // post effect 用と その他用は分けたほうが良いかもしれない
            var directory = AssetConstants.ShaderDirectory;
            var postEffect = AssetConstants.ShaderDirectory + @"\PostEffect";
            var lighthing = AssetConstants.ShaderDirectory + @"\Lighthing";
            DefaultShader = new Dictionary<RenderTechniqueType, ShaderFile>();
            DefaultShader.Add(RenderTechniqueType.Shadow,    new ShaderFile() { Vertex = directory  + @"\shadow.vert",      Frag = directory  + @"\shadow.frag" });
            DefaultShader.Add(RenderTechniqueType.IBL,       new ShaderFile() { Vertex = lighthing  + @"\ibl.vert",         Frag = lighthing  + @"\ibl.frag" });
            DefaultShader.Add(RenderTechniqueType.Deferred,  new ShaderFile() { Vertex = lighthing  + @"\defferd.vert",     Frag = lighthing  + @"\defferd.frag" });
            DefaultShader.Add(RenderTechniqueType.Dof,       new ShaderFile() { Vertex = postEffect + @"\dof.vert",         Frag = postEffect + @"\dof.frag" });
            DefaultShader.Add(RenderTechniqueType.GrayScale, new ShaderFile() { Vertex = postEffect + @"\plane.vert",       Frag = postEffect + @"\grayscale.frag"});
            DefaultShader.Add(RenderTechniqueType.Sobel,     new ShaderFile() { Vertex = postEffect + @"\sobel.vert",       Frag = postEffect + @"\sobel.frag" });
            DefaultShader.Add(RenderTechniqueType.SSAO,      new ShaderFile() { Vertex = postEffect + @"\ssao.vert",        Frag = postEffect + @"\ssao.frag" });
            DefaultShader.Add(RenderTechniqueType.Bloom,     new ShaderFile() { Vertex = postEffect + @"\bloom.vert",       Frag = postEffect + @"\bloom.frag" });
            DefaultShader.Add(RenderTechniqueType.SSLIC,     new ShaderFile() { Vertex = postEffect + @"\sslic.vert",       Frag = postEffect + @"\sslic.frag" });
            DefaultShader.Add(RenderTechniqueType.Selection, new ShaderFile() { Vertex = postEffect + @"\selection.vert",   Frag = postEffect + @"\selection.frag" });
            DefaultShader.Add(RenderTechniqueType.Output,    new ShaderFile() { Vertex = postEffect + @"\output.vert",      Frag = postEffect + @"\output.frag" });
        }

        public RenderSystem RendererSystem
        {
            get;
            set;
        }

        /// <summary>
        /// レンダーテクニックの作成
        /// </summary>
        /// <param name="type">テクニックのタイプ</param>
        /// <returns>レンダーテクニック</returns>
        public RenderTechnique CreateRenderTechnique(RenderTechniqueType type, RenderTarget target = null)
        {
            if (RendererSystem == null)
            {
                Logger.Log(Logger.LogLevel.Error, "Set Renderer");
            }

            RenderTechnique technique = null;
            switch (type)
            {
                case RenderTechniqueType.Shadow:
                    technique = new ShadowMap(RendererSystem, DefaultShader[RenderTechniqueType.Shadow].Vertex, DefaultShader[RenderTechniqueType.Shadow].Frag);
                    break;
                case RenderTechniqueType.GBuffer:
                    technique = new GBuffer(RendererSystem);
                    break;
                case RenderTechniqueType.Deferred:
                    technique = new DeferredRendering(RendererSystem, DefaultShader[RenderTechniqueType.Deferred].Vertex, DefaultShader[RenderTechniqueType.Deferred].Frag);
                    break;
                case RenderTechniqueType.IBL:
                    technique = new ImageBasedLighting(RendererSystem, DefaultShader[RenderTechniqueType.IBL].Vertex, DefaultShader[RenderTechniqueType.IBL].Frag);
                    break;
                case RenderTechniqueType.Selection:
                    technique = new Selection(RendererSystem, DefaultShader[RenderTechniqueType.Selection].Vertex, DefaultShader[RenderTechniqueType.Selection].Frag);
                    break;
                case RenderTechniqueType.Sobel:
                    technique = new Sobel(RendererSystem, DefaultShader[RenderTechniqueType.Sobel].Vertex, DefaultShader[RenderTechniqueType.Sobel].Frag);
                    break;
                case RenderTechniqueType.GrayScale:
                    technique = new GrayScale(RendererSystem, DefaultShader[RenderTechniqueType.GrayScale].Vertex, DefaultShader[RenderTechniqueType.GrayScale].Frag);
                    break;
                case RenderTechniqueType.Bloom:
                    technique = new Bloom(RendererSystem, DefaultShader[RenderTechniqueType.Bloom].Vertex, DefaultShader[RenderTechniqueType.Bloom].Frag);
                    break;
                case RenderTechniqueType.Output:
                    technique = new OutputBuffer(RendererSystem, DefaultShader[RenderTechniqueType.Output].Vertex, DefaultShader[RenderTechniqueType.Output].Frag);
                    break;
                case RenderTechniqueType.SSAO:
                    technique = new SSAO(RendererSystem, DefaultShader[RenderTechniqueType.SSAO].Vertex, DefaultShader[RenderTechniqueType.SSAO].Frag);
                    break;
                case RenderTechniqueType.SSLIC:
                    technique = new SSLIC(RendererSystem, DefaultShader[RenderTechniqueType.SSLIC].Vertex, DefaultShader[RenderTechniqueType.SSLIC].Frag);
                    break;
                case RenderTechniqueType.HUD:
                    technique = new HUDTechnique(RendererSystem);
                    break;
                case RenderTechniqueType.ZPrePass:
                    technique = new ZPrepassRender(RendererSystem);
                    break;
                default:
                    break;
            }

            if (technique != null)
            {
                technique.InitializeTechnique();
                AddItem(technique);
            }

            return technique;
        }
    }
}
