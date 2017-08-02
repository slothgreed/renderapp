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
        private Stack<CommandInfo> commands;

        public CommandStack()
        {
            commands = new Stack<CommandInfo>();
        }

        public void Clear()
        {
            commands.Clear();
        }

        public void Push(CommandInfo info)
        {
            commands.Push(info);
        }

        public void Push(ICommand command, string commandArg)
        {
            commands.Push(new CommandInfo(command, commandArg));
        }

        public CommandInfo Pop()
        {
            return commands.Pop();
        }
    }
}
