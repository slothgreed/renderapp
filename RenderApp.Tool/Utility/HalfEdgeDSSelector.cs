using System.Linq;
using KI.Analyzer;
using KI.Asset;
using KI.Foundation.Tree;
using KI.Gfx.GLUtil;
using KI.Mathmatics;
using KI.Renderer;
using OpenTK;
using RenderApp.Model;

namespace RenderApp.Tool.Utility
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
        /// <param name="selectNode">選択形状</param>
        /// <param name="vertex">選択頂点番号</param>
        /// <returns>成功か</returns>
        public static bool PickPoint(Vector2 mouse, ref PolygonNode selectNode, ref HalfEdgeVertex vertex)
        {
            bool select = false;
            float maxInner = -1;
            Vector3 near = Vector3.Zero;
            Vector3 far = Vector3.Zero;
            GetMouseClipPosition(mouse, out near, out far);

            PolygonNode polygonNode = null;
            foreach (SceneNode sceneNode in Workspace.Instance.RenderSystem.ActiveScene.RootNode.AllChildren())
            {
                if (sceneNode is PolygonNode)
                {
                    polygonNode = sceneNode as PolygonNode;
                }
                else
                {
                    continue;
                }

                if (PickPointCore(near, far, polygonNode, ref maxInner, ref vertex))
                {
                    selectNode = polygonNode;
                    select = true;
                }
            }

            return select;
        }

        /// <summary>
        /// 線の選択
        /// </summary>
        /// <param name="mouse">マウス</param>
        /// <param name="selectNode">選択形状</param>
        /// <param name="halfEdge">線情報</param>
        /// <returns>成功したか</returns>
        public static bool PickLine(Vector2 mouse, ref PolygonNode selectNode, ref HalfEdge halfEdge)
        {
            float minLength = float.MaxValue;
            Vector3 near = Vector3.Zero;
            Vector3 far = Vector3.Zero;
            GetMouseClipPosition(mouse, out near, out far);

            PolygonNode polygonNode;
            foreach (SceneNode sceneNode in Workspace.Instance.RenderSystem.ActiveScene.RootNode.AllChildren())
            {
                polygonNode = null;
                if (sceneNode is PolygonNode)
                {
                    polygonNode = sceneNode as PolygonNode;
                }
                else
                {
                    continue;
                }

                if (PickLineCore(near, far, polygonNode, ref minLength, ref halfEdge))
                {
                    selectNode = polygonNode;
                }
            }

            if (selectNode == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ポリゴンごとに行うので、CPUベースで頂点番号を取得
        /// </summary>
        /// <param name="mouse">マウス座標</param>
        /// <param name="selectNode">選択形状</param>
        /// <param name="mesh">選択した三角形</param>
        /// <returns>成功か</returns>
        public static bool PickTriangle(Vector2 mouse, ref PolygonNode selectNode, ref HalfEdgeMesh mesh)
        {
            float minLength = float.MaxValue;
            Vector3 near = Vector3.Zero;
            Vector3 far = Vector3.Zero;
            GetMouseClipPosition(mouse, out near, out far);

            PolygonNode polygonNode;
            foreach (SceneNode sceneNode in Workspace.Instance.RenderSystem.ActiveScene.RootNode.AllChildren())
            {
                polygonNode = null;
                if (sceneNode is PolygonNode)
                {
                    polygonNode = sceneNode as PolygonNode;
                }
                else
                {
                    continue;
                }

                if (PickTriangleCore(near, far, selectNode, ref minLength, ref mesh))
                {
                    selectNode = polygonNode;
                }
            }

            if (selectNode == null)
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
        private static bool CanSelect(PolygonNode selectObject)
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

            GLUtility.GetClickPos(
                Workspace.Instance.RenderSystem.ActiveScene.MainCamera.Matrix,
                Workspace.Instance.RenderSystem.ActiveScene.MainCamera.ProjMatrix,
                viewport, mouse, out near, out far);
        }

        /// <summary>
        /// 頂点選択
        /// </summary>
        /// <param name="near">近クリップ面</param>
        /// <param name="far">遠クリップ面</param>
        /// <param name="selectNode">選択形状</param>
        /// <param name="maxInner">この内積値以上の頂点を取得</param>
        /// <param name="vertex">選択Index</param>
        /// <returns>成功か</returns>
        private static bool PickPointCore(Vector3 near, Vector3 far, PolygonNode selectNode, ref float maxInner, ref HalfEdgeVertex vertex)
        {
            if (!CanSelect(selectNode))
            {
                return false;
            }

#if false

            bool select = false;
            Vector3 crossPos = Vector3.Zero;
            var halfEdgeDS = renderObject.Polygon as HalfEdgeDS;
            foreach (var halfVertex in halfEdgeDS.HalfEdgeVertexs)
            {
                Vector3 point = halfVertex.Position;
                point = Calculator.Multiply(renderObject.ModelMatrix, point);

                if (Interaction.PerpendicularPoint(point, near, far, out crossPos))
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
#else
            bool select = false;
            Vector3 crossPos = Vector3.Zero;
            var halfEdgeDS = selectNode.Polygon as HalfEdgeDS;
            foreach (var halfVertex in halfEdgeDS.HalfEdgeVertexs)
            {
                Vector3 point = halfVertex.Position;
                point = Calculator.Multiply(selectNode.ModelMatrix, point);

                if (Interaction.PerpendicularPoint(point, near, far, out crossPos))
                {
                    Vector3 clickDirection = Vector3.Normalize(far - near);
                    Vector3 pointDirection = Vector3.Normalize(point - near);

                    float inner = Vector3.Dot(clickDirection, pointDirection);

                    // 視線方向の頂点は裏向きなので選択しない。
                    float pointOrient = Vector3.Dot(halfVertex.Normal, clickDirection);
                    if (pointOrient > 0)
                    {
                        continue;
                    }

                    if (maxInner < inner)
                    {
                        maxInner = inner;
                        vertex = halfVertex;
                        select = true;
                    }
                }
            }

            return select;
#endif
        }

        /// <summary>
        /// 線選択
        /// </summary>
        /// <param name="near">近クリップ面</param>
        /// <param name="far">遠クリップ面</param>
        /// <param name="selectNode">選択形状</param>
        /// <param name="minLength">この長さ以下の頂点を取得</param>
        /// <param name="halfEdge">選択Line</param>
        /// <returns></returns>
        private static bool PickLineCore(Vector3 near, Vector3 far, PolygonNode selectNode, ref float minLength, ref HalfEdge halfEdge)
        {
            if (!CanSelect(selectNode))
            {
                return false;
            }

            bool select = false;
            float distance = 0;

            var halfEdgeDS = selectNode.Polygon as HalfEdgeDS;
            if (selectNode.Polygon.Index.Count != 0)
            {
                foreach (var edge in halfEdgeDS.HalfEdges)
                {
                    Vector3 startPos = Calculator.Multiply(selectNode.ModelMatrix, edge.Start.Position);
                    Vector3 endPos = Calculator.Multiply(selectNode.ModelMatrix, edge.End.Position);

                    if (Distance.LineToLine(near, far, startPos, endPos, out distance))
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
        /// <param name="selectNode">選択形状</param>
        /// <param name="minLength">この長さ以下の頂点を取得</param>
        /// <param name="mesh">選択Triangle</param>
        /// <returns>成功か</returns>
        private static bool PickTriangleCore(Vector3 near, Vector3 far, PolygonNode selectNode, ref float minLength, ref HalfEdgeMesh mesh)
        {
            if (!CanSelect(selectNode))
            {
                return false;
            }

            bool select = false;

            var halfEdgeDS = selectNode.Polygon as HalfEdgeDS;
            //頂点配列の時
            if (selectNode.Polygon.Index.Count != 0)
            {
                foreach (var halfMesh in halfEdgeDS.HalfEdgeMeshs)
                {
                    var vertexs = halfMesh.AroundVertex.ToArray();
                    Vector3 multiVertex1 = Calculator.Multiply(selectNode.ModelMatrix, vertexs[0].Position);
                    Vector3 multiVertex2 = Calculator.Multiply(selectNode.ModelMatrix, vertexs[1].Position);
                    Vector3 multiVertex3 = Calculator.Multiply(selectNode.ModelMatrix, vertexs[2].Position);
                    Vector3 result = Vector3.Zero;
                    if (Interaction.TriangleToLine(multiVertex1, multiVertex2, multiVertex3, near, far, ref minLength, out result))
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
