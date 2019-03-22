using System.Collections.Generic;
using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// 立方体
    /// </summary>
    public class Cube : ICreateModel
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
        public Cube(Vector3 min, Vector3 max, bool reverse = false)
        {
            this.min = min;
            this.max = max;
            this.reverse = reverse;
        }

        /// <summary>
        /// 形状
        /// </summary>
        public Polygon Model { get; private set; }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateModel()
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

            var mesh = new List<Mesh>();
            if (reverse == false)
            {
                mesh.Add(CreateMesh("Front" , v0, v3, v2, v1));
                mesh.Add(CreateMesh("Left"  , v0, v4, v7, v3));
                mesh.Add(CreateMesh("Back"  , v4, v5, v6, v7));
                mesh.Add(CreateMesh("Right" , v1, v2, v6, v5));
                mesh.Add(CreateMesh("Top"   , v2, v3, v7, v6));
                mesh.Add(CreateMesh("Bottom", v1, v5, v4, v0));
            }
            else
            {
                mesh.Add(CreateMesh("Front" , v3, v0, v1, v2));
                mesh.Add(CreateMesh("Left"  , v7, v4, v0, v3));
                mesh.Add(CreateMesh("Back"  , v6, v5, v4, v7));
                mesh.Add(CreateMesh("Right" , v2, v1, v5, v6));
                mesh.Add(CreateMesh("Top"   , v2, v6, v7, v3));
                mesh.Add(CreateMesh("Bottom", v1, v0, v4, v5));
            }

            Model = new Polygon("Cube", mesh, PolygonType.Quads);
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
        private Mesh CreateMesh(string name, Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            var front0 = new Vertex(0, vertex0, Vector2.Zero);
            var front1 = new Vertex(1, vertex1, Vector2.UnitY);
            var front2 = new Vertex(2, vertex2, Vector2.UnitX + Vector2.UnitY);
            var front3 = new Vertex(3, vertex3, Vector2.UnitX);
            return new Mesh(front0, front1, front2, front3);
        }
    }
}
