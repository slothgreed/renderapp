using System.Collections.Generic;
using KI.Analyzer;
using KI.Asset;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Control
{
    class DijkstraControl : IControl
    {
        private DijkstraAlgorithm dijkstra;

        private KIObject SelectGeometry;

        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            int vertex_Index = 0;
            Vector3 tri1 = Vector3.Zero;
            Vector3 tri2 = Vector3.Zero;
            Vector3 tri3 = Vector3.Zero;
            RenderObject renderObject = null;
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                var SelectObjectController = ControlManager.Instance.Controllers[ControlManager.CONTROL_MODE.SelectTriangle] as SelectTriangleControl;
                if (SelectObjectController.PickTriangle(leftMouse.Click, ref renderObject, ref vertex_Index))
                {
                    dijkstra.SetGeometry(renderObject.Geometry.HalfEdge as HalfEdge);

                    if (dijkstra.StartIndex == -1)
                    {
                        dijkstra.StartIndex = vertex_Index;
                    }
                    else
                    {
                        dijkstra.EndIndex = vertex_Index;
                    }

                    tri1 = renderObject.Geometry.Position[vertex_Index];
                    tri2 = renderObject.Geometry.Position[vertex_Index + 1];
                    tri3 = renderObject.Geometry.Position[vertex_Index + 2];

                    if (tri1 != Vector3.Zero && tri2 != Vector3.Zero && tri3 != Vector3.Zero)
                    {
                        Vector3 normal = KICalc.Normal(tri1, tri2, tri3);
                        tri1 += normal * 0.01f;
                        tri2 += normal * 0.01f;
                        tri3 += normal * 0.01f;
                        var picking = Global.Scene.FindObject("Picking") as RenderObject;
                        if (picking == null)
                        {
                            RenderObject triangle = RenderObjectFactory.Instance.CreateRenderObject("Picking");
                            Geometry info = new Geometry("Picking", new List<Vector3>() { tri1, tri2, tri3 }, null, KICalc.RandomColor(), null, null, GeometryType.Triangle);
                            triangle.SetGeometryInfo(info);
                            Global.Scene.AddObject(triangle);
                        }
                        else if (picking.Geometry.TriangleNum == 2)
                        {
                            picking.Dispose();
                            //picking.AddVertex(new List<Vector3>() { tri1, tri2, tri3 }, KICalc.RandomColor());
                        }
                        else
                        {
                            //picking.AddVertex(new List<Vector3>() { tri1, tri2, tri3 }, KICalc.RandomColor());
                            dijkstra.Execute();
                        }
                    }
                }
            }

            return true;
        }

        public override bool Binding()
        {
            dijkstra = new DijkstraAlgorithm();
            return true;
        }

        public override bool Execute()
        {
            //if(Dijkstra.CanExecute())
            //{
            //    return Dijkstra.Execute();
            //}
            //return false;
            return false;
        }

        public override bool Reset()
        {
            return dijkstra.Reset();
        }

        /// <summary>
        /// ピッキング終了処理
        /// </summary>
        /// <returns></returns>
        public override bool UnBinding()
        {
            Global.Scene.DeleteObject("Picking");
            return true;
        }

        private void SelectObject(KIObject select)
        {
            SelectGeometry = select;
        }
    }
}
