using System;
using System.IO;
using System.Windows.Forms;
using KI.Asset;
using KI.Analyzer;
using KI.Foundation.Command;
using KI.Foundation.Tree;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using KI.Gfx.Render;
using KI.UI.ViewModel;
using KI.Renderer;
using KI.Tool.Command;
using KI.Tool.Control;
using RenderApp.Globals;

namespace RenderApp.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region [property method]

        private DockWindowViewModel activePane = null;

        public DockWindowViewModel ActivePane
        {
            get
            {
                return activePane;
            }

            set
            {
                if (activePane != value)
                {
                    SetValue(ref activePane, value);
                }
            }
        }

        public DockWindowViewModel ActiveWindow { get; set; }

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

        #endregion

        #region [Member]

        private static MainWindowViewModel instance;

        public static MainWindowViewModel Instance
        {
            get
            {
                return instance;
            }
        }

        private Viewport viewport;

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
            :base(null)
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
            OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
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
        private void LoadAssetCommand(object loadAssetMenuParam)
        {
            RAAsset menuParam = (RAAsset)loadAssetMenuParam;

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
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
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
                case RAGeometry.Cube:
                    CreateCubeCommand();
                    break;
                case RAGeometry.Sphere:
                    CreateSphereCommand();
                    break;
                case RAGeometry.Plane:
                    CreatePlaneCommand();
                    break;
                case RAGeometry.WireFrame:
                    command = new CreateWireFrameCommand(Workspace.MainScene.SelectNode);
                    CommandManager.Instance.Execute(command, null, true);
                    break;
                case RAGeometry.Polygon:
                    command = new CreatePolygonCommand(Workspace.MainScene.SelectNode);
                    CommandManager.Instance.Execute(command, null, true);
                    break;
                case RAGeometry.HalfEdge:
                    command = new CreateHalfEdgeCommand(Workspace.MainScene.SelectNode);
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
                case RAGeometry.CalculateVertexParameter:
                    command = new CalculateVertexCurvature(Workspace.MainScene.SelectNode);
                    CommandManager.Instance.Execute(command, null, true);
                    break;
                case RAGeometry.HalfEdgeWireFrame:
                    command = new CreateHalfEdgeWireFrameCommand(Workspace.MainScene.SelectNode);
                    CommandManager.Instance.Execute(command, null, true);
                    //select = Workspace.SceneManager.ActiveScene.SelectAsset;
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
                default:
                    break;
            }

            Viewport.Instance.GLControl_Paint(null, null);
        }

        private void CreateCubeCommand()
        {
            //Cube cube = new Cube(RAFile.GetNameFromType(EAssetType.Geometry), Workspace.SceneManager.ActiveScene.WorldMin, Workspace.SceneManager.ActiveScene.WorldMax);
            //AssetFactory.Instance.CreateGeometry(cube);
        }

        private void CreateSphereCommand()
        {
            //Sphere sphere = new Sphere(RAFile.GetNameFromType(EAssetType.Geometry), 5, 5, 5, true, OpenTK.Vector3.UnitY);
            //AssetFactory.Instance.CreateGeometry(sphere);
        }

        private void CreatePlaneCommand()
        {
            //Plane plane = new Plane(KIFile.GetNameFromType(EAssetType.Geometry));
            //AssetFactory.Instance.CreateGeometry(plane.CreateRenderObject().First());
        }
        #endregion

        #region [swintch controller command]
        public CONTROL_MODE ControlMode
        {
            get
            {
                return ControlManager.Instance.Mode;
            }
        }

        private void ControllerCommand(object controllerMenu)
        {
            CONTROL_MODE menuParam = (CONTROL_MODE)controllerMenu;
            OnPropertyChanging(nameof(ControlMode));
            ControlManager.Instance.Mode = menuParam;
            OnPropertyChanged(nameof(ControlMode));
        }

        #endregion

        #region [MainWindow Event Command]
        private void WindowCloseCommand()
        {
            Workspace.RenderSystem.Dispose();
            ShaderFactory.Instance.Dispose();
            RenderTargetFactory.Instance.Dispose();
            TextureFactory.Instance.Dispose();
            BufferFactory.Instance.Dispose();
            AssetFactory.Instance.Dispose();
        }

        private void SizeChangedCommand()
        {
        }

        public void ContentRenderedCommand()
        {
            WorkspaceViewModel.ViewportViewModel.Intialize();
        }

        public void ClosedCommand()
        {
            if (viewport != null)
            {
                viewport.Dispose();
            }

            WindowCloseCommand();
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
            var renderObject =  Workspace.MainScene.SelectNode as RenderObject;
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
            if(parameter is AppWindow)
            {
                var windowType = (AppWindow)parameter;
                WorkspaceViewModel.OpenWindow(windowType);
            }
        }

        #region [Update Method]

        public override void UpdateProperty()
        {
            Viewport.Instance.GLControl_Paint(null, null);
        }
        #endregion
    }
}
