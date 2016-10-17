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
        public virtual bool Click(MouseEventArgs mouse)
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
        public virtual bool Move(MouseEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    LeftMouse.Move(mouse.X, mouse.Y);
                    break;
                case System.Windows.Forms.MouseButtons.Middle:
                    MiddleMouse.Move(mouse.X, mouse.Y);
                    Scene.ActiveScene.MainCamera.Translate(new Vector3(MiddleMouse.Delta.X, MiddleMouse.Delta.Y, 0));
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    RightMouse.Move(mouse.X, mouse.Y);
                    Scene.ActiveScene.MainCamera.Rotate(new Vector3(RightMouse.Delta.X, RightMouse.Delta.Y, 0));
                    break;
            }
            return true;
        }
        public virtual bool Up(MouseEventArgs mouse)
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
        public virtual bool Wheel(MouseEventArgs mouse)
        {
            switch (mouse.Button)
            {
                case MouseButtons.None:
                    Scene.ActiveScene.MainCamera.Zoom((int)mouse.Delta);
                    break;
            }
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
