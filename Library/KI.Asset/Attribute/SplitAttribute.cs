using KI.Gfx;
using KI.Gfx.Buffer;

namespace KI.Asset.Attribute
{
    /// <summary>
    /// ディスプレイスメントマップ
    /// </summary>
    public class SplitAttribute : AttributeBase
    {
        public int uOuter
        {
            get
            {
                return (int)Material.Shader.GetValue(nameof(uOuter));
            }

            set
            {
                Material.Shader.SetValue(nameof(uOuter), value);
            }
        }

        public int uInner
        {
            get
            {
                return (int)Material.Shader.GetValue(nameof(uInner));
            }

            set
            {
                Material.Shader.SetValue(nameof(uInner), value);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="vertexBuffer">線分</param>
        /// <param name="material">マテリアル</param>
        public SplitAttribute(string name, VertexBuffer vertexBuffer, Material material)
            : base(name, vertexBuffer, material)
        {
            uOuter = 2;
            uInner = 2;

            vertexBuffer.Type = KIPrimitiveType.Patches;
        }
    }
}
