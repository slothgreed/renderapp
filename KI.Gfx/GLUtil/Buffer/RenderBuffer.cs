using KI.Foundation.Utility;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil
{
    /// <summary>
    /// レンダーバッファ
    /// </summary>
    public class RenderBuffer : BufferObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal RenderBuffer()
        {
        }

        RenderbufferStorage BufferStorage { get; set; }

        /// <summary>
        /// バッファの生成
        /// </summary>
        public override void GenBufferCore()
        {
            DeviceID = GL.GenRenderbuffer();
        }

        /// <summary>
        /// バッファのバインド
        /// </summary>
        public override void BindBufferCore()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, DeviceID);
        }

        /// <summary>
        /// バッファのバインド解除
        /// </summary>
        public override void UnBindBufferCore()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }

        public void Storage(RenderbufferStorage storage, int width, int height)
        {
            BufferStorage = storage;
            SizeChanged(width, height);
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SizeChanged(int width, int height)
        {
            BindBuffer();
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, BufferStorage, width, height);
            UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// バッファの解放
        /// </summary>
        public override void DisposeCore()
        {
            GL.DeleteRenderbuffer(DeviceID);
        }
    }
}
