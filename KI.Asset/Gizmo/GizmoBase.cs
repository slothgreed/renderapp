using OpenTK;

namespace KI.Asset.Gizmo
{
    /// <summary>
    /// Gizmo のベースクラス
    /// </summary>
    public class GizmoBase
    {
        /// <summary>
        /// 頂点情報
        /// </summary>
        public Vector3[] Vertex { get; protected set; }

        /// <summary>
        /// 色情報
        /// </summary>
        public Vector3[] Color { get; protected set; }

        /// <summary>
        /// 要素番号
        /// </summary>
        public int[] Index { get; protected set; }
    }
}
