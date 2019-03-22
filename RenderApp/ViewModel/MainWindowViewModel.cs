using System;
using System.IO;
using KI.Asset;
using KI.Foundation.Command;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using KI.Gfx.Render;
using KI.UI.ViewModel;
using KI.Tool.Command;
using KI.Tool.Control;
using RenderApp.Globals;
using System.Windows;
using KI.Renderer;
using System.Reflection;

namespace RenderApp.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region [Member]

        private static MainWindowViewModel instance;

        public static MainWindowViewModel Instance
        {
            get
            {
                return instance;
            }
        }

        public Workspace workspace;
        private WorkspaceViewModel workspaceViewModel;
        public WorkspaceViewModel WorkspaceViewModel
        {
            get
            {
                return workspaceViewModel;
            }

            set
            {
                if (workspaceViewModel == null)
                {
                    SetValue(ref workspaceViewModel, value);
                }
            }
        }

        private string taskBarText;

        public string TaskBarText
        {
            get
            {
                return taskBarText;
            }

            set
            {
                SetValue(ref taskBarText, value);
            }
        }
        #endregion

        #region [constructor]

        public MainWindowViewModel()
            : base(null)
        {
            workspace = Workspace.Instance;
            WorkspaceViewModel = new WorkspaceViewModel(this, workspace);
            instance = this;
        }

        #endregion

        #region [Project Menu Command]
        private void NewProjectCommand()
        {
            if (!ProjectInfo.IsOpen)
            {
                ProjectInfo.IsOpen = true;
            }
        }

        private void OpenProjectCommand()
        {
            ProjectInfo.IsOpen = true;
            var dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = ProjectInfo.ResourceDirectory;
            dlg.Filter = "projファイル(*.@proj)|*.proj;";
            dlg.FilterIndex = 1;
            dlg.Title = "開くファイルを選択してください。";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
            }
        }

        private void SaveProjectCommand()
        {
        }

        private void SaveAsProjectCommand()
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = ProjectInfo.ResourceDirectory;
            dlg.Filter = "projファイル(*.@proj)|*.proj;";
            dlg.FilterIndex = 1;
            dlg.Title = "開くファイルを選択してください。";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
            }
        }
        #endregion

        #region [Edit Menu Command]

        private void ScreenShotCommand()
        {
            var renderTarget = Workspace.Instance.Renderer.OutputBuffer.RenderTarget;
            RendererUtility.ScreenShot("OutputBuffer", ".bmp", DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        private void ScreenShotAllCommand()
        {
            foreach (var renderer in Workspace.Instance.Renderer.RenderQueue.Items)
            {
                RendererUtility.ScreenShot(renderer.Name, ".bmp", renderer.RenderTarget);
            }

            foreach (var renderer in Workspace.Instance.Renderer.PostEffect.Items)
            {
                RendererUtility.ScreenShot(renderer.Name, ".bmp", renderer.RenderTarget);
            }
        }

        #endregion

        #region [Asset Menu Command]
        private void LoadAssetCommand(object parameter)
        {
            RAAsset menuParam = (RAAsset)parameter;

            switch (menuParam)
            {
                case RAAsset.Model:
                    Load3DModelCommand();
                    break;
                case RAAsset.Texture:
                    LoadTextureCommand();
                    break;
                case RAAsset.Shader:
                    LoadShaderCommand();
                    break;
            }
        }

        private void Load3DModelCommand()
        {
            var dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = ProjectInfo.ResourceDirectory;
            dlg.Filter = "objファイル(*.obj)|*.obj;|stlファイル(*.stl)|*.stl;|halfファイル(*.half)|*.half;|plyファイル(*.ply)|*.ply;|すべてのファイル(*.*)|*.*";
            dlg.Multiselect = true;
            dlg.Title = "開くファイルを選択してください。";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var filename in dlg.FileNames)
                {
                    var polygons = AssetFactory.Instance.CreateLoad3DModel(filename);
                    var renderObject = RenderObjectFactory.Instance.CreateRenderObject(filename, polygons);
                    renderObject.Scale = new OpenTK.Vector3(10);
                    workspace.MainScene.AddObject(renderObject);
                }
            }
        }

        private void LoadTextureCommand()
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = ProjectInfo.ResourceDirectory;
            dlg.Filter = "画像ファイル(*.bmp;*.jpg;*png;tga;*.hdr)|*.bmp;*.jpg;*.png;*.tga;*.hdr;";
            dlg.Title = "開くファイルを選択してください。";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var filename in dlg.FileNames)
                {
                    var texture = TextureFactory.Instance.CreateTexture(filename);
                    Project.ActiveProject.AddChild(texture);
                }
            }
        }

        private void LoadShaderCommand()
        {
            //System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            //dlg.InitialDirectory = Project.ProjectDirectory;
            //dlg.Filter = "シェーダファイル(*.vert;*.frag;*.geom;*.tes;*.tcs)|*.vert;*.frag;*.geom;*.tes;*.tcs;";
            //dlg.Title = "開くファイルを選択してください。";
            //dlg.Multiselect = true;

            //if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    foreach (var filename in dlg.FileNames)
            //    {
            //        string extension = Path.GetExtension(filename);

            //        switch (extension)
            //        {
            //            case ".vert":
            //            case ".frag":
            //            case ".geom":
            //            case ".tcs":
            //            case ".tes":
            //                AssetFactory.Instance.CreateShaderProgram(filename);
            //                break;
            //        }
            //    }
            //}
        }
        #endregion

        #region [Model Menu Command]
        private void CreateObjectCommand(object createObjectMenu)
        {
            AnalyzeCommand menuParam = (AnalyzeCommand)createObjectMenu;
            CommandBase command = null;
            var targetObject = workspace.MainScene.SelectNode as RenderObject;
            switch (menuParam)
            {
                case AnalyzeCommand.WireFrame:
                    command = new CreateWireFrameCommand(new WireFrameCommandArgs(targetObject, workspace.MainScene, OpenTK.Vector3.Zero));
                    CommandManager.Instance.Execute(command, true);
                    break;
                case AnalyzeCommand.ConvexHull:
                    command = new CreateConvexHullCommand(new ConvexHullCommandArgs(targetObject, workspace.MainScene));
                    CommandManager.Instance.Execute(command, true);
                    break;
                case AnalyzeCommand.MarchingCube:
                    command = new CreateMarchingCubeCommand(new MarchingCubeCommandArgs(targetObject, workspace.MainScene, 128));
                    CommandManager.Instance.Execute(command, true);
                    break;
                case AnalyzeCommand.IsoLine:
                    var icommandWindow = new View.DebugWindow();
                    var islolineCommandView = new View.CommandView.IsoLineCommandView();
                    islolineCommandView.DataContext = new IsoLineCommandViewModel(this);
                    icommandWindow.Owner = Application.Current.MainWindow;
                    icommandWindow.Content = islolineCommandView;
                    icommandWindow.Show();
                    break;
                case AnalyzeCommand.HalfEdgeWireFrame:
                    command = new CreateHalfEdgeWireFrameCommand(new HalfEdgeWireFrameCommandArgs(targetObject, workspace.MainScene));
                    CommandManager.Instance.Execute(command, true);
                    break;
                case AnalyzeCommand.AdaptiveMesh:
                    command = new AdaptiveMeshCommand(new AdaptiveMeshCommandArgs(targetObject));
                    CommandManager.Instance.Execute(command, true);
                        break;
                case AnalyzeCommand.QEM:
                    command = new QEMCommand(new QEMCommandArgs(targetObject));
                    CommandManager.Instance.Execute(command, true);
                    break;
                case AnalyzeCommand.Perceptron:
                    command = new PerceptronCommand(new PerceptronCommandArgs(workspace.MainScene));
                    CommandManager.Instance.Execute(command, true);
                    break;
                case AnalyzeCommand.Curvature:
                    command = new VertexCurvatureCommand(new VertexCurvatureCommandArgs(targetObject, workspace.MainScene));
                    CommandManager.Instance.Execute(command, true);
                    break;
                case AnalyzeCommand.Kmeans:
                    command = new KMeansCommand(new KMeansCommandArgs(targetObject, workspace.MainScene, 40, 10));
                    CommandManager.Instance.Execute(command, true);
                    break;
                case AnalyzeCommand.Smoothing:
                    var commandWindow = new View.DebugWindow();
                    var smoothingCommandView = new View.CommandView.SmoothingCommandView();
                    smoothingCommandView.DataContext = new SmoothingCommandViewModel(this);
                    commandWindow.Owner = Application.Current.MainWindow;
                    commandWindow.Content = smoothingCommandView;
                    commandWindow.Show();
                    break;
                case AnalyzeCommand.FeatureLine:
                    command = new CreateFeatureLineCommand(new FeatureLineCommandArgs(targetObject, workspace.MainScene));
                    CommandManager.Instance.Execute(command, true);
                    break;
                case AnalyzeCommand.Voxelize:
                    var window = new View.DebugWindow();
                    var voxelView = new View.Command.VoxelCommandView();
                    voxelView.DataContext = new VoxelCommandViewModel(this);
                    window.Owner = Application.Current.MainWindow;
                    window.Content = voxelView;
                    window.Show();
                    break;
                default:
                    break;
            }

            WorkspaceViewModel.ViewportViewModel.Invalidate();
        }

        #endregion

        #region [swintch controller command]
        public CONTROL_MODE ControlMode
        {
            get
            {
                return ControllerManager.Instance.Mode;
            }

            set
            {
                OnPropertyChanging(nameof(ControlMode));
                ControllerManager.Instance.Mode = value;
                OnPropertyChanged(nameof(ControlMode));
            }
        }

        private void ControllerCommand(object controllerMenu)
        {
            ControlMode = (CONTROL_MODE)controllerMenu;
        }

        #endregion

        #region [MainWindow Event Command]

        public void ContentRenderedCommand()
        {
            WorkspaceViewModel.ViewportViewModel.Intialize();
        }

        public void ClosedCommand()
        {
            Viewport.Instance.Dispose();
            workspace.Renderer.Dispose();
            ShaderFactory.Instance.Dispose();
            RenderTargetFactory.Instance.Dispose();
            TextureFactory.Instance.Dispose();
            BufferFactory.Instance.Dispose();
            AssetFactory.Instance.Dispose();
            GC.Collect();
        }

        #endregion

        private void OpenExplorerCommand(object directoryPath)
        {
            string path = string.Empty;
            if (directoryPath is string)
            {
                path = directoryPath as string;
            }

            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            {
                return;
            }

            System.Diagnostics.Process.Start(path);
        }

        private void OpenDebugWindowCommand()
        {
            View.DebugWindow window = new View.DebugWindow();
            window.Show();
        }

        private void DataVisualizationCommand()
        {
            //View.DebugWindow window = new View.DebugWindow();
            //View.DataVisualization dataVisualize = new View.DataVisualization();
            //var renderObject = workspace.MainScene.SelectNode as RenderObject;
            //dataVisualize.GraphName = "vertexParameter";
            //dataVisualize.ParameterList = ((HalfEdgeDS)renderObject.Polygon).Parameter;
            //window.Content = dataVisualize;
            //window.Show();
        }

        private void UndoCommand()
        {
            CommandManager.Instance.Undo();
        }

        private void RedoCommand()
        {
            CommandManager.Instance.Redo();
        }

        private void OpenWindowCommand(object parameter)
        {
            if (parameter is AppWindow)
            {
                var windowType = (AppWindow)parameter;
                WorkspaceViewModel.OpenWindow(windowType);
            }
        }

        private void OpenAboutCommand()
        {
            var window = new View.AboutView();
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        private void OpenExecuteFolderCommand()
        {
            var exeFilePath = Assembly.GetEntryAssembly().Location;

            System.Diagnostics.Process.Start("explorer.exe", @"/select," + exeFilePath);  
        }
    }
}
