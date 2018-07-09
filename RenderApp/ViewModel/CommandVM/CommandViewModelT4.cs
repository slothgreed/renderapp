using System.Windows.Input;
namespace RenderApp.ViewModel
{
	public partial class VoxelCommandViewModel : DockWindowViewModel
	{
		private ICommand _Execute;
		public ICommand Execute
		{
			get
			{
				if (_Execute == null)
				{
					return _Execute = CreateCommand(ExecuteCommand);						
				}

				return _Execute;
			}
		}
	}
	public partial class SmoothingCommandViewModel : DockWindowViewModel
	{
		private ICommand _Execute;
		public ICommand Execute
		{
			get
			{
				if (_Execute == null)
				{
					return _Execute = CreateCommand(ExecuteCommand);						
				}

				return _Execute;
			}
		}
	}
}