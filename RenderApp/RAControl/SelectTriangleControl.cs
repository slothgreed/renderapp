using KI.Asset;
using KI.Foundation.Tree;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using OpenTK;
using RenderApp.Globals;

namespace RenderApp.RAControl
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
        /// <param name="geometry">形状データ</param>
        /// <param name="minLength">この数値以下のポリゴンを取得</param>
        /// <param name="selectIndex">選択したデータの頂点番号</param>
        /// <returns></returns>
        private bool PickTriangleCore(Vector3 near, Vector3 far, Geometry geometry, ref float minLength, ref int selectIndex)
        {
            bool select = false;
            //頂点配列の時
            if (geometry.geometryInfo.Index.Count != 0)
            {
                for (int i = 0; i < geometry.geometryInfo.Index.Count; i += 3)
                {
                    Vector3 vertex1 = geometry.geometryInfo.Position[geometry.geometryInfo.Index[i]];
                    Vector3 vertex2 = geometry.geometryInfo.Position[geometry.geometryInfo.Index[i + 1]];
                    Vector3 vertex3 = geometry.geometryInfo.Position[geometry.geometryInfo.Index[i + 2]];
                    vertex1 = KICalc.Multiply(geometry.ModelMatrix, vertex1);
                    vertex2 = KICalc.Multiply(geometry.ModelMatrix, vertex2);
                    vertex3 = KICalc.Multiply(geometry.ModelMatrix, vertex3);
                    Vector3 result = Vector3.Zero;
                    if (KICalc.CrossPlanetoLinePos(vertex1, vertex2, vertex3, near, far, ref minLength, out result))
                    {
                        selectIndex = geometry.geometryInfo.Index[i];
                        select = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < geometry.geometryInfo.Position.Count / 3; i++)
                {
                    Vector3 vertex1 = KICalc.Multiply(geometry.ModelMatrix, geometry.geometryInfo.Position[3 * i]);
                    Vector3 vertex2 = KICalc.Multiply(geometry.ModelMatrix, geometry.geometryInfo.Position[3 * i + 1]);
                    Vector3 vertex3 = KICalc.Multiply(geometry.ModelMatrix, geometry.geometryInfo.Position[3 * i + 2]);
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
        public bool PickTriangle(Vector2 mouse, ref Geometry selectGeometry, ref int selectIndex)
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

            Scene activeScene = Workspace.SceneManager.ActiveScene;
            KICalc.GetClickPos(
                activeScene.MainCamera.Matrix,
                activeScene.MainCamera.ProjMatrix,
                viewport, mouse, out near, out far);

            Geometry geometry;
            foreach (KINode geometryNode in activeScene.RootNode.AllChildren())
            {
                geometry = null;
                if (geometryNode.KIObject is Geometry)
                {
                    geometry = geometryNode.KIObject as Geometry;
                }
                else
                {
                    continue;
                }

                if (PickTriangleCore(near, far, geometry, ref minLength, ref selectIndex))
                {
                    selectGeometry = geometry;
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
