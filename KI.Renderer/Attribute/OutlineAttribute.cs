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
        public float Offset
        {
            get
            {
                return (float)Shader.GetValue(nameof(Offset));
            }

            set
            {
                Shader.SetValue(nameof(Offset), value);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">レンダリングタイプ</param>
        /// <param name="shader">シェーダ</param>
        public OutlineAttribute(string name, VertexBuffer vertexBuffer, PrimitiveType type, Shader shader) 
            : base(name, type, shader)
        {
            VertexBuffer = vertexBuffer;
            Offset = 0;
        }
    }
}
