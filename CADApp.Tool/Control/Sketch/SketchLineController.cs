using System.Windows.Forms;
using CADApp.Model;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Mathmatics;
using KI.Tool.Control;
using OpenTK;

namespace CADApp.Tool.Control
{
    public class SketchLineController : IController
    {
        /// <summary>
        /// 配置するZ位置
        /// </summary>
        private float zPosition = 0;

        RenderObject pointObject;

        RenderObject lineObject;

        public override bool Move(MouseEventArgs mouse)
        {
            if (mouse.Button == MouseButtons.Left)
            {
                Vector2 clickPos = new Vector2(mouse.X, mouse.Y);
                Camera camera = Workspace.Instance.MainScene.MainCamera;

                Vector3 near;
                Vector3 far;
                GLUtility.GetClickPos(camera.Matrix, camera.ProjMatrix, Viewport.Instance.ViewportRect, clickPos, out near, out far);

                Vector3 direction = (camera.Position - far).Normalized();
                Vector3 interPoint;
                if (Interaction.PlaneToLine(camera.Position, far, Workspace.Instance.WorkPlane.Formula, out interPoint))
                {
                    pointObject.Visible = true;
                    pointObject.Polygon.Vertexs.Add(new Vertex(0, interPoint, Vector3.UnitX));
                    pointObject.UpdateVertexBufferObject();
                }
            }

            return true;
        }

        public override bool Click(MouseEventArgs mouse)
        {
            return base.Click(mouse);
        }

        public override bool Binding()
        {
            Polygon point = new Polygon("Point");
            Polygon line = new Polygon("Line", PolygonType.Lines);
            var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
            pointObject = new RenderObject("Point", point, shader);
            pointObject.Visible = false;
            Workspace.Instance.MainScene.AddObject(pointObject);

            lineObject = new RenderObject("Line", line, shader);
            lineObject.Visible = false;
            Workspace.Instance.MainScene.AddObject(lineObject);
            return base.Binding();
        }

        public override bool UnBinding()
        {
            return base.UnBinding();
        }
    }
}
