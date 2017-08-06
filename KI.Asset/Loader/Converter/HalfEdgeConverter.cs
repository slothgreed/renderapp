using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Analyzer;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// ハーフエッジを独自形式に変換
    /// </summary>
    public class HalfEdgeConverter : IGeometry
    {
        /// <summary>
        /// ハーフエッジ
        /// </summary>
        private HalfEdge half;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public HalfEdgeConverter(string filePath)
        {
            half = new HalfEdge();
            HalfEdgeIO.ReadFile(filePath, half);
            CreateGeometry();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="half">ハーフエッジ</param>
        public HalfEdgeConverter(HalfEdge half)
        {
            CreateGeometry();
        }

        /// <summary>
        /// 形状
        /// </summary>
        public Geometry[] Geometrys { get; private set; }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateGeometry()
        {
            var position = new List<Vector3>();
            var normal = new List<Vector3>();
            var color = new List<Vector3>();
            var index = new List<int>();

            var gray = new Vector3(0.8f);
            foreach (var vertex in half.Vertexs)
            {
                position.Add(vertex.Position);
                normal.Add(vertex.Normal);
                color.Add(gray);
            }

            foreach (var mesh in half.Meshs)
            {
                foreach (var vertex in mesh.AroundVertex)
                {
                    index.Add(vertex.Index);
                }
            }

            Geometry info = new Geometry("HalfEdge", position, normal, color, null, index, Gfx.GLUtil.GeometryType.Triangle);
            Geometrys = new Geometry[] { info };
        }
    }
}
