using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RenderApp;
using RenderApp.Utility;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using RenderApp.Globals;
namespace RenderApp.Control
{
    class DijkstraControl : IControl
    {
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            Vector3 tri1 = Vector3.Zero;
            Vector3 tri2 = Vector3.Zero;
            Vector3 tri3 = Vector3.Zero;
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Scene.ActiveScene.Picking(LeftMouse.Click, ref tri1, ref tri2, ref tri3);
                if (tri1 != Vector3.Zero && tri2 != Vector3.Zero && tri3 != Vector3.Zero)
                {
                    Vector3 normal = CCalc.Normal(tri1, tri2, tri3);
                    tri1 += normal * 0.01f;
                    tri2 += normal * 0.01f;
                    tri3 += normal * 0.01f;
                    var picking = Scene.ActiveScene.FindObject("Picking") as Primitive;
                    if (picking == null)
                    {
                        Primitive triangle = new Primitive("Picking", new List<Vector3>() { tri1, tri2, tri3 }, CCalc.RandomColor(), OpenTK.Graphics.OpenGL.PrimitiveType.Triangles);
                        triangle.MaterialItem = new Material("Picking:Material");
                        triangle.MaterialItem.SetShader(ShaderFactory.Instance.DefaultAnalyzeShader);
                        AssetFactory.Instance.CreateGeometry(triangle);
                    }
                    else if (picking.TriangleNum == 2)
                    {
                        picking.Dispose();
                        picking.AddVertex(new List<Vector3>() { tri1, tri2, tri3 }, CCalc.RandomColor());
                    }
                    else
                    {
                        picking.AddVertex(new List<Vector3>() { tri1, tri2, tri3 }, CCalc.RandomColor());
                    }
                }

            }
            return true;
        }
        /// <summary>
        /// ピッキング終了処理
        /// </summary>
        /// <returns></returns>
        public override bool UnBinding()
        {
            Scene.ActiveScene.DeleteNode("Picking");
            return true;
        }
    }
}
