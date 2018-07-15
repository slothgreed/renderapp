using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Asset;
using KI.Gfx;

namespace KI.Tool.Command
{
    /// <summary>
    /// スムージングのコマンド
    /// </summary>
    public class SmoothingCommand : CommandBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public SmoothingCommand(SmoothingCommandArgs commandArgs)
            : base(commandArgs)
        {
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public override CommandResult CanExecute(CommandArgsBase commandArg)
        {
            SmoothingCommandArgs smoothingCommandArgs = commandArg as SmoothingCommandArgs;
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
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public override CommandResult Execute(CommandArgsBase commandArg)
        {
            SmoothingCommandArgs smoothingCommandArgs = commandArg as SmoothingCommandArgs;

            var halfDS = smoothingCommandArgs.TargetObject.Polygon as HalfEdgeDS;

            var smoothing = new LaplaceSmoothingAlgorithm(halfDS, smoothingCommandArgs.LoopNum);
            smoothing.Calculate();

            halfDS.UpdateVertexArray();

            return CommandResult.Success;
        }

        public override CommandResult Undo(CommandArgsBase commandArg)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// スムージングコマンド
    /// </summary>
    public class SmoothingCommandArgs : CommandArgsBase
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
