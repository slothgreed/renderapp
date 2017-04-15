using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Foundation.Command
{
    public class CommandInfo
    {
        public CommandInfo()
        {

        }
        public CommandInfo(ICommand com,string str)
        {
            Command = com;
            CommandArg = str;
        }
        public ICommand Command;
        public string CommandArg;
    }

    public class CommandStack
    {
        public Stack<CommandInfo> Commands;

        public void Push(ICommand command, string commandStr)
        {
            Commands.Push(new CommandInfo(command, commandStr));
        }

        public void Push(CommandInfo command)
        {
            Commands.Push(command);
        }
        
        public CommandInfo Pop()
        {
            return Commands.Pop();
        }
    }
}
