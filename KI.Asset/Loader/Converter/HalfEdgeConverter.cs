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
    class HalfEdgeConverter : IGeometry
    {
        public HalfEdgeConverter(string filePath)
        {
            var half = new HalfEdge();
            HalfEdgeIO.ReadFile(filePath, half);
            CreateGeometryInfo(half);
        }

        public HalfEdgeConverter(HalfEdge half)
        {
            CreateGeometryInfo(half);
        }

        public Geometry[] GeometryInfos
        {
            get;
            private set;
        }

        public void CreateGeometryInfo(HalfEdge half)
        {
            var position = new List<Vector3>();
            var normal = new List<Vector3>();
            var color = new List<Vector3>();
            var index = new List<int>();

            var gray = new Vector3(0.8f);
            foreach (var vertex in half.vertexs)
            {
                position.Add(vertex.Position);
                normal.Add(vertex.Normal);
                color.Add(gray);
            }

            foreach (var mesh in half.meshs)
            {
                foreach (var vertex in mesh.AroundVertex)
                {
                    index.Add(vertex.Index);
                }
            }

            Geometry info = new Geometry(position, normal, color, null, index, Gfx.GLUtil.GeometryType.Triangle);
            GeometryInfos = new Geometry[] { info };
        }
    }
}
