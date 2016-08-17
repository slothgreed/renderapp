using System;
using System.Collections.Generic;
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
			private ICommand _Load3DModel;
			public ICommand Load3DModel
			{
				get
				{
					if(_Load3DModel == null)
					{
						return _Load3DModel = CreateCommand(Load3DModelCommand);						
					}
					return _Load3DModel;
				}
			}
			private ICommand _LoadTexture;
			public ICommand LoadTexture
			{
				get
				{
					if(_LoadTexture == null)
					{
						return _LoadTexture = CreateCommand(LoadTextureCommand);						
					}
					return _LoadTexture;
				}
			}
			private ICommand _CreateCube;
			public ICommand CreateCube
			{
				get
				{
					if(_CreateCube == null)
					{
						return _CreateCube = CreateCommand(CreateCubeCommand);						
					}
					return _CreateCube;
				}
			}
			private ICommand _CreateSphere;
			public ICommand CreateSphere
			{
				get
				{
					if(_CreateSphere == null)
					{
						return _CreateSphere = CreateCommand(CreateSphereCommand);						
					}
					return _CreateSphere;
				}
			}
			private ICommand _CreatePlane;
			public ICommand CreatePlane
			{
				get
				{
					if(_CreatePlane == null)
					{
						return _CreatePlane = CreateCommand(CreatePlaneCommand);						
					}
					return _CreatePlane;
				}
			}
			private ICommand _LoadShader;
			public ICommand LoadShader
			{
				get
				{
					if(_LoadShader == null)
					{
						return _LoadShader = CreateCommand(LoadShaderCommand);						
					}
					return _LoadShader;
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

	}

}