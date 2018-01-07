﻿using System.Linq;
using KI.Analyzer;
using KI.Foundation.Tree;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Utility
{
    /// <summary>
    /// ハーフエッジの選択関連クラス
    /// </summary>
    public static class HalfEdgeDSSelector
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
        /// <param name="vertex">選択頂点番号</param>
        /// <returns>成功か</returns>
        public static bool PickPoint(Vector2 mouse, ref RenderObject selectObject, ref HalfEdgeVertex vertex)
        {
            bool select = false;
            float minLength = float.MaxValue;
            Vector3 near = Vector3.Zero;
            Vector3 far = Vector3.Zero;
            GetMouseClipPosition(mouse, out near, out far);

            RenderObject renderObject = null;
            foreach (KINode polygonNode in Global.Renderer.ActiveScene.RootNode.AllChildren())
            {
                if (polygonNode.KIObject is RenderObject)
                {
                    renderObject = polygonNode.KIObject as RenderObject;
                }
                else
                {
                    continue;
                }

                if (PickPointCore(near, far, renderObject, ref minLength, ref vertex))
                {
                    selectObject = renderObject;
                    select = true;
                }
            }

            return select;
        }

        /// <summary>
        /// 線の選択
        /// </summary>
        /// <param name="mouse">マウス</param>
        /// <param name="selectObject">選択形状</param>
        /// <param name="halfEdge">線情報</param>
        /// <returns>成功したか</returns>
        public static bool PickLine(Vector2 mouse, ref RenderObject selectObject, ref HalfEdge halfEdge)
        {
            float minLength = float.MaxValue;
            Vector3 near = Vector3.Zero;
            Vector3 far = Vector3.Zero;
            GetMouseClipPosition(mouse, out near, out far);

            RenderObject renderObject;
            foreach (KINode polygonNode in Global.Renderer.ActiveScene.RootNode.AllChildren())
            {
                renderObject = null;
                if (polygonNode.KIObject is RenderObject)
                {
                    renderObject = polygonNode.KIObject as RenderObject;
                }
                else
                {
                    continue;
                }

                if (PickLineCore(near, far, renderObject, ref minLength, ref halfEdge))
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
        /// ポリゴンごとに行うので、CPUベースで頂点番号を取得
        /// </summary>
        /// <param name="mouse">マウス座標</param>
        /// <param name="selectObject">選択形状</param>
        /// <param name="mesh">選択した三角形</param>
        /// <returns>成功か</returns>
        public static bool PickTriangle(Vector2 mouse, ref RenderObject selectObject, ref HalfEdgeMesh mesh)
        {
            float minLength = float.MaxValue;
            Vector3 near = Vector3.Zero;
            Vector3 far = Vector3.Zero;
            GetMouseClipPosition(mouse, out near, out far);

            RenderObject renderObject;
            foreach (KINode polygonNode in Global.Renderer.ActiveScene.RootNode.AllChildren())
            {
                renderObject = null;
                if (polygonNode.KIObject is RenderObject)
                {
                    renderObject = polygonNode.KIObject as RenderObject;
                }
                else
                {
                    continue;
                }

                if (PickTriangleCore(near, far, renderObject, ref minLength, ref mesh))
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
        /// 選択できるか
        /// </summary>
        /// <param name="selectObject">形状</param>
        /// <returns></returns>
        private static bool CanSelect(RenderObject selectObject)
        {
            if (selectObject == null)
            {
                return false;
            }

            if (selectObject.Polygon is HalfEdgeDS)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// マウスでクリックした線分の取得
        /// </summary>
        /// <param name="mouse">マウス</param>
        /// <param name="near">ニア</param>
        /// <param name="far">ファ</param>
        private static void GetMouseClipPosition(Vector2 mouse, out Vector3 near, out Vector3 far)
        {
            int[] viewport = new int[4];
            viewport[0] = 0;
            viewport[1] = 0;
            viewport[2] = DeviceContext.Instance.Width;
            viewport[3] = DeviceContext.Instance.Height;

            KICalc.GetClickPos(
                Global.Renderer.ActiveScene.MainCamera.Matrix,
                Global.Renderer.ActiveScene.MainCamera.ProjMatrix,
                viewport, mouse, out near, out far);
        }

        /// <summary>
        /// 頂点選択
        /// </summary>
        /// <param name="near">近クリップ面</param>
        /// <param name="far">遠クリップ面</param>
        /// <param name="renderObject">選択形状</param>
        /// <param name="minLength">この長さ以下の頂点を取得</param>
        /// <param name="vertex">選択Index</param>
        /// <returns>成功か</returns>
        private static bool PickPointCore(Vector3 near, Vector3 far, RenderObject renderObject, ref float minLength, ref HalfEdgeVertex vertex)
        {
            if (!CanSelect(renderObject))
            {
                return false;
            }

            bool select = false;
            Vector3 crossPos = Vector3.Zero;
            var halfEdgeDS = renderObject.Polygon as HalfEdgeDS;
            foreach (var halfVertex in halfEdgeDS.HalfEdgeVertexs)
            {
                Vector3 point = halfVertex.Position;
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
                            vertex = halfVertex;
                            select = true;
                        }
                    }
                }
            }

            return select;
        }

        /// <summary>
        /// 線選択
        /// </summary>
        /// <param name="near">近クリップ面</param>
        /// <param name="far">遠クリップ面</param>
        /// <param name="renderObject">選択形状</param>
        /// <param name="minLength">この長さ以下の頂点を取得</param>
        /// <param name="halfEdge">選択Line</param>
        /// <returns></returns>
        private static bool PickLineCore(Vector3 near, Vector3 far, RenderObject renderObject, ref float minLength, ref HalfEdge halfEdge)
        {
            if (!CanSelect(renderObject))
            {
                return false;
            }

            bool select = false;
            float distance = 0;

            var halfEdgeDS = renderObject.Polygon as HalfEdgeDS;
            if (renderObject.Polygon.Index.Count != 0)
            {
                foreach (var edge in halfEdgeDS.HalfEdges)
                {
                    Vector3 startPos = KICalc.Multiply(renderObject.ModelMatrix, edge.Start.Position);
                    Vector3 endPos = KICalc.Multiply(renderObject.ModelMatrix, edge.End.Position);

                    if (KICalc.DistanceLineToLine(near, far, startPos, endPos, out distance))
                    {
                        //線分から点までの距離が範囲内の頂点のうち
                        if (distance < THRESHOLD)
                        {
                            //最も視点に近い点を取得
                            if (distance < minLength)
                            {
                                minLength = distance;
                                halfEdge = edge;
                                select = true;
                            }
                        }
                    }
                }
            }

            return select;
        }

        /// <summary>
        /// 三角形選択
        /// </summary>
        /// <param name="near">近クリップ面</param>
        /// <param name="far">遠クリップ面</param>
        /// <param name="renderObject">選択形状</param>
        /// <param name="minLength">この長さ以下の頂点を取得</param>
        /// <param name="mesh">選択Triangle</param>
        /// <returns>成功か</returns>
        private static bool PickTriangleCore(Vector3 near, Vector3 far, RenderObject renderObject, ref float minLength, ref HalfEdgeMesh mesh)
        {
            if (!CanSelect(renderObject))
            {
                return false;
            }

            bool select = false;

            var halfEdgeDS = renderObject.Polygon as HalfEdgeDS;
            //頂点配列の時
            if (renderObject.Polygon.Index.Count != 0)
            {
                foreach (var halfMesh in halfEdgeDS.HalfEdgeMeshs)
                {
                    var vertexs = halfMesh.AroundVertex.ToArray();
                    Vector3 multiVertex1 = KICalc.Multiply(renderObject.ModelMatrix, vertexs[0].Position);
                    Vector3 multiVertex2 = KICalc.Multiply(renderObject.ModelMatrix, vertexs[1].Position);
                    Vector3 multiVertex3 = KICalc.Multiply(renderObject.ModelMatrix, vertexs[2].Position);
                    Vector3 result = Vector3.Zero;
                    if (KICalc.CrossPlanetoLinePos(multiVertex1, multiVertex2, multiVertex3, near, far, ref minLength, out result))
                    {
                        mesh = halfMesh;
                        select = true;
                    }
                }
            }

            return select;
        }
    }
}
