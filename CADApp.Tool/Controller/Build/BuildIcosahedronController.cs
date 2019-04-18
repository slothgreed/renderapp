using CADApp.Model;
using CADApp.Model.Node;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Tool.Controller;
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
                        mode = BuildIcosahedronMode.SelectSize;
                    }
                    else
                    {
                        UnBinding();
                        Binding();
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
                        var icosahedron = new Icosahedron(distance, 0);
                        sketchNode.Translate = startPoint;
                        sketch.BeginEdit();
                        sketch.SetVertex(icosahedron.Position);
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
            Binding();

            return base.DoubleClick(mouse);
        }

        public override bool Binding()
        {
            mode = BuildIcosahedronMode.SelectStart;

            Assembly sketch = new Assembly("Icosahedron");
            var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
            sketchNode = new AssemblyNode("Icosahedron", sketch, shader);
            sketchNode.Visible = false;
            Workspace.Instance.MainScene.AddObject(sketchNode);
            return base.Binding();
        }

        public override bool UnBinding()
        {

            return base.UnBinding();
        }
    }
}
