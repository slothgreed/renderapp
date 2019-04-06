using KI.Asset;
using KI.Tool.Command;
using KI.UI.ViewModel;
using RenderApp.Model;
using RenderApp.Tool.Command;

namespace RenderApp.ViewModel
{
    public partial class IsoLineCommandViewModel : ViewModelBase
    {
        private float space = 0.05f;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        public IsoLineCommandViewModel(ViewModelBase parent)
            : base(parent)
        {
        }

        public string TargetObject
        {
            get
            {
                if (Workspace.Instance.MainScene.SelectNode == null)
                {
                    return string.Empty;
                }

                return Workspace.Instance.MainScene.SelectNode.Name;
            }
            set
            {

            }
        }

        public float Space
        {
            get
            {
                return space;
            }

            set
            {
                SetValue<float>(ref space, value);
            }
        }

        private void ExecuteCommand()
        {
            CommandBase command = new CreateIsoLineCommand(new IsoLineCommandArgs(Workspace.Instance.MainScene.SelectNode as RenderObject,Workspace.Instance.MainScene, Space));
            CommandManager.Instance.Execute(command, true);
        }
    }
}
