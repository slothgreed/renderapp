using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// グリッド付き平面
    /// </summary>
    public class GridPlane 
    {
        /// <summary>
        /// グリッドの範囲
        /// </summary>
        private float area;

        /// <summary>
        /// グリッドの幅
        /// </summary>
        private float delta;

        /// <summary>
        /// 色
        /// </summary>
        private Vector3 color;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="area">大きさ</param>
        /// <param name="space">間隔</param>
        public GridPlane(float area, float delta, Vector3 color)
        {
            this.area = area;
            this.delta = delta;
            this.color = color;
            CreateModel();
        }

        /// <summary>
        /// 位置情報
        /// </summary>
        public Vector3[] Position { get; private set; }

        /// <summary>
        /// 要素番号
        /// </summary>
        public Vector3[] Color { get; private set; }

        /// <summary>
        /// 要素番号
        /// </summary>
        public int[] Index { get; private set; }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateModel()
        {
            List<Vector3> vertexs = new List<Vector3>();

            float world = area;

            for (float i = -world; i < world; i += delta)
            {
                if (i != 0)
                {
                    vertexs.Add(new Vector3(-world, 0, i));
                    vertexs.Add(new Vector3(world, 0, i));
                    vertexs.Add(new Vector3(i, 0, -world));
                    vertexs.Add(new Vector3(i, 0, world));
                }
            }

            Position = vertexs.ToArray();
            Color = new Vector3[Position.Length];
            for(int i = 0; i < Color.Length; i++)
            {
                Color[i] = new Vector3(color);
            }

            Index = Enumerable.Range(0, vertexs.Count).ToArray();
        }
    }
}
