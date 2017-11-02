using System;
using System.Collections.ObjectModel;
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

        public override string Title
        {
            get
            {
                return "Voxel";
            }
        }

        private void ExecuteCommand()
        {
            ICommand command = new CreateVoxelCommand(Workspace.MainScene.SelectNode as RenderObject, PartitionNum);
            CommandManager.Instance.Execute(command, null, true);
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
