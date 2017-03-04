using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using RenderApp.AssetModel;
using RenderApp.Globals;
using RenderApp.GLUtil;
using RenderApp.GLUtil.ShaderModel;
using RenderApp.Utility;
using RenderApp.ViewModel.Dialog;
using RenderApp.RA_Control;
using RenderApp.Render_System;
using RenderApp.AssetModel.RA_Geometry;
using KI.Foundation.ViewModel;
using KI.Foundation.Tree;
using KI.Gfx.GLUtil;
using KI.Gfx;
using KI.Gfx.KIAsset;
namespace RenderApp.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region [property method]

        private DockWindowViewModel _activePane = null;
        public DockWindowViewModel ActivePane
        {
            get
            {
                return _activePane;
            }
            set
            {
                if (_activePane != value)
                {
                    SetValue<DockWindowViewModel>(ref _activePane, value);
                }
            }
        }
        public DockWindowViewModel ActiveWindow
        {
            get;
            set;
        }

        public bool PostProcessMode
        {
            get
            {
                if (SceneManager.Instance.RenderSystem == null)
                {
                    return false;
                }
                return SceneManager.Instance.RenderSystem.PostProcessMode;
            }
        }
        #endregion

        #region [Member]

        private static MainWindowViewModel _instance;
        public static MainWindowViewModel Instance
        {
            get
            {
                return _instance;
            }
        }

        private Viewport m_Viewport;
        private string _taskBarText;
        public string TaskBarText
        {
            get
            {
                return _taskBarText;
            }
            set
            {
                SetValue(ref _taskBarText, value);
            }
        }
        #endregion


        #region [constructor]
 

        public MainWindowViewModel()
        {
            LeftUpDockPanel = new TabControlViewModel();
            LeftDownDockPanel = new TabControlViewModel();
            RightUpDockPanel = new TabControlViewModel();
            RightDownDockPanel = new TabControlViewModel();
            CenterDockPanel = new TabControlViewModel();

            LeftUpDockPanel.Add( new RootNodeViewModel(Project.ActiveProject.RootNode, "Project"));
            CenterDockPanel.Add( new ViewportViewModel());
            RightDownDockPanel = new TabControlViewModel();
            RightDownDockPanel.Add(new ShaderProgramViewModel(null));
            RightDownDockPanel.Add(new VoxelViewModel());
            Viewport.Instance.OnLoaded += OnLoadedEvent;
            Viewport.Instance.OnMouseDown += OnMouseDownEvent;
            Viewport.Instance.OnMouseMove += OnMouseMoveEvent;
            Viewport.Instance.OnMouseUp += OnMouseMoveUpEvent;
            Viewport.Instance.OnMouseWheel += OnMouseWheelEvent;
            Viewport.Instance.OnRender += OnRenderEvent;
            Viewport.Instance.OnResize += OnResizeEvent;
            _instance = this;
        }
        #region [Viewport Method]
        public void OnLoadedEvent(object sender, EventArgs e)
        {
            SceneManager.Instance.RenderSystem.Initialize(DeviceContext.Instance.Width,DeviceContext.Instance.Height);
            SceneManager.Instance.Create("MainScene");
            SceneManager.Instance.CreateMainCamera();
            SceneManager.Instance.CreateSceneLight();
            LeftUpDockPanel.Add(new RootNodeViewModel(SceneManager.Instance.ActiveScene.RootNode, "Scene"));
            LeftDownDockPanel.Add(new RenderSystemViewModel(SceneManager.Instance.RenderSystem));
        }
        private void OnResizeEvent(object sender, EventArgs e)
        {
            SceneManager.Instance.ActiveScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
            SceneManager.Instance.RenderSystem.SizeChanged(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        private void OnRenderEvent(object sender, PaintEventArgs e)
        {
            SceneManager.Instance.RenderSystem.Render();
        }

        private void OnMouseWheelEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ControlManager.Instance.ProcessInput(e, ControlManager.MOUSE_STATE.WHEEL);
        }

        private void OnMouseMoveUpEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ControlManager.Instance.ProcessInput(e, ControlManager.MOUSE_STATE.UP);
        }

        private void OnMouseMoveEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ControlManager.Instance.ProcessInput(e, ControlManager.MOUSE_STATE.MOVE);
        }

        private void OnMouseDownEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ControlManager.Instance.ProcessInput(e, ControlManager.MOUSE_STATE.DOWN);
        }



        #endregion
        
        #endregion

        #region [Project Menu Command]
        private void NewProjectCommand()
        {
            if (!ProjectInfo.IsOpen)
            {
                ProjectInfo.IsOpen = true;
            }

            List<RenderObject> geometrys = AssetFactory.Instance.CreateLoad3DModel(@"C:/Users/ido/Documents/GitHub/renderapp/RenderApp/Resource/Model/StanfordBunny.stl");
            foreach (var geometry in geometrys)
            {
                geometry.RotateX(-90);
                geometry.RotateY(0);
                Project.ActiveProject.AddChild(AssetFactory.Instance.CreateGeometry(geometry));
            }
        }
        private void OpenProjectCommand()
        {
            ProjectInfo.IsOpen = true;
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = ProjectInfo.ProjectDirectory;
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
            dlg.InitialDirectory = ProjectInfo.ProjectDirectory;
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
            dlg.InitialDirectory = ProjectInfo.ProjectDirectory;
            dlg.Filter = "objファイル(*.obj)|*.obj;|stlファイル(*.stl)|*.stl;|すべてのファイル(*.*)|*.*";
            dlg.Multiselect = true;
            dlg.Title = "開くファイルを選択してください。";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var filename in dlg.FileNames)
                {
                    List<RenderObject> geometrys = AssetFactory.Instance.CreateLoad3DModel(filename);
                    foreach(var geometry in geometrys)
                    {
                        Project.ActiveProject.AddChild(AssetFactory.Instance.CreateGeometry(geometry));
                    }
                }
            }
        }
        private void LoadTextureCommand()
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = ProjectInfo.ProjectDirectory;
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
                    CreateWireFrameCommand();
                    break;
                case RAGeometry.Polygon:
                    CreatePolygonCommand();
                    break;
                default:
                    break;
            }
        }
        private void CreateCubeCommand()
        {
            //Cube cube = new Cube(RAFile.GetNameFromType(EAssetType.Geometry), SceneManager.Instance.ActiveScene.WorldMin, SceneManager.Instance.ActiveScene.WorldMax);
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
        private void CreateWireFrameCommand()
        {
            if (!AssetFactory.Instance.CreateWireFrame(SceneManager.Instance.ActiveScene.SelectAsset))
            {
                MessageBox.Show("Trianglesのポリゴンモデルのみで作成できます。");
            }
        }
        private void CreatePolygonCommand()
        {
            if (!AssetFactory.Instance.CreatePolygon(SceneManager.Instance.ActiveScene.SelectAsset))
            {
                MessageBox.Show("Trianglesのポリゴンモデルのみで作成できます。");
            }
        }
        #endregion

        #region [swintch controller command]
        public ControlManager.CONTROL_MODE ControlMode
        {
            get
            {
                return ControlManager.Instance.Mode;
            }
        }
        private void ControllerCommand(object controllerMenu)
        {
            RAController menuParam = (RAController)controllerMenu;
            OnPropertyChanging("ControlMode");
            switch(menuParam)
            {
                case RAController.Default:
                    ControlManager.Instance.Mode = ControlManager.CONTROL_MODE.SelectObject;
                    break;
                case RAController.Dijkstra:
                    ControlManager.Instance.Mode = ControlManager.CONTROL_MODE.Dijkstra;
                    break;
            }
            OnPropertyChanged("ControlMode");
        }
        #endregion

        #region [MainWindow Event Command]
        private void WindowCloseCommand()
        {
            SceneManager.Instance.ActiveScene.Dispose();
            Project.ActiveProject.Dispose();
            ShaderFactory.Instance.Dispose();
            RenderPassFactory.Instance.Dispose();
        }
        private void SizeChangedCommand()
        {

        }
        public void LoadedCommand()
        {
            m_Viewport = Viewport.Instance;
        }
        public void ClosedCommand()
        {
            if (m_Viewport != null)
            {
                m_Viewport.Dispose();
            }
            GC.Collect();
        }
        #endregion

        #region [Rendering Menu Command]
        public void TogglePostProcessCommand()
        {
            SceneManager.Instance.RenderSystem.TogglePostProcess();
            OnPropertyChanged("PostProcessMode");
        }
        #endregion
        
        #region [Analyze Menu Command]
        private void VoxelizeCommand()
        {
        }
        private void OctreeCommand()
        {
            if (!AssetFactory.Instance.CreateOctree(SceneManager.Instance.ActiveScene.SelectAsset))
            {
                MessageBox.Show("Trianglesのポリゴンモデルのみで作成できます。");
            }
        }
        #endregion

        #region [Update Method]
        public void UpdateSelectNode(KINode node)
        {
            if(node.KIObject == null)
            {
                return;
            }
            TabItemViewModel vm = null;
            if(node.KIObject is Geometry)
            {
                vm = new GeometryViewModel((Geometry)node.KIObject);
                SceneManager.Instance.ActiveScene.SelectAsset = (Geometry)node.KIObject;
            }
            else if (node.KIObject is ShaderProgram)
            {
                vm = new ShaderProgramViewModel((ShaderProgram)node.KIObject);
            }
            ReplaceTabWindow(vm);
        }
        public void ReplaceTabWindow(TabItemViewModel window)
        {
            if(window is GeometryViewModel)
            {
                var oldItem = LeftDownDockPanel.FindVM<GeometryViewModel>();
                LeftDownDockPanel.ReplaceVM(oldItem,window);
            }
            if (window is ShaderProgramViewModel)
            {
                var oldItem = RightDownDockPanel.FindVM<ShaderProgramViewModel>();
                RightDownDockPanel.ReplaceVM(oldItem,window);
            }

        }

        public override void UpdateProperty()
        {
            Viewport.Instance.glControl_Paint(null, null);
        }
        #endregion
    }
}
