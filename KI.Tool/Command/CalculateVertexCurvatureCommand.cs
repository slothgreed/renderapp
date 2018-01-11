using System;
using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Parameter;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using KI.Gfx.KIShader;
using KI.Renderer;
using KI.Renderer.Material;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Tool.Command
{
    /// <summary>
    /// 曲率の算出コマンド
    /// </summary>
    public class CalculateVertexCurvatureCommand : ICommand
    {
        /// <summary>
        /// レンダリング形状
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// シーン
        /// </summary>
        private Scene scene;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="asset">算出オブジェクト</param>
        public CalculateVertexCurvatureCommand(Scene scene, KIObject asset)
        {
            this.scene = scene;
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

            if (renderObject.Polygon is HalfEdgeDS)
            {
                return CommandResult.Success;
            }

            return CommandResult.Failed;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public CommandResult Execute(string commandArg)
        {
            var halfDS = renderObject.Polygon as HalfEdgeDS;
            //var vertexInfo = new VertexCurvatureAlgorithm(halfDS);

            var voronoiParam = new ScalarParameter("Voronoi", halfDS.HalfEdgeVertexs.Select(p => p.Voronoi).ToArray());
            var meanParam = new ScalarParameter("MeanCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MeanCurvature).ToArray());
            var gaussParam = new ScalarParameter("GaussCurvature", halfDS.HalfEdgeVertexs.Select(p => p.GaussCurvature).ToArray());
            var minParam = new ScalarParameter("MinCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MinCurvature).ToArray());
            var maxParam = new ScalarParameter("MaxCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MaxCurvature).ToArray());
            var minVecParam = new VectorParameter("MinVector", halfDS.HalfEdgeVertexs.Select(p => p.MinDirection).ToArray());
            var maxVecParam = new VectorParameter("MaxVector", halfDS.HalfEdgeVertexs.Select(p => p.MaxDirection).ToArray());

            halfDS.AddParameter(voronoiParam);
            halfDS.AddParameter(meanParam);
            halfDS.AddParameter(gaussParam);
            halfDS.AddParameter(minParam);
            halfDS.AddParameter(maxParam);
            halfDS.AddParameter(minVecParam);
            halfDS.AddParameter(maxVecParam);

            var dirMinLine = new List<Vector3>();
            var dirMaxLine = new List<Vector3>();
            foreach (var position in halfDS.HalfEdgeVertexs)
            {
                var minStart = new Vector3(position.Position - position.MinDirection * 0.01f);
                var minEnd = new Vector3(position.Position + position.MinDirection * 0.01f);

                var maxStart = new Vector3(position.Position - position.MaxDirection * 0.01f);
                var maxEnd = new Vector3(position.Position + position.MaxDirection * 0.01f);

                dirMinLine.Add(minStart);
                dirMinLine.Add(minEnd);
                dirMaxLine.Add(maxStart);
                dirMaxLine.Add(maxEnd);
            }

            var parentNode = Global.Renderer.ActiveScene.FindNode(renderObject);

            VertexParameterMaterial voronoiMaterial = new VertexParameterMaterial(renderObject.Name + " : Voronoi",
                renderObject.PolygonMaterial.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                voronoiParam.Values);

            VertexParameterMaterial meanMaterial = new VertexParameterMaterial(renderObject.Name + " : MeanCurvature",
                renderObject.PolygonMaterial.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                meanParam.Values);

            VertexParameterMaterial gaussMaterial = new VertexParameterMaterial(renderObject.Name + " : GaussCurvature",
                renderObject.PolygonMaterial.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                gaussParam.Values);

            VertexParameterMaterial minMaterial = new VertexParameterMaterial(renderObject.Name + " : MinCurvature",
                renderObject.PolygonMaterial.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                minParam.Values);

            VertexParameterMaterial maxMaterial = new VertexParameterMaterial(renderObject.Name + " : MaxCurvature",
                renderObject.PolygonMaterial.VertexBuffer.ShallowCopy(),
                renderObject.Polygon.Type,
                renderObject.Shader,
                maxParam.Values);

            renderObject.Materials.Add(voronoiMaterial);
            renderObject.Materials.Add(meanMaterial);
            renderObject.Materials.Add(gaussMaterial);
            renderObject.Materials.Add(minMaterial);
            renderObject.Materials.Add(maxMaterial);

            scene.AddObject(voronoiMaterial, parentNode);
            scene.AddObject(meanMaterial, parentNode);
            scene.AddObject(gaussMaterial, parentNode);
            scene.AddObject(minMaterial, parentNode);
            scene.AddObject(maxMaterial, parentNode);

            var wireFrameShader = ShaderFactory.Instance.CreateShaderVF(ShaderCreater.Instance.Directory + @"GBuffer\WireFrame");
            var dirMinMaterial = new DirectionMaterial(renderObject.Name + " : MinDirection", dirMinLine.ToArray(), new Vector4(1, 0, 0, 1), wireFrameShader);
            var dirMaxMaterial = new DirectionMaterial(renderObject.Name + " : MaxDirection", dirMaxLine.ToArray(), new Vector4(0, 1, 0, 1), wireFrameShader);
            renderObject.Materials.Add(dirMinMaterial);
            renderObject.Materials.Add(dirMaxMaterial);
            scene.AddObject(dirMinMaterial, parentNode);
            scene.AddObject(dirMaxMaterial, parentNode);

            return CommandResult.Success;
        }

        public CommandResult Undo(string commandArg)
        {
            throw new NotImplementedException();
        }
    }
}
