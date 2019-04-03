using KI.Asset;
using KI.Tool.Command;
using KI.UI.ViewModel;
using RenderApp.Model;
using RenderApp.Tool.Command;

namespace RenderApp.ViewModel
{
    public partial class SmoothingCommandViewModel : ViewModelBase
    {
        private int loopNum = 100;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        public SmoothingCommandViewModel(ViewModelBase parent)
            : base(parent)
        {
        }

        public string TargetObject
        {
            get
            {
                return Workspace.Instance.MainScene.SelectNode.Name;
            }
            set
            {

            }
        }

        public int LoopNum
        {
            get
            {
                return loopNum;
            }

            set
            {
                SetValue<int>(ref loopNum, value);
            }
        }

        private void ExecuteCommand()
        {
            CommandBase command = new SmoothingCommand(new SmoothingCommandArgs(Workspace.Instance.MainScene.SelectNode as RenderObject, loopNum));
            CommandManager.Instance.Execute(command, true);
        }
    }
}
