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
					if(_NewProject == null)
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
					if(_OpenProject == null)
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
					if(_SaveProject == null)
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
					if(_SaveAsProject == null)
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
					if(_LoadAsset == null)
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
					if(_CreateObject == null)
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
					if(_Controller == null)
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
					if(_WindowClose == null)
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
					if(_SizeChanged == null)
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
					if(_TogglePostProcess == null)
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
					if(_Voxelize == null)
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
					if(_Octree == null)
					{
						return _Octree = CreateCommand(OctreeCommand);						
					}
					return _Octree;
				}
			}

			private ObservableCollection<DockWindowViewModel> _CenterItemsSource = new ObservableCollection<DockWindowViewModel>();
			public ObservableCollection<DockWindowViewModel> CenterItemsSource
			{
				get
				{
					return _CenterItemsSource;
				}
			}
			private ObservableCollection<DockWindowViewModel> _LeftUpItemsSource = new ObservableCollection<DockWindowViewModel>();
			public ObservableCollection<DockWindowViewModel> LeftUpItemsSource
			{
				get
				{
					return _LeftUpItemsSource;
				}
			}
			private ObservableCollection<DockWindowViewModel> _LeftDownItemsSource = new ObservableCollection<DockWindowViewModel>();
			public ObservableCollection<DockWindowViewModel> LeftDownItemsSource
			{
				get
				{
					return _LeftDownItemsSource;
				}
			}
			private ObservableCollection<DockWindowViewModel> _RightUpItemsSource = new ObservableCollection<DockWindowViewModel>();
			public ObservableCollection<DockWindowViewModel> RightUpItemsSource
			{
				get
				{
					return _RightUpItemsSource;
				}
			}
			private ObservableCollection<DockWindowViewModel> _RightDownItemsSource = new ObservableCollection<DockWindowViewModel>();
			public ObservableCollection<DockWindowViewModel> RightDownItemsSource
			{
				get
				{
					return _RightDownItemsSource;
				}
			}

			private DockWindowViewModel _CenterSelectItem;
			public DockWindowViewModel CenterSelectItem
			{
				get
				{
					return _CenterSelectItem;
				}
				set
				{
					SetValue(ref _CenterSelectItem,value);
				}
			}
			private DockWindowViewModel _LeftUpSelectItem;
			public DockWindowViewModel LeftUpSelectItem
			{
				get
				{
					return _LeftUpSelectItem;
				}
				set
				{
					SetValue(ref _LeftUpSelectItem,value);
				}
			}
			private DockWindowViewModel _LeftDownSelectItem;
			public DockWindowViewModel LeftDownSelectItem
			{
				get
				{
					return _LeftDownSelectItem;
				}
				set
				{
					SetValue(ref _LeftDownSelectItem,value);
				}
			}
			private DockWindowViewModel _RightUpSelectItem;
			public DockWindowViewModel RightUpSelectItem
			{
				get
				{
					return _RightUpSelectItem;
				}
				set
				{
					SetValue(ref _RightUpSelectItem,value);
				}
			}
			private DockWindowViewModel _RightDownSelectItem;
			public DockWindowViewModel RightDownSelectItem
			{
				get
				{
					return _RightDownSelectItem;
				}
				set
				{
					SetValue(ref _RightDownSelectItem,value);
				}
			}



	}

}