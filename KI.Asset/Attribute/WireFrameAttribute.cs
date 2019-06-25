using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset.Attribute
{
    /// <summary>
    /// ワイヤフレームのアトリビュート
    /// </summary>
    public class WireFrameAttribute : AttributeBase
    {
        /// <summary>
        /// カラーバッファ
        /// </summary>
        private ArrayBuffer vertexColorBuffer { get; set; }

        /// <summary>
        /// 色
        /// </summary>
        public Vector4 Color { get;  set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">種類</param>
        /// <param name="material">マテリアル</param>
        /// <param name="Color">色情報</param>
        public WireFrameAttribute(string name, VertexBuffer vertexBuffer, Vector4 color)
            : base(name, vertexBuffer, KIPrimitiveType.Triangles, new Material())
        {
            Color = color;

            Material.Shader = ShaderCreater.Instance.CreateShader(SHADER_TYPE.SingleColor);
        }

        public override void Binding()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            foreach (var info in Material.Shader.GetShaderVariable())
            {
                if (info.Name == "u_Color")
                {
                    info.Variable = Color;
                }
            }
        }

        public override void UnBinding()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            base.UnBinding();
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            vertexColorBuffer.Dispose();
            base.Dispose();
        }
    }
}
