using CADApp.Model;
using CADApp.Model.Node;
using CADApp.Tool.Command;
using KI.Asset;
using KI.Asset.Primitive;
using KI.Gfx.Buffer;
using KI.Foundation.Controller;
using OpenTK;


namespace CADApp.Tool.Controller
{
    public class BuildIcosahedronController : ControllerBase
    {
        AssemblyNode sketchNode;

        Vector3 startPoint;

        public enum BuildIcosahedronMode
        {
            SelectStart,
            SelectSize
        }

        private BuildIcosahedronMode mode;

        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;
                Vector3 worldPoint;

                if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
                {
                    if (mode == BuildIcosahedronMode.SelectStart)
                    {
                        startPoint = worldPoint;
                        Assembly sketch = new Assembly("BuildIcosahedron");
                        var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointNormalColor);
                        sketchNode = new AssemblyNode("BuildIcosahedron", sketch, shader);
                        sketchNode.Visible = false;

                        var addNodeCommand = new AddAssemblyNodeCommand(sketchNode, sketch, Workspace.Instance.MainScene.RootNode);
                        Workspace.Instance.CommandManager.Execute(addNodeCommand);

                        mode = BuildIcosahedronMode.SelectSize;
                    }
                    else
                    {
                        UnBinding();
                        Binding(ControllerArgs);
                    }
                }
            }

            return true;
        }

        public override bool Move(KIMouseEventArgs mouse)
        {
            if (mode == BuildIcosahedronMode.SelectSize)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;
                Vector3 worldPoint;
                if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
                {
                    var distance = (startPoint - worldPoint).Length;
                    if (distance > 0)
                    {
                        var sketch = sketchNode.Assembly;
                        var icosahedron = new Icosahedron(distance, 20, startPoint);
                        sketch.BeginEdit();
                        sketch.SetVertex(icosahedron.Vertexs);
                        sketch.SetTriangleIndex(icosahedron.Index);
                        sketchNode.Visible = true;
                        sketch.EndEdit();
                    }
                }
            }

            return base.Move(mouse);
        }

        public override bool DoubleClick(KIMouseEventArgs mouse)
        {
            UnBinding();
            Binding(ControllerArgs);

            return base.DoubleClick(mouse);
        }

        public override bool Binding(IControllerArgs args)
        {
            mode = BuildIcosahedronMode.SelectStart;

            return base.Binding(args);
        }

        public override bool UnBinding()
        {

            return base.UnBinding();
        }
    }
}
