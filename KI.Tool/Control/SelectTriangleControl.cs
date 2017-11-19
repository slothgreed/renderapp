using KI.Analyzer;
using KI.Renderer;
using KI.Tool.Utility;
using OpenTK.Graphics.OpenGL;

namespace KI.Tool.Control
{
    /// <summary>
    /// 三角形の選択
    /// </summary>
    public class SelectTriangleControl : IControl
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
                HalfEdgeMesh mesh = null;

                if (HalfEdgeDSSelector.PickTriangle(leftMouse.Click, ref renderObject, ref mesh))
                {
                    foreach (var vertex in mesh.AroundVertex)
                    {
                        vertex.IsSelect = true;
                        renderObject.Polygon.UpdateVertexArray(PrimitiveType.Points);
                    }
                }
            }

            return true;
        }
    }
}
