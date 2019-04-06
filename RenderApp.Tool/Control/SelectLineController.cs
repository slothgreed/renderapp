using KI.Analyzer;
using KI.Asset;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Renderer;
using KI.Tool.Control;
using OpenTK;
using RenderApp.Tool.Utility;

namespace RenderApp.Tool.Control
{

    /// <summary>
    /// 点を選択したイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnLineSelectedHandler(object sender, ItemSelectedEventArgs e);

    /// <summary>
    /// 線分の選択
    /// </summary>
    public class SelectLineController : IController
    {

        /// <summary>
        /// Viewport上で頂点したイベント
        /// </summary>
        public event OnLineSelectedHandler LineSelected;

        /// <summary>
        /// 頂点の選択
        /// </summary>
        private HalfEdge selectEdge;

        /// <summary>
        /// 描画オブジェクト
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// マウス押下処理
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                HalfEdge halfEdge = null;

                Clear();

                if (HalfEdgeDSSelector.PickLine(mouse.Current, ref renderObject, ref halfEdge))
                {
                    halfEdge.Start.Color = Vector3.UnitY;
                    halfEdge.End.Color = Vector3.UnitY;

                    selectEdge = halfEdge;

                    renderObject.UpdateVertexBufferObject();
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
            selectEdge = null;

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

        /// <summary>
        /// 選択解除
        /// </summary>
        private void Clear()
        {
            if (selectEdge != null)
            {
                selectEdge.Start.Color = new Vector3(0.8f);
                selectEdge.End.Color = new Vector3(0.8f);
                renderObject.UpdateVertexBufferObject();
            }

            selectEdge = null;
        }


        private void OnSelectLine(Vertex item)
        {
            LineSelected?.Invoke(this, new ItemSelectedEventArgs(item));
        }
    }

}
