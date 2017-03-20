
namespace KI.Foundation.Command
{
    public interface ICommand
    {
        /// <summary>
        /// 処理できるか？
        /// </summary>
        /// <returns></returns>
        bool CanExecute();
        /// <summary>
        /// 処理の実行
        /// </summary>
        /// <returns></returns>
        bool Execute();
        /// <summary>
        /// 処理のリセット
        /// </summary>
        /// <returns></returns>
        bool Reset();
        /// <summary>
        /// Undo
        /// </summary>
        /// <returns></returns>
        bool Undo();
    }
}
