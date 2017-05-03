using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Foundation.Command
{
    public class CommandStack
    {
        public Stack<ICommand> Commands;

        public CommandStack()
        {
            Commands = new Stack<ICommand>();
        }
        public void Push(ICommand command)
        {
            Commands.Push(command);
        }

        public ICommand Pop()
        {
            return Commands.Pop();
        }
    }
}
