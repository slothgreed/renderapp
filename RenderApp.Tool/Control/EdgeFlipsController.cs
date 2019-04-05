using System.Linq;
using System.Windows.Forms;
using KI.Analyzer;
using KI.Asset;
using KI.Gfx.GLUtil;
using KI.Tool.Control;
using OpenTK;
using RenderApp.Tool.Utility;

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
        /// マウス押下処理
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                RenderObject renderObject = null;
                HalfEdgeVertex halfEdgeVertex = null;

                if (HalfEdgeDSSelector.PickPoint(mouse.Current, ref renderObject, ref halfEdgeVertex))
                {
                    HalfEdge halfEdge = halfEdgeVertex.AroundEdge.First();
                    halfEdge.Start.Color = Vector3.UnitY;
                    halfEdge.End.Color = Vector3.UnitY;

                    selectObject = renderObject;
                    selectHalfEdge = halfEdge;

                    renderObject.UpdateVertexBufferObject();

                }
            }

            if (mouse.Button == MOUSE_BUTTON.Middle)
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
                    selectObject.UpdateVertexBufferObject();
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
                selectObject.UpdateVertexBufferObject();
            }

            return base.UnBinding();
        }
    }
}
