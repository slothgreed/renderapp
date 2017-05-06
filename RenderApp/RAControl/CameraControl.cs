using System.Windows.Forms;

namespace RenderApp.RAControl
{
    class CameraControl : IControl
    {
        public override bool Down(MouseEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case MouseButtons.Left:
                    LeftMouse.Down(mouse.X, mouse.Y);
                    break;
                case MouseButtons.Middle:
                    MiddleMouse.Down(mouse.X, mouse.Y);
                    break;
                case MouseButtons.Right:
                    RightMouse.Down(mouse.X, mouse.Y);
                    break;
            }
            return true;
        }
        public override bool Click(MouseEventArgs mouse)
        {
            //switch (mouse.Button)
            //{
            //    case MouseButtons.Left:
            //        LeftMouse.Click(mouse.X, mouse.Y);
            //        break;
            //    case MouseButtons.Middle:
            //        MiddleMouse.Click(mouse.X, mouse.Y);
            //        break;
            //    case MouseButtons.Right:
            //        LeftMouse.Click(mouse.X, mouse.Y);
            //        break;
            //}
            return true;
        }
        public override bool Move(MouseEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    LeftMouse.Move(mouse.X, mouse.Y);
                    break;
                case System.Windows.Forms.MouseButtons.Middle:
                    MiddleMouse.Move(mouse.X, mouse.Y);
                    SceneManager.Instance.ActiveScene.MainCamera.Translate(MiddleMouse.Delta.X, MiddleMouse.Delta.Y, 0);
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    RightMouse.Move(mouse.X, mouse.Y);
                    SceneManager.Instance.ActiveScene.MainCamera.Rotate(RightMouse.Delta.X, RightMouse.Delta.Y, 0);
                    break;
            }
            return true;
        }
        public override bool Up(MouseEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case MouseButtons.Left:
                    LeftMouse.Up(mouse.X, mouse.Y);
                    break;
                case MouseButtons.Middle:
                    MiddleMouse.Up(mouse.X, mouse.Y);
                    break;
                case MouseButtons.Right:
                    LeftMouse.Up(mouse.X, mouse.Y);
                    break;
            }
            return true;
        }
        public override bool Wheel(MouseEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case MouseButtons.None:
                    SceneManager.Instance.ActiveScene.MainCamera.Zoom((int)mouse.Delta);
                    break;
            }
            return true;
        }
        /// <summary>
        /// コントローラ終了処理
        /// </summary>
        /// <returns></returns>
        public override bool UnBinding()
        {
            return true;
        }
        /// <summary>
        /// コントローラ開始処理
        /// </summary>
        /// <returns></returns>
        public override bool Binding()
        {
            return true;
        }
    }
}
