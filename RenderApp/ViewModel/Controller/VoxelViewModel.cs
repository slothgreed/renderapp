using System;
using KI.Foundation.Command;
using KI.Renderer;
using KI.Tool.Command;
using RenderApp.Globals;


namespace RenderApp.ViewModel
{
    public partial class VoxelViewModel : TabItemViewModel, IControllerViewModelBase
    {
        private int partitionNum = 64;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VoxelViewModel()
        {
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

        public override string Title
        {
            get
            {
                return "Voxel";
            }
        }

        private void ExecuteCommand()
        {
            ICommand command = new CreateVoxelCommand(Workspace.SceneManager.ActiveScene.SelectNode as RenderObject, PartitionNum);
            CommandManager.Instance.Execute(command, null, true);
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
