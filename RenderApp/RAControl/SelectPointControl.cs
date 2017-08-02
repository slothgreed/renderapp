using KI.Foundation.Tree;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using OpenTK;
using System.Collections.Generic;
using KI.Asset;
using RenderApp.Globals;
using KI.Renderer;

namespace RenderApp.RAControl
{
    class SelectPointControl : IControl
    {
        readonly float THRESHOLD = 1.0f;
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            Geometry geometry = null;
            int vertex_Index = 0;

            if (PickPoint(LeftMouse.Click, ref geometry, ref vertex_Index))
            {
                Vector3 pos = geometry.geometryInfo.Position[vertex_Index];
                RenderObject point = RenderObjectFactory.Instance.CreateRenderObject("SelectPoint :" + geometry.Name + ":" + vertex_Index.ToString());
                point.SetGeometryInfo(new GeometryInfo(new List<Vector3>() { pos }, null, KICalc.RandomColor(), null, null, GeometryType.Point));
                point.ModelMatrix = geometry.ModelMatrix;
                Workspace.SceneManager.ActiveScene.AddObject(point);
            }

            return true;
        }

        public bool PickPoint(Vector2 mouse, ref Geometry selectGeometry, ref int selectIndex)
        {
            bool select = false;
            float minLength = float.MaxValue;
            Vector3 near = Vector3.Zero;
            Vector3 far = Vector3.Zero;
            int[] viewport = new int[4];
            viewport[0] = 0;
            viewport[1] = 0;
            viewport[2] = DeviceContext.Instance.Width;
            viewport[3] = DeviceContext.Instance.Height;

            Scene activeScene = Workspace.SceneManager.ActiveScene;
            KICalc.GetClickPos(
                activeScene.MainCamera.Matrix,
                activeScene.MainCamera.ProjMatrix,
                viewport, mouse, out near, out far);

            Geometry geometry = null;
            foreach (KINode geometryNode in activeScene.RootNode.AllChildren())
            {
                if (geometryNode.KIObject is Geometry)
                {
                    geometry = geometryNode.KIObject as Geometry;
                }
                else
                {
                    continue;
                }

                if (PickPointCore(near, far, geometry, ref minLength, ref selectIndex))
                {
                    selectGeometry = geometry;
                    select = true;
                }
            }

            return select;
        }

        private bool PickPointCore(Vector3 near, Vector3 far, Geometry geometry, ref float minLength, ref int selectIndex)
        {
            bool select = false;
            Vector3 crossPos = Vector3.Zero;
            for (int i = 0; i < geometry.geometryInfo.Position.Count; i++)
            {
                Vector3 point = geometry.geometryInfo.Position[i];
                point = KICalc.Multiply(geometry.ModelMatrix, point);

                if (KICalc.PerpendicularPoint(point, near, far, out crossPos))
                {
                    //線分から点までの距離が範囲内の頂点のうち
                    if ((crossPos - point).Length < THRESHOLD)
                    {
                        //最も視点に近い点を取得
                        float length = (near - point).Length;
                        if (length < minLength)
                        {
                            minLength = length;
                            selectIndex = i;
                            select = true;
                        }
                    }
                }
            }

            return select;
        }
    }
}
