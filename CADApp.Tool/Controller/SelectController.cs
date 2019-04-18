using CADApp.Model;
using CADApp.Model.Node;
using KI.Asset;
using KI.Gfx.GLUtil;
using KI.Mathmatics;
using KI.Renderer;
using KI.Tool.Controller;
using OpenTK;

namespace CADApp.Tool.Controller
{
    public class SelectController : ControllerBase
    {
        private float VERTEX_DISTANCE_THRESHOLD = 0.01f;

        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button != MOUSE_BUTTON.Left)
            {
                return true;
            }

            Scene scene = Workspace.Instance.MainScene;
            Camera camera = Workspace.Instance.MainScene.MainCamera;
            Vector3 worldPoint;

            Vector3 near;
            Vector3 far;
            GLUtility.GetClickPos(camera.Matrix, camera.ProjMatrix, Viewport.Instance.ViewportRect, mouse.Current, out near, out far);

            if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
            {
                bool isSelected = false;
                foreach (SceneNode node in scene.RootNode.AllChildren())
                {
                    if (node is AssemblyNode)
                    {
                        var sketchNode = node as AssemblyNode;
                        sketchNode.Assembly.ClearSelect();

                        if (sketchNode.Assembly.ControlPoint.Count > 0)
                        {
                            for (int i = 0; i < sketchNode.Assembly.ControlPoint.Count; i++)
                            {
                                var distance = (sketchNode.Assembly.ControlPoint[i].Position - worldPoint).Length;
                                if (distance < VERTEX_DISTANCE_THRESHOLD)
                                {
                                    sketchNode.Assembly.AddSelectControlPoint(i);
                                    isSelected = true;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < sketchNode.Assembly.Vertex.Count; i++)
                            {
                                var distance = (sketchNode.Assembly.Vertex[i].Position - worldPoint).Length;
                                if (distance < VERTEX_DISTANCE_THRESHOLD)
                                {
                                    sketchNode.Assembly.AddSelectVertex(i);
                                    isSelected = true;
                                }
                            }
                        }

                        for(int i = 0; i < sketchNode.Assembly.LineNum; i++)
                        {
                            int start;
                            int end;
                            sketchNode.Assembly.GetLine(i, out start, out end);

                            if (Interaction.LineToLine(near, far,
                                sketchNode.Assembly.Vertex[start].Position,
                                sketchNode.Assembly.Vertex[end].Position,
                                VERTEX_DISTANCE_THRESHOLD))
                            {
                                sketchNode.Assembly.AddSelectLine(i);
                                isSelected = true;
                            }
                        }
                        
                        for (int i = 0; i < sketchNode.Assembly.TriangleNum; i++)
                        {
                            int triangle0;
                            int triangle1;
                            int triangle2;
                            Vector3 interPoint;
                            var sketch = sketchNode.Assembly;
                            sketchNode.Assembly.GetTriangle(i, out triangle0, out triangle1, out triangle2);

                            if (Interaction.TriangleToLine(
                                sketch.Vertex[triangle0].Position,
                                sketch.Vertex[triangle1].Position,
                                sketch.Vertex[triangle2].Position,
                                near, far, out interPoint
                                ))
                            {
                                sketchNode.Assembly.AddSelectTriangle(i);
                                isSelected = true;
                            }
                        }
                    }
                }

                if(isSelected)
                {
                    var rootNode = scene.RootNode as AppRootNode;
                    rootNode.UpdateSelectObject();
                }
            }

            return base.Down(mouse);
        }
    }
}
