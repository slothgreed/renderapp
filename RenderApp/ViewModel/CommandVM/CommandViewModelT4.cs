using System.Windows.Input;
using KI.UI.ViewModel;

namespace RenderApp.ViewModel
{
    public partial class VoxelCommandViewModel : ViewModelBase
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
	public partial class SmoothingCommandViewModel : ViewModelBase
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
	public partial class IsoLineCommandViewModel : ViewModelBase
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