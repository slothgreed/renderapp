using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Renderer;
using OpenTK;

namespace KI.Tool.Command
{
    /// <summary>
    /// ワイヤフレームの作成
    /// </summary>
    public class CreateWireFrameCommand : CreateModelCommandBase, ICommand
    {
        /// <summary>
        /// 形状
        /// </summary>
        private RenderObject renderObject;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="asset">作成するオブジェクト</param>
        public CreateWireFrameCommand(KIObject asset)
        {
            renderObject = asset as RenderObject;
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult CanExecute(string commandArg)
        {
            return CanCreatePolygon(renderObject);
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult Execute(string commandArg)
        {
            renderObject.Polygon.CreateWireFrame(Vector3.Zero);
            return CommandResult.Success;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public CommandResult Undo(string commandArg)
        {
            return CommandResult.Failed;
        }
    }
}
