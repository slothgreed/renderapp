﻿using System;
using System.Collections.Generic;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Gfx.Geometry;
using KI.Renderer;
using KI.Foundation.Command;

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

            var convexLine = new List<Vertex>();
            for (int i = 0; i < featureLine.GetConvexLines().Length; i++)
            {
                foreach (var line in featureLine.GetConvexLines()[i])
                {
                    convexLine.Add(new Vertex(convexLine.Count, line.Start.Position, OpenTK.Vector3.UnitX));
                    convexLine.Add(new Vertex(convexLine.Count, line.End.Position, OpenTK.Vector3.UnitX));
                }
            }

            var concaveLine = new List<Vertex>();
            for (int i = 0; i < featureLine.GetConcaveLine().Length; i++)
            {
                foreach (var line in featureLine.GetConcaveLine()[i])
                {
                    concaveLine.Add(new Vertex(convexLine.Count, line.Start.Position, OpenTK.Vector3.UnitX));
                    concaveLine.Add(new Vertex(convexLine.Count, line.End.Position, OpenTK.Vector3.UnitX));
                }
            }

            Polygon convexModel = new Polygon("Convex:" + targetObject.Name, convexLine, KI.Gfx.KIPrimitiveType.Lines);
            PolygonUtility.Setup(convexModel);
            PolygonNode convexObject = new PolygonNode(convexModel);
            convexObject.ModelMatrix = targetObject.ModelMatrix;
            scene.AddObject(convexObject);

            Polygon concaveModel = new Polygon("Concave:" + targetObject.Name, concaveLine, KI.Gfx.KIPrimitiveType.Lines);
            PolygonUtility.Setup(concaveModel);
            PolygonNode concaveObject = new PolygonNode(concaveModel);
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
        public PolygonNode TargetObject { get; private set; }

        /// <summary>
        /// シーン
        /// </summary>
        public Scene Scene { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetNode">対象オブジェクト</param>
        /// <param name="loopNum">ループ回数</param>
        public FeatureLineCommandArgs(PolygonNode targetNode, Scene scene)
        {
            this.TargetObject = targetNode;
            this.Scene = scene;
        }
    }
}
