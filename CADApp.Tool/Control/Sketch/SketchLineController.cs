using CADApp.Model;
using CADApp.Model.Assembly;
using CADApp.Model.Node;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Tool.Control;
using OpenTK;

namespace CADApp.Tool.Control
{
    public class SketchLineController : IController
    {
        /// <summary>
        /// 配置するZ位置
        /// </summary>
        private float zPosition = 0;

        SketchNode sketchNode;

        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;
                Vector3 worldPoint;

                if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
                {
                    var sketch = sketchNode.Sketch;
                    int pointIndex = sketch.Vertex.Count;
                    sketch.BeginEdit();
                    sketch.AddVertex(worldPoint);
                    sketchNode.Visible = true;
                    
                    if (sketch.Vertex.Count >= 2)
                    {
                        sketch.AddLineIndex(sketch.Vertex.Count - 2);
                        sketch.AddLineIndex(sketch.Vertex.Count - 1);
                    }

                    sketch.EndEdit();
                }
            }

            return true;
        }

        public override bool DoubleClick(KIMouseEventArgs mouse)
        {
            UnBinding();
            Binding();

            return base.DoubleClick(mouse);
        }

        public override bool Binding()
        {
            Sketch sketch = new Sketch("Sketch");
            var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
            sketchNode = new SketchNode("SketchNode", sketch, shader);
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
