using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset.Primitive
{
    /// <summary>
    /// 立方体
    /// </summary>
    public class Cube : PrimitiveBase
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
        /// <param name="storeType">頂点格納方法</param>
        /// <param name="reverse">向き</param>
        public Cube(Vector3 min, Vector3 max, bool reverse = false)
        {
            this.min = min;
            this.max = max;
            this.reverse = reverse;
            CreateVertexArrayCube();
        }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateVertexArrayCube()
        {
            Vertexs = new Vertex[8];
            Index = new int[12 * 3]; // 面数 * 三角形の頂点数

            Vertexs[0] = new Vertex(0, new Vector3(min.X, min.Y, min.Z));
            Vertexs[1] = new Vertex(1, new Vector3(max.X, min.Y, min.Z));
            Vertexs[2] = new Vertex(2, new Vector3(max.X, max.Y, min.Z));
            Vertexs[3] = new Vertex(3, new Vector3(min.X, max.Y, min.Z));
            Vertexs[4] = new Vertex(4, new Vector3(min.X, min.Y, max.Z));
            Vertexs[5] = new Vertex(5, new Vector3(max.X, min.Y, max.Z));
            Vertexs[6] = new Vertex(6, new Vector3(max.X, max.Y, max.Z));
            Vertexs[7] = new Vertex(7, new Vector3(min.X, max.Y, max.Z));

            var center = (max + min) * 0.5f;
            for (int i = 0; i < Vertexs.Length; i++)
            {
                Vertexs[i].Normal = (Vertexs[i].Position - center).Normalized();
            }

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
