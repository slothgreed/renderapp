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
			private ICommand _WindowClose;
			public ICommand WindowClose
			{
				get
				{
					if (_WindowClose == null)
					{
						return _WindowClose = CreateCommand(WindowCloseCommand);						
					}

					return _WindowClose;
				}
			}
			private ICommand _SizeChanged;
			public ICommand SizeChanged
			{
				get
				{
					if (_SizeChanged == null)
					{
						return _SizeChanged = CreateCommand(SizeChangedCommand);						
					}

					return _SizeChanged;
				}
			}
			private ICommand _TogglePostProcess;
			public ICommand TogglePostProcess
			{
				get
				{
					if (_TogglePostProcess == null)
					{
						return _TogglePostProcess = CreateCommand(TogglePostProcessCommand);						
					}

					return _TogglePostProcess;
				}
			}
			private ICommand _Voxelize;
			public ICommand Voxelize
			{
				get
				{
					if (_Voxelize == null)
					{
						return _Voxelize = CreateCommand(VoxelizeCommand);						
					}

					return _Voxelize;
				}
			}
			private ICommand _Octree;
			public ICommand Octree
			{
				get
				{
					if (_Octree == null)
					{
						return _Octree = CreateCommand(OctreeCommand);						
					}

					return _Octree;
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

			private TabControlViewModel _LeftUpDockPanel;
			public TabControlViewModel LeftUpDockPanel
			{
				get
				{
					return _LeftUpDockPanel;
				}

				set
				{
					SetValue(ref _LeftUpDockPanel, value);
				}
			}
			private TabControlViewModel _LeftDownDockPanel;
			public TabControlViewModel LeftDownDockPanel
			{
				get
				{
					return _LeftDownDockPanel;
				}

				set
				{
					SetValue(ref _LeftDownDockPanel, value);
				}
			}
			private TabControlViewModel _RightUpDockPanel;
			public TabControlViewModel RightUpDockPanel
			{
				get
				{
					return _RightUpDockPanel;
				}

				set
				{
					SetValue(ref _RightUpDockPanel, value);
				}
			}
			private TabControlViewModel _RightDownDockPanel;
			public TabControlViewModel RightDownDockPanel
			{
				get
				{
					return _RightDownDockPanel;
				}

				set
				{
					SetValue(ref _RightDownDockPanel, value);
				}
			}
			private TabControlViewModel _CenterDockPanel;
			public TabControlViewModel CenterDockPanel
			{
				get
				{
					return _CenterDockPanel;
				}

				set
				{
					SetValue(ref _CenterDockPanel, value);
				}
			}

	}

}