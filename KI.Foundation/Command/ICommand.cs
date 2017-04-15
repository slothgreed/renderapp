
namespace KI.Foundation.Command
{
    public interface ICommand
    {
        /// <summary>
        /// 処理できるか？
        /// </summary>
        /// <returns></returns>
        string CanExecute(string commandArg);
        /// <summary>
        /// 処理の実行
        /// </summary>
        /// <returns></returns>
        string Execute(string commandArg);
        /// <summary>
        /// 処理のリセット
        /// </summary>
        /// <returns></returns>
        bool Reset();
        /// <summary>
        /// Undo
        /// </summary>
        /// <returns></returns>
        bool Undo(string commandArg);
    }
}
