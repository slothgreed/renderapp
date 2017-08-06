namespace KI.Foundation.Command
{
    public enum CommandState
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
        CommandState CanExecute(string commandArg);

        /// <summary>
        /// 処理の実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        CommandState Execute(string commandArg);

        /// <summary>
        /// Undo
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        CommandState Undo(string commandArg);
    }
}
