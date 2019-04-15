using System.Collections.Generic;
using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// 立方体
    /// </summary>
    public class Cube
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
            Vertex = new Vector3[8];
            Index = new int[12 * 3]; // 面数 * 三角形の頂点数
            CreateModel();
        }

        public Vector3[] Vertex { get; private set; }

        public int[] Index { get; private set; }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateModel()
        {
            Vertex[0] = new Vector3(min.X, min.Y, min.Z);
            Vertex[1] = new Vector3(max.X, min.Y, min.Z);
            Vertex[2] = new Vector3(max.X, max.Y, min.Z);
            Vertex[3] = new Vector3(min.X, max.Y, min.Z);
            Vertex[4] = new Vector3(min.X, min.Y, max.Z);
            Vertex[5] = new Vector3(max.X, min.Y, max.Z);
            Vertex[6] = new Vector3(max.X, max.Y, max.Z);
            Vertex[7] = new Vector3(min.X, max.Y, max.Z);

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
                AddTrianlgeIndexFromRectangle(0, 0, 3, 2, 1);
                AddTrianlgeIndexFromRectangle(1, 0, 4, 7, 3);
                AddTrianlgeIndexFromRectangle(2, 4, 5, 6, 7);
                AddTrianlgeIndexFromRectangle(3, 1, 2, 6, 5);
                AddTrianlgeIndexFromRectangle(4, 2, 3, 7, 6);
                AddTrianlgeIndexFromRectangle(5, 1, 5, 4, 0);
            }
            else
            {
                AddTrianlgeIndexFromRectangle(0, 3, 0, 1, 2);
                AddTrianlgeIndexFromRectangle(1, 7, 4, 0, 3);
                AddTrianlgeIndexFromRectangle(2, 6, 5, 4, 7);
                AddTrianlgeIndexFromRectangle(3, 2, 1, 5, 6);
                AddTrianlgeIndexFromRectangle(4, 2, 6, 7, 3);
                AddTrianlgeIndexFromRectangle(5, 1, 0, 4, 5);
            }
        }

        /// <summary>
        /// 三角形の頂点配列に値を入れる。(4角形ループで入れて3角形に分解してIndexに格納する)
        /// </summary>
        /// <param name="index">面番号</param>
        /// <param name="vertex0">四角形頂点0</param>
        /// <param name="vertex1">四角形頂点1</param>
        /// <param name="vertex2">四角形頂点2</param>
        /// <param name="vertex3">四角形頂点3</param>
        private void AddTrianlgeIndexFromRectangle(int index, int vertex0, int vertex1, int vertex2, int vertex3)
        {
            index *= 6;
            Index[index]     = vertex0; Index[index + 1] = vertex1; Index[index + 2] = vertex2;
            Index[index + 3] = vertex0; Index[index + 4] = vertex2; Index[index + 5] = vertex3;
        }
    }
}
