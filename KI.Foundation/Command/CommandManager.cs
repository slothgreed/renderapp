using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Utility;
namespace KI.Foundation.Command
{
    public class CommandManager
    {
        private static CommandManager _instance = new CommandManager();
        public static CommandManager Instance
        {
            get
            {
                return _instance;
            }
        }
        /// <summary>
        /// Listで管理、各ツールでenumで設定できる
        /// </summary>
        private List<CommandStack> StackList;

        public CommandManager()
        {
            StackList = new List<CommandStack>();
            StackList.Add(new CommandStack());
        }



        /// <summary>
        /// コマンドスタックの追加
        /// </summary>
        public void AddCommandStack()
        {
            StackList.Add(new CommandStack());
        }
        
        /// <summary>
        /// コマンドのクリア
        /// </summary>
        /// <param name="stack"></param>
        public void Clear(int stack = 0)
        {
            if (StackList.Count < stack)
            {
                StackList[stack].Commands.Clear();
            }
        }
        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="command"></param>
        /// <param name="undo"></param>
        /// <param name="stack"></param>
        /// <returns></returns>
        private string Execute(ICommand command, string commandArg, bool undo, int stack = 0)
        {
            if (StackList.Count < stack)
            {
                Logger.Log(Logger.LogLevel.Error, "command Stack Error");   
            }
            string error = "";
            error = command.CanExecute(commandArg);
            if (error != "")
            {
                Logger.Log(Logger.LogLevel.Warning, "Command Error",error); 

                return error;
            }

            error = command.Execute(commandArg);
            if (error != "")
            {
                Logger.Log(Logger.LogLevel.Warning, "Command Error", error); 
                return error;
            }

            if (undo == true)
            {
                StackList[stack].Push(command, commandArg);
            }

            return "";
        }

        /// <summary>
        /// Undo
        /// </summary>
        /// <param name="stack">コマンドリスト番号</param>
        public void Undo(int stack = 0)
        {
            if(StackList.Count < stack)
            {
                if(StackList[stack].Commands != null)
                {
                    CommandInfo param = StackList[stack].Pop();
                    if(!param.Command.Undo(param.CommandArg))
                    {
                        Logger.Log(Logger.LogLevel.Warning, "Undo Error"); 
                    }
                }
            }
        }
    }
}
