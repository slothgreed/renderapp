using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Core;

namespace KI.Tool.Command
{
    /// <summary>
    /// 複数のコマンドを一つのコマンドとして管理
    /// </summary>
    public class CommandGroup : CommandBase
    {
        /// <summary>
        /// グループ名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// コマンドスタック
        /// </summary>
        Stack<CommandBase> commandStack;

        /// <summary>
        /// UndoList
        /// </summary>
        private Stack<CommandBase> undoStack;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">グループ名</param>
        public CommandGroup(string name)
        {
            Name = name;
            commandStack = new Stack<CommandBase>();
        }

        /// <summary>
        /// コマンドの追加
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <param name="execute">即時実行するかどうか</param>
        public void AddCommand(CommandBase command, bool execute = true)
        {
            commandStack.Push(command);
            if (execute == true)
            {
                command.Execute();
            }
        }

        /// <summary>
        /// コマンドが実行できるかどうか
        /// </summary>
        /// <returns>成功</returns>
        public override CommandResult CanExecute()
        {
            foreach (var stack in commandStack)
            {
                if (stack.CanExecute() == CommandResult.Failed)
                {
                    return CommandResult.Failed;
                }
            }

            return CommandResult.Success;
        }

        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <returns>成功</returns>
        public override CommandResult Execute()
        {
            foreach (var stack in commandStack)
            {
                if (stack.Execute() == CommandResult.Failed)
                {
                    return CommandResult.Failed;
                }
            }

            return CommandResult.Success;
        }

        /// <summary>
        /// Undo
        /// </summary>
        /// <returns>成功</returns>
        public override CommandResult Undo()
        {
            IEnumerable<CommandBase> undoStack = commandStack.Reverse();
            foreach (var stack in undoStack)
            {
                if (stack.Undo() == CommandResult.Failed)
                {
                    return CommandResult.Failed;
                }
            }

            return CommandResult.Success;
        }

        /// <summary>
        /// Undo 1回のみ
        /// </summary>
        public CommandResult UndoOnce()
        {
            CommandBase command = commandStack.Pop();
            if (command.Undo() == CommandResult.Failed)
            {
                Logger.Log(Logger.LogLevel.Warning, "Undo Error");
                return CommandResult.Failed;
            }
            else
            {
                undoStack.Push(command);
                return CommandResult.Success;
            }
        }

        /// <summary>
        /// Redo 1回のみ
        /// </summary>
        /// <returns></returns>
        public CommandResult RedoOnce()
        {
            var command = undoStack.Pop();
            return command.Execute();
        }
    }
}
