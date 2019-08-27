using KI.Foundation.Command;
using KI.Presenter.ViewModel;
using KI.Renderer;
using RenderApp.Model;
using RenderApp.Tool.Command;

namespace RenderApp.ViewModel
{
    public partial class SubdivisionCommandViewModel : ViewModelBase
    {
        private int loopNum = 4;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        public SubdivisionCommandViewModel(ViewModelBase parent)
            : base(parent)
        {
        }

        public SceneNode TargetObject
        {
            get
            {
                return Workspace.Instance.MainScene.SelectNode;
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
                CommandBase command = new SubdivisionCommand(new SudivisionCommandArgs(TargetObject as AnalyzePolygonNode, loopNum));
                MainWindowViewModel.Instance.CommandManager.Execute(command, true);
            }
        }
    }
}
