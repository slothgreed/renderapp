using System;
using System.Windows.Forms;
using KI.Foundation.Core;

namespace KI.Tool.Control
{
    /// <summary>
    /// コントロールのインタフェース
    /// </summary>
    public abstract class IControl
    {
        /// <summary>
        /// 左ボタン
        /// </summary>
        protected static KIMouse leftMouse { get; } = new KIMouse();

        /// <summary>
        /// 真ん中ボタン
        /// </summary>
        protected static KIMouse middleMouse { get; } = new KIMouse();

        /// <summary>
        /// 右ボタン
        /// </summary>
        protected static KIMouse rightMouse { get; } = new KIMouse();

        /// <summary>
        /// マウス押下
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public virtual bool Down(MouseEventArgs mouse)
        {
            return true;
        }

        /// <summary>
        /// マウスクリック
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public virtual bool Click(MouseEventArgs mouse)
        {
            return true;
        }

        /// <summary>
        /// マウス移動
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public virtual bool Move(MouseEventArgs mouse)
        {
            return true;
        }

        /// <summary>
        /// マウス押上げ
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public virtual bool Up(MouseEventArgs mouse)
        {
            return true;
        }

        /// <summary>
        /// ホイール
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public virtual bool Wheel(MouseEventArgs mouse)
        {
            return true;
        }

        /// <summary>
        /// コントローラ開始処理
        /// </summary>
        /// <returns>成功</returns>
        public virtual bool Binding()
        {
            return true;
        }

        /// <summary>
        /// コントローラ終了処理
        /// </summary>
        /// <returns>成功</returns>
        public virtual bool UnBinding()
        {
            return true;
        }

        /// <summary>
        /// キー押下イベント
        /// </summary>
        /// <param name="e">キー</param>
        /// <returns>成功</returns>
        public virtual bool KeyDown(KeyEventArgs e)
        {
            return true;
        }
    }
}
