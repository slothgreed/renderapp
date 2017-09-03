using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Gfx.Geometry;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset
{
    /// <summary>
    /// ハーフエッジを独自形式に変換
    /// </summary>
    public class HalfEdgeConverter : IPolygon
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
            halfEdge = new HalfEdgeDS(filePath);
            HalfEdgeIO.ReadFile(filePath, halfEdge);
            CreatePolygon();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="half">ハーフエッジ</param>
        public HalfEdgeConverter(HalfEdgeDS half)
        {
            halfEdge = half;
            CreatePolygon();
        }

        /// <summary>
        /// 形状
        /// </summary>
        public Polygon[] Polygons { get; private set; }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreatePolygon()
        {
            halfEdge.Index[PrimitiveType.Triangles] = new List<int>();
            foreach (var mesh in halfEdge.HalfEdgeMeshs)
            {
                halfEdge.Index[PrimitiveType.Triangles].AddRange(mesh.AroundVertex.Select(p => p.Index));
            }

            Polygons = new Polygon[] { halfEdge };
        }
    }
}
