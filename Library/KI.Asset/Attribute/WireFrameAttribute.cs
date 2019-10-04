using KI.Gfx;
using KI.Gfx.Buffer;
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
        /// 色のバッキングフィールド
        /// </summary>
        private Vector4 color;

        /// <summary>
        /// 色
        /// </summary>
        public Vector4 Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    Material.Shader.SetValue("u_Color", color);
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">種類</param>
        /// <param name="material">マテリアル</param>
        /// <param name="Color">色情報</param>
        public WireFrameAttribute(string name, VertexBuffer vertexBuffer, Vector4 color)
            : base(name, vertexBuffer, new Material())
        {
            Material.Shader = ShaderCreater.Instance.CreateShader(SHADER_TYPE.SingleColor);
            Color = color;
        }

        public override void Binding()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        }

        public override void UnBinding()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            base.UnBinding();
        }
    }
}
