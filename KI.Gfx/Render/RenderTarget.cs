using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Gfx.KIAsset;
using KI.Foundation.Utility;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.GLUtil.Buffer;
namespace KI.Gfx.Render
{
    public class RenderTarget : KIObject 
    {
        /// <summary>
        /// 幅
        /// </summary>
        public int Width = 0;
        /// <summary>
        /// 高さ
        /// </summary>
        public int Height = 0;
        /// <summary>
        /// フレームバッファ
        /// </summary>
        public FrameBuffer FrameBuffer { get; private set; }
        /// <summary>
        /// レンダーバッファID
        /// </summary>
        public RenderBuffer RenderBuffer { get; private set; }
        /// <summary>
        /// テクスチャのアタッチメント
        /// </summary>
        public List<FramebufferAttachment> Attachment = new List<FramebufferAttachment>();
        /// <summary>
        /// 描画場所
        /// </summary>
        public List<DrawBuffersEnum> OutputBuffers = new List<DrawBuffersEnum>();
        /// <summary>
        /// デフォルトの出力先
        /// </summary>
        public static DrawBufferMode DefaultOutBuffer = DrawBufferMode.Back;

        internal RenderTarget(string name, int width, int height,int num)
            :base(name)
        {
            FrameBuffer = BufferFactory.Instance.CreateFrameBuffer(name);
            Initialize(width, height, num);
        }

        private void Initialize(int width, int height, int num)
        {
            RenderBuffer = new RenderBuffer();
            Width = width;
            Height = height;

            for (int i = 0; i < num; i++)
            {
                Attachment.Add(FramebufferAttachment.ColorAttachment0 + i);
                OutputBuffers.Add(DrawBuffersEnum.ColorAttachment0 + i);
            }

            CreateFrameBuffer();
        }

        private void CreateFrameBuffer()
        {
            FrameBuffer.GenBuffer();
            CreateRenderBuffer();
        }
        private void CreateRenderBuffer()
        {
            FrameBuffer.BindBuffer();
            RenderBuffer.GenBuffer();
            RenderBuffer.Storage(RenderbufferStorage.DepthComponent, Width, Height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, RenderBuffer.DeviceID);
            RenderBuffer.UnBindBuffer();
            FrameBuffer.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }
        public void SizeChanged(int width, int height)
        {
            if(width == Width && Height == height)
            {
                return;
            }
            Height = height;
            Width = width;
            RenderBuffer.SizeChanged(width, height);

        }

        public void ClearBuffer()
        {
            FrameBuffer.BindBuffer();
            DeviceContext.Instance.Clear();
            FrameBuffer.UnBindBuffer();
        }

        public override void Dispose()
        {
            RenderBuffer.Dispose();
            FrameBuffer.Dispose();
        }

        public void BindRenderTarget(Texture output)
        {
            BindRenderTarget(new Texture[] { output });
        }

        public void BindRenderTarget(Texture[] outputs)
        {
            FrameBuffer.BindBuffer();
            for (int i = 0; i < outputs.Length; i++)
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, Attachment[i], TextureTarget.Texture2D, outputs[i].DeviceID, 0);

                if(outputs[i].Width != Width || outputs[i].Height != Height)
                {
                    outputs[i].TextureBuffer.SizeChanged(Width, Height);
                }
            }
            GL.DrawBuffers(OutputBuffers.Count, OutputBuffers.ToArray());
        }
        public void UnBindRenderTarget()
        {
            FrameBuffer.UnBindBuffer();
            GL.DrawBuffer(DefaultOutBuffer);
        }
    }
}
