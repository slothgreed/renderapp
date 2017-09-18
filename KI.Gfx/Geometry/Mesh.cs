using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Utility;
using OpenTK;

namespace KI.Gfx.Geometry
{
    /// <summary>
    /// 面クラス
    /// </summary>
    public class Mesh
    {

        /// <summary>
        /// 面積
        /// </summary>
        private float area = 0;

        /// <summary>
        /// エッジ
        /// </summary>
        private List<Line> lines = new List<Line>();

        /// <summary>
        /// 法線
        /// </summary>
        private Vector3 normal = Vector3.Zero;

        /// <summary>
        /// 平面の公式
        /// </summary>
        private Vector4 plane = Vector4.Zero;

        /// <summary>
        /// 重心
        /// </summary>
        private Vector3 gravity = Vector3.Zero;

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


        /// <summary>
        /// 面積
        /// </summary>
        public float Area
        {
            get
            {
                if (area == 0)
                {
                    area = KICalc.Area(lines[0].Start.Position, lines[1].Start.Position, lines[2].Start.Position);
                }

                return area;
            }
        }

        /// <summary>
        /// 法線
        /// </summary>
        public Vector3 Normal
        {
            get
            {
                if (normal == Vector3.Zero)
                {
                    normal = KICalc.Normal(
                        lines[1].Start.Position - lines[0].Start.Position,
                        lines[2].Start.Position - lines[0].Start.Position);
                }

                return normal;
            }
        }

        /// <summary>
        /// 平面の公式
        /// </summary>
        public Vector4 Plane
        {
            get
            {
                if (plane == Vector4.Zero)
                {
                    var positions = lines.Select(p => p.Start.Position).ToArray();
                    plane = KICalc.GetPlaneFormula(positions[0], positions[1], positions[2]);
                }

                return plane;
            }
        }

        /// <summary>
        /// 重心
        /// </summary>
        public Vector3 Gravity
        {
            get
            {
                if (gravity == Vector3.Zero)
                {
                    foreach (var position in lines.Select(p => p.Start.Position))
                    {
                        gravity += position;
                    }

                    gravity /= lines.Count;
                }

                return gravity;
            }
        }

        /// <summary>
        /// 編集したときに呼ぶ
        /// </summary>
        public virtual void Modified()
        {
            area = 0;
            normal = Vector3.Zero;
            plane = Vector4.Zero;
            gravity = Vector3.Zero;
        }
    }
}
