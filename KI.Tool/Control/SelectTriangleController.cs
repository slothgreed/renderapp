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
    /// 三角形の選択
    /// </summary>
    public class SelectTriangleController : IController
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
                Clear();

                HalfEdgeMesh mesh = null;

                if (HalfEdgeDSSelector.PickTriangle(leftMouse.Click, ref renderObject, ref mesh))
                {
                    foreach (var vertex in mesh.AroundVertex)
                    {
                        vertex.Color = Vector3.UnitY;
                        selectVertex.Add(vertex);
                        renderObject.Polygon.UpdateVertexArray();
                    }
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
            Clear();

            return base.UnBinding();
        }

        /// <summary>
        /// 選択解除
        /// </summary>
        private void Clear()
        {
            foreach (var vertex in selectVertex)
            {
                vertex.Color = new Vector3(0.8f);
            }

            if (renderObject != null)
            {
                renderObject.Polygon.UpdateVertexArray();
            }

            selectVertex.Clear();
        }
    }
}
