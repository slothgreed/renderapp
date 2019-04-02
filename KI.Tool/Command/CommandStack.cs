using System.Collections.Generic;

namespace KI.Tool.Command
{
    /// <summary>
    /// コマンドスタック
    /// </summary>
    public class CommandStack
    {
        /// <summary>
        /// コマンドリスト
        /// </summary>
        private Stack<CommandBase> commands;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandStack()
        {
            commands = new Stack<CommandBase>();
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            commands.Clear();
        }

        /// <summary>
        /// コマンドのpush
        /// </summary>
        /// <param name="info">コマンド情報</param>
        public void Push(CommandBase info)
        {
            commands.Push(info);
        }

        /// <summary>
        /// コマンドのPop
        /// </summary>
        /// <returns>コマンド</returns>
        public CommandBase Pop()
        {
            return commands.Pop();
        }
    }
}
