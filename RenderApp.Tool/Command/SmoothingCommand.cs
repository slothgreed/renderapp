using System;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Renderer;
using KI.Tool.Command;

namespace RenderApp.Tool.Command
{
    /// <summary>
    /// スムージングのコマンド
    /// </summary>
    public class SmoothingCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        private SmoothingCommandArgs smoothingCommandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public SmoothingCommand(SmoothingCommandArgs commandArgs)
        {
            smoothingCommandArgs = commandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <returns>結果</returns>
        public override CommandResult CanExecute()
        {
            if (smoothingCommandArgs.TargetObject == null)
            {
                return CommandResult.Failed;
            }

            if (smoothingCommandArgs.TargetObject.Polygon is HalfEdgeDS)
            {
                return CommandResult.Success;
            }

            return CommandResult.Failed;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns>結果</returns>
        public override CommandResult Execute()
        {
            var halfDS = smoothingCommandArgs.TargetObject.Polygon as HalfEdgeDS;

            var smoothing = new LaplaceSmoothingAlgorithm(halfDS, smoothingCommandArgs.LoopNum);
            smoothing.Calculate();

            smoothingCommandArgs.TargetObject.UpdateVertexBufferObject();

            return CommandResult.Success;
        }

        public override CommandResult Undo()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// スムージングコマンド
    /// </summary>
    public class SmoothingCommandArgs
    {
        /// <summary>
        /// 対象オブジェクト
        /// </summary>
        public RenderObject TargetObject { get; private set; }

        /// <summary>
        /// ループ回数
        /// </summary>
        public int LoopNum { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetNode">対象オブジェクト</param>
        /// <param name="loopNum">ループ回数</param>
        public SmoothingCommandArgs(RenderObject targetNode, int loopNum)
        {
            this.TargetObject = targetNode;
            this.LoopNum = loopNum;
        }
    }
}
