using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using CADApp.Model;
using CADApp.Tool.Control;
using KI.Asset;
using KI.Asset.Technique;
using KI.Gfx.GLUtil;
using KI.Tool;
using KI.Tool.Control;
using KI.UI.ViewModel;
using OpenTK;

namespace CADApp.ViewModel
{
    public class ViewportViewModel : ViewModelBase
    {
        private WindowsFormsHost _glContext;
        public WindowsFormsHost GLContext
        {
            get
            {
                if (_glContext == null)
                {
                    _glContext = new WindowsFormsHost()
                    {
                        Child = Viewport.Instance.GLControl
                    };
                }
                return _glContext;
            }
        }

        public Viewport Viewport
        {
            get
            {
                return Viewport.Instance;
            }
        }

        public string FolderPath
        {
            get
            {
                return "AAA";
            }
        }

        public ViewportViewModel(ViewModelBase parent)
            : base(parent)
        {
            Viewport.Instance.OnLoaded += OnLoadedEvent;
            Viewport.Instance.OnResize += OnResizeEvent;
            Viewport.Instance.OnRender += OnRenderEvent;
            Viewport.Instance.OnMouseWheel += OnMouseWheelEvent;
            Viewport.Instance.OnMouseUp += OnMouseMoveUpEvent;
            Viewport.Instance.OnMouseMove += OnMouseMoveEvent;
            Viewport.Instance.OnMouseDown += OnMouseDownEvent;
        }

        /// <summary>
        /// カメラコントローラ
        /// </summary>
        private IController cameraController = new CameraController();

        /// <summary>
        /// シーン
        /// </summary>
        public Scene MainScene { get; set; }

        /// <summary>
        /// レンダラー
        /// </summary>
        public Renderer Renderer { get; set; }

        public void OnLoadedEvent(object sender, EventArgs e)
        {
            DeviceContext.Instance.SetClearColor(1, 1, 1, 1);
            MainScene = new Scene("MainScene");
            Renderer = new Renderer();
            Renderer.ActiveScene = MainScene;

            Workspace.Instance.MainScene = MainScene;
            Workspace.Instance.Renderer = Renderer;

            InitializeScene();
            InitializeRenderer();

        }

        private void OnResizeEvent(object sender, EventArgs e)
        {
            if (MainScene != null)
            {
                MainScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
            }
            
            Renderer.SizeChanged(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        private void OnRenderEvent(object sender, PaintEventArgs e)
        {
            Renderer.Render();
        }

        private void OnMouseWheelEvent(object sender, MouseEventArgs e)
        {
            ProcessMouseInput(e, MOUSE_STATE.WHEEL);
        }

        private void OnMouseMoveUpEvent(object sender, MouseEventArgs e)
        {
            ProcessMouseInput(e, MOUSE_STATE.UP);
        }

        private void OnMouseMoveEvent(object sender, MouseEventArgs e)
        {
            ProcessMouseInput(e, MOUSE_STATE.MOVE);
        }

        private void OnMouseDownEvent(object sender, MouseEventArgs e)
        {
            ProcessMouseInput(e, MOUSE_STATE.DOWN);
        }

        private void InitializeScene()
        {
            MainScene.MainCamera = AssetFactory.Instance.CreateCamera("MainCamera");
            MainScene.SunLight = RenderObjectFactory.Instance.CreateDirectionLight("SunLight", Vector3.UnitY + Vector3.UnitX, Vector3.Zero);
            var sphere = AssetFactory.Instance.CreateSphere("sphere", 0.1f, 32, 32, true);
            MainScene.SunLight.Model = RenderObjectFactory.Instance.CreateRenderObject("SunLight", sphere);
            MainScene.AddObject(MainScene.MainCamera);
            MainScene.AddObject(MainScene.SunLight);

            var axis = AssetFactory.Instance.CreateAxis("axis", Vector3.Zero, MainScene.WorldMax);
            var axisObject = RenderObjectFactory.Instance.CreateRenderObject(axis.ToString(), axis);
            MainScene.AddObject(axisObject);

            var grid = AssetFactory.Instance.CreateGridPlane("gridPlane", 1, 0.01f, Vector3.One);
            var girdObject = RenderObjectFactory.Instance.CreateRenderObject(grid.ToString(), grid);
            MainScene.AddObject(girdObject);
        }

        private void InitializeRenderer()
        {
            RenderTechniqueFactory.Instance.Renderer = Renderer;
            Renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer));
            var gBufferTexture = Renderer.RenderQueue.OutputTexture<GBuffer>();
            Renderer.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred));
            Renderer.OutputBuffer  = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output) as OutputBuffer;
            Renderer.OutputTexture = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
        }

        /// <summary>
        /// マウス入力
        /// </summary>
        /// <param name="mouse">マウス情報</param>
        /// <param name="state">状態</param>
        public void ProcessMouseInput(MouseEventArgs mouse, MOUSE_STATE state)
        {
            switch (state)
            {
                case MOUSE_STATE.DOWN:
                    cameraController.Down(mouse);
                    break;
                case MOUSE_STATE.CLICK:
                    cameraController.Click(mouse);
                    break;
                case MOUSE_STATE.MOVE:
                    cameraController.Move(mouse);
                    break;
                case MOUSE_STATE.UP:
                    cameraController.Up(mouse);
                    break;
                case MOUSE_STATE.WHEEL:
                    cameraController.Wheel(mouse);
                    break;
            }
        }
    }
}
