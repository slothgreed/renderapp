using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Asset;
using KI.Foundation.Core;
using KI.Foundation.KIMath;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Renderer;
using KI.Tool.Utility;
using OpenTK;

namespace KI.Tool.Control
{
    /// <summary>
    /// ダイクストラ用のコントローラ
    /// </summary>
    public class DijkstraControl : IControl
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
        private int selectStartIndex = -1;

        /// <summary>
        /// 選択した形状の頂点2
        /// </summary>
        private int selectEndIndex = -1;

        /// <summary>
        /// マウス押下
        /// </summary>
        /// <param name="mouse">マウス</param>
        /// <returns>成功</returns>
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            RenderObject renderObject = null;
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                var vertex_Index = 0;
                if (Selector.PickPoint(leftMouse.Click, ref renderObject, ref vertex_Index))
                {
                    // 初回
                    if (selectObject == null)
                    {
                        selectObject = renderObject;
                        selectStartIndex = vertex_Index;
                    }
                    else if (selectObject != renderObject)
                    {
                        Global.RenderSystem.ActiveScene.DeleteObject("Picking");
                        // 前回と選択した形状が違う
                        selectObject = renderObject;
                        selectStartIndex = vertex_Index;
                    }
                    else
                    {
                        // 前回と選択した形状が同じ
                        if (selectStartIndex == -1)
                        {
                            selectStartIndex = vertex_Index;
                        }
                        else
                        {
                            selectEndIndex = vertex_Index;
                        }
                    }

                    Vector3 vertex = renderObject.Geometry.Position[vertex_Index];
                    RenderObject point = RenderObjectFactory.Instance.CreateRenderObject("Picking");
                    Geometry geometry = new Geometry("Picking", new List<Vector3>() { vertex }, null, KICalc.RandomColor(), null, null, GeometryType.Point);
                    point.SetGeometryInfo(geometry);
                    point.ModelMatrix = selectObject.ModelMatrix;
                    Global.RenderSystem.ActiveScene.AddObject(point);

                    if (selectStartIndex != -1 && selectEndIndex != -1)
                    {
                        Execute();
                        selectObject = null;
                        selectStartIndex = -1;
                        selectEndIndex = -1;
                        Global.RenderSystem.ActiveScene.DeleteObject("Picking");
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
            Global.RenderSystem.ActiveScene.DeleteObject("Picking");
            return true;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns>成功</returns>
        private bool Execute()
        {
            dijkstra = new DijkstraAlgorithm(selectObject.Geometry.HalfEdgeDS, selectStartIndex, selectEndIndex);
            dijkstra.Execute();

            RenderObject lines = RenderObjectFactory.Instance.CreateRenderObject("DijkstraLine");
            Geometry geometry = new Geometry("DijkstraLine", dijkstra.DijkstraLine(), null, Vector3.UnitZ, null, null, GeometryType.Line);
            lines.SetGeometryInfo(geometry);
            lines.ModelMatrix = selectObject.ModelMatrix;
            Global.RenderSystem.ActiveScene.AddObject(lines);
            return true;
        }
    }
}
