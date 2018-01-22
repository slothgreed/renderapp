using System.Windows.Input;
namespace RenderApp.ViewModel
{
	public partial class VoxelViewModel : DockWindowViewModel
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