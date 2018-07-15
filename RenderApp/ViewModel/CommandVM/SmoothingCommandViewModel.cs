using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Asset;
using KI.Foundation.Command;
using KI.Tool.Command;
using KI.UI.ViewModel;
using RenderApp.Globals;

namespace RenderApp.ViewModel
{
    public partial class SmoothingCommandViewModel : DockWindowViewModel
    {
        private int loopNum = 100;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        public SmoothingCommandViewModel(ViewModelBase parent)
            : base(parent, null, "Smoothing Command", Place.Floating)
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
