using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.Gizmo
{
    /// <summary>
    /// 平行移動用ギズモ
    /// </summary>
    public class MoveGizmo : GizmoBase
    {
        public MoveGizmo()
        {
            Create();
        }

        public void Create()
        {
            Cube xAxis = new Cube(new Vector3(0, 0, 0), new Vector3(1, 0.1f, 0.1f));
            Cube yAxis = new Cube(new Vector3(0, 0, 0), new Vector3(0.1f, 1f, 0.1f));
            Cube zAxis = new Cube(new Vector3(0, 0, 0), new Vector3(0.1f, 0.1f, 1f));
            Cube xPoint = new Cube(new Vector3(0.9f, -0.1f, -0.1f), new Vector3(1, 0.1f, 0.1f));
            Cube yPoint = new Cube(new Vector3(-0.1f, 0.9f, -0.1f), new Vector3(0.1f, 1, 0.1f));
            Cube zPoint = new Cube(new Vector3(-0.1f, -0.1f, 0.9f), new Vector3(0.1f, 0.1f, 1));

            List<Vector3> vertex = new List<Vector3>();
            vertex.AddRange(xAxis.Position); vertex.AddRange(yAxis.Position); vertex.AddRange(zAxis.Position);
            vertex.AddRange(xPoint.Position); vertex.AddRange(yPoint.Position); vertex.AddRange(zPoint.Position);

            List<Vector3> normal = new List<Vector3>();
            normal.AddRange(xAxis.Normal); normal.AddRange(yAxis.Normal); normal.AddRange(zAxis.Normal);
            normal.AddRange(xPoint.Normal); normal.AddRange(yPoint.Normal); normal.AddRange(zPoint.Normal);

            List<int> index = new List<int>();
            int perCount = xAxis.Index.Length;
            index.AddRange(xAxis.Index);
            index.AddRange(yAxis.Index.Select(x => x + perCount));
            index.AddRange(zAxis.Index.Select(x => x + perCount * 2));
            index.AddRange(xPoint.Index.Select(x => x + perCount * 3));
            index.AddRange(yPoint.Index.Select(x => x + perCount * 4));
            index.AddRange(zPoint.Index.Select(x => x + perCount * 5));

            Vertex = vertex.ToArray();
            Color = normal.ToArray();
            Index = index.ToArray();

        }
    }
}
