using System;
using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Foundation.Parameter;
using KI.Asset.Attribute;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// 曲率の算出コマンド
    /// </summary>
    public class VertexCurvatureCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        private VertexCurvatureCommandArgs curvateureCommandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public VertexCurvatureCommand(VertexCurvatureCommandArgs commandArgs)
        {
            curvateureCommandArgs = commandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <returns>結果</returns>
        public override CommandResult CanExecute()
        {
            return CommandUtility.CanCreatePolygon(curvateureCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns>結果</returns>
        public override CommandResult Execute()
        {
            var targetObject = curvateureCommandArgs.TargetObject;
            var halfDS = curvateureCommandArgs.TargetObject.Polygon as HalfEdgeDS;
            var scene = curvateureCommandArgs.Scene;
            //var vertexInfo = new VertexCurvatureAlgorithm(halfDS);

            var voronoiParam = new ScalarParameter("Voronoi", halfDS.HalfEdgeVertexs.Select(p => p.Voronoi).ToArray());
            var laplaceParam = new ScalarParameter("Laplace", halfDS.HalfEdgeVertexs.Select(p => p.Laplace).ToArray());
            var meanParam = new ScalarParameter("MeanCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MeanCurvature).ToArray());
            var gaussParam = new ScalarParameter("GaussCurvature", halfDS.HalfEdgeVertexs.Select(p => p.GaussCurvature).ToArray());
            var minParam = new ScalarParameter("MinCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MinDerivaribe).ToArray());
            var maxParam = new ScalarParameter("MaxCurvature", halfDS.HalfEdgeVertexs.Select(p => p.MaxDerivaribe).ToArray());

            var dirMinLine = halfDS.HalfEdgeVertexs.Select(p => p.MinDirection).ToArray();
            var dirMaxLine = halfDS.HalfEdgeVertexs.Select(p => p.MaxDirection).ToArray();
            var laplaceLine = halfDS.HalfEdgeVertexs.Select(p => p.LaplaceVector).ToArray();

            var parentNode = Global.Renderer.ActiveScene.FindNode(targetObject);

            var vertexShader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);

            VertexParameterAttribute voronoiAttribute = new VertexParameterAttribute(targetObject.Name + " : Voronoi",
                targetObject.VertexBuffer.ShallowCopy(),
                targetObject.Polygon.Type,
                vertexShader,
                voronoiParam.Values);

            VertexParameterAttribute laplaceAttribute = new VertexParameterAttribute(targetObject.Name + " : Laplace",
                targetObject.VertexBuffer.ShallowCopy(),
                targetObject.Polygon.Type,
                vertexShader,
                laplaceParam.Values);

            VertexParameterAttribute meanAttribute = new VertexParameterAttribute(targetObject.Name + " : MeanCurvature",
                targetObject.VertexBuffer.ShallowCopy(),
                targetObject.Polygon.Type,
                vertexShader,
                meanParam.Values);

            VertexParameterAttribute gaussAttribute = new VertexParameterAttribute(targetObject.Name + " : GaussCurvature",
                targetObject.VertexBuffer.ShallowCopy(),
                targetObject.Polygon.Type,
                vertexShader,
                gaussParam.Values);

            VertexParameterAttribute minAttribute = new VertexParameterAttribute(targetObject.Name + " : MinCurvature",
                targetObject.VertexBuffer.ShallowCopy(),
                targetObject.Polygon.Type,
                vertexShader,
                minParam.Values);

            VertexParameterAttribute maxAttribute = new VertexParameterAttribute(targetObject.Name + " : MaxCurvature",
                targetObject.VertexBuffer.ShallowCopy(),
                targetObject.Polygon.Type,
                vertexShader,
                maxParam.Values);

            //targetObject.Attributes.Add(voronoiAttribute);
            //targetObject.Attributes.Add(laplaceAttribute);
            //targetObject.Attributes.Add(meanAttribute);
            //targetObject.Attributes.Add(gaussAttribute);
            targetObject.Attributes.Add(minAttribute);
            targetObject.Attributes.Add(maxAttribute);

            //scene.AddObject(voronoiAttribute, parentNode);
            //scene.AddObject(laplaceAttribute, parentNode);
            //scene.AddObject(meanAttribute, parentNode);
            //scene.AddObject(gaussAttribute, parentNode);

            var wireFrameShader = ShaderCreater.Instance.CreateShader(ShaderType.WireFrame);

            var vertexs = targetObject.Polygon.Vertexs.Select(p =>p.Position).ToArray();
            var normals = targetObject.Polygon.Vertexs.Select(p => p.Normal).ToArray();
            var dirMinAttribute = new VertexDirectionAttribute(targetObject.Name + " : MinDirection", wireFrameShader, vertexs, dirMinLine, new Vector4(1, 0, 0, 1), normals);
            var dirMaxAttribute = new VertexDirectionAttribute(targetObject.Name + " : MaxDirection", wireFrameShader, vertexs, dirMaxLine, new Vector4(0, 1, 0, 1), normals);
            targetObject.Attributes.Add(dirMinAttribute);
            targetObject.Attributes.Add(dirMaxAttribute);

            var laplaceVecAttribute = new VertexDirectionAttribute(targetObject.Name + " : LaplaceVec", wireFrameShader, vertexs, laplaceLine, new Vector4(1, 0, 0, 1), normals);
            targetObject.Attributes.Add(laplaceVecAttribute);

            return CommandResult.Success;
        }

        public override CommandResult Undo()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 曲率コマンド
    /// </summary>
    public class VertexCurvatureCommandArgs
    {
        /// <summary>
        /// 対象オブジェクト
        /// </summary>
        public RenderObject TargetObject { get; private set; }

        /// <summary>
        /// シーン
        /// </summary>
        public Scene Scene { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetNode">対象オブジェクト</param>
        /// <param name="scene">シーン</param>
        public VertexCurvatureCommandArgs(RenderObject targetNode, Scene scene)
        {
            this.TargetObject = targetNode;
            this.Scene = scene;
        }
    }
}
