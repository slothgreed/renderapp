using System.Collections.Generic;
using KI.Analyzer;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Renderer;
using KI.Tool.Control;
using OpenTK;
using RenderApp.Model;
using RenderApp.Tool.Utility;

namespace RenderApp.Tool.Control
{
    /// <summary>
    /// ダイクストラ用のコントローラ
    /// </summary>
    public class DijkstraController : IController
    {
        /// <summary>
        /// ダイクストラアルゴリズム
        /// </summary>
        private DijkstraAlgorithm dijkstra;

        /// <summary>
        /// 形状の選択
        /// </summary>
        private RenderObject selectObject;

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
            RenderObject renderObject = null;
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                HalfEdgeVertex vertex = null;
                if (HalfEdgeDSSelector.PickPoint(mouse.Current, ref renderObject, ref vertex))
                {
                    // 初回
                    if (selectObject == null)
                    {
                        selectObject = renderObject;
                        selectStart = vertex;
                    }
                    else if (selectObject != renderObject)
                    {
                        Workspace.Instance.RenderSystem.ActiveScene.DeleteObject("Picking");
                        // 前回と選択した形状が違う
                        selectObject = renderObject;
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

                    Polygon polygon = new Polygon("Picking", new List<Vertex>() { new Vertex(0, vertex.Position, Vector3.UnitY) });
                    RenderObject pointObject = RenderObjectFactory.Instance.CreateRenderObject("Picking", polygon);
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

            Polygon polygon = new Polygon("DijkstraLine", vertexs, indexs, PolygonType.Lines);
            RenderObject lineObject = RenderObjectFactory.Instance.CreateRenderObject("DijkstraLine", polygon);
            lineObject.ModelMatrix = selectObject.ModelMatrix;
            Workspace.Instance.RenderSystem.ActiveScene.AddObject(lineObject);
            return true;
        }
    }
}
