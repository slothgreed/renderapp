using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;
using System.Windows;
using KI.Foundation.Command;
using RenderApp.RACommand;
namespace RenderApp.ViewModel
{
    public partial class VoxelViewModel : TabItemViewModel, IControllerViewModelBase
    {
        private int _partitionNum = 64; 
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
        public VoxelViewModel()
        {
        }
        private void ExecuteCommand()
        {
            ICommand command = new CreateVoxelCommand(SceneManager.Instance.ActiveScene.SelectAsset, PartitionNum);
            CommandManager.Instance.Execute(command, true);
        }


        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}
