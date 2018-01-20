using System;
using System.Windows.Forms.Integration;
using System.Windows.Forms;
using RenderApp;
using RenderApp.RAControl;
using KI.Foundation.ViewModel;
using KI.Gfx.GLUtil;
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
                        Child = Viewport.Instance.glControl
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

        public ViewportViewModel()
        {
            Viewport.Instance.OnLoaded += OnLoadedEvent;
            Viewport.Instance.OnResize += OnResizeEvent;
            Viewport.Instance.OnRender += OnRenderEvent;
            Viewport.Instance.OnMouseWheel += OnMouseWheelEvent;
            Viewport.Instance.OnMouseUp += OnMouseMoveUpEvent;
            Viewport.Instance.OnMouseMove += OnMouseMoveEvent;
            Viewport.Instance.OnMouseDown += OnMouseDownEvent;
        }

        public void OnLoadedEvent(object sender, EventArgs e)
        {
            SceneManager.Instance.Create("MainScene");
            SceneManager.Instance.CreateMainCamera();
            SceneManager.Instance.CreateSceneLight();
            SceneManager.Instance.RenderSystem.Initialize(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }
        private void OnResizeEvent(object sender, EventArgs e)
        {
            if (SceneManager.Instance.ActiveScene != null)
            {
                SceneManager.Instance.ActiveScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
            }
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
    }
}
