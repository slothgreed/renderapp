using System.Collections.Generic;
using System.Windows.Forms;
using CADApp.Model;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Mathmatics;
using KI.Tool.Control;
using OpenTK;

namespace CADApp.Tool.Control
{
    public class SketchLineController : IController
    {
        /// <summary>
        /// 配置するZ位置
        /// </summary>
        private float zPosition = 0;

        RenderObject pointObject;

        RenderObject lineObject;

        List<Vertex> pointList;

        public override bool Down(KIMouseEventArgs mouse)
        {
            if (mouse.Button == MOUSE_BUTTON.Left)
            {
                Camera camera = Workspace.Instance.MainScene.MainCamera;

                Vector3 near;
                Vector3 far;
                GLUtility.GetClickPos(camera.Matrix, camera.ProjMatrix, Viewport.Instance.ViewportRect, mouse.Current, out near, out far);

                Vector3 direction = (camera.Position - far).Normalized();
                Vector3 interPoint;
                if (Interaction.PlaneToLine(camera.Position, far, Workspace.Instance.WorkPlane.Formula, out interPoint))
                {
                    int pointIndex = pointList.Count;
                    pointList.Add(new Vertex(pointIndex, interPoint, Vector3.UnitX));

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
            pointObject = new RenderObject("Point", point, shader);
            pointObject.Visible = false;
            Workspace.Instance.MainScene.AddObject(pointObject);

            lineObject = new RenderObject("Line", line, shader);
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
