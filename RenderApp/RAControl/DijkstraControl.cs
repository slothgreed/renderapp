using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RenderApp;
using RenderApp.Utility;
using RenderApp.AssetModel;
using RenderApp.AssetModel.RA_Geometry;
using RenderApp.Globals;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.Analyzer;

namespace RenderApp.RA_Control
{
    class DijkstraControl : IControl
    {
        private Dijkstra Dijkstra;
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
                var SelectObjectController = ControlManager.Instance.Controllers[ControlManager.CONTROL_MODE.SelectObject] as SelectObjectControl;
                if (SelectObjectController.Picking(LeftMouse.Click, ref geometry, ref vertex_Index))
                {
                    if(Dijkstra.Geometry == null)
                    {
                        Dijkstra.Geometry = geometry.GeometryInfo;
                    }
                    if(Dijkstra.StartIndex == -1)
                    {
                        Dijkstra.StartIndex = vertex_Index;
                    }
                    else
                    {
                        Dijkstra.EndIndex = vertex_Index;
                    }
                    tri1 = geometry.GeometryInfo.Position[vertex_Index];
                    tri2 = geometry.GeometryInfo.Position[vertex_Index + 1];
                    tri3 = geometry.GeometryInfo.Position[vertex_Index + 2];

                    if (tri1 != Vector3.Zero && tri2 != Vector3.Zero && tri3 != Vector3.Zero)
                    {
                        Vector3 normal = KICalc.Normal(tri1, tri2, tri3);
                        tri1 += normal * 0.01f;
                        tri2 += normal * 0.01f;
                        tri3 += normal * 0.01f;
                        var picking = SceneManager.Instance.ActiveScene.FindObject("Picking") as RenderObject;
                        if (picking == null)
                        {
                            RenderObject triangle = AssetFactory.Instance.CreateRenderObject("Picking");
                            triangle.CreatePC(new List<Vector3>() { tri1, tri2, tri3 }, KICalc.RandomColor(), OpenTK.Graphics.OpenGL.PrimitiveType.Triangles);
                            AssetFactory.Instance.CreateGeometry(triangle);
                        }
                        else if (picking.GeometryInfo.TriangleNum == 2)
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
            Dijkstra = new Dijkstra();
            return true;
        }
        public override bool Execute()
        {
            if(Dijkstra.CanExecute())
            {
                return Dijkstra.Execute();
            }
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
            SceneManager.Instance.ActiveScene.DeleteNode("Picking");
            return true;
        }
        private void SelectObject(KIObject select)
        {
            SelectGeometry = select;
        }
    }
}
