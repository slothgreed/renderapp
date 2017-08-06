using System.Collections.Generic;
using KI.Asset;
using KI.Gfx.GLUtil;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// 立方体
    /// </summary>
    public class Cube : IGeometry
    {
        /// <summary>
        /// 最小値
        /// </summary>
        private Vector3 min;

        /// <summary>
        /// 最大値
        /// </summary>
        private Vector3 max;

        /// <summary>
        /// 立方体の向き
        /// </summary>
        private bool reverse;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <param name="reverse">向き</param>
        public Cube(Vector3 min, Vector3 max, bool reverse)
        {
            this.min = min;
            this.max = max;
            this.reverse = reverse;
        }

        /// <summary>
        /// 形状
        /// </summary>
        public Geometry[] Geometrys { get; private set; }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateGeometry()
        {
            Vector3 v0 = new Vector3(min.X, min.Y, min.Z);
            Vector3 v1 = new Vector3(max.X, min.Y, min.Z);
            Vector3 v2 = new Vector3(max.X, max.Y, min.Z);
            Vector3 v3 = new Vector3(min.X, max.Y, min.Z);
            Vector3 v4 = new Vector3(min.X, min.Y, max.Z);
            Vector3 v5 = new Vector3(max.X, min.Y, max.Z);
            Vector3 v6 = new Vector3(max.X, max.Y, max.Z);
            Vector3 v7 = new Vector3(min.X, max.Y, max.Z);

            List<Vector2> TexCoord = new List<Vector2>()
            {
                Vector2.Zero,
                Vector2.UnitY,
                Vector2.UnitX + Vector2.UnitY,
                Vector2.UnitX
            };

            if (reverse == false)
            {
                Geometrys = new Geometry[6]
                    {
                        new Geometry("Front", new List<Vector3> { v0, v3, v2, v1 }, null, null, TexCoord, null, GeometryType.Quad),
                        new Geometry("Left", new List<Vector3> { v0, v4, v7, v3 }, null, null, TexCoord, null, GeometryType.Quad),
                        new Geometry("Back", new List<Vector3> { v4, v5, v6, v7 }, null, null, TexCoord, null, GeometryType.Quad),
                        new Geometry("Right", new List<Vector3> { v1, v2, v6, v5 }, null, null, TexCoord, null, GeometryType.Quad),
                        new Geometry("Top", new List<Vector3> { v2, v3, v7, v6 }, null, null, TexCoord, null, GeometryType.Quad),
                        new Geometry("Bottom", new List<Vector3> { v1, v5, v4, v0 }, null, null, TexCoord, null, GeometryType.Quad),
                    };
            }
            else
            {
                Geometrys = new Geometry[6]
                    {
                        new Geometry("Front", new List<Vector3> { v3, v0, v1, v2 }, null, null, TexCoord, null, GeometryType.Quad),
                        new Geometry("Left", new List<Vector3> { v7, v4, v0, v3 }, null, null, TexCoord, null, GeometryType.Quad),
                        new Geometry("Back", new List<Vector3> { v6, v5, v4, v7 }, null, null, TexCoord, null, GeometryType.Quad),
                        new Geometry("Right", new List<Vector3> { v2, v1, v5, v6 }, null, null, TexCoord, null, GeometryType.Quad),
                        new Geometry("Top", new List<Vector3> { v2, v6, v7, v3 }, null, null, TexCoord, null, GeometryType.Quad),
                        new Geometry("Bottom", new List<Vector3> { v1, v0, v4, v5 }, null, null, TexCoord, null, GeometryType.Quad),
                    };
            }
        }
    }
}
