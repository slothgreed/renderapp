using System;
using System.Collections.Generic;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Renderer;
using KI.Foundation.Command;

namespace RenderApp.Tool.Command
{
    /// <summary>
    /// Convexhullの作成コマンド
    /// </summary>
    public class CreateConvexHullCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        private ConvexHullCommandArgs convexCommandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public CreateConvexHullCommand(ConvexHullCommandArgs commandArgs)
        {
            convexCommandArgs = commandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult CanExecute()
        {
            return CommandUtility.CanCreatePolygon(convexCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns>成功値</returns>
        public override CommandResult Execute()
        {
            var targetObject = convexCommandArgs.TargetObject;
            var scene = convexCommandArgs.Scene;

            ConvexHullAlgorithm convexHull = new ConvexHullAlgorithm(targetObject.Polygon.Vertexs);

            Polygon polygon = new Polygon("ConvexHull:" + targetObject.Name, convexHull.ConvexPoint, KIPrimitiveType.Triangles);
            PolygonUtility.Setup(polygon);
            PolygonNode convex = new PolygonNode(polygon);
            convex.ModelMatrix = targetObject.ModelMatrix;
            scene.AddObject(convex);

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
    public class ConvexHullCommandArgs
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
        public ConvexHullCommandArgs(PolygonNode targetNode, Scene scene)
        {
            this.TargetObject = targetNode;
            this.Scene = scene;
        }
    }
}
