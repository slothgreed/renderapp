using System.Collections.Generic;
using CADApp.Model;
using CADApp.Model.Node;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Mathmatics;
using KI.Tool.Controller;
using OpenTK;

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
                        Assembly sketch = new Assembly("Line");
                        var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
                        sketchNode = new AssemblyNode("RectangleLine", sketch, shader);

                        Cube cube = new Cube(worldPoint, worldPoint + Vector3.One);
                        sketch.BeginEdit();
                        sketch.SetVertex(cube.Vertex);
                        sketch.SetTriangleIndex(cube.Index);
                        sketch.EndEdit();

                        Workspace.Instance.MainScene.AddObject(sketchNode);
                        mode = BuildCubeMode.SelectSize;
                    }
                    else if (mode == BuildCubeMode.SelectSize)
                    {
                        mode = BuildCubeMode.SelectHeight;
                    }
                    else if (mode == BuildCubeMode.SelectHeight)
                    {
                        sketchNode = null;
                        mode = BuildCubeMode.SelectStart;
                    }
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
                    Vector3 startPosition = sketch.GetVertex(0).Position;
                    if ((startPosition - interPoint).Length <= 0)
                    {
                        return true;
                    }

                    sketch.BeginEdit();
                    for (int i = 1; i < sketch.Vertex.Count; i++)
                    {
                        Vector3 objectDirection = (sketch.Vertex[i].Position - startPosition).Normalized();
                        Vector3 mouseDirection = (interPoint - startPosition).Normalized();
                        float scale = Vector3.Dot(objectDirection, mouseDirection);
                        var xz = Vector3.UnitX + Vector3.UnitZ;
                        sketch.SetVertex(i, startPosition + objectDirection * xz * scale);
                    }

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
                    Vector3 startPosition = sketch.GetVertex(0).Position;

                    sketch.BeginEdit();
                    for (int i = 1; i < sketch.Vertex.Count; i++)
                    {
                        Vector3 objectDirection = (sketch.Vertex[i].Position - startPosition).Normalized();
                        Vector3 mouseDirection = (interPoint - startPosition).Normalized();
                        float scale = Vector3.Dot(objectDirection, mouseDirection);
                        sketch.SetVertex(i, startPosition + objectDirection * scale);
                    }

                    sketch.EndEdit();
                }
            }

            return base.Move(mouse);
        }

        public override bool Binding()
        {
            mode = BuildCubeMode.SelectStart;
            return base.Binding();  
        }
    }
}
