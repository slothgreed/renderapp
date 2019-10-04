using KI.Asset;
using KI.Asset.Primitive;
using KI.Gfx;
using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;
using KI.Renderer;
using KI.Renderer.Technique;
using KI.Presenter.ViewModel;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KI.Foundation.Controller;
using ShaderTraining.Tool.Controller;

namespace ShaderTraining.ViewModel
{
    public enum VisibleItem
    {
        Plane,
        Sphere
    }

    public class ViewportViewModel : ViewportViewModelBase
    {
        public Viewport Viewport
        {
            get
            {
                return Viewport.Instance;
            }
        }

        CameraController cameraController;

        public ViewportViewModel(ViewModelBase parent)
            : base(parent)
        {
            cameraController = new CameraController();
        }
  
        /// <summary>
        /// シーン
        /// </summary>
        public Scene MainScene { get; set; }

        /// <summary>
        /// レンダラー
        /// </summary>
        public RenderSystem RenderSystem { get; set; }

        private CameraMode cameraMode;
        public CameraMode CameraMode
        {
            get
            {
                return cameraMode;
            }
            private set
            {
                cameraMode = value;
                MainScene.MainCamera.InitCamera(value);
            }
        }

        protected override void InitializeDeviceContext()
        {
            DeviceContext.Instance.SetClearColor(1, 1, 1, 1);
        }

        protected override void InitializeScene()
        {
            MainScene = new Scene("MainScene", new EmptyNode("Root"));
            MainScene.MainCamera = new Camera("Camera");
            MainScene.MainLight = new DirectionLight("SunLight", Vector3.UnitY + Vector3.UnitX, Vector3.Zero);
            RenderSystem = new RenderSystem();
            RenderSystem.ActiveScene = MainScene;

            var mainTexture = TextureFactory.Instance.CreateTexture("E:\\cgModel\\Image\\Contact_Cover.jpg");

            var rectangle = new Rectangle();
            var rectangleObject = PolygonUtility.CreatePolygon("Rectangle", rectangle);
            var shader = ShaderCreater.Instance.CreateShader(GBufferType.Albedo);
            var textures = new Dictionary<TextureKind, TextureBuffer>();
            textures.Add(TextureKind.Albedo, mainTexture);

            rectangleObject.Material = new Material(shader, textures);
            MainScene.AddObject(new PolygonNode(rectangleObject));
            CameraMode = CameraMode.Ortho;


            var icosahedron = new Icosahedron(0.5f, 1, Vector3.Zero);

            var sphereObject = PolygonUtility.CreatePolygon("Sphere", icosahedron);
            PolygonNode sphereNode = new PolygonNode(sphereObject);
            sphereNode.Visible = false;
            var sphereShader = ShaderCreater.Instance.CreateShader(GBufferType.PointColor);
            sphereObject.Material = new Material(sphereShader);
            MainScene.AddObject(new PolygonNode(sphereObject));

            cameraController.TargetCamera = MainScene.MainCamera;
        }

        protected override void InitializeRenderer()
        {
            RenderTechniqueFactory.Instance.RendererSystem = RenderSystem;
            RenderSystem.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer));
            var gBufferTexture = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            RenderSystem.RenderQueue.AddTechnique(RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred));
            RenderSystem.OutputBuffer = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output) as OutputBuffer;
            //RenderSystem.OutputTexture = RenderSystem.RenderQueue.OutputTexture<DeferredRendering>()[0];
            RenderSystem.OutputTexture = gBufferTexture[(int)GBuffer.OutputTextureType.Color];

            Bloom bloom = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Bloom) as Bloom;
            bloom.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            RenderSystem.PostEffect.AddTechnique(bloom);

            Sobel sobel = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Sobel) as Sobel;
            sobel.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            RenderSystem.PostEffect.AddTechnique(sobel);

            GrayScale grayScale = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GrayScale) as GrayScale;
            grayScale.uTarget = gBufferTexture[(int)GBuffer.OutputTextureType.Color];
            RenderSystem.PostEffect.AddTechnique(grayScale);
        }

        protected override void ProcessKeyInput(KeyEventArgs e)
        {
        }

        protected override void OnResizeEvent(object sender, EventArgs e)
        {
            if (MainScene != null)
            {
                if (CameraMode == CameraMode.Perspective)
                {
                    MainScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
                }
            }

            RenderSystem.SizeChanged(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        protected override void OnRenderEvent(object sender, PaintEventArgs e)
        {
            RenderSystem.Render();
        }

        public void Invalidate()
        {
            Viewport.Instance.GLControl_Paint(this, null);
        }

        public void ChangeVisible(VisibleItem item)
        {
            MainScene.SetAllVisible(false);

            if (item == VisibleItem.Plane)
            {
                var sceneNode = MainScene.FindNode("Rectangle");
                sceneNode.Visible = true;
                CameraMode = CameraMode.Ortho;
            }
            else if (item == VisibleItem.Sphere)
            {
                var sceneNode = MainScene.FindNode("Sphere");
                sceneNode.Visible = true;
                CameraMode = CameraMode.Perspective;
            }

            Invalidate();
        }

        /// <summary>
        /// マウス入力
        /// </summary>
        /// <param name="mouse">マウス情報</param>
        /// <param name="state">状態</param>
        protected override void ProcessMouseInput(KIMouseEventArgs mouse)
        {
            if(CameraMode == CameraMode.Ortho)
            {
                return; 
            }

            switch (mouse.MouseState)
            {
                case MOUSE_STATE.DOWN:
                    cameraController.Down(mouse);
                    break;
                case MOUSE_STATE.MOVE:
                    cameraController.Move(mouse);
                    break;
                case MOUSE_STATE.UP:
                    break;
                case MOUSE_STATE.WHEEL:
                    cameraController.Wheel(mouse);
                    break;
            }
        }
    }
}
