using KI.Analyzer;
using KI.Asset;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Renderer;
using KI.Foundation.Controller;
using OpenTK;
using RenderApp.Tool.Utility;

namespace RenderApp.Tool.Controller
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
    public class SelectPointController : ControllerBase
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
        private PolygonNode polygonNode = null;

        /// <summary>
        /// マウスダウン
        /// </summary>
        /// <param name="mouse">マウス</param>
        /// <returns>成功</returns>
        public override bool Down(KIMouseEventArgs mouse)
        {
            Clear();

            HalfEdgeVertex vertex = null;

            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                if (HalfEdgeDSSelector.PickPoint(mouse.Current, ref polygonNode, ref vertex))
                {
                    vertex.Color = Vector3.UnitY;
                    selectVertex = vertex;
                    polygonNode.UpdateVertexBufferObject();

                    OnSelectPoint(selectVertex);
                }
            }

            return true;
        }

        /// <summary>
        /// コントローラの開始
        /// </summary>
        /// <returns>成功</returns>
        public override bool Binding(IControllerArgs args)
        {

            return base.Binding(args);
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
            if (selectVertex != null)
            {
                selectVertex.Color = new Vector3(0.8f);
                polygonNode.UpdateVertexBufferObject();
            }
        }

        private void OnSelectPoint(Vertex item)
        {
            PointSelected?.Invoke(this, new ItemSelectedEventArgs(item));
        }
    }
}
