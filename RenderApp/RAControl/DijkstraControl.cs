using System.Collections.Generic;
using OpenTK;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Analyzer;
using KI.Asset;
using KI.Gfx.GLUtil;
using RenderApp.Globals;
using KI.Renderer;

namespace RenderApp.RAControl
{
    class DijkstraControl : IControl
    {
        private DijkstraAlgorithm Dijkstra;
        private KIObject SelectGeometry;
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            int vertex_Index = 0;
            Vector3 tri1 = Vector3.Zero;
            Vector3 tri2 = Vector3.Zero;
            Vector3 tri3 = Vector3.Zero;
            Geometry geometry = null;
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                var SelectObjectController = ControlManager.Instance.Controllers[ControlManager.CONTROL_MODE.SelectTriangle] as SelectTriangleControl;
                if (SelectObjectController.PickTriangle(LeftMouse.Click, ref geometry, ref vertex_Index))
                {
                    Dijkstra.SetGeometry(geometry.HalfEdge as HalfEdge);

                    if (Dijkstra.StartIndex == -1)
                    {
                        Dijkstra.StartIndex = vertex_Index;
                    }
                    else
                    {
                        Dijkstra.EndIndex = vertex_Index;
                    }
                    tri1 = geometry.geometryInfo.Position[vertex_Index];
                    tri2 = geometry.geometryInfo.Position[vertex_Index + 1];
                    tri3 = geometry.geometryInfo.Position[vertex_Index + 2];

                    if (tri1 != Vector3.Zero && tri2 != Vector3.Zero && tri3 != Vector3.Zero)
                    {
                        Vector3 normal = KICalc.Normal(tri1, tri2, tri3);
                        tri1 += normal * 0.01f;
                        tri2 += normal * 0.01f;
                        tri3 += normal * 0.01f;
                        var picking = Workspace.SceneManager.ActiveScene.FindObject("Picking") as RenderObject;
                        if (picking == null)
                        {
                            RenderObject triangle = RenderObjectFactory.Instance.CreateRenderObject("Picking");
                            GeometryInfo info = new GeometryInfo(new List<Vector3>() { tri1, tri2, tri3 }, null, KICalc.RandomColor(), null, null, GeometryType.Triangle);
                            triangle.SetGeometryInfo(info);
                            Workspace.SceneManager.ActiveScene.AddObject(triangle);
                        }
                        else if (picking.geometryInfo.TriangleNum == 2)
                        {
                            picking.Dispose();
                            picking.AddVertex(new List<Vector3>() { tri1, tri2, tri3 }, KICalc.RandomColor());
                        }
                        else
                        {
                            picking.AddVertex(new List<Vector3>() { tri1, tri2, tri3 }, KICalc.RandomColor());
                            Dijkstra.Execute();

                        }
                    }
                }
            }
            return true;
        }
        public override bool Binding()
        {
            Dijkstra = new DijkstraAlgorithm();
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
            return Dijkstra.Reset();
        }
        /// <summary>
        /// ピッキング終了処理
        /// </summary>
        /// <returns></returns>
        public override bool UnBinding()
        {
            Workspace.SceneManager.ActiveScene.DeleteObject("Picking");
            return true;
        }
        private void SelectObject(KIObject select)
        {
            SelectGeometry = select;
        }
    }
}
