using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using KI.Asset;
using KI.Gfx.GLUtil;
using KI.Renderer;
using KI.Renderer.Technique;
using KI.Foundation.Controller;
using KI.Presentation.ViewModel;
using OpenTK;
using RenderApp.Tool.Controller;

namespace STLBrowser.ViewModel
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
        private ControllerBase cameraController = new CameraController();

        /// <summary>
        /// シーン
        /// </summary>
        public Scene MainScene { get; set; }

        /// <summary>
        /// レンダラー
        /// </summary>
        public RenderSystem RenderSystem { get; set; }

        public void OnLoadedEvent(object sender, EventArgs e)
        {
            MainScene = new Scene("MainScene", new EmptyNode("ROOT"));
            RenderSystem = new RenderSystem();
            RenderSystem.ActiveScene = MainScene;

            MainScene.MainCamera = AssetFactory.Instance.CreateCamera("MainCamera");
            var light = new DirectionLight("SunLight", Vector3.UnitY + Vector3.UnitX, Vector3.Zero);
            var sphere = AssetFactory.Instance.CreateSphere("sphere", 0.1f, 32, 32, true);
            MainScene.MainLight = new LightNode("SunLight", light, SceneNodeFactory.Instance.CreatePolygonNode("SunLight", sphere, null));
            //MainScene.AddObject(MainScene.MainCamera);
            MainScene.AddObject(MainScene.MainLight);

            RenderTechniqueFactory.Instance.RendererSystem = RenderSystem;

            RenderSystem.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer));
            var gBufferTexture = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            RenderSystem.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred));
            RenderSystem.OutputBuffer = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output) as OutputBuffer;
            RenderSystem.OutputTexture = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
        }

        private void OnResizeEvent(object sender, EventArgs e)
        {
            if (MainScene != null)
            {
                MainScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
            }
            
            RenderSystem.SizeChanged(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        private void OnRenderEvent(object sender, PaintEventArgs e)
        {
            RenderSystem.Render();
        }

        private void OnMouseWheelEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e);
        }

        private void OnMouseMoveUpEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e);
        }

        private void OnMouseMoveEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e);
        }

        private void OnMouseDownEvent(object sender, KIMouseEventArgs e)
        {
            ProcessMouseInput(e);
        }



        /// <summary>
        /// マウス入力
        /// </summary>
        /// <param name="mouse">マウス情報</param>
        /// <param name="state">状態</param>
        public void ProcessMouseInput(KIMouseEventArgs mouse)
        {
            switch (mouse.MouseState)
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
