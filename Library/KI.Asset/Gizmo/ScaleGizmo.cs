using KI.Asset.Primitive;
using KI.Gfx.Geometry;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.Gizmo
{
    /// <summary>
    /// 拡縮用ギズモ
    /// </summary>
    public class ScaleGizmo : GizmoBase
    {
        public ScaleGizmo()
        {
            Cube xAxis = new Cube(new Vector3(0, 0, 0), new Vector3(1, 0.1f, 0.1f));
            Cube yAxis = new Cube(new Vector3(0, 0, 0), new Vector3(0.1f, 1f, 0.1f));
            Cube zAxis = new Cube(new Vector3(0, 0, 0), new Vector3(0.1f, 0.1f, 1f));
            Cube xPoint = new Cube(new Vector3(0.9f, -0.1f, -0.1f), new Vector3(1, 0.1f, 0.1f));
            Cube yPoint = new Cube(new Vector3(-0.1f, 0.9f, -0.1f), new Vector3(0.1f, 1, 0.1f));
            Cube zPoint = new Cube(new Vector3(-0.1f, -0.1f, 0.9f), new Vector3(0.1f, 0.1f, 1));

            List<Vertex> vertex = new List<Vertex>();
            vertex.AddRange(xAxis.Vertexs); vertex.AddRange(yAxis.Vertexs); vertex.AddRange(zAxis.Vertexs);
            vertex.AddRange(xPoint.Vertexs); vertex.AddRange(yPoint.Vertexs); vertex.AddRange(zPoint.Vertexs);

            List<int> index = new List<int>();
            int perCount = xAxis.Vertexs.Length;
            index.AddRange(xAxis.Index);
            index.AddRange(yAxis.Index.Select(x => x + perCount));
            index.AddRange(zAxis.Index.Select(x => x + perCount * 2));
            index.AddRange(xPoint.Index.Select(x => x + perCount * 3));
            index.AddRange(yPoint.Index.Select(x => x + perCount * 4));
            index.AddRange(zPoint.Index.Select(x => x + perCount * 5));

            Vertex = vertex.Select(p => p.Position).ToArray();
            Color = vertex.Select(p => p.Normal).ToArray();
            Index = index.ToArray();
        }
    }
}
