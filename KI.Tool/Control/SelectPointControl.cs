using System.Collections.Generic;
using KI.Asset;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Renderer;
using KI.Tool.Utility;
using OpenTK;

namespace KI.Tool.Control
{
    /// <summary>
    /// 頂点選択コントローラ
    /// </summary>
    public class SelectPointControl : IControl
    {
        /// <summary>
        /// マウスダウン
        /// </summary>
        /// <param name="mouse">マウス</param>
        /// <returns>成功</returns>
        public override bool Down(System.Windows.Forms.MouseEventArgs mouse)
        {
            RenderObject renderObject = null;
            int vertex_Index = 0;

            if (Selector.PickPoint(leftMouse.Click, ref renderObject, ref vertex_Index))
            {
                Vector3 pos = renderObject.Geometry.Position[vertex_Index];
                RenderObject point = RenderObjectFactory.Instance.CreateRenderObject("SelectPoint :" + renderObject.Name + ":" + vertex_Index.ToString());
                point.SetGeometryInfo(new Geometry("select", new List<Vector3>() { pos }, null, KICalc.RandomColor(), null, null, GeometryType.Point));
                point.ModelMatrix = renderObject.ModelMatrix;
                Global.RenderSystem.ActiveScene.AddObject(point);
            }

            return true;
        }
    }
}
