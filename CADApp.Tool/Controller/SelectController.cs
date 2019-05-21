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
        public SelectMode mode = SelectMode.Geometry;

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
            ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint);
            bool isSelected = false;
            foreach (SceneNode node in scene.RootNode.AllChildren())
            {
                if (node is AssemblyNode)
                {
                    var sketchNode = node as AssemblyNode;
                    sketchNode.Assembly.ClearSelect();
                }
            }

            foreach (SceneNode node in scene.RootNode.AllChildren())
            {
                if (node is AssemblyNode)
                {
                    var sketchNode = node as AssemblyNode;

                    if (mode == SelectMode.Point)
                    {
                        if (sketchNode.Assembly.ControlPoint.Count > 0)
                        {
                            isSelected = SelectControlPoint(sketchNode.Assembly, camera.Position, worldPoint);
                        }
                        else
                        {
                            isSelected = SelectPoint(sketchNode.Assembly, camera.Position, worldPoint);
                        }
                    }

                    if (mode == SelectMode.Triangle)
                    {
                        isSelected = SelectTriangle(sketchNode.Assembly, near, far);
                    }

                    if (mode == SelectMode.Geometry)
                    {
                        isSelected = SelectGeometry(sketchNode.Assembly, camera.Position, worldPoint, near, far);
                    }
                }

                if (isSelected == true)
                {
                    break;
                }

            }

            var rootNode = scene.RootNode as AppRootNode;
            rootNode.UpdateSelectObject();

            return base.Down(mouse);
        }

        public bool SelectGeometry(Assembly assembly, Vector3 cameraPosition, Vector3 clickPosition, Vector3 near, Vector3 far)
        {
            bool isSelected = false;
            if(assembly.ControlPoint.Count > 0)
            {
                isSelected = SelectControlPoint(assembly, cameraPosition, clickPosition);
            }
            else
            {
                isSelected = SelectPoint(assembly, cameraPosition, clickPosition);
            }

            if(isSelected == false)
            {
                isSelected = SelectTriangle(assembly, near, far);
            }

            if (isSelected == true)
            {
                assembly.ClearSelect();
                for (int i = 0; i < assembly.TriangleNum; i++)
                {
                    assembly.AddSelectTriangle(i);
                }
            }

            return isSelected;
        }

        public bool SelectTriangle(Assembly assembly, Vector3 near, Vector3 far)
        {
            bool isSelected = false;
            int selectIndex = -1;
            float minDistance = float.MaxValue;
            Vector3 lookAtDir = (near - far).Normalized();
            for (int i = 0; i < assembly.TriangleNum; i++)
            {
                int triangle0;
                int triangle1;
                int triangle2;
                Vector3 interPoint;
                assembly.GetTriangle(i, out triangle0, out triangle1, out triangle2);
                var normal = Calculator.Normal(
                        assembly.Vertex[triangle0].Position,
                        assembly.Vertex[triangle1].Position,
                        assembly.Vertex[triangle2].Position);

                if (Vector3.Dot(lookAtDir, normal) > 0)
                {
                    if (Interaction.TriangleToLine(
                        assembly.Vertex[triangle0].Position,
                        assembly.Vertex[triangle1].Position,
                        assembly.Vertex[triangle2].Position,
                        near, far, out interPoint))
                    {
                        var distance = (interPoint - near).Length;
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            selectIndex = i;
                            isSelected = true;
                        }
                    }
                }
            }

            if(selectIndex != -1)
            {
                assembly.AddSelectTriangle(selectIndex);
            }

            return isSelected;
        }

        public bool SelectControlPoint(Assembly assembly, Vector3 cameraPosition, Vector3 clickPosition)
        {
            Vector3 clickDirection = clickPosition - cameraPosition;
            clickDirection.Normalize();
            bool isSelect = false;
            float maxInner = 0.999f;
            int selectIndex = -1;

            for (int i = 0; i < assembly.ControlPoint.Count; i++)
            {
                Vector3 pointDirection = assembly.ControlPoint[i].Position - cameraPosition;
                pointDirection.Normalize();

                float dot = Vector3.Dot(clickDirection, pointDirection);

                if (maxInner < dot)
                {
                    maxInner = dot;
                    selectIndex = i;
                    isSelect = true;
                }
            }

            if (selectIndex != -1)
            {
                assembly.AddSelectControlPoint(selectIndex);
            }

            return isSelect;
        }

        public bool SelectPoint(Assembly assembly, Vector3 cameraPosition, Vector3 clickPosition)
        {
            Vector3 clickDirection = clickPosition - cameraPosition;
            clickDirection.Normalize();
            bool isSelect = false;
            float maxInner = 0.999f;
            int selectIndex = -1;

            for (int i = 0; i < assembly.Vertex.Count; i++)
            {
                Vector3 pointDirection = assembly.Vertex[i].Position - cameraPosition;
                pointDirection.Normalize();

                float dot = Vector3.Dot(clickDirection, pointDirection);

                if (maxInner < dot)
                {
                    maxInner = dot;
                    selectIndex = i;
                    isSelect = true;
                }
            }

            if (selectIndex != -1)
            {
                assembly.AddSelectVertex(selectIndex);
            }

            return isSelect;
        }

        public override bool Binding(IControllerArgs args)
        {
            if (args is ControllerArgs)
            {
                var controllerArgs = (ControllerArgs)args;
                if (controllerArgs.Parameter[0] is SelectMode)
                {
                    mode = (SelectMode)controllerArgs.Parameter[0];
                }
            }

            return base.Binding(args);
        }
    }
}
