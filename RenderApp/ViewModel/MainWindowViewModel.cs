using System;
using System.IO;
using KI.Asset;
using KI.Analyzer;
using KI.Foundation.Command;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using KI.Gfx.Render;
using KI.UI.ViewModel;
using KI.Renderer;
using KI.Tool.Command;
using KI.Tool.Control;
using RenderApp.Globals;
using System.Windows;

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
            WorkspaceViewModel = new WorkspaceViewModel(this);
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
                    Workspace.MainScene.AddObject(renderObject);
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
            RAGeometry menuParam = (RAGeometry)createObjectMenu;
            ICommand command = null;
            switch (menuParam)
            {
                case RAGeometry.WireFrame:
                    command = new CreateWireFrameCommand(Workspace.MainScene.SelectNode);
                    CommandManager.Instance.Execute(command, null, true);
                    break;
                case RAGeometry.ConvexHull:
                    command = new CreateConvexHullCommand(Workspace.MainScene.SelectNode);
                    CommandManager.Instance.Execute(command, null, true);
                    break;
                case RAGeometry.MarchingCube:
                    command = new CreateMarchingCubeCommand(Workspace.MainScene.SelectNode, 64);
                    CommandManager.Instance.Execute(command, null, true);
                    break;
                case RAGeometry.HalfEdgeWireFrame:
                    command = new CreateHalfEdgeWireFrameCommand(Workspace.MainScene.SelectNode);
                    CommandManager.Instance.Execute(command, null, true);
                    break;
                case RAGeometry.AdaptiveMesh:
                    command = new AdaptiveMeshCommand(Workspace.MainScene.SelectNode);
                    CommandManager.Instance.Execute(command, null, true);
                        break;
                case RAGeometry.QEM:
                    command = new QEMCommand(Workspace.MainScene.SelectNode);
                    CommandManager.Instance.Execute(command, null, true);
                    break;
                case RAGeometry.Perceptron:
                    command = new PerceptronCommand();
                    CommandManager.Instance.Execute(command, null, true);
                    break;
                case RAGeometry.Kmeans:
                    command = new KMeansCommand(Workspace.MainScene.SelectNode, 40, 10);
                    CommandManager.Instance.Execute(command, null, true);
                    break;
                case RAGeometry.Voxelize:
                    var window = new View.DebugWindow();
                    var voxelView = new View.Controller.VoxelView();
                    voxelView.DataContext = new VoxelViewModel(this);
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
                return ControlManager.Instance.Mode;
            }

            set
            {
                OnPropertyChanging(nameof(ControlMode));
                ControlManager.Instance.Mode = value;
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
            Workspace.RenderSystem.Dispose();
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
            View.DebugWindow window = new View.DebugWindow();
            View.DataVisualization dataVisualize = new View.DataVisualization();
            var renderObject = Workspace.MainScene.SelectNode as RenderObject;
            dataVisualize.GraphName = "vertexParameter";
            dataVisualize.ParameterList = ((HalfEdgeDS)renderObject.Polygon).Parameter;
            dataVisualize.Update(((HalfEdgeDS)renderObject.Polygon).Parameter);
            window.Content = dataVisualize;
            window.Show();
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
    }
}
