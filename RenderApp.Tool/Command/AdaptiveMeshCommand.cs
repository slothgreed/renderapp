using System;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Renderer;
using KI.Tool.Command;

namespace RenderApp.Tool.Command
{
    /// <summary>
    /// 解適合格子メッシュの作成
    /// </summary>
    public class AdaptiveMeshCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        AdaptiveMeshCommandArgs adaptiveCommandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public AdaptiveMeshCommand(AdaptiveMeshCommandArgs commandArgs)
        {
            adaptiveCommandArgs = commandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <returns>結果</returns>
        public override CommandResult CanExecute()
        {
            return CommandUtility.CanCreatePolygon(adaptiveCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns>結果</returns>
        public override CommandResult Execute()
        {
            var halfDS = adaptiveCommandArgs.TargetObject.Polygon as HalfEdgeDS;
            var adaptiveMesh = new AdaptiveMeshAlgorithm(halfDS, 1);
            halfDS.UpdateVertexArray();

            return CommandResult.Success;
        }

        public override CommandResult Undo()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 解適合格子メッシュのコマンド引数
    /// </summary>
    public class AdaptiveMeshCommandArgs
    {
        /// <summary>
        /// ターゲットオブジェクト
        /// </summary>
        public PolygonNode TargetObject { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetObject">ターゲットオブジェクト</param>
        public AdaptiveMeshCommandArgs(PolygonNode targetObject)
        {
            TargetObject = targetObject;
        }
    }
}
