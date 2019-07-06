using KI.Asset;
using KI.Renderer;
using KI.Foundation.Command;
using KI.Presentation.ViewModel;
using RenderApp.Model;
using RenderApp.Tool.Command;

namespace RenderApp.ViewModel
{
    public partial class VoxelCommandViewModel : ViewModelBase
    {
        private int partitionNum = 64;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent">親ビューモデル</param>
        public VoxelCommandViewModel(ViewModelBase parent)
            : base(parent, null)
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
            var commandArgs = new VoxelCommandArgs(Workspace.Instance.MainScene.SelectNode as PolygonNode, Workspace.Instance.MainScene, PartitionNum, color);
            var command = new CreateVoxelCommand(commandArgs);
            MainWindowViewModel.Instance.CommandManager.Execute(command, true);
        }
    }
}
