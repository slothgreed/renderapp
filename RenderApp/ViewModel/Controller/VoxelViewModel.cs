using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using System.Windows;
using KI.Foundation.Command;
using RenderApp.RACommand;
using RenderApp.Globals;

namespace RenderApp.ViewModel
{
    public partial class VoxelViewModel : TabItemViewModel, IControllerViewModelBase
    {
        private int _partitionNum = 64;

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
                return _partitionNum;
            }

            set
            {
                SetValue<int>(ref _partitionNum, value);
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
            ICommand command = new CreateVoxelCommand(Workspace.SceneManager.ActiveScene.SelectAsset, PartitionNum);
            CommandManager.Instance.Execute(command, null, true);
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
