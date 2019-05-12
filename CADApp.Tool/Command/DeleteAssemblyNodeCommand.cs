using CADApp.Model;
using CADApp.Model.Node;
using KI.Foundation.Tree;
using KI.Renderer;
using KI.Tool.Command;

namespace CADApp.Tool.Command
{
    /// <summary>
    /// アセンブリの追加コマンド
    /// </summary>
    public class DeleteAssemblyNodeCommand : CommandBase
    {
        public SceneNode parentNode;
        public Assembly assembly;
        private DeleteAssemblyNodeCommandArgs commandArgs;

        public DeleteAssemblyNodeCommand(DeleteAssemblyNodeCommandArgs args)
        {
            commandArgs = args;
            assembly = commandArgs.Node.Assembly;
            parentNode = (SceneNode)args.Node.Parent;
        }

        public override CommandResult CanExecute()
        {
            return CommandResult.Success;
        }

        public override CommandResult Execute()
        {
            parentNode.RemoveChild(commandArgs.Node);
            commandArgs.Node.Dispose();
            return CommandResult.Success;
        }

        public override CommandResult Undo()
        {
            // assenblyの設定時にVertexBufferが作られる。
            commandArgs.Node.Assembly = assembly;

            parentNode.AddChild(commandArgs.Node);
            return CommandResult.Success;
        }
    }


    /// <summary>
    /// アセンブリ削除コマンド
    /// </summary>
    public class DeleteAssemblyNodeCommandArgs
    {
        /// <summary>
        /// 追加オブジェクト
        /// </summary>
        public AssemblyNode Node { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetNode">対象オブジェクト</param>
        public DeleteAssemblyNodeCommandArgs(AssemblyNode targetNode)
        {
            this.Node = targetNode;
        }
    }
}
