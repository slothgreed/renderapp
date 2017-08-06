using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil
{
    /// <summary>
    /// フレームバッファ
    /// </summary>
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

        /// <summary>
        /// バッファのバインド
        /// </summary>
        public override void BindBufferCore()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, DeviceID);
        }

        /// <summary>
        /// バッファのバインド解除
        /// </summary>
        public override void UnBindBufferCore()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        /// <summary>
        /// バッファの生成
        /// </summary>
        public override void GenBufferCore()
        {
            DeviceID = GL.GenFramebuffer();
        }

        /// <summary>
        /// バッファの解放
        /// </summary>
        public override void DisposeCore()
        {
            GL.DeleteFramebuffer(DeviceID);
        }

        /// <summary>
        /// 文字列
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
