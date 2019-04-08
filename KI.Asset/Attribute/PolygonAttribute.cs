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
        /// <param name="shader">シェーダ</param>
        public PolygonAttribute(string name, VertexBuffer vertexBuffer, PolygonType type, Shader shader)
            : base(name, vertexBuffer, type, shader)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="type">ポリゴン種類</param>
        public PolygonAttribute(string name, PolygonType type)
            : base(name, type)
        {
        }
    }
}
