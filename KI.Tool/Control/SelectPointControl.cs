using System.Collections.Generic;
using KI.Analyzer;
using KI.Asset;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Renderer;
using KI.Tool.Utility;
using OpenTK;

namespace KI.Tool.Control
{
    /// <summary>
    /// 頂点選択コントローラ
    /// </summary>
    public class SelectPointControl : IControl
    {
        /// <summary>
        /// マウスダウン
        /// </summary>
        /// <param name="mouse">マウス</param>
        /// <returns>成功</returns>
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            RenderObject renderObject = null;
            Vertex vertex = null;

            if (HalfEdgeDSSelector.PickPoint(leftMouse.Click, ref renderObject, ref vertex))
            {
                vertex.IsSelect = true;
                renderObject.Geometry.UpdateHalfEdge();

            }

            return true;
        }
    }
}
