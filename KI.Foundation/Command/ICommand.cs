
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
        /// Undo
        /// </summary>
        /// <returns></returns>
        string Undo(string commandArg);
    }
}
