using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// 軸
    /// </summary>
    public class Axis
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
        /// 頂点情報
        /// </summary>
        public Vector3[] Vertex { get; private set; }

        /// <summary>
        /// 色情報
        /// </summary>
        public Vector3[] Color { get; private set; }

        /// <summary>
        /// Lineの要素番号
        /// </summary>
        public int[] Index { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public Axis(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
            CreateModel();
        }

        /// <summary>
        /// 軸作成
        /// </summary>
        public void CreateModel()
        {
            var color = new List<Vector3>();
            var vertexs = new List<Vertex>();
            var lines = new List<int>();

            Vertex = new Vector3[6];
            Color = new Vector3[6];
            Index = new int[6];

            Vertex[0] = new Vector3(max.X, 0.0f, 0.0f);
            Vertex[1] = new Vector3(min.X, 0.0f, 0.0f);
            Vertex[2] = new Vector3(0.0f, max.Y, 0.0f);
            Vertex[3] = new Vector3(0.0f, min.Y, 0.0f);
            Vertex[4] = new Vector3(0.0f, 0.0f, max.Z);
            Vertex[5] = new Vector3(0.0f, 0.0f, min.Z);

            Color[0] = new Vector3(1, 0, 0);
            Color[1] = new Vector3(1, 0, 0);
            Color[2] = new Vector3(0, 1, 0);
            Color[3] = new Vector3(0, 1, 0);
            Color[4] = new Vector3(0, 0, 1);
            Color[5] = new Vector3(0, 0, 1);

            for(int i = 0; i < Index.Length; i++)
            {
                Index[i] = i;
            }
        }
    }
}
