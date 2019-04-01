using System.Collections.Generic;
using KI.Analyzer;
using KI.Gfx.Geometry;
using KI.Asset;
using RenderApp.Tool.Utility;
using OpenTK;
using KI.Tool.Control;

namespace RenderApp.Tool.Control
{

    /// <summary>
    /// 点を選択したイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnPointSelectedHandler(object sender, ItemSelectedEventArgs e);

    /// <summary>
    /// 頂点選択コントローラ
    /// </summary>
    public class SelectPointController : IController
    {

        /// <summary>
        /// Viewport上で頂点したイベント
        /// </summary>
        public event OnPointSelectedHandler PointSelected;

        /// <summary>
        /// 選択頂点
        /// </summary>
        private Vertex selectVertex;

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
            Clear();

            HalfEdgeVertex vertex = null;

            if (HalfEdgeDSSelector.PickPoint(leftMouse.Click, ref renderObject, ref vertex))
            {
                vertex.Color = Vector3.UnitY;
                selectVertex = vertex;
                renderObject.Polygon.UpdateVertexArray();

                OnSelectPoint(selectVertex);
            }

            return true;
        }

        /// <summary>
        /// コントローラの開始
        /// </summary>
        /// <returns>成功</returns>
        public override bool Binding()
        {

            return base.Binding();
        }

        /// <summary>
        /// コントローラの終了
        /// </summary>
        /// <returns>成功</returns>
        public override bool UnBinding()
        {
            Clear();

            return base.UnBinding();
        }

        private void Clear()
        {
            if(selectVertex != null)
            {
                selectVertex.Color = new Vector3(0.8f);
            }

            if (renderObject != null)
            {
                renderObject.Polygon.UpdateVertexArray();
            }

        }

        private void OnSelectPoint(Vertex item)
        {
            PointSelected?.Invoke(this, new ItemSelectedEventArgs(item));
        }
    }
}
