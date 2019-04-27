using CADApp.Model;
using CADApp.Model.Node;
using CADApp.Tool.Command;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Tool.Controller;
using OpenTK;

namespace CADApp.Tool.Controller
{
    public class SketchSplineController : ControllerBase
    {
        private AssemblyNode sketchNode;

        enum SketchSplineMode
        {
            Start,
            Write
        }

        SketchSplineMode mode;

        public override bool Click(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;
                Vector3 worldPoint;

                if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
                {
                    if (mode == SketchSplineMode.Start)
                    {
                        SplineCurvature newSketch = new SplineCurvature("Spline");
                        var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
                        sketchNode = new AssemblyNode("SketchSpline", newSketch, shader);
                        sketchNode.VisibleVertex = false;

                        var command = new AddAssemblyNodeCommand(sketchNode, newSketch, Workspace.Instance.MainScene.RootNode);
                        Workspace.Instance.CommandManager.Execute(command);
                        mode = SketchSplineMode.Write;
                    }

                    if (mode == SketchSplineMode.Write)
                    {
                        var sketch = sketchNode.Assembly as SplineCurvature;
                        sketch.BeginEdit();
                        sketch.AddControlPoint(worldPoint);
                        sketch.EndEdit();
                    }
                }
            }

            return base.Click(mouse);
        }

        public override bool DoubleClick(KIMouseEventArgs mouse)
        {
            UnBinding();
            Binding();

            return base.DoubleClick(mouse);
        }

        public override bool Binding()
        {
            mode = SketchSplineMode.Start;

            return base.Binding();
        }
    }
}
