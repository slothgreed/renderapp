using CADApp.Model;
using CADApp.Model.Node;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Tool.Control;
using OpenTK;

namespace CADApp.Tool.Control
{
    public class SketchSplineCurvature : IController
    {
        private AssemblyNode sketchNode;

        public override bool Click(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;
                Vector3 worldPoint;

                if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
                {
                    var sketch = sketchNode.Assembly as SplineCurvature;
                    sketch.BeginEdit();
                    sketch.AddControlPoint(worldPoint);
                    sketch.EndEdit();
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
            SplineCurvature sketch = new SplineCurvature("Spline");
            var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
            sketchNode = new AssemblyNode("Spline", sketch, shader);
            sketchNode.VisibleVertex = false;
            Workspace.Instance.MainScene.AddObject(sketchNode);
            return base.Binding();
        }
    }
}
