using System.Collections.Generic;
using KI.Analyzer;
using KI.Gfx.Geometry;
using KI.Asset;
using KI.Tool.Utility;
using OpenTK;
using KI.Gfx;

namespace KI.Tool.Control
{
    /// <summary>
    /// 頂点選択コントローラ
    /// </summary>
    public class SelectPointControl : IControl
    {
        /// <summary>
        /// 選択頂点
        /// </summary>
        private List<Vertex> selectVertex;

        /// <summary>
        /// 描画オブジェクト
        /// </summary>
        private RenderObject renderObject = null;

        /// <summary>
        /// マウスダウン
        /// </summary>
        /// <param name="mouse">マウス</param>
        /// <returns>成功</returns>
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            HalfEdgeVertex vertex = null;

            if (HalfEdgeDSSelector.PickPoint(leftMouse.Click, ref renderObject, ref vertex))
            {
                vertex.Color = Vector3.UnitY;
                selectVertex.Add(vertex);
                renderObject.Polygon.UpdateVertexArray(PolygonType.Points);
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
                renderObject.Polygon.UpdateVertexArray(PolygonType.Points);
            }

            selectVertex = null;
            return base.UnBinding();
        }
    }
}
