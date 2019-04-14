using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CADApp.Model;
using CADApp.Model.Node;
using KI.Asset;
using KI.Gfx.GLUtil;
using KI.Renderer;
using KI.Tool.Controller;
using OpenTK;

namespace CADApp.Tool.Controller
{
    public class SelectController : ControllerBase
    {
        private float VERTEX_DISTANCE_THRESHOLD = 0.1f;

        public override bool Down(KIMouseEventArgs mouse)
        {
            AppRootNode rootNode = Workspace.Instance.MainScene.RootNode as AppRootNode;
            Camera camera = Workspace.Instance.MainScene.MainCamera;
            Vector3 worldPoint;

            if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
            {
                bool isSelected = false;
                foreach (SceneNode node in rootNode.AllChildren().OfType<SceneNode>())
                {
                    if (node is AssemblyNode)
                    {
                        var sketchNode = node as AssemblyNode;
                        int index = 0;
                        if (sketchNode.Assembly.ControlPoint.Count > 0)
                        {
                            foreach (var vertex in sketchNode.Assembly.ControlPoint)
                            {
                                var distance = (vertex.Position - worldPoint).Length;
                                if (distance < VERTEX_DISTANCE_THRESHOLD)
                                {
                                    sketchNode.Assembly.AddSelectControlPoint(index);
                                    isSelected = true;
                                }

                                index++;
                            }
                        }
                        else
                        {
                            foreach (var vertex in sketchNode.Assembly.Vertex)
                            {
                                var distance = (vertex.Position - worldPoint).Length;
                                if (distance < VERTEX_DISTANCE_THRESHOLD)
                                {
                                    sketchNode.Assembly.AddSelectVertex(index);
                                    isSelected = true;
                                }

                                index++;
                            }
                        }
                    }
                }

                if(isSelected)
                {
                    rootNode.UpdateSelectObject();
                }
            }

            return base.Down(mouse);
        }
    }
}
