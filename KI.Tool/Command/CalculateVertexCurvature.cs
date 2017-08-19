﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// 曲率の算出コマンド
    /// </summary>
    public class CalculateVertexCurvature : ICommand
    {
        /// <summary>
        /// レンダリング形状
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">算出オブジェクト</param>
        public CalculateVertexCurvature(KIObject asset)
        {
            renderObject = asset as RenderObject;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public CommandResult CanExecute(string commandArg)
        {
            if (renderObject == null)
            {
                return CommandResult.Failed;
            }

            if (renderObject.Geometry.HalfEdgeDS == null)
            {
                return CommandResult.Failed;
            }

            return CommandResult.Success;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public CommandResult Execute(string commandArg)
        {
            var vertexInfo = new VertexCurvatureAlgorithm(renderObject.Geometry.HalfEdgeDS);
            var halfDS = renderObject.Geometry.HalfEdgeDS;

            List<Vector3> position = new List<Vector3>();// poly.Vertexs.Select(p => p.Position).ToList();
            List<Vector3> voronoi = new List<Vector3>();
            List<Vector3> mean = new List<Vector3>();
            List<Vector3> gauss = new List<Vector3>();
            List<Vector3> min = new List<Vector3>();
            List<Vector3> max = new List<Vector3>();
            List<Vector3> minVector = new List<Vector3>();
            List<Vector3> maxVector = new List<Vector3>();
            List<Vector3> minVecColor = new List<Vector3>();
            List<Vector3> maxVecColor = new List<Vector3>();

            var voronoiParam = new ScalarParameter(halfDS.Vertexs.Select(p => (float)p.GetParameter(VertexParam.Voronoi)).ToArray());
            var meanParam = new ScalarParameter(halfDS.Vertexs.Select(p => (float)p.GetParameter(VertexParam.MeanCurvature)).ToArray());
            var gaussParam = new ScalarParameter(halfDS.Vertexs.Select(p => (float)p.GetParameter(VertexParam.GaussCurvature)).ToArray());
            var minParam = new ScalarParameter(halfDS.Vertexs.Select(p => (float)p.GetParameter(VertexParam.MinCurvature)).ToArray());
            var maxParam = new ScalarParameter(halfDS.Vertexs.Select(p => (float)p.GetParameter(VertexParam.MaxCurvature)).ToArray());
            var minVecParam = new VectorParameter<Vector3>(halfDS.Vertexs.Select(p => (Vector3)p.GetParameter(VertexParam.MinVector)).ToArray());
            var maxVecParam = new VectorParameter<Vector3>(halfDS.Vertexs.Select(p => (Vector3)p.GetParameter(VertexParam.MaxVector)).ToArray());

            for (int j = 0; j < halfDS.Meshs.Count; j++)
            {
                foreach (var vertex in halfDS.Meshs[j].AroundVertex)
                {
                    int i = vertex.Index;
                    position.Add(vertex.Position);
                    voronoi.Add(KICalc.GetPseudoColor((float)voronoiParam.GetValue(i), voronoiParam.Min, voronoiParam.Max));
                    mean.Add(KICalc.GetPseudoColor((float)meanParam.GetValue(i), meanParam.Min, meanParam.Max));
                    gauss.Add(KICalc.GetPseudoColor((float)gaussParam.GetValue(i), gaussParam.Min, gaussParam.Max));
                    min.Add(KICalc.GetPseudoColor((float)minParam.GetValue(i), minParam.Min, minParam.Max));
                    max.Add(KICalc.GetPseudoColor((float)maxParam.GetValue(i), maxParam.Min, maxParam.Max));

                    minVector.Add(vertex.Position);
                    minVector.Add(vertex.Position + (Vector3)minVecParam.GetValue(i) * 0.05f);
                    minVector.Add(vertex.Position);
                    minVector.Add(vertex.Position - (Vector3)minVecParam.GetValue(i) * 0.05f);
                    minVecColor.Add(Vector3.UnitX);
                    minVecColor.Add(Vector3.UnitX);
                    minVecColor.Add(Vector3.UnitX);
                    minVecColor.Add(Vector3.UnitX);


                    maxVector.Add(vertex.Position);
                    maxVector.Add(vertex.Position + (Vector3)maxVecParam.GetValue(i) * 0.05f);
                    maxVector.Add(vertex.Position);
                    maxVector.Add(vertex.Position - (Vector3)maxVecParam.GetValue(i) * 0.05f);

                    maxVecColor.Add(Vector3.UnitY);
                    maxVecColor.Add(Vector3.UnitY);
                    maxVecColor.Add(Vector3.UnitY);
                    maxVecColor.Add(Vector3.UnitY);
                }
            }

            AddObject("voronoi", position, voronoi, GeometryType.Triangle);
            AddObject("mean", position, mean, GeometryType.Triangle);
            AddObject("gauss", position, gauss, GeometryType.Triangle);
            AddObject("min", position, min, GeometryType.Triangle);
            AddObject("max", position, max, GeometryType.Triangle);
            AddObject("minVector", minVector, minVecColor, GeometryType.Line);
            AddObject("maxVector", maxVector, maxVecColor, GeometryType.Line);

            return CommandResult.Success;
        }

        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// オブジェクトの追加
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="position">位置情報</param>
        /// <param name="color">色情報</param>
        private void AddObject(string name, List<Vector3> position, List<Vector3> color, GeometryType type)
        {
            Geometry geometry = new Geometry(name + " : " + renderObject.Name, position, null, color, null, null, type);
            RenderObject render = RenderObjectFactory.Instance.CreateRenderObject(name);
            render.SetGeometryInfo(geometry);
            render.ModelMatrix = renderObject.ModelMatrix;
            Global.RenderSystem.ActiveScene.AddObject(render);
        }
    }
}