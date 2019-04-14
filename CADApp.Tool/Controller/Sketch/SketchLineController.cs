using CADApp.Model;
using CADApp.Model.Node;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Tool.Controller;
using OpenTK;

namespace CADApp.Tool.Controller
{
    public class SketchLineController : ControllerBase
    {
        /// <summary>
        /// 配置するZ位置
        /// </summary>
        private float zPosition = 0;

        AssemblyNode sketchNode;

        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;
                Vector3 worldPoint;

                if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
                {
                    var sketch = sketchNode.Assembly;
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
            Assembly sketch = new Assembly("Sketch");
            var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
            sketchNode = new AssemblyNode("SketchNode", sketch, shader);
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
