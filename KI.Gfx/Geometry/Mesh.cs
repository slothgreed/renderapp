using System.Collections.Generic;
using System.Linq;
using KI.Mathmatics;
using OpenTK;

namespace KI.Gfx.Geometry
{
    /// <summary>
    /// 面クラス
    /// </summary>
    public class Mesh
    {
        /// <summary>
        /// エッジ
        /// </summary>
        private List<Line> lines = new List<Line>();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Mesh()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="v0">頂点0</param>
        /// <param name="v1">頂点1</param>
        /// <param name="v2">頂点2</param>
        public Mesh(Vertex v0, Vertex v1, Vertex v2)
        {
            lines.Add(new Line(v0, v1));
            lines.Add(new Line(v1, v2));
            lines.Add(new Line(v2, v0));
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="v0">頂点0</param>
        /// <param name="v1">頂点1</param>
        /// <param name="v2">頂点2</param>
        /// <param name="v3">頂点3</param>
        public Mesh(Vertex v0, Vertex v1, Vertex v2, Vertex v3)
        {
            lines.Add(new Line(v0, v1));
            lines.Add(new Line(v1, v2));
            lines.Add(new Line(v2, v3));
            lines.Add(new Line(v3, v0));
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="line">線分リスト</param>
        public Mesh(List<Line> line)
        {
            lines = line;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="line0">線分0</param>
        /// <param name="line1">線分1</param>
        /// <param name="line2">線分2</param>
        public Mesh(Line line0, Line line1, Line line2)
        {
            lines.Add(line0);
            lines.Add(line1);
            lines.Add(line2);
        }

        /// <summary>
        /// 線分リスト
        /// </summary>
        public List<Line> Lines
        {
            get
            {
                return lines;
            }
        }

        /// <summary>
        /// 頂点リスト
        /// </summary>
        public List<Vertex> Vertexs
        {
            get
            {
                return lines.Select(p => p.Start).ToList();
            }
        }
    }
}
