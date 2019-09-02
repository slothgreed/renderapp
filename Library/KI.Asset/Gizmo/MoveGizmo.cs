using KI.Asset.Primitive;
using KI.Gfx.Geometry;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

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
            Cone xArrow = new Cone(1, 1, 16);
            Cone yArrow = new Cone(1, 1, 16);
            Cone zArrow = new Cone(1, 1, 16);

            PrimitiveUtility.Rotate(xArrow, Vector3.UnitZ, -90);
            PrimitiveUtility.Move(xArrow, Vector3.UnitX);

            PrimitiveUtility.Move(yArrow, Vector3.UnitY);

            PrimitiveUtility.Rotate(zArrow, Vector3.UnitX, 90);
            PrimitiveUtility.Move(zArrow, Vector3.UnitZ);


            List<Vertex> vertex = new List<Vertex>();
            vertex.AddRange(xAxis.Vertexs); vertex.AddRange(yAxis.Vertexs); vertex.AddRange(zAxis.Vertexs);
            vertex.AddRange(xArrow.Vertexs); vertex.AddRange(yArrow.Vertexs); vertex.AddRange(zArrow.Vertexs);



            List<int> index = new List<int>();
            int cubePerCount = xAxis.Vertexs.Length;
            index.AddRange(xAxis.Index);
            index.AddRange(yAxis.Index.Select(x => x + cubePerCount));
            index.AddRange(zAxis.Index.Select(x => x + cubePerCount * 2));

            int conePerCount = xArrow.Vertexs.Length;
            index.AddRange(xArrow.Index.Select(x => x + cubePerCount * 3));
            index.AddRange(yArrow.Index.Select(x => x + cubePerCount * 3 + conePerCount));
            index.AddRange(zArrow.Index.Select(x => x + cubePerCount * 3 + conePerCount * 2));

            Vertex = vertex.Select(p => p.Position).ToArray();
            Color = vertex.Select(p => p.Normal).ToArray();
            Index = index.ToArray();

        }
    }
}
