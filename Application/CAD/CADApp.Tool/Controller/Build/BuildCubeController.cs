using System.Collections.Generic;
using CADApp.Model;
using CADApp.Model.Node;
using CADApp.Tool.Command;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Buffer;
using KI.Mathmatics;
using KI.Foundation.Command;
using KI.Foundation.Controller;
using OpenTK;
using KI.Asset.Primitive;

namespace CADApp.Tool.Controller
{
    public class BuildCubeController : ControllerBase
    {
        public enum BuildCubeMode
        {
            SelectStart,
            SelectSize,
            SelectHeight
        }

        private BuildCubeMode mode;

        private AssemblyNode sketchNode;

        private Vector3 startPosition;

        CommandBase addNodeCommand = null;

        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;
                Vector3 worldPoint;

                if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
                {
                    if (mode == BuildCubeMode.SelectStart)
                    {
                        Assembly sketch = new Assembly("BuildCube");
                        var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointNormalColor);
                        sketchNode = new AssemblyNode("BuildCube", sketch, shader);

                        addNodeCommand = new AddAssemblyNodeCommand(sketchNode, sketch, Workspace.Instance.MainScene.RootNode);
                        Workspace.Instance.CommandManager.Execute(addNodeCommand);
                        startPosition = worldPoint;
                        mode = BuildCubeMode.SelectSize;
                    }
                    else if (mode == BuildCubeMode.SelectSize)
                    {
                        mode = BuildCubeMode.SelectHeight;
                        return base.Down(mouse);
                    }
                }

                // 平面に当たる必要はない。
                if (mode == BuildCubeMode.SelectHeight)
                {
                    sketchNode = null;
                    mode = BuildCubeMode.SelectStart;
                }
            }

            return base.Down(mouse);
        }

        public override bool Move(KIMouseEventArgs mouse)
        {
            if (mode == BuildCubeMode.SelectSize)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;

                Vector3 near;
                Vector3 far;
                GLUtility.GetClickPos(camera.Matrix, camera.ProjMatrix, Viewport.Instance.ViewportRect, mouse.Current, out near, out far);

                Vector3 interPoint;
                if (Interaction.PlaneToLine(camera.Position, far, Workspace.Instance.WorkPlane.Formula, out interPoint))
                {
                    Assembly sketch = sketchNode.Assembly;
                    if ((startPosition - interPoint).Length <= 0)
                    {
                        return true;
                    }

                    interPoint.Y = 0.01f;
                    var cube = new Cube(startPosition, interPoint);
                    sketch.BeginEdit();
                    sketch.ClearVertex();
                    sketch.SetVertex(cube.Vertexs);
                    sketch.SetTriangleIndex(cube.Index);
                    sketch.EndEdit();
                }
            }
            else if (mode == BuildCubeMode.SelectHeight)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;

                Vector3 near;
                Vector3 far;
                GLUtility.GetClickPos(camera.Matrix, camera.ProjMatrix, Viewport.Instance.ViewportRect, mouse.Current, out near, out far);
                Vector3 interPoint;
                Assembly sketch = sketchNode.Assembly;
                var sum = Vector3.Zero;
                foreach (var vertex in sketchNode.Assembly.Vertex)
                {
                    sum += vertex.Position;
                }

                sum.X /= sketchNode.Assembly.Vertex.Count;
                sum.Y /= sketchNode.Assembly.Vertex.Count;
                sum.Z /= sketchNode.Assembly.Vertex.Count;

                Vector3 normal = sum - camera.Position;

                var formula = Plane.Formula(sum, normal);
                if (Interaction.PlaneToLine(camera.Position, far, formula, out interPoint))
                {
                    float distance = startPosition.Y + interPoint.Y;
                    sketch.BeginEdit();
                    for (int i = 0; i < sketch.Vertex.Count; i++)
                    {
                        if (startPosition.Y != sketch.Vertex[i].Position.Y)
                        {
                            Vector3 newPosition = sketch.Vertex[i].Position;
                            newPosition.Y = distance;
                            sketch.SetVertex(i, newPosition);
                        }
                    }

                    sketch.EndEdit();
                }
            }

            return base.Move(mouse);
        }

        public override bool Binding(IControllerArgs args)
        {
            mode = BuildCubeMode.SelectStart;
            return base.Binding(args);  
        }

        public override bool UnBinding()
        {
            if (mode == BuildCubeMode.SelectHeight)
            {
                Workspace.Instance.CommandManager.RemoveCommand(addNodeCommand);
                addNodeCommand = null;
            }

            return base.UnBinding();
        }
    }
}
