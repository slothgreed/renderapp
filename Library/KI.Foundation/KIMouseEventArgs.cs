using System.Windows.Forms;
using OpenTK;

namespace KI.Foundation.Controller
{
    /// <summary>
    /// マウスの状態
    /// </summary>
    public enum MOUSE_STATE
    {
        DOWN,
        MOVE,
        UP,
        CLICK,
        DOUBLECLICK,
        WHEEL,
    }

    /// <summary>
    /// マウスの状態
    /// </summary>
    public enum MOUSE_BUTTON
    {
        None,
        Left,
        Right,
        Middle
    }

    /// <summary>
    /// OpenTKのマウスのイベント情報のラッパ
    /// </summary>
    public class KIMouseEventArgs
    {
        /// <summary>
        /// マウスの状態
        /// </summary>
        public MOUSE_STATE MouseState { get; private set; }

        /// <summary>
        /// 状態が発生したボタンの種類
        /// </summary>
        public MOUSE_BUTTON Button { get; private set; }

        /// <summary>
        /// 現在の座標
        /// </summary>
        public Vector2 Current { get; private set; }

        /// <summary>
        /// 前回の座標からの差分
        /// </summary>
        public Vector2 Delta { get; private set; }

        /// <summary>
        /// ホイール
        /// </summary>
        public int Wheel { get; private set; }

        /// <summary>
        /// デルタを求めるための前回のマウス位置staticで保持
        /// </summary>
        private static Vector2 beforePosition;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="e">OpenTKから発生したイベント</param>
        /// <param name="state">マウスの状態</param>
        public KIMouseEventArgs(MouseEventArgs e, MOUSE_STATE state)
        {
            MouseState = state;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    Button = MOUSE_BUTTON.Left;
                    break;
                case MouseButtons.None:
                    Button = MOUSE_BUTTON.None;
                    break;
                case MouseButtons.Right:
                    Button = MOUSE_BUTTON.Right;
                    break;
                case MouseButtons.Middle:
                    Button = MOUSE_BUTTON.Middle;
                    break;
                default:
                    break;
            }

            if (beforePosition == Vector2.Zero)
            {
                Delta = Vector2.Zero;
            }
            else
            {
                Delta = new Vector2(e.X - beforePosition.X, e.Y - beforePosition.Y);
            }

            Current = new Vector2(e.X, e.Y);
            beforePosition = Current;
            Wheel = e.Delta;
        }
    }
}
