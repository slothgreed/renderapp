using System;
using KI.Foundation.Command;
using KI.Renderer;
using KI.Tool.Command;
using KI.UI.ViewModel;
using RenderApp.Globals;


namespace RenderApp.ViewModel
{
    public partial class VoxelViewModel : TabItemViewModel, IControllerViewModelBase
    {
        private int partitionNum = 64;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        public VoxelViewModel(ViewModelBase parent)
            : base(parent)
        {
        }

        public string TargetObject
        {
            get
            {
                return Workspace.MainScene.SelectNode.Name;
            }
            set
            {

            }
        }

        public int PartitionNum
        {
            get
            {
                return partitionNum;
            }

            set
            {
                SetValue<int>(ref partitionNum, value);
            }
        }

        public System.Windows.Media.Color SelectColor
        {
            get;
            set;
        }

        public override string Title
        {
            get
            {
                return "Voxel";
            }
        }

        private void ExecuteCommand()
        {
            ICommand command = new CreateVoxelCommand(Workspace.MainScene.SelectNode as RenderObject, new OpenTK.Vector3(SelectColor.R, SelectColor.G, SelectColor.B), PartitionNum);
            CommandManager.Instance.Execute(command, null, true);
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
