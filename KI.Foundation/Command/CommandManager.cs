using System.Collections.Generic;
using KI.Foundation.Core;
using System;

namespace KI.Foundation.Command
{

    /// <summary>
    /// コマンド実行後イベントハンドラ
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void NotifyCommandExecutedHandler(object sender, EventArgs e);

    /// <summary>
    /// コマンドUndo後イベントハンドラ
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void NotifyCommandUndoPerformedHandler(object sender, EventArgs e);

    /// <summary>
    /// コマンドRedo後イベントハンドラ
    /// </summary>
    /// <param name="sender">発生元</param>
    /// <param name="e">イベント</param>
    public delegate void NotifyCommandRedoPerformedHandler(object sender, EventArgs e);


    /// <summary>
    /// コマンドマネージャ
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// コマンド実行後イベントハンドラ
        /// </summary>
        public event NotifyCommandExecutedHandler OnCommandExecuted;

        /// <summary>
        /// コマンドUndo後イベントハンドラ
        /// </summary>
        public event NotifyCommandUndoPerformedHandler OnCommandUndoPerformed;

        /// <summary>
        /// コマンドRedo後イベントハンドラ
        /// </summary>
        public event NotifyCommandRedoPerformedHandler OnCommandRedoPerformed;

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
                    NotifyCommandUndoPerformed();
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
                NotifyCommandRedoPerformed();
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

        /// <summary>
        /// コマンド実行後イベント
        /// </summary>
        public virtual void NotifyCommandExecuted()
        {
            if(OnCommandExecuted != null)
            {
                OnCommandExecuted(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// コマンドUndo後イベント
        /// </summary>
        public virtual void NotifyCommandUndoPerformed()
        {
            if(OnCommandUndoPerformed != null)
            {
                OnCommandUndoPerformed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// コマンドRedoイベント
        /// </summary>
        public virtual void NotifyCommandRedoPerformed()
        {
            if (OnCommandRedoPerformed != null)
            {
                OnCommandRedoPerformed(this, EventArgs.Empty);
            }
        }
    }
}
