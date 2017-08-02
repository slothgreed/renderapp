using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil
{
    public class FrameBuffer : BufferObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        internal FrameBuffer(string name)
        {
            this.Name = name;
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
