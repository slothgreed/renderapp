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
    /// コマンドのインタフェースクラス
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 処理できるか？
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        CommandResult CanExecute(CommandArgs commandArg);

        /// <summary>
        /// 処理の実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        CommandResult Execute(CommandArgs commandArg);

        /// <summary>
        /// Undo
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        CommandResult Undo(CommandArgs commandArg);
    }

    /// <summary>
    /// コマンド引数
    /// </summary>
    public interface CommandArgs
    {

    }
}
