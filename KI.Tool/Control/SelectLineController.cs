﻿using System.Collections.Generic;
using KI.Analyzer;
using KI.Gfx.Geometry;
using KI.Asset;
using KI.Tool.Utility;
using OpenTK;
using KI.Gfx;

namespace KI.Tool.Control
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
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                HalfEdge halfEdge = null;

                Clear();

                if (HalfEdgeDSSelector.PickLine(leftMouse.Click, ref renderObject, ref halfEdge))
                {
                    halfEdge.Start.Color = Vector3.UnitY;
                    halfEdge.End.Color = Vector3.UnitY;

                    selectEdge = halfEdge;

                    renderObject.Polygon.UpdateVertexArray();
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
            if(selectEdge != null)
            {
                selectEdge.Start.Color = new Vector3(0.8f);
                selectEdge.End.Color = new Vector3(0.8f);
            }

            if (renderObject != null)
            {
                renderObject.Polygon.UpdateVertexArray();
            }

            selectEdge = null;
        }


        private void OnSelectLine(Vertex item)
        {
            LineSelected?.Invoke(this, new ItemSelectedEventArgs(item));
        }
    }

}
