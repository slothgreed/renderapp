using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Foundation.Command
{
    public class CommandInfo
    {
        public CommandInfo(ICommand command, string commandArg)
        {
            Command = command;
            CommandArg = commandArg;
        }

        public ICommand Command { get; set; }
        public string CommandArg { get; set; }
    }

    public class CommandStack
    {
        private Stack<CommandInfo> Commands;

        public CommandStack()
        {
            Commands = new Stack<CommandInfo>();
        }
        public void Clear()
        {
            Commands.Clear();
        }
        public void Push(CommandInfo info)
        {
            Commands.Push(info);
        }
        public void Push(ICommand command, string commandArg)
        {
            Commands.Push(new CommandInfo(command, commandArg));
        }

        public CommandInfo Pop()
        {
            return Commands.Pop();
        }
    }
}
