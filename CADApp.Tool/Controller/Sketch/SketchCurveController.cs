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
    public class SketchCurveController : ControllerBase
    {
        private AssemblyNode sketchNode;

        enum SketchSplineMode
        {
            Start,
            Write
        }

        SketchSplineMode mode;

        GeometryType curvatureType;

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
                        CurveAssembly newSketch = null;
                        if (curvatureType == GeometryType.Spline)
                        {
                            newSketch = new SplineAssembly("SketchSpline");
                        }
                        else
                        {
                            var bezier = new Model.BezierAssembly("SketchBezier");
                            newSketch = bezier;
                        }

                        var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
                        sketchNode = new AssemblyNode(newSketch.Name, newSketch, shader);
                        sketchNode.VisibleVertex = false;

                        var command = new AddAssemblyNodeCommand(sketchNode, newSketch, Workspace.Instance.MainScene.RootNode);
                        Workspace.Instance.CommandManager.Execute(command);
                        mode = SketchSplineMode.Write;
                    }

                    if (mode == SketchSplineMode.Write)
                    {
                        var sketch = sketchNode.Assembly;
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
            Binding(ControllerArgs);

            return base.DoubleClick(mouse);
        }

        public override bool Binding(IControllerArgs args)
        {
            mode = SketchSplineMode.Start;

            if(args is ControllerArgs)
            {
                var controllerArgs = (ControllerArgs)args;
                if (controllerArgs.Parameter[0] is GeometryType)
                {
                    curvatureType = (GeometryType)controllerArgs.Parameter[0];
                }
            }

            return base.Binding(args);
        }
    }
}
