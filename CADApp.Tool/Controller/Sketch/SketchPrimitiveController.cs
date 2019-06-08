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
using KI.Asset.Primitive;

namespace CADApp.Tool.Controller
{
    public class SketchPrimitiveController : ControllerBase
    {
        public enum CreatePrimitiveMode
        {
            SelectStart,
            SelectEnd
        }

        private Vector3 selectStartPoint;
        private CreatePrimitiveMode mode;
        private GeometryType primitiveType;

        private AssemblyNode sketchNode;

        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;
                Vector3 worldPoint;

                if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
                {
                    if (mode == CreatePrimitiveMode.SelectStart)
                    {
                        var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
                        sketchNode = new AssemblyNode("SketchCircle", shader);

                        Assembly sketch = new Assembly("Circle");
                        var circle = new Circle(0.001f, worldPoint, Vector3.UnitY, 10);
                        sketch.BeginEdit();
                        sketch.SetVertex(circle.Position);
                        sketch.SetTriangleIndex(circle.Index);
                        sketch.EndEdit();
                        selectStartPoint = worldPoint;
                        AddAssemblyNodeCommand command = new AddAssemblyNodeCommand(sketchNode, sketch, Workspace.Instance.MainScene.RootNode);
                        Workspace.Instance.CommandManager.Execute(command);

                        mode = CreatePrimitiveMode.SelectEnd;
                    }
                    else
                    {
                        sketchNode = null;
                        mode = CreatePrimitiveMode.SelectStart;
                    }
                }
            }

            return base.Down(mouse);
        }

        public override bool Move(KIMouseEventArgs mouse)
        {
            if (mode == CreatePrimitiveMode.SelectEnd)
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
                    var radius = (interPoint - selectStartPoint).Length;
                    var circle = new Circle(radius, selectStartPoint, Vector3.UnitY, 10);
                    sketch.BeginEdit();
                    sketch.SetVertex(circle.Position);
                    sketch.SetTriangleIndex(circle.Index);
                    sketch.EndEdit();
                }
            }

            return base.Move(mouse);
        }

        public override bool Binding(IControllerArgs args)
        {
            mode = CreatePrimitiveMode.SelectStart;

            if (args is ControllerArgs)
            {
                var controllerArgs = (ControllerArgs)args;
                if (controllerArgs.Parameter[0] is GeometryType)
                {
                    primitiveType = (GeometryType)controllerArgs.Parameter[0];
                }
            }

            return base.Binding(args);
        }
    }
}
