using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using KI.Gfx.GLUtil;
using KI.Tool.Control;
using KI.UI.ViewModel;
using RenderApp.Globals;

namespace RenderApp.ViewModel
{
    public class ViewportViewModel : TabItemViewModel
    {
        private WindowsFormsHost glContext;

        public override string Title
        {
            get
            {
                return "RenderApp";
            }
        }

        public ViewportViewModel(ViewModelBase parent)
            : base(parent)
        {
        }

        public WindowsFormsHost GLContext
        {
            get
            {
                return glContext;
            }
            private set
            {
                SetValue(ref glContext, value);
            }
        }

        public void Intialize()
        {
            Viewport.Instance.OnLoaded += OnLoadedEvent;
            Viewport.Instance.OnMouseDown += OnMouseDownEvent;
            Viewport.Instance.OnMouseMove += OnMouseMoveEvent;
            Viewport.Instance.OnMouseUp += OnMouseMoveUpEvent;
            Viewport.Instance.OnMouseWheel += OnMouseWheelEvent;
            Viewport.Instance.OnRender += OnRenderEvent;
            Viewport.Instance.OnResize += OnResizeEvent;
            GLContext = new WindowsFormsHost()
            {
                Child = Viewport.Instance.GLControl
            };
        }

        #region [Viewport Method]
        public void OnLoadedEvent(object sender, EventArgs e)
        {
            Workspace.RenderSystem.Initialize(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
            MainWindowViewModel.Instance.WorkspaceViewModel.RendererViewModel.Model = Workspace.RenderSystem;
            Workspace.MainScene.Initialize();
        }

        private void OnResizeEvent(object sender, EventArgs e)
        {
            Workspace.MainScene.MainCamera.SetProjMatrix((float)DeviceContext.Instance.Width / DeviceContext.Instance.Height);
            Workspace.RenderSystem.SizeChanged(DeviceContext.Instance.Width, DeviceContext.Instance.Height);
        }

        private void OnRenderEvent(object sender, PaintEventArgs e)
        {
            Workspace.RenderSystem.Render();
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

        public override void UpdateProperty()
        {
        }
    }
}
