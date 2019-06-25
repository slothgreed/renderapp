using KI.Gfx;
using OpenTK;

namespace KI.Asset.Primitive
{
    public abstract class PrimitiveBase
    {
        /// <summary>
        /// 頂点情報
        /// </summary>
        public Vector3[] Position { get; protected set; }

        /// <summary>
        /// 法線
        /// </summary>
        public Vector3[] Normal { get; protected set; }

        /// <summary>
        /// 頂点インデクス
        /// </summary>
        public int[] Index { get; protected set; }

        /// <summary>
        /// タイプ
        /// </summary>
        public KIPrimitiveType Type { get; protected set; }
    }
}
