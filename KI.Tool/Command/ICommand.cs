namespace KI.Tool.Command
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
    }
}
