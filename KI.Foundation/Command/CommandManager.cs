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
        private List<CommandStack> CommandList;
        private List<CommandStack> UndoList;

        public CommandManager()
        {
            CommandList = new List<CommandStack>();
            CommandList.Add(new CommandStack());
            UndoList = new List<CommandStack>();
            UndoList.Add(new CommandStack());
        }



        /// <summary>
        /// コマンドスタックの追加
        /// </summary>
        public void AddCommandStack()
        {
            CommandList.Add(new CommandStack());
            UndoList.Add(new CommandStack());
        }
        
        /// <summary>
        /// コマンドのクリア
        /// </summary>
        /// <param name="stack"></param>
        public void Clear(int stack = 0)
        {
            if (CommandList.Count < stack)
            {
                CommandList[stack].Commands.Clear();
                UndoList[stack].Commands.Clear();
            }
        }
        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="command"></param>
        /// <param name="undo"></param>
        /// <param name="stack"></param>
        /// <returns></returns>
        public string Execute(ICommand command, bool undo, int stack = 0)
        {
            if (CommandList.Count < stack)
            {
                Logger.Log(Logger.LogLevel.Error, "command Stack Error");   
            }
            string error = string.Empty;
            error = command.CanExecute();
            if (error != CommandResource.Success)
            {
                Logger.Log(Logger.LogLevel.Warning, "Command CanExecute Error",error); 
                return error;
            }

            error = command.Execute();
            if (error != CommandResource.Success)
            {
                Logger.Log(Logger.LogLevel.Warning, "Command Execute Error", error); 
                return error;
            }

            if (undo == true)
            {
                CommandList[stack].Push(command);
            }

            return CommandResource.Success;
        }

        /// <summary>
        /// Undo
        /// </summary>
        /// <param name="stack">コマンドリスト番号</param>
        public void Undo(int stack = 0)
        {
            if(CommandList.Count < stack)
            {
                if(CommandList[stack].Commands != null)
                {
                    ICommand command = CommandList[stack].Pop();
                    if (command.Undo() == CommandResource.Failed)
                    {
                        Logger.Log(Logger.LogLevel.Warning, "Undo Error");
                    }
                    else
                    {
                        UndoList[stack].Push(command);
                    }
                }
            }
        }

        /// <summary>
        /// Redo
        /// </summary>
        /// <param name="stack"></param>
        public void Redo(int stack = 0)
        {
            if (UndoList.Count < stack)
            {
                if (UndoList[stack].Commands != null)
                {
                    Execute(UndoList[stack].Commands.Pop(), true, stack);
                }
            }
        }
    }
}
