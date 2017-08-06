using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Foundation.Command
{
    /// <summary>
    /// コマンドスタック
    /// </summary>
    public class CommandStack
    {
        /// <summary>
        /// コマンドリスト
        /// </summary>
        private Stack<CommandInfo> commands;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandStack()
        {
            commands = new Stack<CommandInfo>();
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
        public void Push(CommandInfo info)
        {
            commands.Push(info);
        }

        /// <summary>
        /// コマンドのpush
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <param name="commandArg">コマンド引数</param>
        public void Push(ICommand command, string commandArg)
        {
            Push(new CommandInfo(command, commandArg));
        }

        /// <summary>
        /// コマンドのPop
        /// </summary>
        /// <returns>コマンド</returns>
        public CommandInfo Pop()
        {
            return commands.Pop();
        }
    }

    /// <summary>
    /// コマンド情報
    /// </summary>
    public class CommandInfo
    {
        /// <summary>
        /// コマンド情報
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <param name="commandArg">コマンド引数</param>
        public CommandInfo(ICommand command, string commandArg)
        {
            Command = command;
            CommandArg = commandArg;
        }

        /// <summary>
        /// コマンド
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// コマンド引数
        /// </summary>
        public string CommandArg { get; private set; }
    }
}
