using KI.Asset;
using KI.Foundation.Tree;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Control
{
    class SelectTriangleControl : IControl
    {
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
            }

            return true;
        }

        /// <summary>
        /// Picking
        /// </summary>
        /// <param name="mouse">マウス座標</param>
        /// <param name="renderObject">形状データ</param>
        /// <param name="minLength">この数値以下のポリゴンを取得</param>
        /// <param name="selectIndex">選択したデータの頂点番号</param>
        /// <returns></returns>
        private bool PickTriangleCore(Vector3 near, Vector3 far, RenderObject renderObject, ref float minLength, ref int selectIndex)
        {
            bool select = false;
            //頂点配列の時
            if (renderObject.Geometry.Index.Count != 0)
            {
                for (int i = 0; i < renderObject.Geometry.Index.Count; i += 3)
                {
                    Vector3 vertex1 = renderObject.Geometry.Position[renderObject.Geometry.Index[i]];
                    Vector3 vertex2 = renderObject.Geometry.Position[renderObject.Geometry.Index[i + 1]];
                    Vector3 vertex3 = renderObject.Geometry.Position[renderObject.Geometry.Index[i + 2]];
                    vertex1 = KICalc.Multiply(renderObject.ModelMatrix, vertex1);
                    vertex2 = KICalc.Multiply(renderObject.ModelMatrix, vertex2);
                    vertex3 = KICalc.Multiply(renderObject.ModelMatrix, vertex3);
                    Vector3 result = Vector3.Zero;
                    if (KICalc.CrossPlanetoLinePos(vertex1, vertex2, vertex3, near, far, ref minLength, out result))
                    {
                        selectIndex = renderObject.Geometry.Index[i];
                        select = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < renderObject.Geometry.Position.Count / 3; i++)
                {
                    Vector3 vertex1 = KICalc.Multiply(renderObject.ModelMatrix, renderObject.Geometry.Position[3 * i]);
                    Vector3 vertex2 = KICalc.Multiply(renderObject.ModelMatrix, renderObject.Geometry.Position[3 * i + 1]);
                    Vector3 vertex3 = KICalc.Multiply(renderObject.ModelMatrix, renderObject.Geometry.Position[3 * i + 2]);
                    Vector3 result = Vector3.Zero;
                    if (KICalc.CrossPlanetoLinePos(vertex1, vertex2, vertex3, near, far, ref minLength, out result))
                    {
                        selectIndex = 3 * i;
                        select = true;
                    }
                }
            }

            return select;
        }

        /// <summary>
        /// ポリゴンごとに行うので、CPUベースで頂点番号を取得
        /// </summary>
        /// <param name="mouse"></param>
        public bool PickTriangle(Vector2 mouse, ref RenderObject selectGeometry, ref int selectIndex)
        {
            selectIndex = -1;
            float minLength = float.MaxValue;
            Vector3 near = Vector3.Zero;
            Vector3 far = Vector3.Zero;
            int[] viewport = new int[4];
            viewport[0] = 0;
            viewport[1] = 0;
            viewport[2] = DeviceContext.Instance.Width;
            viewport[3] = DeviceContext.Instance.Height;

            KICalc.GetClickPos(
                Global.Scene.MainCamera.Matrix,
                Global.Scene.MainCamera.ProjMatrix,
                viewport, mouse, out near, out far);

            RenderObject renderObject;
            foreach (KINode geometryNode in Global.Scene.RootNode.AllChildren())
            {
                renderObject = null;
                if (geometryNode.KIObject is Geometry)
                {
                    renderObject = geometryNode.KIObject as RenderObject;
                }
                else
                {
                    continue;
                }

                if (PickTriangleCore(near, far, renderObject, ref minLength, ref selectIndex))
                {
                    selectGeometry = renderObject;
                }
            }

            if (selectIndex == -1)
            {
                return false;
            }

            return true;
        }
    }
}
