using System.Windows.Input;
namespace RenderApp.ViewModel
{
    public partial class RootNodeViewModel
	{
		private ICommand _Delete;
		public ICommand Delete
		{
			get
			{
				if (_Delete == null)
				{
					return _Delete = CreateCommand(DeleteCommand);						
				}

				return _Delete;
			}
		}
		private ICommand _OpenExploler;
		public ICommand OpenExploler
		{
			get
			{
				if (_OpenExploler == null)
				{
					return _OpenExploler = CreateCommand(OpenExplolerCommand);						
				}

				return _OpenExploler;
			}
		}
	}
}