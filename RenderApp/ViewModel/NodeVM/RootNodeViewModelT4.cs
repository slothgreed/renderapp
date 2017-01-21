using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace RenderApp.ViewModel.NodeVM
{
	public partial class RootNodeViewModel
	{
			private ICommand _Delete;
			public ICommand Delete
			{
				get
				{
					if(_Delete == null)
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
					if(_OpenExploler == null)
					{
						return _OpenExploler = CreateCommand(OpenExplolerCommand);						
					}
					return _OpenExploler;
				}
			}

	}

}