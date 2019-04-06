using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Asset.Attribute;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Mathmatics;
using KI.Tool.Control;
using OpenTK;
using RenderApp.Model;
using RenderApp.Tool.Utility;

namespace RenderApp.Tool.Control
{
    /// <summary>
    /// 距離場算出用のコントローラ
    /// </summary>
    public class GeodesicDistanceController : IController
    {
        /// <summary>
        /// 測地距離アルゴリズム
        /// </summary>
        private GeodesicDistanceAlgorithm geodesic;

        /// <summary>
        /// マウス押下
        /// </summary>
        /// <param name="mouse">マウス座標</param>
        /// <returns>成功</returns>
        public override bool Down(KIMouseEventArgs mouse)
        {
            RenderObject renderObject = null;
            if (mouse.Button == MOUSE_BUTTON.Middle)
            {
                HalfEdgeVertex selectVertex = null;
                if (HalfEdgeDSSelector.PickPoint(mouse.Current, ref renderObject, ref selectVertex))
                {
                    if (renderObject.Polygon is HalfEdgeDS)
                    {
                        geodesic = new GeodesicDistanceAlgorithm(renderObject.Polygon as HalfEdgeDS);
                        Polygon polygon = new Polygon("Picking", new List<Vertex>() { new Vertex(0, selectVertex.Position, Vector3.UnitY) });
                        RenderObject pointObject = RenderObjectFactory.Instance.CreateRenderObject("Picking", polygon);
                        pointObject.ModelMatrix = renderObject.ModelMatrix;
                        Workspace.Instance.Renderer.ActiveScene.AddObject(pointObject);

                        geodesic.SelectPoint(selectVertex.Index);
                        geodesic.Compute();
                        var geodesicDistance = geodesic.DistanceField;
                        var max = geodesicDistance.Max();

                        var distBetweenLines = max / 20;
                        List<Line> lines = new List<Line>();
                        foreach (var mesh in ((HalfEdgeDS)renderObject.Polygon).HalfEdgeMeshs)
                        {
                            var segment = new List<Vector3>();
                            foreach (var edge in mesh.AroundEdge)
                            {
                                var region1 = (float)Math.Floor(geodesicDistance[edge.Start.Index] / distBetweenLines);
                                var region2 = (float)Math.Floor(geodesicDistance[edge.End.Index] / distBetweenLines);

                                if (Math.Abs(region1 - region2) > Calculator.THRESHOLD05)
                                {
                                    float lamda = 0;
                                    if (region1 < region2)
                                    {
                                        lamda = (region2 * distBetweenLines - geodesicDistance[edge.Start.Index]) /
                                            (geodesicDistance[edge.End.Index] - geodesicDistance[edge.Start.Index]);
                                    }
                                    else
                                    {
                                        lamda = (region1 * distBetweenLines - geodesicDistance[edge.Start.Index]) /
                                            (geodesicDistance[edge.End.Index] - geodesicDistance[edge.Start.Index]);
                                    }

                                    var p1 = edge.Start.Position;
                                    var p2 = edge.End.Position;
                                    var point = p1 + ((p2 - p1) * lamda);
                                    segment.Add(point);
                                }
                            }

                            if (segment.Count == 2)
                            {
                                Line line = new Line(new Vertex(0, segment[0], Vector3.UnitX), new Vertex(1, segment[1], Vector3.UnitX));
                                lines.Add(line);
                            }
                        }

                        var parentNode = Workspace.Instance.Renderer.ActiveScene.FindNode(renderObject);
                        
                        var colorAttribute = new VertexParameterAttribute("distanceColor", 
                            renderObject.VertexBuffer.ShallowCopy(), 
                            renderObject.Type, 
                            renderObject.Shader, 
                            geodesicDistance);

                        Polygon lineGeometry = new Polygon("geodesicDistance", lines);
                        var vertexBuffer = new VertexBuffer();
                        vertexBuffer.SetupLineBuffer(lineGeometry.Vertexs, lineGeometry.Index, lineGeometry.Lines);
                        var lineAttribute = new PolygonAttribute("geodesicDistance", vertexBuffer, lineGeometry.Type, renderObject.Shader);
                        renderObject.Attributes.Add(lineAttribute);
                        Workspace.Instance.Renderer.ActiveScene.AddObject(lineAttribute, parentNode);
                    }
                }
            }

            return true;
        }
    }
}
