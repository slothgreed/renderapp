using System.Collections.Generic;
using CADApp.Model;
using CADApp.Model.Node;
using CADApp.Tool.Command;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Mathmatics;
using KI.Tool.Controller;
using OpenTK;

namespace CADApp.Tool.Controller
{
    public class SketchRectangleController : ControllerBase
    {
        public enum CreateRectangleMode
        {
            SelectStart,
            SelectEnd
        }

        private CreateRectangleMode mode;

        private AssemblyNode sketchNode;

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
                        var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
                        sketchNode = new AssemblyNode("RectangleLine", shader);

                        Assembly sketch = new Assembly("Line");
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

                        AddAssemblyNodeCommand command = new AddAssemblyNodeCommand(sketchNode, sketch, Workspace.Instance.MainScene.RootNode);
                        Workspace.Instance.CommandManager.Execute(command);

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
                    Assembly sketch = sketchNode.Assembly;
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
            mode = CreateRectangleMode.SelectStart;

            return base.Binding();
        }
    }
}
