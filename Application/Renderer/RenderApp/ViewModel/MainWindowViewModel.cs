﻿using System;
using System.IO;
using System.Reflection;
using System.Windows;
using KI.Asset;
using KI.Gfx.Buffer;
using KI.Gfx.KIShader;
using KI.Gfx.Render;
using KI.Renderer;
using KI.Foundation.Command;
using KI.Presenter.ViewModel;
using RenderApp.Model;
using RenderApp.Tool;
using RenderApp.Tool.Command;
using KI.Foundation.Core;
using System.Windows.Controls;
using KI.Analyzer;
using KI.Presenter.KIEvent;
using KI.Gfx;

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

        public CommandManager CommandManager { get; private set; }
        #endregion

        #region [constructor]

        public MainWindowViewModel()
            : base(null)
        {
            workspace = Workspace.Instance;
            WorkspaceViewModel = new WorkspaceViewModel(this, workspace);
            CommandManager = new CommandManager();
            CommandManager.OnCommandExecuted += CommandManager_OnCommandExecuted;
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
            var renderTarget = Workspace.Instance.RenderSystem.OutputBuffer.RenderTarget;
            RendererUtility.ScreenShot("OutputBuffer", ".bmp", DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        private void ScreenShotAllCommand()
        {
            foreach (var renderer in Workspace.Instance.RenderSystem.RenderQueue.Items)
            {
                RendererUtility.ScreenShot(renderer.Name, ".bmp", renderer.RenderTarget);
            }

            foreach (var renderer in Workspace.Instance.RenderSystem.PostEffect.Items)
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
            dlg.Filter = "stlファイル(*.stl)|*.stl;|objファイル(*.obj)|*.obj;|halfファイル(*.half)|*.half;|plyファイル(*.ply)|*.ply;|offファイル(*.off)|*.off;|xyzファイル(*.xyz)|*.xyz;|xyz_bファイル(*.xyz_b)|*.xyz_b;|すべてのファイル(*.*)|*.*";
            dlg.Multiselect = true;
            dlg.Title = "開くファイルを選択してください。";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var filename in dlg.FileNames)
                {
                    var polygons = AssetFactory.Instance.CreateLoad3DModel(filename);
                    if(polygons == null)
                    {
                        Logger.Log(Logger.LogLevel.Error, "ファイルの読み込みに失敗しました。" + filename);
                        continue;
                    }

                    PolygonUtility.Setup(polygons.Model);
                    var name = Path.GetFileName(filename);
                    PolygonNode node = null;
                    if (polygons.Model is HalfEdgeDS)
                    {
                        node = new AnalyzePolygonNode(name, polygons.Model);
                    }else
                    {
                        node = new PolygonNode(name,polygons.Model);
                    }

                    workspace.MainScene.AddObject(node);
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
            var targetObject = workspace.MainScene.SelectNode as AnalyzePolygonNode;
            switch (menuParam)
            {
                case AnalyzeCommand.WireFrame:
                    command = new CreateWireFrameCommand(new WireFrameCommandArgs(targetObject, workspace.MainScene, OpenTK.Vector4.Zero));
                    CommandManager.Execute(command, true);
                    break;
                case AnalyzeCommand.ConvexHull:
                    command = new CreateConvexHullCommand(new ConvexHullCommandArgs(targetObject, workspace.MainScene));
                    CommandManager.Execute(command, true);
                    break;
                case AnalyzeCommand.MarchingCube:
                    command = new CreateMarchingCubeCommand(new MarchingCubeCommandArgs(targetObject, workspace.MainScene, 128));
                    CommandManager.Execute(command, true);
                    break;
                case AnalyzeCommand.IsoLine:
                    ShowCommandDialog(new View.CommandView.IsoLineCommandView(), new IsoLineCommandViewModel(this));
                    break;
                case AnalyzeCommand.HalfEdgeWireFrame:
                    command = new CreateHalfEdgeWireFrameCommand(new HalfEdgeWireFrameCommandArgs(targetObject, workspace.MainScene));
                    CommandManager.Execute(command, true);
                    break;
                case AnalyzeCommand.AdaptiveMesh:
                    command = new AdaptiveMeshCommand(new AdaptiveMeshCommandArgs(targetObject));
                    CommandManager.Execute(command, true);
                    break;
                case AnalyzeCommand.QEM:
                    command = new QEMCommand(new QEMCommandArgs(targetObject));
                    CommandManager.Execute(command, true);
                    break;
                case AnalyzeCommand.Perceptron:
                    command = new PerceptronCommand(new PerceptronCommandArgs(workspace.MainScene));
                    CommandManager.Execute(command, true);
                    break;
                case AnalyzeCommand.Curvature:
                    command = new VertexCurvatureCommand(new VertexCurvatureCommandArgs(targetObject, workspace.MainScene));
                    CommandManager.Execute(command, true);
                    break;
                case AnalyzeCommand.Kmeans:
                    command = new KMeansCommand(new KMeansCommandArgs(targetObject, workspace.MainScene, 40, 10));
                    CommandManager.Execute(command, true);
                    break;
                case AnalyzeCommand.Smoothing:
                    ShowCommandDialog(new View.CommandView.SmoothingCommandView(), new SmoothingCommandViewModel(this));
                    break;
                case AnalyzeCommand.Subdivision:
                    ShowCommandDialog(new View.CommandView.SubdivCommandView(), new SubdivisionCommandViewModel(this));
                    break;
                case AnalyzeCommand.FeatureLine:
                    command = new CreateFeatureLineCommand(new FeatureLineCommandArgs(targetObject, workspace.MainScene));
                    CommandManager.Execute(command, true);
                    break;
                case AnalyzeCommand.Voxelize:
                    ShowCommandDialog(new View.Command.VoxelCommandView(), new VoxelCommandViewModel(this));
                    break;
                default:
                    break;
            }
        }

        private void ShowCommandDialog(UserControl userControl, ViewModelBase viewModel)
        {
            var window = new View.DebugWindow();
            userControl.DataContext = viewModel;
            viewModel.PropertyChanged += CommandDialog_PropertyChanged;
            window.Owner = Application.Current.MainWindow;
            window.Content = userControl;
            window.Show();
        }

        private void CommandDialog_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            WorkspaceViewModel.ViewportViewModel.Invalidate();
        }

        #endregion

        #region [swintch controller command]
        public CONTROL_MODE ControlMode
        {
            get
            {
                return WorkspaceViewModel.ViewportViewModel.Mode;
            }

            set
            {
                OnPropertyChanging(nameof(ControlMode));
                WorkspaceViewModel.ViewportViewModel.Mode = value;
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
            workspace.RenderSystem.Dispose();
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
            //var polygonNode = workspace.MainScene.SelectNode as PolygonNode;
            //dataVisualize.GraphName = "vertexParameter";
            //var points = new int[1000];
            //for (int i = 0; i < 1000; i++)
            //{
            //    points[i] = i;
            //}
            //dataVisualize.ParameterList = ((HalfEdgeDS)polygonNode.Polygon).Parameter;
            //window.Content = dataVisualize;
            //window.Show();
        }

        private void UndoCommand()
        {
            CommandManager.Undo();
        }

        private void RedoCommand()
        {
            CommandManager.Redo();
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

        KITimer timer;
        private void AnimationCommand(object parameter)
        {
            if (parameter is AnimationAction)
            {
                var action = (AnimationAction)parameter;
                if (action == AnimationAction.Start)
                {
                    if (timer == null)
                    {
                        timer = new KITimer(10, TimerEvent);
                    }

                    timer.Start();
                }
                else
                {
                    timer.Stop();
                }
            }
        }

        private void TimerEvent(object source, EventArgs e)
        {
            WorkspaceViewModel.ViewportViewModel.Invalidate();
        }

        /// <summary>
        /// コマンドの実行後イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandManager_OnCommandExecuted(object sender, EventArgs e)
        {
            WorkspaceViewModel.ViewportViewModel.Invalidate();
        }
    }
}
