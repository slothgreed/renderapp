using System.Linq;
using System.Windows.Forms;
using KI.Analyzer;
using KI.Renderer;
using KI.Tool.Utility;

namespace KI.Tool.Control
{
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

            if (mouse.Button == MouseButtons.Right)
            {
                if (selectHalfEdge != null)
                {
                    selectHalfEdge.Start.IsSelect = false;
                    selectHalfEdge.End.IsSelect = false;
                    var halfEdgeDS = selectObject.Polygon as HalfEdgeDS;
                    halfEdgeDS.Editor.EdgeFlips(selectHalfEdge);
                    selectHalfEdge = null;
                    selectObject.Polygon.Update(OpenTK.Graphics.OpenGL.PrimitiveType.Lines);
                }
            }

            return base.Down(mouse);
        }
    }
}
