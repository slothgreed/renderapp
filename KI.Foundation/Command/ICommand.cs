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
        public CommandArgsBase CommandArgs { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandArgs">コマンド引数</param>
        public CommandBase(CommandArgsBase commandArgs)
        {
            CommandArgs = commandArgs;
        }

        /// <summary>
        /// 処理できるか？
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public abstract CommandResult CanExecute(CommandArgsBase commandArg);

        /// <summary>
        /// 処理の実行
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public abstract CommandResult Execute(CommandArgsBase commandArg);

        /// <summary>
        /// Undo
        /// </summary>
        /// <param name="commandArg">コマンド引数</param>
        /// <returns>成功値</returns>
        public abstract CommandResult Undo(CommandArgsBase commandArg);
    }

    /// <summary>
    /// コマンド引数
    /// </summary>
    public interface CommandArgsBase
    {

    }
}
