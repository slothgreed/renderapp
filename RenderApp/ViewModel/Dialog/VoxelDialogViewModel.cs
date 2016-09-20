using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.ViewModel;
namespace RenderApp.ViewModel.Dialog
{
    public class VoxelDialogViewModel : DialogViewModelBase
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
        public VoxelDialogViewModel()
        {

        }


    }
}
