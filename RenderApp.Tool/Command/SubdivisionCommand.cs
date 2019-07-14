using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Foundation.Command;
using RenderApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.Tool.Command
{
    public class SubdivisionCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        private SudivisionCommandArgs subdivisionCommandArgs;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public SubdivisionCommand(SudivisionCommandArgs commandArgs)
        {
            subdivisionCommandArgs = commandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <returns>結果</returns>
        public override CommandResult CanExecute()
        {
            if (subdivisionCommandArgs.TargetObject == null)
            {
                return CommandResult.Failed;
            }

            if (subdivisionCommandArgs.TargetObject.Polygon is HalfEdgeDS)
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
            var halfDS = subdivisionCommandArgs.TargetObject.Polygon as HalfEdgeDS;

            var subdivision = new LoopSubdivisionAlgorithm();
            subdivision.Calculate(halfDS, subdivisionCommandArgs.LoopNum);

            subdivisionCommandArgs.TargetObject.UpdateVertexBufferObject();

            return CommandResult.Success;
        }

        public override CommandResult Undo()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 細分割コマンド引数
    /// </summary>
    public class SudivisionCommandArgs
    {
        /// <summary>
        /// 対象オブジェクト
        /// </summary>
        public AnalyzePolygonNode TargetObject { get; private set; }

        /// <summary>
        /// ループ回数
        /// </summary>
        public int LoopNum { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetNode">対象オブジェクト</param>
        /// <param name="loopNum">ループ回数</param>
        public SudivisionCommandArgs(AnalyzePolygonNode targetNode, int loopNum)
        {
            this.TargetObject = targetNode;
            this.LoopNum = loopNum;
        }
    }
}
