using CADApp.Model;
using CADApp.Model.Node;
using CADApp.Tool.Command;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Foundation.Controller;
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

        enum SketchLineMode
        {
            Start,
            Write
        }

        SketchLineMode mode;

        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;
                Vector3 worldPoint;

                if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
                {
                    if (mode == SketchLineMode.Start)
                    {
                        Assembly newSketch = new Assembly("Sketch");
                        var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
                        sketchNode = new AssemblyNode("SketchLine", newSketch, shader);
                        sketchNode.Visible = false;
                        var command = new AddAssemblyNodeCommand(sketchNode, newSketch, Workspace.Instance.MainScene.RootNode);
                        Workspace.Instance.CommandManager.Execute(command);
                        mode = SketchLineMode.Write;
                    }
                    
                    if(mode == SketchLineMode.Write)
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
            }

            return true;
        }

        public override bool DoubleClick(KIMouseEventArgs mouse)
        {
            UnBinding();
            Binding(ControllerArgs);

            return base.DoubleClick(mouse);
        }

        public override bool Binding(IControllerArgs args)
        {
            mode = SketchLineMode.Start;

            return base.Binding(args);
        }

        public override bool UnBinding()
        {
            return base.UnBinding();
        }
    }
}
