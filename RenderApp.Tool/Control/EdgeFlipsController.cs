using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using KI.Analyzer;
using KI.Gfx.Geometry;
using KI.Asset;
using RenderApp.Tool.Utility;
using OpenTK;
using KI.Gfx;

namespace RenderApp.Tool.Control
{
    /// <summary>
    /// エッジ編集モード
    /// </summary>
    public enum EdgeEditMode
    {
        EdgeCollapse,
        EdgeFlips,
        EdgeSplit
    }

    /// <summary>
    /// エッジフリップ確認用
    /// </summary>
    public class EdgeFlipsController : IController
    {
        /// <summary>
        /// 選択形状
        /// </summary>
        private RenderObject selectObject = null;

        /// <summary>
        /// 選択エッジ
        /// </summary>
        private HalfEdge selectHalfEdge = null;

        /// <summary>
        /// エッジ編集モード
        /// </summary>
        public EdgeEditMode Mode { get; private set; } = EdgeEditMode.EdgeCollapse;

        /// <summary>
        /// 頂点の選択
        /// </summary>
        private List<HalfEdge> selectVertex;

        /// <summary>
        /// マウス押下処理
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Down(MouseEventArgs mouse)
        {
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                RenderObject renderObject = null;
                HalfEdgeVertex halfEdgeVertex = null;

                if (HalfEdgeDSSelector.PickPoint(leftMouse.Click, ref renderObject, ref halfEdgeVertex))
                {
                    HalfEdge halfEdge = halfEdgeVertex.AroundEdge.First();
                    halfEdge.Start.Color = Vector3.UnitY;
                    halfEdge.End.Color = Vector3.UnitY;

                    selectObject = renderObject;
                    selectHalfEdge = halfEdge;

                    renderObject.Polygon.UpdateVertexArray();

                }
            }

            if (mouse.Button == MouseButtons.Middle)
            {
                if (selectHalfEdge != null)
                {
                    var halfEdgeDS = selectObject.Polygon as HalfEdgeDS;

                    switch (Mode)
                    {
                        case EdgeEditMode.EdgeCollapse:
                            halfEdgeDS.Editor.EdgeCollapse(selectHalfEdge,(selectHalfEdge.Start.Position + selectHalfEdge.End.Position) / 2);
                            break;
                        case EdgeEditMode.EdgeFlips:
                            halfEdgeDS.Editor.EdgeFlips(selectHalfEdge);
                            break;
                        case EdgeEditMode.EdgeSplit:
                            halfEdgeDS.Editor.EdgeSplit(selectHalfEdge);
                            break;
                    }

                    selectHalfEdge = null;
                    selectObject.Polygon.UpdateVertexArray();
                }
            }

            return base.Down(mouse);
        }

        /// <summary>
        /// コントローラの終了
        /// </summary>
        /// <returns>成功</returns>
        public override bool UnBinding()
        {
            if (selectHalfEdge != null)
            {
                selectHalfEdge.Start.Color = Vector3.Zero;
                selectHalfEdge.End.Color = Vector3.Zero;
                selectHalfEdge = null;
                selectObject.Polygon.UpdateVertexArray();
            }

            return base.UnBinding();
        }
    }
}
