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
    /// 三角形の選択
    /// </summary>
    public class SelectTriangleControl : IControl
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
                Triangle triangle = null;

                if (Selector.PickTriangle(leftMouse.Click, ref renderObject, ref triangle))
                {
                    RenderObject point = RenderObjectFactory.Instance.CreateRenderObject("SelectTriangle :" + renderObject.Name);
                    Vector3 tri0 = triangle.Vertex0 + triangle.Normal * 0.01f;
                    Vector3 tri1 = triangle.Vertex1 + triangle.Normal * 0.01f;
                    Vector3 tri2 = triangle.Vertex2 + triangle.Normal * 0.01f;

                    point.SetGeometryInfo(new Geometry("select", new List<Vector3>() { tri0, tri1, tri2 }, null, Vector3.UnitX, null, null, GeometryType.Triangle));
                    point.ModelMatrix = renderObject.ModelMatrix;
                    Global.RenderSystem.ActiveScene.AddObject(point);
                }
            }

            return true;
        }
    }
}
