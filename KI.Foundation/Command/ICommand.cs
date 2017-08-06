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
        CommandResult CanExecute(string commandArg);

        /// <summary>
        /// 処理の実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        CommandResult Execute(string commandArg);

        /// <summary>
        /// Undo
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        CommandResult Undo(string commandArg);
    }
}
