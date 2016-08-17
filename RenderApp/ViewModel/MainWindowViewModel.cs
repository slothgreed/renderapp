using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RenderApp.AssetModel;
using System.IO;
using System.Collections.ObjectModel;
using RenderApp.GLUtil;
using RenderApp.Utility;
using RenderApp.GLUtil.ShaderModel;
using RenderApp.AssetModel.LightModel;
namespace RenderApp.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region [property method]
        private readonly ObservableCollection<AvalonWindowViewModel> _documents = new ObservableCollection<AvalonWindowViewModel>();
        ReadOnlyObservableCollection<AvalonWindowViewModel> _readDocument;
        public ReadOnlyObservableCollection<AvalonWindowViewModel> Documents
        {
            get
            {
                if (_readDocument == null)
                {
                    _readDocument = new ReadOnlyObservableCollection<AvalonWindowViewModel>(_documents);
                }
                return _readDocument;
            }
        }

        private readonly ObservableCollection<AvalonWindowViewModel> _anchorables = new ObservableCollection<AvalonWindowViewModel>();
        private ReadOnlyObservableCollection<AvalonWindowViewModel> _readAnchorable;
        public ReadOnlyObservableCollection<AvalonWindowViewModel> Anchorables
        {
            get
            {
                if (_readAnchorable == null)
                {
                    _readAnchorable = new ReadOnlyObservableCollection<AvalonWindowViewModel>(_anchorables);
                }
                return _readAnchorable;
            }
        }
        private AvalonWindowViewModel _activePane = null;
        public AvalonWindowViewModel ActivePane
        {
            get
            {
                return _activePane;
            }
            set
            {
                if (_activePane != value)
                {
                    SetValue<AvalonWindowViewModel>(ref _activePane, value);
                }
            }
        }

        public bool PostProcessMode
        {
            get
            {
                if(m_Viewport == null)
                {
                    return false;
                }
                if(m_Viewport.RenderSystem == null)
                {
                    return false;
                }
                return m_Viewport.RenderSystem.PostProcessMode;
            }
        }
        #endregion
        private static MainWindowViewModel _instance;
        public static MainWindowViewModel Instance
        { 
            get
            {
                return _instance;
            }
        }
        

        public AssetTreeViewModel AssetWindow;
        private ShaderProgramViewModel ShaderProgramWindow;
        private ViewportViewModel ViewportWindow;
        private MaterialViewModel MaterialWindow;
        private GeometryViewModel GeometryWindow;

        #region [Member変数]

        private Viewport m_Viewport;

        #endregion

        
        public MainWindowViewModel()
        {
            AssetWindow = new AssetTreeViewModel(null, "Asset");
            ShaderProgramWindow = new ShaderProgramViewModel(null);
            ViewportWindow = new ViewportViewModel();
            MaterialWindow = new MaterialViewModel();
            GeometryWindow = new GeometryViewModel();
            _anchorables.Add(AssetWindow);
            _anchorables.Add(GeometryWindow);
            _anchorables.Add(MaterialWindow);
            _anchorables.Add(ShaderProgramWindow);
            _documents.Add(ViewportWindow);
            _instance = this;
        }

        private void NewProjectCommand()
        {
            if(!Project.IsOpen)
            {
                Project.IsOpen = true;
            }
        }
        private void OpenProjectCommand()
        {
            Project.IsOpen = true;
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = Project.ProjectDirectory;
            dlg.Filter = "projファイル(*.@proj)|*.proj;";
            dlg.FilterIndex = 1;
            dlg.Title = "開くファイルを選択してください。";
            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }
        private void SaveProjectCommand()
        {
            
        }
        private void SaveAsProjectCommand()
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = Project.ProjectDirectory;
            dlg.Filter = "projファイル(*.@proj)|*.proj;";
            dlg.FilterIndex = 1;
            dlg.Title = "開くファイルを選択してください。";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        private void Load3DModelCommand()
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = Project.ProjectDirectory;
            dlg.Filter = "objファイル(*.obj)|*.obj;|stlファイル(*.stl)|*.stl;|すべてのファイル(*.*)|*.*";
            dlg.Multiselect = true; 
            dlg.Title = "開くファイルを選択してください。";
            Geometry geometry = null;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach(var filename in dlg.FileNames)
                {
                    string extension = Path.GetExtension(filename);
                    switch(extension)
                    {
                        case ".obj":
                            geometry = new CObjFile(Asset.GetNameFromPath(filename),filename);
                            break;
                        case ".stl":
                            geometry = new StlFile(Asset.GetNameFromPath(filename), filename);
                            break;
                    }
                    if(geometry != null)
                    {
                        AssetFactory.Instance.CreateGeometry(geometry);
                    }
                }
            }
        }
        private void LoadTextureCommand()
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = Project.ProjectDirectory;
            dlg.Filter = "画像ファイル(*.bmp;*.jpg;*png)|*.bmp;*.jpg;*.png;";
            dlg.Title = "開くファイルを選択してください。";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach(var filename in dlg.FileNames)
                {
                    string extension = Path.GetExtension(filename);
                    if (extension == ".bmp" || extension == ".png" || extension == ".jpg")
                    {
                        AssetFactory.Instance.CreateTexture(filename);
                    }
                }
            }
        }
        private void LoadShaderCommand()
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = Project.ProjectDirectory;
            dlg.Filter = "シェーダファイル(*.vert;*.frag;*.geom;*.tes;*.tcs)|*.vert;*.frag;*.geom;*.tes;*.tcs;";
            dlg.Title = "開くファイルを選択してください。";
            dlg.Multiselect = true;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var filename in dlg.FileNames)
                {
                    string extension = Path.GetExtension(filename);

                    switch (extension)
                    {
                        case ".vert":
                        case ".frag":
                        case ".geom":
                        case ".tcs":
                        case ".tes":
                            AssetFactory.Instance.CreateShaderProgram(filename);
                            break;
                    }
                }
            }
        }
        private void CreateCubeCommand()
        {
            Cube cube = new Cube(Asset.GetNameFromType(EAssetType.Geometry), Scene.ActiveScene.WorldMin, Scene.ActiveScene.WorldMax);
            AssetFactory.Instance.CreateGeometry(cube);
        }
        private void CreateSphereCommand()
        {
            Sphere sphere = new Sphere(Asset.GetNameFromType(EAssetType.Geometry), 5, 5, 5, true, OpenTK.Vector3.UnitY);
            AssetFactory.Instance.CreateGeometry(sphere);
        }
        private void CreatePlaneCommand()
        {
            Plane plane = new Plane(Asset.GetNameFromType(EAssetType.Geometry));
            AssetFactory.Instance.CreateGeometry(plane);
        }

        private void WindowCloseCommand()
        {
            Scene.AllDispose();
        }
        private void SizeChangedCommand()
        {
            foreach(var loop in Documents)
            {
                loop.SizeChanged();
            }
            foreach (var loop in Anchorables)
            {
                loop.SizeChanged();
            }
        }
        public void LoadedCommand()
        {
            m_Viewport = Viewport.Instance;

            if (m_Viewport != null)
            {
                m_Viewport.Initialize();
            }
        }
        public void ClosedCommand()
        {
            if (m_Viewport != null)
            {
                m_Viewport.Closed();
            }
            GC.Collect();
        }
        public void TogglePostProcessCommand()
        {
            m_Viewport.RenderSystem.TogglePostProcess();
            OnPropertyChanged("PostProcessMode");
        }
        public void UpdateMaterialView(TreeItemViewModel node)
        {
            switch(node.AssetType)
            {
                case EAssetType.Geometry:
                    if(node.Model is Camera)
                    {

                    }
                    else if(node.Model is Light)
                    {

                    }
                    else
                    {
                        AddWindow(new GeometryViewModel((Geometry)node.Model));
                    }
                    break;
                case EAssetType.Materials:
                    AddWindow(new MaterialViewModel((Material)node.Model));
                    AddWindow(new ShaderViewModel((Material)node.Model));
                    break;
                case EAssetType.ShaderProgram:
                    AddWindow(new ShaderProgramViewModel((ShaderProgram)node.Model));
                    break;
                case EAssetType.Textures:
                    AddWindow(new TextureViewModel((Texture)node.Model));
                    break;
                default:
                    break;
            }
        }
        private void AddWindow(AvalonWindowViewModel newWindow)
        {
            var oldWindow = _anchorables.Where(x => x.WindowPosition == newWindow.WindowPosition).First();
            _anchorables.Add(newWindow);

            if (oldWindow != null)
            {
                _anchorables.Remove(oldWindow);
            }
        }
        public override void UpdateProperty()
        {

        }
    }
}
