using System.Collections.Generic;
using KI.Analyzer;
using KI.Asset;
using KI.Foundation.KIMath;
using KI.Gfx.GLUtil;
using KI.Renderer;
using KI.Tool.Utility;
using OpenTK;

namespace KI.Tool.Control
{
    /// <summary>
    /// 線分の選択
    /// </summary>
    public class SelectLineControl : IControl
    {
        /// <summary>
        /// マウス押下処理
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                RenderObject renderObject = null;
                HalfEdge halfEdge = null;

                if (HalfEdgeDSSelector.PickLine(leftMouse.Click, ref renderObject, ref halfEdge))
                {
                    halfEdge.Start.IsSelect = true;
                    halfEdge.End.IsSelect = true;

                    renderObject.Geometry.UpdateHalfEdge();
                }
            }

            return true;
        }
    }
}
