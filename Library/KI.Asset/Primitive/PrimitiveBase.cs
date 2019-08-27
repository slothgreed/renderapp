using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset.Primitive
{
    public abstract class PrimitiveBase
    {
        /// <summary>
        /// 頂点インデクス
        /// </summary>
        public int[] Index { get; protected set; }

        /// <summary>
        /// 頂点情報
        /// </summary>
        public Vertex[] Vertexs { get; protected set; }
        
        /// <summary>
        /// タイプ
        /// </summary>
        public KIPrimitiveType Type { get; protected set; }
    }
}
