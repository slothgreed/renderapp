
namespace KI.Foundation.Command
{
    public interface ICommand
    {
        /// <summary>
        /// 処理できるか？
        /// </summary>
        /// <returns></returns>
        string CanExecute();
        /// <summary>
        /// 処理の実行
        /// </summary>
        /// <returns></returns>
        string Execute();
        /// <summary>
        /// Undo
        /// </summary>
        /// <returns></returns>
        string Undo();
    }
}
