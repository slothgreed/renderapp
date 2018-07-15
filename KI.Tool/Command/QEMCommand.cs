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
    /// QEMの実行
    /// </summary>
    public class QEMCommand : CommandBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">算出オブジェクト</param>
        public QEMCommand(QEMCommandArgs commandArgs)
            :base(commandArgs)
        {
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public override CommandResult CanExecute(CommandArgsBase commandArg)
        {
            var qemCommandArgs = commandArg as VertexCurvatureCommandArgs;
            return CommandUtility.CanCreatePolygon(qemCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public override CommandResult Execute(CommandArgsBase commandArg)
        {
            var qemCommandArgs = commandArg as VertexCurvatureCommandArgs;
            var halfDS = qemCommandArgs.TargetObject.Polygon as HalfEdgeDS;

            var adaptiveMesh = new QEMAlgorithm(halfDS, halfDS.Vertexs.Count / 2);

            halfDS.UpdateVertexArray();

            return CommandResult.Success;
        }

        public override CommandResult Undo(CommandArgsBase commandArg)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// QEMのコマンド引数
    /// </summary>
    public class QEMCommandArgs : CommandArgsBase
    {
        /// <summary>
        /// ターゲットオブジェクト
        /// </summary>
        public RenderObject TargetObject { get; private set; }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetObject">ターゲットオブジェクト</param>
        public QEMCommandArgs(RenderObject targetObject)
        {
            TargetObject = targetObject;
        }
    }

}
