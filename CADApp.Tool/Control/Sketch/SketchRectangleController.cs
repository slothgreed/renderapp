using System.Collections.Generic;
using CADApp.Model;
using CADApp.Model.Assembly;
using CADApp.Model.Node;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Mathmatics;
using KI.Renderer;
using KI.Tool.Control;
using OpenTK;

namespace CADApp.Tool.Control
{
    public class SketchRectangleController : IController
    {
        public enum CreateRectangleMode
        {
            SelectStart,
            SelectEnd
        }

        private CreateRectangleMode mode;

        private SketchNode sketchNode;

        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;
                Vector3 worldPoint;

                if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
                {
                    if (mode == CreateRectangleMode.SelectStart)
                    {
                        Sketch sketch = new Sketch("Line");
                        var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
                        sketchNode = new SketchNode("RectangleLine", sketch, shader);

                        sketch.BeginEdit();
                        sketch.AddVertex(worldPoint);
                        sketch.AddVertex(worldPoint);
                        sketch.AddVertex(worldPoint);
                        sketch.AddVertex(worldPoint);
                        sketch.SetLineIndex(
                            new List<int>()
                            {
                                0,1,
                                1,2,
                                2,3,
                                3,0
                            }
                            );

                        sketch.SetTriangleIndexFromLineIndex(camera.LookAtDirection);
                        sketch.EndEdit();


                        Workspace.Instance.MainScene.AddObject(sketchNode);
                        mode = CreateRectangleMode.SelectEnd;
                    }
                    else
                    {
                        sketchNode = null;
                        mode = CreateRectangleMode.SelectStart;
                    }
                }
            }

            return base.Down(mouse);
        }

        public override bool Move(KIMouseEventArgs mouse)
        {
            if (mode == CreateRectangleMode.SelectEnd)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;

                Vector3 near;
                Vector3 far;
                GLUtility.GetClickPos(camera.Matrix, camera.ProjMatrix, Viewport.Instance.ViewportRect, mouse.Current, out near, out far);

                Vector3 direction = (camera.Position - far).Normalized();
                Vector3 interPoint;
                if (Interaction.PlaneToLine(camera.Position, far, Workspace.Instance.WorkPlane.Formula, out interPoint))
                {
                    Sketch sketch = sketchNode.Sketch;
                    Vector3 startPosition = sketch.GetVertex(0).Position;
                    sketch.BeginEdit();
                    sketch.SetVertex(1, new Vector3(interPoint.X, 0, startPosition.Z));
                    sketch.SetVertex(2, new Vector3(interPoint.X, 0, interPoint.Z));
                    sketch.SetVertex(3, new Vector3(startPosition.X, 0, interPoint.Z));
                    sketch.SetTriangleIndexFromLineIndex(camera.LookAtDirection);
                    sketch.EndEdit();
                }
            }

            return base.Move(mouse);
        }

        public override bool Binding()
        {

            return base.Binding();
        }
    }
}
