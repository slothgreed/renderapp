using KI.Analyzer;
using KI.Renderer;
using KI.Tool.Utility;
using OpenTK.Graphics.OpenGL;

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
            HalfEdgeVertex vertex = null;

            if (HalfEdgeDSSelector.PickPoint(leftMouse.Click, ref renderObject, ref vertex))
            {
                vertex.IsSelect = true;
                renderObject.Polygon.UpdateVertexArray(PrimitiveType.Points);
            }

            return true;
        }
    }
}
