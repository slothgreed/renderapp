using KI.Foundation.Core;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.Render;
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
            DepthTexture = null;
        }

        public RenderBuffer RenderBuffer
        {
            get;
            private set;
        }

        public RenderTexture DepthTexture
        {
            get;
            private set;
        }

        /// <summary>
        /// オブジェクトを表す文字列
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// バッファのバインド
        /// </summary>
        protected override void BindBufferCore()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, DeviceID);
        }

        /// <summary>
        /// バッファのバインド解除
        /// </summary>
        protected override void UnBindBufferCore()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        /// <summary>
        /// バッファの生成
        /// </summary>
        protected override void GenBufferCore()
        {
            DeviceID = GL.GenFramebuffer();
        }

        /// <summary>
        /// バッファの解放
        /// </summary>
        protected override void DisposeCore()
        {
            if (RenderBuffer != null)
            {
                RenderBuffer.Dispose();
            }

            if(DepthTexture != null)
            {
                DepthTexture.Dispose();
            }

            GL.DeleteFramebuffer(DeviceID);
        }

        /// <summary>
        /// レンダーバッファの設定
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        public void SetupRenderBuffer(int width, int height)
        {
            BindBuffer();
            RenderBuffer = BufferFactory.Instance.CreateRenderBuffer();
            RenderBuffer.GenBuffer();
            RenderBuffer.Storage(RenderbufferStorage.DepthComponent, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, RenderBuffer.DeviceID);
            RenderBuffer.UnBindBuffer();
            UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// レンダーバッファの設定(Depthバッファもテクスチャを利用する)
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        public void SetupRenderBufferUseDepthTexture(int width, int height)
        {
            BindBuffer();
            DepthTexture = new RenderTexture("Depth Texture", width, height, PixelFormat.DepthComponent);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, DepthTexture.DeviceID, 0);
            UnBindBuffer();

        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        public void SizeChanged(int width, int height)
        {
            if (RenderBuffer != null)
            {
                RenderBuffer.SizeChanged(width, height);
            }

            if (DepthTexture != null)
            {
                DepthTexture.TextureBuffer.SizeChanged(width, height);
            }
        }
    }
}
