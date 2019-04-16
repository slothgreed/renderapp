using System.Collections.Generic;
using KI.Foundation.Core;

namespace KI.Tool.Command
{
    /// <summary>
    /// コマンドマネージャ
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// Listで管理、各ツールでenumで設定できる
        /// </summary>
        private Stack<CommandBase> commandStack;

        /// <summary>
        /// UndoList
        /// </summary>
        private Stack<CommandBase> undoStack;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandManager()
        {
            commandStack = new Stack<CommandBase>();
            undoStack = new Stack<CommandBase>();
        }

        /// <summary>
        /// コマンドのクリア
        /// </summary>
        public void Clear()
        {
            commandStack.Clear();
            undoStack.Clear();
        }

        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <param name="commandArg">コマンド引数</param>
        /// <param name="undo">undoできるか</param>
        /// <returns>成功したか</returns>
        public CommandResult Execute(CommandBase command, bool undo = true)
        {
            CommandResult error = CommandResult.None;
            error = command.CanExecute();
            if (error != CommandResult.Success)
            {
                Logger.Log(Logger.LogLevel.Warning, "Command CanExecute Error", error.ToString());
                return error;
            }

            error = command.Execute();
            if (error != CommandResult.Success)
            {
                Logger.Log(Logger.LogLevel.Warning, "Command Execute Error", error.ToString());
                return error;
            }

            if (undo == true)
            {
                commandStack.Push(command);
            }

            return CommandResult.Success;
        }

        /// <summary>
        /// Undo
        /// </summary>
        public void Undo()
        {
            CommandBase command = commandStack.Pop();
            if (command.Undo() == CommandResult.Failed)
            {
                Logger.Log(Logger.LogLevel.Warning, "Undo Error");
            }
            else
            {
                undoStack.Push(command);
            }
        }

        /// <summary>
        /// Redo
        /// </summary>
        public void Redo()
        {
            var command = undoStack.Pop();
            Execute(command, true);
        }
    }
}
