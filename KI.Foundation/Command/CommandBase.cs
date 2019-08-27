using System;

namespace KI.Foundation.Command
{
    /// <summary>
    /// コマンドの結果
    /// </summary>
    public enum CommandResult
    {
        None,
        Success,
        Failed
    }

    /// <summary>
    /// コマンドのベースクラス
    /// </summary>
    public abstract class CommandBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public CommandBase()
        {
        }

        /// <summary>
        /// 処理できるか？
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public abstract CommandResult CanExecute();

        /// <summary>
        /// 処理の実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public abstract CommandResult Execute();

        /// <summary>
        /// Undo
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public abstract CommandResult Undo();

        /// <summary>
        /// コマンド変更後に発行するイベント
        /// </summary>
        /// <returns>変更イベント</returns>
        public virtual EventArgs NotifyExected() { return EventArgs.Empty; }

        /// <summary>
        /// コマンドUndo後に発行するイベント
        /// </summary>
        /// <returns>変更イベント</returns>
        public virtual EventArgs NotifyUndoPerformed() { return NotifyExected(); }

        /// <summary>
        /// コマンドRedo後に発行するイベント
        /// </summary>
        /// <returns>変更イベント</returns>
        public virtual EventArgs NotifyRedoPerformed() { return EventArgs.Empty; }
    }
}
