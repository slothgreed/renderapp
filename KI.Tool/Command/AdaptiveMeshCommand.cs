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
    /// 解適合格子メッシュの作成
    /// </summary>
    public class AdaptiveMeshCommand : CommandBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">算出オブジェクト</param>
        public AdaptiveMeshCommand(AdaptiveMeshCommandArgs commandArgs)
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
            var adaptiveCommandArgs = commandArg as AdaptiveMeshCommandArgs;
            return CommandUtility.CanCreatePolygon(adaptiveCommandArgs.TargetObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>結果</returns>
        public override CommandResult Execute(CommandArgsBase commandArg)
        {
            var adaptiveCommandArgs = commandArg as AdaptiveMeshCommandArgs;
            var halfDS = adaptiveCommandArgs.TargetObject.Polygon as HalfEdgeDS;

            var adaptiveMesh = new AdaptiveMeshAlgorithm(halfDS, 1);

            halfDS.UpdateVertexArray();

            return CommandResult.Success;
        }

        public override CommandResult Undo(CommandArgsBase commandArg)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 解適合格子メッシュのコマンド引数
    /// </summary>
    public class AdaptiveMeshCommandArgs : CommandArgsBase
    {
        /// <summary>
        /// ターゲットオブジェクト
        /// </summary>
        public RenderObject TargetObject { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetObject">ターゲットオブジェクト</param>
        public AdaptiveMeshCommandArgs(RenderObject targetObject)
        {
            TargetObject = targetObject;
        }
    }
}
