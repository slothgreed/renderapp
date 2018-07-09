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
    public class SmoothingCommand : ICommand
    {
        public SmoothingCommandArgs commandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">算出オブジェクト</param>
        public SmoothingCommand(SmoothingCommandArgs smoothingCommandArgs)
        {
            commandArgs = smoothingCommandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public CommandResult CanExecute(CommandArgs commandArg)
        {
            if (commandArgs.TargetObject == null)
            {
                return CommandResult.Failed;
            }

            if (commandArgs.TargetObject.Polygon is HalfEdgeDS)
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
        public CommandResult Execute(CommandArgs commandArg)
        {
            var halfDS = commandArgs.TargetObject.Polygon as HalfEdgeDS;

            var smoothing = new LaplaceSmoothingAlgorithm(halfDS, commandArgs.LoopNum);
            smoothing.Calculate();

            halfDS.UpdateVertexArray();

            return CommandResult.Success;
        }

        public CommandResult Undo(CommandArgs commandArg)
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
        public RenderObject TargetObject;

        /// <summary>
        /// ループ回数
        /// </summary>
        public int LoopNum;

        public SmoothingCommandArgs(RenderObject targetNode, int loopNum)
        {
            this.TargetObject = targetNode;
            this.LoopNum = loopNum;
        }
    }
}
