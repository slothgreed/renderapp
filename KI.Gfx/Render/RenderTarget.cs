using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.GLUtil;
using KI.Gfx.KIAsset;
using KI.Foundation.Utility;
using OpenTK.Graphics.OpenGL;
namespace KI.Gfx.Render
{
    public class RenderTarget
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
        /// テクスチャ
        /// </summary>
        public List<Texture> Textures = new List<Texture>();
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

        public RenderTarget(string name)
        {
            FrameBuffer = new FrameBuffer(name);
        }
        public RenderTarget(string name, int width, int height, Texture[] texture)
        {
            FrameBuffer = new FrameBuffer(name);
            Initialize(width, height, texture);
        }
        public RenderTarget(string name, int width, int height, Texture texture)
        {
            FrameBuffer = new FrameBuffer(name);
            Initialize(width, height, new Texture[] { texture });
        }

        protected void Initialize(int width,int height,Texture[] texture)
        {
            RenderBuffer = new RenderBuffer();
            Width = width;
            Height = height;
            for (int i = 0; i < texture.Length; i++)
            {
                Textures.Add(texture[i]);
                Attachment.Add(FramebufferAttachment.ColorAttachment0 + i);
                OutputBuffers.Add(DrawBuffersEnum.ColorAttachment0 + i);
            }
            CreateFrameBuffer();
        }

        private void CreateFrameBuffer()
        {
            FrameBuffer.GenBuffer();
            FrameBuffer.BindBuffer();
            RenderBuffer.GenBuffer();
            RenderBuffer.Storage(RenderbufferStorage.DepthComponent32, Width, Height);

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, RenderBuffer.DeviceID);

            for (int i = 0; i < Textures.Count; i++)
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, Attachment[i], TextureTarget.Texture2D, Textures[i].DeviceID, 0);
            }

            RenderBuffer.UnBindBuffer();
            FrameBuffer.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        public void SizeChanged(int width, int height)
        {
            Height = height;
            Width = width;
            RenderBuffer.SizeChanged(width, height);

            foreach(var texture in Textures)
            {
                texture.TextureBuffer.SizeChanged(width, height);
            }
        }

        public void ClearBuffer()
        {
            FrameBuffer.BindBuffer();
            DeviceContext.Instance.Clear();
            FrameBuffer.UnBindBuffer();
        }

        public void Dispose()
        {
            FrameBuffer.Dispose();
            RenderBuffer.Dispose();
            foreach(var texture in Textures)
            {
                texture.Dispose();
            }
        }

        public void BindRenderTarget()
        {
            FrameBuffer.BindBuffer();
            GL.DrawBuffers(OutputBuffers.Count, OutputBuffers.ToArray());
        }
        public void UnBindRenderTarget()
        {
            FrameBuffer.UnBindBuffer();
            GL.DrawBuffer(DefaultOutBuffer);
        }
    }
}
