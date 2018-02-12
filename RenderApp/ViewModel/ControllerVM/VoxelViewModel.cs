using System;
using KI.Foundation.Command;
using KI.Asset;
using KI.Tool.Command;
using KI.UI.ViewModel;
using RenderApp.Globals;


namespace RenderApp.ViewModel
{
    public partial class VoxelViewModel : DockWindowViewModel
    {
        private int partitionNum = 64;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        public VoxelViewModel(ViewModelBase parent)
            : base(parent, null, "VoxelView", Place.Floating)
        {
            var color = new System.Windows.Media.Color();
            color.R = 0;
            color.G = 0;
            color.B = 0;
            color.A = 255;
            SelectColor = color;
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

        private void ExecuteCommand()
        {
            var color = new OpenTK.Vector3(SelectColor.R / 255.0f, SelectColor.G / 255.0f, SelectColor.B / 255.0f);
            ICommand command = new CreateVoxelCommand(Workspace.Instance.MainScene, Workspace.Instance.MainScene.SelectNode as RenderObject, color, PartitionNum);
            CommandManager.Instance.Execute(command, null, true);
        }
    }
}
