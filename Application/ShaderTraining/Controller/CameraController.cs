using System.Windows.Forms;
using KI.Asset;
using KI.Gfx.Buffer;
using KI.Foundation.Controller;
using OpenTK;

namespace ShaderTraining.Tool.Controller
{
    /// <summary>
    /// カメラコントローラ
    /// </summary>
    public class CameraController : ControllerBase
    {
        /// <summary>
        /// 操作するカメラ
        /// </summary>
        public Camera TargetCamera
        {
            get;
            set;
        }

        /// <summary>
        /// ズームイン倍率
        /// </summary>
        private float zoomInRatio = 1.1f;

        /// <summary>
        /// ズームアウト倍率
        /// </summary>
        private float zoomOutRatio = 0.9f;

        /// <summary>
        /// マウス移動
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Move(KIMouseEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case MOUSE_BUTTON.Right:
                    Rotate(TargetCamera, new Vector3(-mouse.Delta.X, -mouse.Delta.Y, 0));
                    break;
            }

            return true;
        }

        /// <summary>
        /// ホイール
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public override bool Wheel(KIMouseEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case MOUSE_BUTTON.None:
                    if (mouse.Wheel > 0)
                    {
                        TargetCamera.LookAtDistance = TargetCamera.LookAtDistance * zoomInRatio;
                    }
                    else
                    {
                        TargetCamera.LookAtDistance = TargetCamera.LookAtDistance * zoomOutRatio;
                    }

                    break;
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
        public override bool Binding(IControllerArgs args)
        {
            return true;
        }
    }
}
