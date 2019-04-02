using OpenTK;

namespace KI.Gfx.Geometry
{
    /// <summary>
    /// 三角形クラス
    /// </summary>
    public class Triangle
    {
        /// <summary>
        /// 法線
        /// </summary>
        private Vector3 normal = Vector3.Zero;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vertex0">頂点1</param>
        /// <param name="vertex1">頂点2</param>
        /// <param name="vertex2">頂点3</param>
        public Triangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2)
        {
            Vertex0 = vertex0;
            Vertex1 = vertex1;
            Vertex2 = vertex2;
        }

        /// <summary>
        /// 頂点1
        /// </summary>
        public Vector3 Vertex0 { get; private set; }

        /// <summary>
        /// 頂点2
        /// </summary>
        public Vector3 Vertex1 { get; private set; }

        /// <summary>
        /// 頂点3
        /// </summary>
        public Vector3 Vertex2 { get; private set; }

        /// <summary>
        /// 法線
        /// </summary>
        public Vector3 Normal
        {
            get
            {
                if (normal == Vector3.Zero)
                {
                    return KI.Mathmatics.Geometry.Normal(Vertex1 - Vertex0, Vertex2 - Vertex0);
                }
                else
                {
                    return normal;
                }
            }
        }
    }
}
