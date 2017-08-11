using System.Collections.Generic;
using KI.Asset;
using KI.Foundation.Tree;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Control
{
    class SelectPointControl : IControl
    {
        readonly float THRESHOLD = 1.0f;

        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            RenderObject renderObject = null;
            int vertex_Index = 0;

            if (PickPoint(leftMouse.Click, ref renderObject, ref vertex_Index))
            {
                Vector3 pos = renderObject.Geometry.Position[vertex_Index];
                RenderObject point = RenderObjectFactory.Instance.CreateRenderObject("SelectPoint :" + renderObject.Name + ":" + vertex_Index.ToString());
                point.SetGeometryInfo(new Geometry("select", new List<Vector3>() { pos }, null, KICalc.RandomColor(), null, null, GeometryType.Point));
                point.ModelMatrix = renderObject.ModelMatrix;
                Global.RenderSystem.ActiveScene.AddObject(point);
            }

            return true;
        }

        public bool PickPoint(Vector2 mouse, ref RenderObject selectGeometry, ref int selectIndex)
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

            KICalc.GetClickPos(
                Global.RenderSystem.ActiveScene.MainCamera.Matrix,
                Global.RenderSystem.ActiveScene.MainCamera.ProjMatrix,
                viewport, mouse, out near, out far);

            RenderObject renderObject = null;
            foreach (KINode geometryNode in Global.RenderSystem.ActiveScene.RootNode.AllChildren())
            {
                if (geometryNode.KIObject is Geometry)
                {
                    renderObject = geometryNode.KIObject as RenderObject;
                }
                else
                {
                    continue;
                }

                if (PickPointCore(near, far, renderObject, ref minLength, ref selectIndex))
                {
                    selectGeometry = renderObject;
                    select = true;
                }
            }

            return select;
        }

        private bool PickPointCore(Vector3 near, Vector3 far, RenderObject renderObject, ref float minLength, ref int selectIndex)
        {
            bool select = false;
            Vector3 crossPos = Vector3.Zero;
            for (int i = 0; i < renderObject.Geometry.Position.Count; i++)
            {
                Vector3 point = renderObject.Geometry.Position[i];
                point = KICalc.Multiply(renderObject.ModelMatrix, point);

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
