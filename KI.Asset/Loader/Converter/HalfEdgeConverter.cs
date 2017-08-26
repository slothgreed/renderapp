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
        private HalfEdgeDS halfEdge;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public HalfEdgeConverter(string filePath)
        {
            halfEdge = new HalfEdgeDS();
            HalfEdgeIO.ReadFile(filePath, halfEdge);
            CreateGeometry();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="half">ハーフエッジ</param>
        public HalfEdgeConverter(HalfEdgeDS half)
        {
            halfEdge = half;
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
            position = halfEdge.Vertexs.Select(p => p.Position).ToList();
            normal = halfEdge.Vertexs.Select(p => p.Normal).ToList();
            color = halfEdge.Vertexs.Select(p => p.Color).ToList();
            foreach (var mesh in halfEdge.Meshs)
            {
                index.AddRange(mesh.AroundVertex.Select(p => p.Index));
            }

            Geometry geometry = new Geometry("HalfEdge", position, normal, color, null, index, Gfx.GLUtil.GeometryType.Triangle);
            geometry.HalfEdgeDS = halfEdge;
            Geometrys = new Geometry[] { geometry };
        }
    }
}
