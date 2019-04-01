using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// グリッド付き平面
    /// </summary>
    public class GridPlane : KIObject, ICreateModel
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
        public GridPlane(string name, float area, float space, Vector3 color)
            : base(name)
        {
            this.area = area;
            this.delta = space;
            this.color = color;
            CreateModel();
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
            List<Vertex> vertexs = new List<Vertex>();
            List<Line> lines = new List<Line>();
            List<Vector3> position = new List<Vector3>();

            Vector3 line_start1 = new Vector3();
            Vector3 line_fin1 = new Vector3();
            Vector3 line_start2 = new Vector3();
            Vector3 line_fin2 = new Vector3();
            float world = area;

            for (float i = -world; i < world; i += delta)
            {
                if (i != 0)
                {
                    line_start1 = new Vector3(-world, 0, i);
                    line_fin1 = new Vector3(world, 0, i);

                    line_start2 = new Vector3(i, 0, -world);
                    line_fin2 = new Vector3(i, 0, world);

                    var start1 = new Vertex(vertexs.Count, line_start1, Vector3.Zero);
                    vertexs.Add(start1);
                    var fin1 = new Vertex(vertexs.Count, line_fin1, Vector3.Zero);
                    vertexs.Add(fin1);

                    var start2 = new Vertex(vertexs.Count, line_start2, Vector3.Zero);
                    vertexs.Add(start2);
                    var fin2 = new Vertex(vertexs.Count, line_fin2, Vector3.Zero);
                    vertexs.Add(fin2);

                    lines.Add(new Line(start1, fin1));
                    lines.Add(new Line(start2, fin2));
                }
            }

            Model = new Polygon("GridPlane", lines);
        }
    }
}
