using KI.Asset;
using KI.Foundation.Core;
using KI.Gfx.Render;
using OpenTK.Graphics.OpenGL;
using System.Runtime.CompilerServices;
using KI.Gfx.KIShader;
using KI.Asset.Primitive;

namespace KI.Renderer.Technique
{
    public abstract class DefferedTechnique : RenderTechnique
    {
        string vertexShader;
        string fragShader;
        public DefferedTechnique(string name, RenderSystem renderSystem, string vertexShader, string fragShader)
            : base(name, renderSystem, false)
        {
            this.vertexShader = vertexShader;
            this.fragShader = fragShader;
        }

        /// <summary>
        ///  遅延描画用のプレーン作成
        /// </summary>
        protected PolygonNode Rectangle { get; private set; }


        /// <summary>
        /// シェーダへ値のセット
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="member">変数</param>
        /// <param name="value">値</param>
        /// <param name="memberName">シェーダ変数名</param>
        protected void SetValue<T>(ref T member, T value, [CallerMemberName]string memberName = "")
        {
            if (Rectangle.Polygon.Material.Shader.SetValue(memberName, value))
            {
                member = value;
            }
            else
            {
                Logger.Log(Logger.LogLevel.Error, "Set Shader Error " + memberName);
            }
        }


        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="renderInfo">レンダリング情報</param>
        public override void Render(Scene scene, RenderInfo renderInfo)
        {
            RenderTarget.ClearBuffer();
            RenderTarget.BindRenderTarget();
            Rectangle.Render(scene, renderInfo);
            RenderTarget.UnBindRenderTarget();
        }

        protected override void CreateRenderTarget()
        {
            var texture = new RenderTexture[] { TextureFactory.Instance.CreateRenderTexture("Texture:" + Name) };
            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget("RenderTarget:" + Name, 1, 1, false);
            RenderTarget.SetRenderTexture(texture);
            var polygon = PolygonUtility.CreatePolygon(Name, new Rectangle());
            Rectangle = new PolygonNode(polygon);
            Rectangle.Polygon.Material.Shader = ShaderFactory.Instance.CreateShaderVF(vertexShader, fragShader);
        }
    }
}
