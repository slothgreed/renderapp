using System.Windows.Forms;
using KI.Renderer;

namespace KI.Tool.Control
{
    /// <summary>
    /// カメラコントローラ
    /// </summary>
    public class CameraControl : IControl
    {
        /// <summary>
        /// マウス押下
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Down(MouseEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case MouseButtons.Left:
                    leftMouse.Down(mouse.X, mouse.Y);
                    break;
                case MouseButtons.Middle:
                    middleMouse.Down(mouse.X, mouse.Y);
                    break;
                case MouseButtons.Right:
                    rightMouse.Down(mouse.X, mouse.Y);
                    break;
            }

            return true;
        }

        /// <summary>
        /// マウスクリック
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
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

        /// <summary>
        /// マウス移動
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Move(MouseEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case MouseButtons.Left:
                    leftMouse.Move(mouse.X, mouse.Y);
                    break;
                case MouseButtons.Middle:
                    middleMouse.Move(mouse.X, mouse.Y);
                    Global.Renderer.ActiveScene.MainCamera.Translate(middleMouse.Delta.X, middleMouse.Delta.Y, 0);
                    break;
                case MouseButtons.Right:
                    rightMouse.Move(mouse.X, mouse.Y);
                    Global.Renderer.ActiveScene.MainCamera.Rotate(rightMouse.Delta.X, rightMouse.Delta.Y, 0);
                    break;
            }

            return true;
        }

        /// <summary>
        /// マウス押上げ
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Up(MouseEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case MouseButtons.Left:
                    leftMouse.Up(mouse.X, mouse.Y);
                    break;
                case MouseButtons.Middle:
                    middleMouse.Up(mouse.X, mouse.Y);
                    break;
                case MouseButtons.Right:
                    leftMouse.Up(mouse.X, mouse.Y);
                    break;
            }

            return true;
        }

        /// <summary>
        /// ホイール
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Wheel(MouseEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case MouseButtons.None:
                    Global.Renderer.ActiveScene.MainCamera.Zoom((int)mouse.Delta);
                    break;
            }

            return true;
        }

        /// <summary>
        /// コントローラ終了処理
        /// </summary>
        /// <returns>成功</returns>
        public override bool UnBinding()
        {
            return true;
        }

        /// <summary>
        /// コントローラ開始処理
        /// </summary>
        /// <returns>成功</returns>
        public override bool Binding()
        {
            return true;
        }
    }
}
