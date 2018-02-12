using System.Collections.Generic;
using KI.Analyzer;
using KI.Gfx.Geometry;
using KI.Asset;
using KI.Tool.Utility;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Tool.Control
{
    /// <summary>
    /// 線分の選択
    /// </summary>
    public class SelectLineControl : IControl
    {
        /// <summary>
        /// 頂点の選択
        /// </summary>
        private List<Vertex> selectVertex = new List<Vertex>();

        /// <summary>
        /// 描画オブジェクト
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// マウス押下処理
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                HalfEdge halfEdge = null;

                if (HalfEdgeDSSelector.PickLine(leftMouse.Click, ref renderObject, ref halfEdge))
                {
                    halfEdge.Start.Color = Vector3.UnitY;
                    halfEdge.End.Color = Vector3.UnitY;

                    selectVertex.Add(halfEdge.Start);
                    selectVertex.Add(halfEdge.End);

                    renderObject.Polygon.UpdateVertexArray(OpenTK.Graphics.OpenGL.PrimitiveType.Lines);
                }
            }

            return true;
        }

        /// <summary>
        /// コントローラの開始
        /// </summary>
        /// <returns>成功</returns>
        public override bool Binding()
        {
            selectVertex = new List<Vertex>();

            return base.Binding();
        }

        /// <summary>
        /// コントローラの終了
        /// </summary>
        /// <returns>成功</returns>
        public override bool UnBinding()
        {
            foreach (var vertex in selectVertex)
            {
                vertex.Color = new Vector3(0.8f);
            }

            if (renderObject != null)
            {
                renderObject.Polygon.UpdateVertexArray(PrimitiveType.Points);
            }

            selectVertex = null;
            return base.UnBinding();
        }
    }
}
