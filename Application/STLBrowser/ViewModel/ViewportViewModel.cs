using System;
using System.Windows.Forms;
using KI.Asset;
using KI.Renderer;
using KI.Renderer.Technique;
using KI.Foundation.Controller;
using KI.Presenter.ViewModel;
using OpenTK;
using RenderApp.Tool.Controller;
using KI.Gfx;

namespace STLBrowser.ViewModel
{
    public class ViewportViewModel : ViewportViewModelBase
    {
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

        protected override void InitializeDeviceContext()
        {
            DeviceContext.Instance.SetClearColor(0, 0, 0, 0);
        }

        protected override void InitializeScene()
        {
            MainScene = new Scene("MainScene", new EmptyNode("ROOT"));
            RenderSystem = new RenderSystem();
            RenderSystem.ActiveScene = MainScene;

            MainScene.MainCamera = new Camera("MainCamera");
            MainScene.MainLight = new DirectionLight("SunLight", Vector3.UnitY + Vector3.UnitX, Vector3.Zero);

        }

        protected override void InitializeRenderer()
        {

            RenderTechniqueFactory.Instance.RendererSystem = RenderSystem;
            RenderSystem.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer));
            var gBufferTexture = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            RenderSystem.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred));
            RenderSystem.OutputBuffer = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output) as OutputBuffer;
            RenderSystem.OutputTexture = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
        }

        protected override void OnResizeEvent(object sender, EventArgs e)
        {
            if (MainScene != null)
            {
                MainScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
            }
            
            RenderSystem.SizeChanged(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        protected override void OnRenderEvent(object sender, PaintEventArgs e)
        {
            RenderSystem.Render();
        }

        /// <summary>
        /// マウス入力
        /// </summary>
        /// <param name="mouse">マウス情報</param>
        /// <param name="state">状態</param>
        protected override void ProcessMouseInput(KIMouseEventArgs mouse)
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
