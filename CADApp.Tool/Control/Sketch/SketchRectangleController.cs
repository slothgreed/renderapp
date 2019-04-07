using CADApp.Model;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Mathmatics;
using KI.Renderer;
using KI.Tool.Control;
using OpenTK;

namespace CADApp.Tool.Control.Sketch
{
    public class SketchRectangleController : IController
    {
        PolygonNode lineObject;

        public enum CreateRectangleMode
        {
            SelectStart,
            SelectEnd
        }

        private CreateRectangleMode mode; 


        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;

                Vector3 near;
                Vector3 far;
                GLUtility.GetClickPos(camera.Matrix, camera.ProjMatrix, Viewport.Instance.ViewportRect, mouse.Current, out near, out far);

                Vector3 direction = (camera.Position - far).Normalized();
                Vector3 interPoint;
                if (Interaction.PlaneToLine(camera.Position, far, Workspace.Instance.WorkPlane.Formula, out interPoint))
                {
                    if (mode == CreateRectangleMode.SelectStart)
                    {
                        Polygon line = new Polygon("Line", PolygonType.Lines);
                        line.Vertexs.Add(new Vertex(0, interPoint, Vector3.UnitX));
                        line.Vertexs.Add(new Vertex(1, interPoint, Vector3.UnitX));
                        line.Vertexs.Add(new Vertex(2, interPoint, Vector3.UnitX));
                        line.Vertexs.Add(new Vertex(3, interPoint, Vector3.UnitX));
                        line.Index.Add(0); line.Index.Add(1);
                        line.Index.Add(1); line.Index.Add(2);
                        line.Index.Add(2); line.Index.Add(3);
                        line.Index.Add(3); line.Index.Add(0);

                        var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
                        PolygonNode lineObject = new PolygonNode("RectangleLine", line, shader);
                        Workspace.Instance.MainScene.AddObject(line);
                    }
                    else
                    {

                    }
                }
            }

            return base.Down(mouse);
        }

        public override bool Move(KIMouseEventArgs mouse)
        {
            if (lineObject != null)
            {

            }

            return base.Move(mouse);
        }

        public override bool Binding()
        {

            return base.Binding();
        }
    }
}
