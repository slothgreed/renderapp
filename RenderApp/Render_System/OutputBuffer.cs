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
        private static string vertexShader = ProjectInfo.ShaderDirectory + @"\Output.vert";
        private static string fragShader = ProjectInfo.ShaderDirectory + @"\Output.frag";

        public OutputBuffer()
            : base("OutputBuffer", vertexShader, fragShader, RenderType.Original)
        {
            Plane = AssetFactory.Instance.CreatePostProcessPlane("OutputBuffer");
            Plane.AddTexture(TextureKind.Normal, SceneManager.Instance.RenderSystem.GBufferStage.FindTexture(TextureKind.Normal));
        }

        public override void Initialize()
        {
            uSelectMap = null;
        }

        internal void SetOutputTarget(TextureKind textureKind, Texture OutputTexture)
        {
            Plane.AddTexture(textureKind, OutputTexture);
        }

        public override void Render()
        {
            //最終出力フレームバッファのバインドの必要なし
            Plane.Shader = Shader;
            Plane.Render();
        }
    }
}
