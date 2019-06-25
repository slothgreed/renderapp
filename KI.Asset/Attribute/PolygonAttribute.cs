using KI.Gfx;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;

namespace KI.Asset.Attribute
{
    /// <summary>
    /// 標準アトリビュート
    /// </summary>
    public class PolygonAttribute : AttributeBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="vertexBuffer">頂点バッファ</param>
        /// <param name="type">ポリゴン種類</param>
        /// <param name="material">マテリアル</param>
        public PolygonAttribute(string name, VertexBuffer vertexBuffer, KIPrimitiveType type, Material material)
            : base(name, vertexBuffer, type, material)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">ポリゴン種類</param>
        public PolygonAttribute(string name, KIPrimitiveType type)
            : base(name, type)
        {
        }
    }
}
