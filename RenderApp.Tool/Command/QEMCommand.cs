using System;
using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Asset;
using KI.Renderer;
using KI.Tool.Command;

namespace RenderApp.Tool.Command
{
    /// <summary>
    /// QEMの実行
    /// </summary>
    public class QEMCommand : CommandBase
    {
        /// <summary>
        /// コマンド引数
        /// </summary>
        private QEMCommandArgs qemCommandArgs;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public QEMCommand(QEMCommandArgs commandArgs)
        {
            qemCommandArgs = commandArgs;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public override CommandResult CanExecute()
        {
            return CommandUtility.CanCreatePolygon(qemCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public override CommandResult Execute()
        {
            var halfDS = qemCommandArgs.TargetObject.Polygon as HalfEdgeDS;

            var adaptiveMesh = new QEMAlgorithm(halfDS, halfDS.Vertexs.Count / 2);

            halfDS.UpdateVertexArray();

            return CommandResult.Success;
        }

        public override CommandResult Undo()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// QEMのコマンド引数
    /// </summary>
    public class QEMCommandArgs
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
