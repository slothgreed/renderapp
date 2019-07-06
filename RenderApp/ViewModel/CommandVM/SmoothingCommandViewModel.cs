using KI.Asset;
using KI.Renderer;
using KI.Foundation.Command;
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
            if (Workspace.Instance.MainScene.SelectNode is AnalyzePolygonNode)
            {
                CommandBase command = new SmoothingCommand(new SmoothingCommandArgs(Workspace.Instance.MainScene.SelectNode as AnalyzePolygonNode, loopNum));
                MainWindowViewModel.Instance.CommandManager.Execute(command, true);
            }
        }
    }
}
