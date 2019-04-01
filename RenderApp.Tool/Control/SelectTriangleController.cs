using System.Collections.Generic;
using KI.Analyzer;
using KI.Gfx.Geometry;
using KI.Asset;
using RenderApp.Tool.Utility;
using OpenTK;
using KI.Gfx;

namespace RenderApp.Tool.Control
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
    public class SelectTriangleController : IController
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
                Clear();

                HalfEdgeMesh mesh = null;

                if (HalfEdgeDSSelector.PickTriangle(leftMouse.Click, ref renderObject, ref mesh))
                {
                    if(mesh == null)
                    {
                        return false;
                    }

                    selectMesh = mesh;
                    foreach (var vertex in selectMesh.AroundVertex)
                    {
                        vertex.Color = Vector3.UnitY;
                        renderObject.Polygon.UpdateVertexArray();
                    }

                    OnSelectMesh(selectMesh);

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
            selectMesh = null;

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
            if(selectMesh != null)
            {
                foreach (var vertex in selectMesh.AroundVertex)
                {
                    vertex.Color = new Vector3(0.8f);
                    renderObject.Polygon.UpdateVertexArray();
                }
            }

            if (renderObject != null)
            {
                renderObject.Polygon.UpdateVertexArray();
            }

            selectMesh = null;
        }


        private void OnSelectMesh(HalfEdgeMesh item)
        {
            TriangleSelected?.Invoke(this, new ItemSelectedEventArgs(item));
        }
    }
}
