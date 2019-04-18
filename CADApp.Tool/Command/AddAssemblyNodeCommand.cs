using CADApp.Model;
using CADApp.Model.Node;
using KI.Renderer;
using KI.Tool.Command;

namespace CADApp.Tool.Command
{
    /// <summary>
    /// Rectangle の追加コマンド
    /// </summary>
    public class AddAssemblyNodeCommand : CommandBase
    {

        private AddAssemblyNodeCommandArgs commandArgs;

        public AddAssemblyNodeCommand(AddAssemblyNodeCommandArgs args)
        {
            commandArgs = args;
        }

        public AddAssemblyNodeCommand(AssemblyNode target, Assembly assembly, SceneNode parent)
        {
            commandArgs = new AddAssemblyNodeCommandArgs(target, assembly, parent);
        }

        public override CommandResult CanExecute()
        {
            return CommandResult.Success;
        }

        public override CommandResult Execute()
        {
            var node = commandArgs.Node;
            commandArgs.Node.Assembly = commandArgs.Assembly;
            commandArgs.Parent.AddChild(node);

            return CommandResult.Success;
        }

        public override CommandResult Undo()
        {
            commandArgs.Parent.RemoveChild(commandArgs.Node);
            commandArgs.Node.Dispose();

            return CommandResult.Success;
        }

        /// <summary>
        /// 曲率コマンド
        /// </summary>
        public class AddAssemblyNodeCommandArgs
        {
            /// <summary>
            /// 追加オブジェクト
            /// </summary>
            public AssemblyNode Node { get; private set; }

            /// <summary>
            /// アセンブリ
            /// </summary>
            public Assembly Assembly { get; private set; }

            /// <summary>
            /// 親ノード
            /// </summary>
            public SceneNode Parent { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="targetNode">対象オブジェクト</param>
            /// <param name="parent">親ノード</param>
            public AddAssemblyNodeCommandArgs(AssemblyNode targetNode, Assembly assembly, SceneNode parent)
            {
                this.Node = targetNode;
                this.Assembly = assembly;
                this.Parent = parent;
            }
        }
    }
}
