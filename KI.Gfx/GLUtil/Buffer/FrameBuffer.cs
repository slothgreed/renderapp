using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.GLUtil;
using KI.Foundation.Utility;

namespace KI.Gfx.GLUtil
{
    public class FrameBuffer : BufferObject
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="FrameName"></param>
        internal FrameBuffer(string FrameName)
        {
            this.Name = FrameName;
        }
        public override void PreBindBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, DeviceID);
        }
        public override void PreUnBindBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        public override void PreGenBuffer()
        {
            DeviceID = GL.GenFramebuffer();
        }
        public override void PreDispose()
        {
            GL.DeleteFramebuffer(DeviceID);
        }
        public override string ToString()
        {
            return this.Name;
        }


    }
}
