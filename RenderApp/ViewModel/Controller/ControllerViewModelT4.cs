﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace RenderApp.ViewModel.Controller
{
    public partial class SelectViewModel : DockWindowViewModel
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
	public partial class DijkstraViewModel : DockWindowViewModel
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
	public partial class VoxelViewModel : DockWindowViewModel
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