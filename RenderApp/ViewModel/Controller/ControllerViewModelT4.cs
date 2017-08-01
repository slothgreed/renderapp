using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RenderApp.ViewModel.DockTab;

namespace RenderApp.ViewModel
{
	public partial class SelectViewModel : TabItemViewModel
	{
		private ICommand _Execute;
		public ICommand Execute
		{
			get
			{
				if(_Execute == null)
				{
					return _Execute = CreateCommand(ExecuteCommand);						
				}
				return _Execute;
			}
		}
	}
	public partial class DijkstraViewModel : TabItemViewModel
	{
		private ICommand _Execute;
		public ICommand Execute
		{
			get
			{
				if(_Execute == null)
				{
					return _Execute = CreateCommand(ExecuteCommand);						
				}
				return _Execute;
			}
		}
	}
	public partial class VoxelViewModel : TabItemViewModel
	{
		private ICommand _Execute;
		public ICommand Execute
		{
			get
			{
				if(_Execute == null)
				{
					return _Execute = CreateCommand(ExecuteCommand);						
				}
				return _Execute;
			}
		}
	}
}