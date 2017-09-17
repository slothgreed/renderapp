using System.Linq;
using System.Windows.Forms;
using KI.Analyzer;
using KI.Renderer;
using KI.Tool.Utility;

namespace KI.Tool.Control
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
    public class EdgeFlipsControl : IControl
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
        public override bool Down(MouseEventArgs mouse)
        {
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                RenderObject renderObject = null;
                HalfEdgeVertex halfEdgeVertex = null;

                if (HalfEdgeDSSelector.PickPoint(leftMouse.Click, ref renderObject, ref halfEdgeVertex))
                {
                    HalfEdge halfEdge = halfEdgeVertex.AroundEdge.First();
                    halfEdge.Start.IsSelect = true;
                    halfEdge.End.IsSelect = true;

                    selectObject = renderObject;
                    selectHalfEdge = halfEdge;

                    renderObject.Polygon.Update(OpenTK.Graphics.OpenGL.PrimitiveType.Lines);

                }
            }

            if (mouse.Button == MouseButtons.Middle)
            {
                if (selectHalfEdge != null)
                {
                    selectHalfEdge.Start.IsSelect = false;
                    selectHalfEdge.End.IsSelect = false;
                    var halfEdgeDS = selectObject.Polygon as HalfEdgeDS;

                    switch (Mode)
                    {
                        case EdgeEditMode.EdgeCollapse:
                            halfEdgeDS.Editor.EdgeCollapse(selectHalfEdge);
                            break;
                        case EdgeEditMode.EdgeFlips:
                            halfEdgeDS.Editor.EdgeFlips(selectHalfEdge);
                            break;
                        case EdgeEditMode.EdgeSplit:
                            halfEdgeDS.Editor.EdgeSplit(selectHalfEdge);
                            break;
                    }

                    selectHalfEdge = null;
                    selectObject.Polygon.Update(OpenTK.Graphics.OpenGL.PrimitiveType.Lines);
                }
            }

            return base.Down(mouse);
        }

        public override bool UnBinding()
        {
            if (selectHalfEdge != null)
            {
                selectHalfEdge.Start.IsSelect = false;
                selectHalfEdge.End.IsSelect = false;
                selectHalfEdge = null;
                selectObject.Polygon.Update(OpenTK.Graphics.OpenGL.PrimitiveType.Lines);
            }

            return base.UnBinding();
        }
    }
}
