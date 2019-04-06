using System;
using System.Collections.Generic;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Gfx.Geometry;
using KI.Renderer;
using KI.Tool.Command;

namespace RenderApp.Tool.Command
{
    /// <summary>
    /// Convexhullの作成コマンド
    /// </summary>
    public class CreateFeatureLineCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        FeatureLineCommandArgs featureCommandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public CreateFeatureLineCommand(FeatureLineCommandArgs commandArgs)
        {
            this.featureCommandArgs = commandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult CanExecute()
        {
            return CommandUtility.CanCreatePolygon(featureCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult Execute()
        {
            var targetObject = featureCommandArgs.TargetObject;
            var scene = featureCommandArgs.Scene;

            FeatureLineAlgorithm featureLine = new FeatureLineAlgorithm(targetObject.Polygon as HalfEdgeDS);
            featureLine.Calculate();

            var convexLine = new List<Line>();
            for (int i = 0; i < featureLine.GetConvexLines().Length; i++)
            {
                foreach (var line in featureLine.GetConvexLines()[i])
                {
                    convexLine.Add(new Line(line.Start.Position, line.End.Position, OpenTK.Vector3.UnitX));
                }
            }

            var concaveLine = new List<Line>();
            for (int i = 0; i < featureLine.GetConcaveLine().Length; i++)
            {
                foreach (var line in featureLine.GetConcaveLine()[i])
                {
                    concaveLine.Add(new Line(line.Start.Position, line.End.Position, OpenTK.Vector3.UnitY));
                }
            }

            Polygon convexModel = new Polygon("Convex:" + targetObject.Name, convexLine);
            RenderObject convexObject = RenderObjectFactory.Instance.CreateRenderObject("Convex :" + targetObject.Name, convexModel);
            convexObject.ModelMatrix = targetObject.ModelMatrix;
            scene.AddObject(convexObject);

            Polygon concaveModel = new Polygon("Concave:" + targetObject.Name, concaveLine);
            RenderObject concaveObject = RenderObjectFactory.Instance.CreateRenderObject("Concave :" + targetObject.Name, concaveModel);
            concaveObject.ModelMatrix = targetObject.ModelMatrix;
            scene.AddObject(concaveObject);

            return CommandResult.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult Undo()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 曲率コマンド
    /// </summary>
    public class FeatureLineCommandArgs
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
        /// <param name="loopNum">ループ回数</param>
        public FeatureLineCommandArgs(RenderObject targetNode, Scene scene)
        {
            this.TargetObject = targetNode;
            this.Scene = scene;
        }
    }
}
