using KI.Foundation.Core;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil
{
    /// <summary>
    /// レンダーバッファ
    /// </summary>
    public class RenderBuffer : BufferObject
    {
        /// <summary>
        /// バッファストレージ
        /// </summary>
        private RenderbufferStorage bufferStorage;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal RenderBuffer()
        {
        }

        /// <summary>
        /// ストレージの設定
        /// </summary>
        /// <param name="storage">ストレージ</param>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void Storage(RenderbufferStorage storage, int width, int height)
        {
            bufferStorage = storage;
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
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, bufferStorage, width, height);
            UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// バッファの解放
        /// </summary>
        protected override void DisposeCore()
        {
            GL.DeleteRenderbuffer(DeviceID);
        }

        /// <summary>
        /// バッファの生成
        /// </summary>
        protected override void GenBufferCore()
        {
            DeviceID = GL.GenRenderbuffer();
        }

        /// <summary>
        /// バッファのバインド
        /// </summary>
        protected override void BindBufferCore()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, DeviceID);
        }

        /// <summary>
        /// バッファのバインド解除
        /// </summary>
        protected override void UnBindBufferCore()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }
    }
}
