using System.Collections.Generic;
using System.Windows.Forms;
using CADApp.Model;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Mathmatics;
using KI.Renderer;
using KI.Tool.Control;
using OpenTK;

namespace CADApp.Tool.Control
{
    /// <summary>
    /// TODO: スケッチレクタングルの作り方に修正する。
    /// </summary>
    public class SketchLineController : IController
    {
        /// <summary>
        /// 配置するZ位置
        /// </summary>
        private float zPosition = 0;

        PolygonNode pointObject;

        PolygonNode lineObject;

        List<Vertex> pointList;

        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;
                Vector3 worldPoint;

                if (ControllerUtility.GetClickWorldPosition(camera, Workspace.Instance.WorkPlane.Formula, mouse, out worldPoint))
                {
                    int pointIndex = pointList.Count;
                    pointList.Add(new Vertex(pointIndex, worldPoint, Vector3.UnitX));

                    pointObject.Visible = true;

                    pointObject.Polygon.Vertexs.Add(pointList[pointIndex]);
                    pointObject.UpdateVertexBufferObject();

                    lineObject.Polygon.Vertexs.Add(pointList[pointIndex]);

                    if (pointList.Count >= 2)
                    {
                        lineObject.Visible = true;
                        lineObject.Polygon.Index.Add(lineObject.Polygon.Vertexs.Count - 2);
                        lineObject.Polygon.Index.Add(lineObject.Polygon.Vertexs.Count - 1);
                        lineObject.UpdateVertexBufferObject();
                    }
                }
            }

            return true;
        }

        public override bool Binding()
        {
            pointList = new List<Vertex>();
            Polygon point = new Polygon("Point");
            Polygon line = new Polygon("Line", PolygonType.Lines);
            var shader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
            pointObject = new PolygonNode("Point", point, shader);
            pointObject.Visible = false;
            Workspace.Instance.MainScene.AddObject(pointObject);

            lineObject = new PolygonNode("Line", line, shader);
            lineObject.Visible = false;
            Workspace.Instance.MainScene.AddObject(lineObject);
            return base.Binding();
        }

        public override bool UnBinding()
        {
            return base.UnBinding();
        }
    }
}
