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
        /// <param name="polygon">ポリゴン</param>
        /// <param name="shader">シェーダ</param>
        public PolygonAttribute(string name, VertexBuffer vertexBuffer, PolygonType type, Shader shader)
            : base(name, vertexBuffer, type, shader)
        {
        }
    }
}
