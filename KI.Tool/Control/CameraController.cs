using System.Windows.Forms;
using KI.Asset;
using OpenTK;

namespace KI.Tool.Control
{
    /// <summary>
    /// カメラコントローラ
    /// </summary>
    public class CameraController : IController
    {

        /// <summary>
        /// ズームイン倍率
        /// </summary>
        private float zoomInRatio = 1.1f;

        /// <summary>
        /// ズームアウト倍率
        /// </summary>
        private float zoomOutRatio = 0.9f;

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
                    Translate(Global.Renderer.ActiveScene.MainCamera, new Vector3(-middleMouse.Delta.X, -middleMouse.Delta.Y, 0));
                    break;
                case MouseButtons.Right:
                    rightMouse.Move(mouse.X, mouse.Y);
                    Rotate(Global.Renderer.ActiveScene.MainCamera, new Vector3(-rightMouse.Delta.X, -rightMouse.Delta.Y, 0));
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
                    Camera camera = Global.Renderer.ActiveScene.MainCamera;
                    if (mouse.Delta > 0)
                    {
                        camera.LookAtDistance = camera.LookAtDistance * zoomInRatio;
                    }
                    else
                    {
                        camera.LookAtDistance = camera.LookAtDistance * zoomOutRatio;
                    }
                    break;
            }

            return true;
        }


        /// <summary>
        /// キー押下
        /// </summary>
        /// <param name="e">キー入力</param>
        /// <returns>成功</returns>
        public override bool KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F)
            {
                Global.Renderer.ActiveScene.FitToScene(Global.Renderer.ActiveScene.MainCamera);
            }

            return true;
        }

        /// <summary>
        /// 平行移動
        /// </summary>
        /// <param name="camera">カメラ</param>
        /// <param name="delta">移動量</param>
        private void Translate(Camera camera, Vector3 delta)
        {
            Vector3 vectorX = camera.Matrix.Column0.Xyz;
            Vector3 vectorY = camera.Matrix.Column1.Xyz;

            vectorX *= delta.X;
            vectorY *= delta.Y;
            Vector3 value = (vectorX + vectorY) * camera.LookAtDistance * 0.01f;
            camera.LookAt = camera.LookAt + value;
        }


        /// <summary>
        /// 回転
        /// </summary>
        /// <param name="camera">カメラ</param>
        /// <param name="delta">移動量</param>
        private void Rotate(Camera camera, Vector3 delta)
        {
            camera.Rotate(delta * 0.5f);
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
