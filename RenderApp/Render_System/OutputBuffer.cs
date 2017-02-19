using RenderApp.AssetModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.KIAsset;
using KI.Gfx.Render;
namespace RenderApp.Render_System
{
    public partial class OutputBuffer : RenderTechnique
    {
        private Geometry Plane;

        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\Output.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\Output.frag";

        public OutputBuffer(Geometry plane)
            : base("OutputBuffer", vertexShader, fragShader)
        {
            Material.AddTexture(TextureKind.Normal, SceneManager.Instance.RenderSystem.GBufferStage.FindTexture(TextureKind.Normal));
            Plane = plane;
        }

        public override void Initialize()
        {
            uSelectMap = null;
        }

        internal void SetOutputTarget(TextureKind textureKind, Texture OutputTexture)
        {
            Material.AddTexture(textureKind, OutputTexture);
        }
        public override void CreateRenderTarget(int width, int height)
        {
            Texture texture = TextureFactory.Instance.CreateTexture("Texture:" + Name, width, height);
            RenderTarget = new RenderTarget("RenderTarget:" + Name, width, height, texture);
        }
        internal void Render()
        {
            //最終出力フレームバッファのバインドの必要なし
            Plane.MaterialItem = Material;
            Plane.Render();
        }
    }
}
