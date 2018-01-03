﻿using System;
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
using OpenTK.Graphics.OpenGL;

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
                HalfEdgeVertex selectVertex = null;
                if (HalfEdgeDSSelector.PickPoint(leftMouse.Click, ref renderObject, ref selectVertex))
                {
                    if (renderObject.Polygon is HalfEdgeDS)
                    {
                        geodesic = new GeodesicDistanceAlgorithm(renderObject.Polygon as HalfEdgeDS);
                        RenderObject pointObject = RenderObjectFactory.Instance.CreateRenderObject("Picking");
                        Polygon polygon = new Polygon("Picking", new List<Vertex>() { new Vertex(0, selectVertex.Position, Vector3.UnitY) });
                        pointObject.SetPolygon(polygon);
                        pointObject.ModelMatrix = renderObject.ModelMatrix;
                        Global.RenderSystem.ActiveScene.AddObject(pointObject);


                        geodesic.SelectPoint(selectVertex.Index);
                        var geodesicDistance = geodesic.Compute();
                        var max = geodesicDistance.Max();
                        for (int i = 0; i < renderObject.Polygon.Vertexs.Count; i++)
                        {
                            renderObject.Polygon.Vertexs[i].Color = KICalc.GetPseudoColor(geodesicDistance[i], 0, max);
                        }
                        renderObject.Polygon.UpdateVertexArray(PrimitiveType.Triangles);

                        //var distBetweenLines = max / 10;
                        //List<Line> lines = new List<Line>();
                        //foreach (var mesh in ((HalfEdgeDS)renderObject.Polygon).HalfEdgeMeshs)
                        //{
                        //    var segment = new List<Vector3>();
                        //    foreach (var edge in mesh.AroundEdge)
                        //    {
                        //        var region1 = (float)Math.Floor(geodesicDistance[edge.Start.Index] / distBetweenLines);
                        //        var region2 = (float)Math.Floor(geodesicDistance[edge.End.Index] / distBetweenLines);

                        //        if (Math.Abs(region1 - region2) > KICalc.THRESHOLD05)
                        //        {
                        //            float lamda = 0;
                        //            if (region1 < region2)
                        //            {
                        //                lamda = (region2 * distBetweenLines - geodesicDistance[edge.Start.Index]) /
                        //                    (geodesicDistance[edge.End.Index] - geodesicDistance[edge.Start.Index]);
                        //            }
                        //            else
                        //            {
                        //                lamda = (region1 * distBetweenLines - geodesicDistance[edge.Start.Index]) /
                        //                    (geodesicDistance[edge.End.Index] - geodesicDistance[edge.Start.Index]);
                        //            }

                        //            var p1 = edge.Start.Position;
                        //            var p2 = edge.End.Position;
                        //            var point = p1 + ((p2 - p1) * lamda);
                        //            segment.Add(point);
                        //        }
                        //    }

                        //    if (segment.Count == 2)
                        //    {
                        //        Line line = new Line(new Vertex(0, segment[0], Vector3.UnitX), new Vertex(1, segment[1], Vector3.UnitX));
                        //        lines.Add(line);
                        //    }
                        //}

                        //RenderObject lineObject = RenderObjectFactory.Instance.CreateRenderObject("geodesicDistance");
                        //Polygon lineGeometry = new Polygon("geodesicDistance", lines);
                        //lineObject.SetPolygon(lineGeometry);
                        //lineObject.ModelMatrix = renderObject.ModelMatrix;
                        //Global.RenderSystem.ActiveScene.AddObject(lineObject);
                    }
                }
            }

            return true;
        }
    }
}