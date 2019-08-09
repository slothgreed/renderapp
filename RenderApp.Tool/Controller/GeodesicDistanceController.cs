using System;
using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset.Attribute;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Mathmatics;
using KI.Renderer;
using KI.Foundation.Controller;
using OpenTK;
using RenderApp.Model;
using RenderApp.Tool.Utility;

namespace RenderApp.Tool.Controller
{
    /// <summary>
    /// 距離場算出用のコントローラ
    /// </summary>
    public class GeodesicDistanceController : ControllerBase
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
            PolygonNode polygonNode = null;
            if (mouse.Button == MOUSE_BUTTON.Middle)
            {
                HalfEdgeVertex selectVertex = null;
                if (HalfEdgeDSSelector.PickPoint(mouse.Current, ref polygonNode, ref selectVertex))
                {
                    AnalyzePolygonNode analyzePolygonNode = null;
                    if (polygonNode is AnalyzePolygonNode)
                    {
                        analyzePolygonNode = polygonNode as AnalyzePolygonNode;
                    }

                    if (analyzePolygonNode.Polygon is HalfEdgeDS)
                    {
                        geodesic = new GeodesicDistanceAlgorithm(analyzePolygonNode.Polygon as HalfEdgeDS);
                        Polygon polygon = new Polygon("Picking", new List<Vertex>() { new Vertex(0, selectVertex.Position, Vector3.UnitY) }, KI.Gfx.KIPrimitiveType.Points);
                        PolygonUtility.Setup(polygon);
                        PolygonNode pointObject = new PolygonNode(polygon);
                        pointObject.ModelMatrix = analyzePolygonNode.ModelMatrix;
                        Workspace.Instance.RenderSystem.ActiveScene.AddObject(pointObject);

                        geodesic.SelectPoint(selectVertex.Index);
                        geodesic.Compute();
                        var geodesicDistance = geodesic.DistanceField;
                        var max = geodesicDistance.Max();

                        var distBetweenLines = max / 20;
                        List<Vertex> vertexs = new List<Vertex>();
                        foreach (var mesh in ((HalfEdgeDS)analyzePolygonNode.Polygon).HalfEdgeMeshs)
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
                                vertexs.Add(new Vertex(0, segment[0], Vector3.UnitX));
                                vertexs.Add(new Vertex(1, segment[1], Vector3.UnitX));
                            }
                        }

                        var parentNode = Workspace.Instance.RenderSystem.ActiveScene.FindNode(analyzePolygonNode);
                        
                        var colorAttribute = new VertexParameterAttribute("distanceColor", 
                            analyzePolygonNode.VertexBuffer.ShallowCopy(), 
                            analyzePolygonNode.Type, 
                            analyzePolygonNode.Polygon.Material, 
                            geodesicDistance);

                        Polygon lineGeometry = new Polygon("geodesicDistance", vertexs, KIPrimitiveType.Lines);
                        var vertexBuffer = new VertexBuffer();
                        vertexBuffer.SetupPointBuffer(lineGeometry.Vertexs, lineGeometry.Index);
                        var lineAttribute = new PolygonAttribute("geodesicDistance", vertexBuffer, KIPrimitiveType.Lines, analyzePolygonNode.Polygon.Material);
                        analyzePolygonNode.Attributes.Add(lineAttribute);
                        Workspace.Instance.RenderSystem.ActiveScene.AddObject(new PolygonNode(lineGeometry), parentNode);
                    }
                }
            }

            return true;
        }
    }
}
