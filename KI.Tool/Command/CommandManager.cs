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
        private List<CommandBase> commandStack;

        /// <summary>
        /// UndoList
        /// </summary>
        private List<CommandBase> undoStack;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandManager()
        {
            commandStack = new List<CommandBase>();
            undoStack = new List<CommandBase>();
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
                commandStack.Add(command);
            }

            return CommandResult.Success;
        }

        /// <summary>
        /// Undo
        /// </summary>
        public void Undo()
        {
            if (commandStack.Count > 0)
            {
                CommandBase command = commandStack[commandStack.Count - 1];
                commandStack.RemoveAt(commandStack.Count - 1);
                if (command.Undo() == CommandResult.Failed)
                {
                    Logger.Log(Logger.LogLevel.Warning, "Undo Error");
                }
                else
                {
                    undoStack.Add(command);
                }
            }
        }

        /// <summary>
        /// Redo
        /// </summary>
        public void Redo()
        {
            if (undoStack.Count > 0)
            {
                var command = undoStack[undoStack.Count - 1];
                undoStack.RemoveAt(undoStack.Count - 1);
                Execute(command, true);
            }
        }

        /// <summary>
        /// コマンドの削除
        /// </summary>
        public void RemoveCommand(CommandBase command)
        {
            commandStack.Remove(command);
            undoStack.Remove(command);
        }
    }
}
