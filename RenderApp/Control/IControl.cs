using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RenderApp.Utility;
using OpenTK;
namespace RenderApp.Control
{
    public abstract class IControl
    {
        protected static Mouse LeftMouse = new Mouse();
        protected static Mouse MiddleMouse = new Mouse();
        protected static Mouse RightMouse = new Mouse();

        public virtual bool Down(MouseEventArgs mouse)
        {
            return true;
        }
        public virtual bool Click(MouseEventArgs mouse)
        {
            return true;
        }
        public virtual bool Move(MouseEventArgs mouse)
        {
            return true;
        }
        public virtual bool Up(MouseEventArgs mouse)
        {
            return true;
        }
        public virtual bool Wheel(MouseEventArgs mouse)
        {
            return true;
        }
        /// <summary>
        /// コントローラ終了処理
        /// </summary>
        /// <returns></returns>
        public virtual bool UnBinding()
        {
            return true;
        }
        /// <summary>
        /// コントローラ開始処理
        /// </summary>
        /// <returns></returns>
        public virtual bool Binding()
        {
            return true;
        }
    }
}
