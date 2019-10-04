using KI.Analyzer;
using KI.Asset;
using KI.Gfx.Buffer;
using KI.Renderer;
using KI.Foundation.Controller;
using OpenTK;
using RenderApp.Tool.Utility;

namespace RenderApp.Tool.Controller
{

    /// <summary>
    /// 面を選択したイベント
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void OnTriangleSelectedHandler(object sender, ItemSelectedEventArgs e);

    /// <summary>
    /// 三角形の選択
    /// </summary>
    public class SelectTriangleController : ControllerBase
    {

        /// <summary>
        /// Viewport上で頂点したイベント
        /// </summary>
        public event OnTriangleSelectedHandler TriangleSelected;

        /// <summary>
        /// 面の選択
        /// </summary>
        private HalfEdgeMesh selectMesh;

        /// <summary>
        /// 描画オブジェクト
        /// </summary>
        private PolygonNode polygonNode;
        
        /// <summary>
        /// マウス押下処理
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Clear();

                HalfEdgeMesh mesh = null;

                if (HalfEdgeDSSelector.PickTriangle(mouse.Current, ref polygonNode, ref mesh))
                {
                    if (mesh == null)
                    {
                        return false;
                    }

                    selectMesh = mesh;
                    foreach (var vertex in selectMesh.AroundVertex)
                    {
                        vertex.Color = Vector3.UnitY;
                    }

                    polygonNode.UpdateVertexBufferObject();

                    OnSelectMesh(selectMesh);

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
            selectMesh = null;

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

        /// <summary>
        /// 選択解除
        /// </summary>
        private void Clear()
        {
            if (selectMesh != null)
            {
                foreach (var vertex in selectMesh.AroundVertex)
                {
                    vertex.Color = new Vector3(0.8f);
                }
            }

            if (polygonNode != null)
            {
                polygonNode.UpdateVertexBufferObject();
            }

            selectMesh = null;
        }

        private void OnSelectMesh(HalfEdgeMesh item)
        {
            TriangleSelected?.Invoke(this, new ItemSelectedEventArgs(item));
        }
    }
}
