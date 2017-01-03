﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.ViewModel;
using RenderApp.AssetModel;
using System.Windows;
namespace RenderApp.ViewModel.Controller
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
            if (!AssetFactory.Instance.CreateVoxel(Scene.ActiveScene.SelectAsset, PartitionNum))
            {
                MessageBox.Show("Trianglesのポリゴンモデルのみで作成できます。");
            }
        }

        public override void UpdateProperty()
        {
            throw new NotImplementedException();
        }
    }
}