using KI.Gfx;
using KI.Gfx.Buffer;

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
        public PolygonAttribute(string name, VertexBuffer vertexBuffer, Material material)
            : base(name, vertexBuffer, material)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">ポリゴン種類</param>
        public PolygonAttribute(string name)
            : base(name)
        {
        }
    }
}
