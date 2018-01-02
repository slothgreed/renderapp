using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using KI.Renderer;
using KI.Tool.Utility;
using OpenTK;

namespace KI.Tool.Control
{
    /// <summary>
    /// 距離場算出用のコントローラ
    /// </summary>
    public class GeodesicDistanceControl : IControl
    {
        /// <summary>
        /// 測地距離アルゴリズム
        /// </summary>
        private GeodesicDistanceAlgorithm geodesic;

        /// <summary>
        /// 形状の選択
        /// </summary>
        private RenderObject selectObject;

        /// <summary>
        /// 選択した頂点の要素番号
        /// </summary>
        private int[] selectVertexIndex;

        /// <summary>
        /// マウス押下
        /// </summary>
        /// <param name="mouse">マウス座標</param>
        /// <returns>成功</returns>
        public override bool Down(MouseEventArgs mouse)
        {
            RenderObject renderObject = null;
            if (mouse.Button == System.Windows.Forms.MouseButtons.Left)
            {
                HalfEdgeVertex vertex = null;
                if (HalfEdgeDSSelector.PickPoint(leftMouse.Click, ref renderObject, ref vertex))
                {
                    if (renderObject.Polygon is HalfEdgeDS)
                    {
                        geodesic = new GeodesicDistanceAlgorithm(renderObject.Polygon as HalfEdgeDS);
                        RenderObject pointObject = RenderObjectFactory.Instance.CreateRenderObject("Picking");
                        Polygon polygon = new Polygon("Picking", new List<Vertex>() { new Vertex(0, vertex.Position, Vector3.UnitY) });
                        pointObject.SetPolygon(polygon);
                        pointObject.ModelMatrix = selectObject.ModelMatrix;
                        Global.RenderSystem.ActiveScene.AddObject(pointObject);


                        geodesic.SelectPoint(vertex.Index);
                        var geodesicDistance = geodesic.Compute();
                        var max = geodesicDistance.Max();
                        for (int i = 0; i < renderObject.Polygon.Vertexs.Count; i++)
                        {
                            renderObject.Polygon.Vertexs[i].Color = KICalc.GetPseudoColor(geodesicDistance[i], 0, max);
                        }
                    }
                }
            }

            return true;
        }
    }
}
