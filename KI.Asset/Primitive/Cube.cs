using System.Collections.Generic;
using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// 立方体
    /// </summary>
    public class Cube : IPolygon
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
        public Polygon[] Polygons { get; private set; }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreatePolygon()
        {
            Vector3 v0 = new Vector3(min.X, min.Y, min.Z);
            Vector3 v1 = new Vector3(max.X, min.Y, min.Z);
            Vector3 v2 = new Vector3(max.X, max.Y, min.Z);
            Vector3 v3 = new Vector3(min.X, max.Y, min.Z);
            Vector3 v4 = new Vector3(min.X, min.Y, max.Z);
            Vector3 v5 = new Vector3(max.X, min.Y, max.Z);
            Vector3 v6 = new Vector3(max.X, max.Y, max.Z);
            Vector3 v7 = new Vector3(min.X, max.Y, max.Z);

            List<Vector2> texCoord = new List<Vector2>()
            {
                Vector2.Zero,
                Vector2.UnitY,
                Vector2.UnitX + Vector2.UnitY,
                Vector2.UnitX
            };

            if (reverse == false)
            {
                var front = CreatePolygon("Front", v0, v3, v2, v1);
                var left = CreatePolygon("Left", v0, v4, v7, v3);
                var back = CreatePolygon("Back", v4, v5, v6, v7);
                var right = CreatePolygon("Right", v1, v2, v6, v5);
                var top = CreatePolygon("Top", v2, v3, v7, v6);
                var bottom = CreatePolygon("Bottom", v1, v5, v4, v0);

                Polygons = new Polygon[6]
                    {
                        front,
                        left,
                        back,
                        right,
                        top,
                        bottom,
                    };
            }
            else
            {
                var front = CreatePolygon("Front", v3, v0, v1, v2);
                var left = CreatePolygon("Left", v7, v4, v0, v3);
                var back = CreatePolygon("Back", v6, v5, v4, v7);
                var right = CreatePolygon("Right", v2, v1, v5, v6);
                var top = CreatePolygon("Top", v2, v6, v7, v3);
                var bottom = CreatePolygon("Bottom", v1, v0, v4, v5);

                Polygons = new Polygon[6]
                    {
                        front,
                        left,
                        back,
                        right,
                        top,
                        bottom,
                    };
            }
        }

        /// <summary>
        /// 形状生成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="vertex0">頂点0</param>
        /// <param name="vertex1">頂点1</param>
        /// <param name="vertex2">頂点2</param>
        /// <param name="vertex3">頂点3</param>
        /// <returns>形状</returns>
        private Polygon CreatePolygon(string name, Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            var front0 = new Vertex(0, vertex0, Vector2.Zero);
            var front1 = new Vertex(1, vertex1, Vector2.UnitY);
            var front2 = new Vertex(2, vertex2, Vector2.UnitX + Vector2.UnitY);
            var front3 = new Vertex(3, vertex3, Vector2.UnitX);
            List<Mesh> mesh = new List<Mesh>();
            mesh.Add(new Mesh(front0, front1, front2, front3));

            return new Polygon(name, mesh, PolygonType.Quads);
        }
    }
}
