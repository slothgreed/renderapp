using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// 軸
    /// </summary>
    public class Axis : KIObject, IPolygon
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
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public Axis(string name, Vector3 min, Vector3 max)
            : base(name)
        {
            this.min = min;
            this.max = max;
            CreatePolygon();
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Polygon[] Polygons { get; private set; }

        /// <summary>
        /// 軸作成
        /// </summary>
        public void CreatePolygon()
        {
            var position = new List<Vector3>();
            var color = new List<Vector3>();
            var vertexs = new List<Vertex>();
            var lines = new List<Line>();

            vertexs.Add(new Vertex(0, new Vector3(max.X, 0.0f, 0.0f), new Vector3(1, 0, 0)));
            vertexs.Add(new Vertex(1, new Vector3(min.X, 0.0f, 0.0f), new Vector3(1, 0, 0)));
            vertexs.Add(new Vertex(2, new Vector3(0.0f, max.Y, 0.0f), new Vector3(0, 1, 0)));
            vertexs.Add(new Vertex(3, new Vector3(0.0f, min.Y, 0.0f), new Vector3(0, 1, 0)));
            vertexs.Add(new Vertex(4, new Vector3(0.0f, 0.0f, max.Z), new Vector3(0, 0, 1)));
            vertexs.Add(new Vertex(5, new Vector3(0.0f, 0.0f, min.Z), new Vector3(0, 0, 1)));

            lines.Add(new Line(vertexs[0], vertexs[1]));
            lines.Add(new Line(vertexs[2], vertexs[3]));
            lines.Add(new Line(vertexs[4], vertexs[5]));

            Polygon info = new Polygon("Axis", lines);
            Polygons = new Polygon[] { info };
        }
    }
}
