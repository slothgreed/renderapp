using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CADApp.ViewModel
{
	public partial class MainWindowViewModel
	{
			private ICommand _Undo;
			public ICommand Undo
			{
				get
				{
					if (_Undo == null)
					{
						return _Undo = CreateCommand(UndoCommand);						
					}

					return _Undo;
				}
			}
			private ICommand _Redo;
			public ICommand Redo
			{
				get
				{
					if (_Redo == null)
					{
						return _Redo = CreateCommand(RedoCommand);						
					}

					return _Redo;
				}
			}
			private ICommand _Controller;
			public ICommand Controller
			{
				get
				{
					if (_Controller == null)
					{
						return _Controller = CreateCommand(ControllerCommand);						
					}

					return _Controller;
				}
			}
			private ICommand _DeleteNode;
			public ICommand DeleteNode
			{
				get
				{
					if (_DeleteNode == null)
					{
						return _DeleteNode = CreateCommand(DeleteNodeCommand);						
					}

					return _DeleteNode;
				}
			}
	}
}