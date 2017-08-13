using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.KIMath;
using KI.Foundation.Tree;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Utility
{
    /// <summary>
    /// 選択関連のクラス
    /// </summary>
    public static class Selector
    {
        /// <summary>
        /// 選択範囲閾値
        /// </summary>
        private static readonly float THRESHOLD = 1.0f;
        
        /// <summary>
        /// 頂点の選択
        /// </summary>
        /// <param name="mouse">マウス座標</param>
        /// <param name="selectObject">選択形状</param>
        /// <param name="selectIndex">選択頂点番号</param>
        /// <returns>成功か</returns>
        public static bool PickPoint(Vector2 mouse, ref RenderObject selectObject, ref int selectIndex)
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
                if (geometryNode.KIObject is RenderObject)
                {
                    renderObject = geometryNode.KIObject as RenderObject;
                }
                else
                {
                    continue;
                }

                if (PickPointCore(near, far, renderObject, ref minLength, ref selectIndex))
                {
                    selectObject = renderObject;
                    select = true;
                }
            }

            return select;
        }

        /// <summary>
        /// ポリゴンごとに行うので、CPUベースで頂点番号を取得
        /// </summary>
        /// <param name="mouse">マウス座標</param>
        /// <param name="selectObject">選択形状</param>
        /// <param name="triangle">選択した三角形</param>
        /// <returns>成功か</returns>
        public static bool PickTriangle(Vector2 mouse, ref RenderObject selectObject, ref Triangle triangle)
        {
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

            RenderObject renderObject;
            foreach (KINode geometryNode in Global.RenderSystem.ActiveScene.RootNode.AllChildren())
            {
                renderObject = null;
                if (geometryNode.KIObject is RenderObject)
                {
                    renderObject = geometryNode.KIObject as RenderObject;
                }
                else
                {
                    continue;
                }

                if (PickTriangleCore(near, far, renderObject, ref minLength, ref triangle))
                {
                    selectObject = renderObject;
                }
            }

            if (selectObject == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 頂点選択
        /// </summary>
        /// <param name="near">近クリップ面</param>
        /// <param name="far">遠クリップ面</param>
        /// <param name="renderObject">選択形状</param>
        /// <param name="minLength">この長さ以下の頂点を取得</param>
        /// <param name="triangle">選択Triangle</param>
        /// <returns>成功か</returns>
        private static bool PickTriangleCore(Vector3 near, Vector3 far, RenderObject renderObject, ref float minLength, ref Triangle triangle)
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
                    Vector3 multiVertex1 = KICalc.Multiply(renderObject.ModelMatrix, vertex1);
                    Vector3 multiVertex2 = KICalc.Multiply(renderObject.ModelMatrix, vertex2);
                    Vector3 multiVertex3 = KICalc.Multiply(renderObject.ModelMatrix, vertex3);
                    Vector3 result = Vector3.Zero;
                    if (KICalc.CrossPlanetoLinePos(multiVertex1, multiVertex2, multiVertex3, near, far, ref minLength, out result))
                    {
                        triangle = new Triangle(vertex1, vertex2, vertex3);
                        select = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < renderObject.Geometry.Position.Count / 3; i++)
                {
                    Vector3 vertex1 = renderObject.Geometry.Position[3 * i];
                    Vector3 vertex2 = renderObject.Geometry.Position[3 * i + 1];
                    Vector3 vertex3 = renderObject.Geometry.Position[3 * i + 2];
                    Vector3 multiVertex1 = KICalc.Multiply(renderObject.ModelMatrix, vertex1);
                    Vector3 multiVertex2 = KICalc.Multiply(renderObject.ModelMatrix, vertex2);
                    Vector3 multiVertex3 = KICalc.Multiply(renderObject.ModelMatrix, vertex3);
                    Vector3 result = Vector3.Zero;
                    if (KICalc.CrossPlanetoLinePos(multiVertex1, multiVertex2, multiVertex3, near, far, ref minLength, out result))
                    {
                        triangle = new Triangle(vertex1, vertex2, vertex3);
                        select = true;
                    }
                }
            }

            return select;
        }

        /// <summary>
        /// 頂点選択
        /// </summary>
        /// <param name="near">近クリップ面</param>
        /// <param name="far">遠クリップ面</param>
        /// <param name="renderObject">選択形状</param>
        /// <param name="minLength">この長さ以下の頂点を取得</param>
        /// <param name="selectIndex">選択Index</param>
        /// <returns>成功か</returns>
        private static bool PickPointCore(Vector3 near, Vector3 far, RenderObject renderObject, ref float minLength, ref int selectIndex)
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
