﻿using System.Windows.Forms;
using KI.Analyzer;
using KI.Renderer;
using KI.Tool.Utility;

namespace KI.Tool.Control
{
    /// <summary>
    /// エッジフリップ確認用
    /// </summary>
    public class EdgeFlipsControl : IControl
    {
        /// <summary>
        /// 選択形状
        /// </summary>
        private RenderObject selectObject = null;

        /// <summary>
        /// 選択エッジ
        /// </summary>
        private HalfEdge selectHalfEdge = null;

        /// <summary>
        /// マウス押下処理
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Down(MouseEventArgs mouse)
        {
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                RenderObject renderObject = null;
                HalfEdge halfEdge = null;

                if (HalfEdgeDSSelector.PickLine(leftMouse.Click, ref renderObject, ref halfEdge))
                {
                    halfEdge.Start.IsSelect = true;
                    halfEdge.End.IsSelect = true;

                    selectObject = renderObject;
                    selectHalfEdge = halfEdge;
                    renderObject.Geometry.UpdateHalfEdge();
                }
            }

            if (mouse.Button == MouseButtons.Right)
            {
                if (selectHalfEdge != null)
                {
                    selectObject.Geometry.HalfEdgeDS.Editor.EdgeFlips(selectHalfEdge);
                    selectHalfEdge = null;
                    selectObject.Geometry.UpdateHalfEdge();
                }
            }

            return base.Down(mouse);
        }
    }
}