using System.Collections.Generic;
using KI.Analyzer;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Renderer;
using KI.Foundation.Controller;
using OpenTK;
using RenderApp.Model;
using RenderApp.Tool.Utility;
using KI.Gfx.KIMaterial;

namespace RenderApp.Tool.Controller
{
    /// <summary>
    /// ダイクストラ用のコントローラ
    /// </summary>
    public class DijkstraController : ControllerBase
    {
        /// <summary>
        /// ダイクストラアルゴリズム
        /// </summary>
        private DijkstraAlgorithm dijkstra;

        /// <summary>
        /// 形状の選択
        /// </summary>
        private PolygonNode selectObject;

        /// <summary>
        /// 選択した形状の頂点1
        /// </summary>
        private HalfEdgeVertex selectStart = null;

        /// <summary>
        /// 選択した形状の頂点2
        /// </summary>
        private HalfEdgeVertex selectEnd = null;

        /// <summary>
        /// マウス押下
        /// </summary>
        /// <param name="mouse">マウス</param>
        /// <returns>成功</returns>
        public override bool Down(KIMouseEventArgs mouse)
        {
            PolygonNode polygonNode = null;
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                HalfEdgeVertex vertex = null;
                if (HalfEdgeDSSelector.PickPoint(mouse.Current, ref polygonNode, ref vertex))
                {
                    // 初回
                    if (selectObject == null)
                    {
                        selectObject = polygonNode;
                        selectStart = vertex;
                    }
                    else if (selectObject != polygonNode)
                    {
                        Workspace.Instance.RenderSystem.ActiveScene.DeleteObject("Picking");
                        // 前回と選択した形状が違う
                        selectObject = polygonNode;
                        selectStart = vertex;
                    }
                    else
                    {
                        // 前回と選択した形状が同じ
                        if (selectStart == null)
                        {
                            selectStart = vertex;
                        }
                        else
                        {
                            selectEnd = vertex;
                        }
                    }

                    Polygon polygon = new Polygon("Picking", new List<Vertex>() { new Vertex(0, vertex.Position, Vector3.UnitY) }, KI.Gfx.KIPrimitiveType.Points);
                    PolygonUtility.Setup(polygon);
                    PolygonNode pointObject = new PolygonNode(polygon);
                    pointObject.ModelMatrix = selectObject.ModelMatrix;
                    Workspace.Instance.RenderSystem.ActiveScene.AddObject(pointObject);

                    if (selectStart != null && selectEnd != null)
                    {
                        Execute();
                        selectObject = null;
                        selectStart = null;
                        selectEnd = null;
                        Workspace.Instance.RenderSystem.ActiveScene.DeleteObject("Picking");
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// ピッキング終了処理
        /// </summary>
        /// <returns>成功</returns>
        public override bool UnBinding()
        {
            Workspace.Instance.RenderSystem.ActiveScene.DeleteObject("Picking");
            Workspace.Instance.RenderSystem.ActiveScene.DeleteObject("DijkstraLine");
            return true;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns>成功</returns>
        private bool Execute()
        {
            dijkstra = new DijkstraAlgorithm(selectObject.Polygon as HalfEdgeDS, selectStart, selectEnd);
            dijkstra.Execute();

            List<Vertex> vertexs;
            List<int> indexs;
            dijkstra.CreateDijkstraLine(out vertexs, out indexs);

            Polygon polygon = new Polygon("DijkstraLine", vertexs, indexs, KIPrimitiveType.Lines);
            Material lineMaterial = new LineMaterial(5);
            PolygonUtility.Setup(polygon);
            PolygonNode lineObject = new PolygonNode(polygon);
            lineObject.ModelMatrix = selectObject.ModelMatrix;
            Workspace.Instance.RenderSystem.ActiveScene.AddObject(lineObject);
            return true;
        }
    }
}
