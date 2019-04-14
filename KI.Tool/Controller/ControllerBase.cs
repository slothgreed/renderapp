using System.Windows.Forms;
using KI.Gfx.GLUtil;

namespace KI.Tool.Controller
{
    /// <summary>
    /// コントロールのインタフェース
    /// </summary>
    public abstract class ControllerBase
    {
        /// <summary>
        /// マウス押下
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public virtual bool Down(KIMouseEventArgs mouse)
        {
            return true;
        }

        /// <summary>
        /// マウスクリック
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public virtual bool Click(KIMouseEventArgs mouse)
        {
            return true;
        }


        /// <summary>
        /// マウスダブルクリック
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public virtual bool DoubleClick(KIMouseEventArgs mouse)
        {
            return true;
        }

        /// <summary>
        /// マウス移動
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public virtual bool Move(KIMouseEventArgs mouse)
        {
            return true;
        }

        /// <summary>
        /// マウス押上げ
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public virtual bool Up(KIMouseEventArgs mouse)
        {
            return true;
        }

        /// <summary>
        /// ホイール
        /// </summary>
        /// <param name="mouse">マウスイベント</param>
        /// <returns>成功</returns>
        public virtual bool Wheel(KIMouseEventArgs mouse)
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
