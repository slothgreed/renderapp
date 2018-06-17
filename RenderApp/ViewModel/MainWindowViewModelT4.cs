using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RenderApp.ViewModel
{
	public partial class MainWindowViewModel
	{
			private ICommand _NewProject;
			public ICommand NewProject
			{
				get
				{
					if (_NewProject == null)
					{
						return _NewProject = CreateCommand(NewProjectCommand);						
					}

					return _NewProject;
				}
			}
			private ICommand _OpenProject;
			public ICommand OpenProject
			{
				get
				{
					if (_OpenProject == null)
					{
						return _OpenProject = CreateCommand(OpenProjectCommand);						
					}

					return _OpenProject;
				}
			}
			private ICommand _SaveProject;
			public ICommand SaveProject
			{
				get
				{
					if (_SaveProject == null)
					{
						return _SaveProject = CreateCommand(SaveProjectCommand);						
					}

					return _SaveProject;
				}
			}
			private ICommand _SaveAsProject;
			public ICommand SaveAsProject
			{
				get
				{
					if (_SaveAsProject == null)
					{
						return _SaveAsProject = CreateCommand(SaveAsProjectCommand);						
					}

					return _SaveAsProject;
				}
			}
			private ICommand _ScreenShot;
			public ICommand ScreenShot
			{
				get
				{
					if (_ScreenShot == null)
					{
						return _ScreenShot = CreateCommand(ScreenShotCommand);						
					}

					return _ScreenShot;
				}
			}
			private ICommand _ScreenShotAll;
			public ICommand ScreenShotAll
			{
				get
				{
					if (_ScreenShotAll == null)
					{
						return _ScreenShotAll = CreateCommand(ScreenShotAllCommand);						
					}

					return _ScreenShotAll;
				}
			}
			private ICommand _LoadAsset;
			public ICommand LoadAsset
			{
				get
				{
					if (_LoadAsset == null)
					{
						return _LoadAsset = CreateCommand(LoadAssetCommand);						
					}

					return _LoadAsset;
				}
			}
			private ICommand _CreateObject;
			public ICommand CreateObject
			{
				get
				{
					if (_CreateObject == null)
					{
						return _CreateObject = CreateCommand(CreateObjectCommand);						
					}

					return _CreateObject;
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
			private ICommand _OpenExplorer;
			public ICommand OpenExplorer
			{
				get
				{
					if (_OpenExplorer == null)
					{
						return _OpenExplorer = CreateCommand(OpenExplorerCommand);						
					}

					return _OpenExplorer;
				}
			}
			private ICommand _OpenDebugWindow;
			public ICommand OpenDebugWindow
			{
				get
				{
					if (_OpenDebugWindow == null)
					{
						return _OpenDebugWindow = CreateCommand(OpenDebugWindowCommand);						
					}

					return _OpenDebugWindow;
				}
			}
			private ICommand _DataVisualization;
			public ICommand DataVisualization
			{
				get
				{
					if (_DataVisualization == null)
					{
						return _DataVisualization = CreateCommand(DataVisualizationCommand);						
					}

					return _DataVisualization;
				}
			}
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
			private ICommand _OpenWindow;
			public ICommand OpenWindow
			{
				get
				{
					if (_OpenWindow == null)
					{
						return _OpenWindow = CreateCommand(OpenWindowCommand);						
					}

					return _OpenWindow;
				}
			}
			private ICommand _OpenAbout;
			public ICommand OpenAbout
			{
				get
				{
					if (_OpenAbout == null)
					{
						return _OpenAbout = CreateCommand(OpenAboutCommand);						
					}

					return _OpenAbout;
				}
			}
	}
}