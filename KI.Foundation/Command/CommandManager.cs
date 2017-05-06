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
                CommandList[stack].Clear();
                UndoList[stack].Clear();
            }
        }
        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="command"></param>
        /// <param name="undo"></param>
        /// <param name="stack"></param>
        /// <returns></returns>
        public string Execute(ICommand command, string commandArg = null, bool undo = true, int stack = 0)
        {
            if (CommandList.Count < stack)
            {
                Logger.Log(Logger.LogLevel.Error, "command Stack Error");
            }
            string error = string.Empty;
            error = command.CanExecute(commandArg);
            if (error != CommandResource.Success)
            {
                Logger.Log(Logger.LogLevel.Warning, "Command CanExecute Error", error);
                return error;
            }

            error = command.Execute(commandArg);
            if (error != CommandResource.Success)
            {
                Logger.Log(Logger.LogLevel.Warning, "Command Execute Error", error);
                return error;
            }

            if (undo == true)
            {
                CommandList[stack].Push(command, commandArg);
            }

            return CommandResource.Success;
        }

        /// <summary>
        /// Undo
        /// </summary>
        /// <param name="stack">コマンドリスト番号</param>
        public void Undo(int stack = 0)
        {
            if (CommandList.Count < stack)
            {
                CommandInfo info = CommandList[stack].Pop();
                if (info.Command.Undo(info.CommandArg) == CommandResource.Failed)
                {
                    Logger.Log(Logger.LogLevel.Warning, "Undo Error");
                }
                else
                {
                    UndoList[stack].Push(info);
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
                CommandInfo info = UndoList[stack].Pop();
                Execute(info.Command, info.CommandArg, true, stack);
            }
        }
    }
}
