using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer.Attribute
{
    /// <summary>
    /// 輪郭線のアトリビュート
    /// </summary>
    public class OutlineAttribute : AttributeBase
    {
        public float uOffset
        {
            get
            {
                return (float)Shader.GetValue(nameof(uOffset));
            }

            set
            {
                Shader.SetValue(nameof(uOffset), value);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">レンダリングタイプ</param>
        /// <param name="shader">シェーダ</param>
        public OutlineAttribute(string name, VertexBuffer vertexBuffer, PrimitiveType type, Shader shader)
            : base(name, vertexBuffer, type, shader)
        {
            uOffset = 0.5f;
        }
    }
}
