using KI.Gfx;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;

namespace KI.Asset.Attribute
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
                return (float)Material.Shader.GetValue(nameof(uOffset));
            }

            set
            {
                Material.Shader.SetValue(nameof(uOffset), value);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">レンダリングタイプ</param>
        /// <param name="material">マテリアル</param>
        public OutlineAttribute(string name, VertexBuffer vertexBuffer, KIPrimitiveType type, Material material)
            : base(name, vertexBuffer, type, material)
        {
            uOffset = 0.5f;
        }
    }
}
