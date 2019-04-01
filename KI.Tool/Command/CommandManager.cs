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
        private List<CommandStack> commandList;

        /// <summary>
        /// UndoList
        /// </summary>
        private List<CommandStack> undoList;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandManager()
        {
            commandList = new List<CommandStack>();
            commandList.Add(new CommandStack());
            undoList = new List<CommandStack>();
            undoList.Add(new CommandStack());
        }

        /// <summary>
        /// シングルトン
        /// </summary>
        public static CommandManager Instance { get; } = new CommandManager();

        /// <summary>
        /// コマンドスタックの追加
        /// </summary>
        public void AddCommandStack()
        {
            commandList.Add(new CommandStack());
            undoList.Add(new CommandStack());
        }

        /// <summary>
        /// コマンドのクリア
        /// </summary>
        /// <param name="stack">コマンドリスト番号</param>
        public void Clear(int stack = 0)
        {
            if (commandList.Count < stack)
            {
                commandList[stack].Clear();
                undoList[stack].Clear();
            }
        }

        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <param name="commandArg">コマンド引数</param>
        /// <param name="undo">undoできるか</param>
        /// <param name="stack">コマンドリスト番号</param>
        /// <returns>成功したか</returns>
        public CommandResult Execute(CommandBase command, bool undo = true, int stack = 0)
        {
            if (commandList.Count < stack)
            {
                Logger.Log(Logger.LogLevel.Error, "command Stack Error");
            }

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
                commandList[stack].Push(command);
            }

            return CommandResult.Success;
        }

        /// <summary>
        /// Undo
        /// </summary>
        /// <param name="stack">コマンドリスト番号</param>
        public void Undo(int stack = 0)
        {
            if (commandList.Count < stack)
            {
                CommandBase command = commandList[stack].Pop();
                if (command.Undo() == CommandResult.Failed)
                {
                    Logger.Log(Logger.LogLevel.Warning, "Undo Error");
                }
                else
                {
                    undoList[stack].Push(command);
                }
            }
        }

        /// <summary>
        /// Redo
        /// </summary>
        /// <param name="stack">コマンドリスト番号</param>
        public void Redo(int stack = 0)
        {
            if (undoList.Count < stack)
            {
                var command = undoList[stack].Pop();
                Execute(command, true, stack);
            }
        }
    }
}
