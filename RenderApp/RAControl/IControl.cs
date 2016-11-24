using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RenderApp.Utility;
using OpenTK;
namespace RenderApp.RA_Control
{
    public abstract class IControl
    {
        protected static RAMouse LeftMouse = new RAMouse();
        protected static RAMouse MiddleMouse = new RAMouse();
        protected static RAMouse RightMouse = new RAMouse();

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
        /// コントローラ開始処理
        /// </summary>
        /// <returns></returns>
        public virtual bool Binding()
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
        public virtual bool Execute()
        {
            return true;
        }
        public virtual bool Reset()
        {
            return true;
        }
    }
}
