using System.Collections.Generic;
using KI.Asset;
using KI.Foundation.KIMath;
using KI.Gfx.GLUtil;
using KI.Renderer;
using KI.Tool.Utility;
using OpenTK;

namespace KI.Tool.Control
{
    /// <summary>
    /// 線分の選択
    /// </summary>
    public class SelectLineControl : IControl
    {
        /// <summary>
        /// マウス押下処理
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                RenderObject renderObject = null;
                Line line = null;

                if (Selector.PickLine(leftMouse.Click, ref renderObject, ref line))
                {
                    RenderObject lineObject = RenderObjectFactory.Instance.CreateRenderObject("selectLine :" + renderObject.Name);

                    lineObject.SetGeometryInfo(new Geometry("select", new List<Vector3>() { line.Start,line.End }, null, Vector3.UnitX, null, null, GeometryType.Line));
                    lineObject.ModelMatrix = renderObject.ModelMatrix;
                    Global.RenderSystem.ActiveScene.AddObject(lineObject);
                }
            }

            return true;
        }
    }
}
