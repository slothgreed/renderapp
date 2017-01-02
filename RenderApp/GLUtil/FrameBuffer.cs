using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
using RenderApp.GLUtil.Buffer;
namespace RenderApp.GLUtil
{
    public class FrameBuffer : BufferObject
    {
        public string Name { get; private set; }
        /// <summary>
        /// 幅
        /// </summary>
        public int Width = 0;
        /// <summary>
        /// 高さ
        /// </summary>
        public int Height = 0;
        /// <summary>
        /// レンダーバッファID
        /// </summary>
        public RenderBuffer RenderBuffer { get; private set; }
        /// <summary>
        /// テクスチャ
        /// </summary>
        public List<Texture> TextureList = new List<Texture>();
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
        #region [constructor]
        public FrameBuffer(string FrameName)
        {
            this.Name = FrameName;
        }
        public FrameBuffer(string FrameName,int width, int height,string textureName)
        {
            this.Name = FrameName;
            Initialize(width, height, new string[] { textureName });
        }

        public FrameBuffer(string FrameName,int width,int height,string[] textureName)
        {
            this.Name = FrameName;
            Initialize(width, height, textureName);
        }

        protected void Initialize(int width, int height, string[] textureName)
        {
            RenderBuffer = new RenderBuffer();
            Width = width;
            Height = height;
            for(int i = 0; i < textureName.Length; i++)
            {
                TextureList.Add(TextureFactory.Instance.CreateTexture(textureName[i],width,height));
                Attachment.Add(FramebufferAttachment.ColorAttachment0 + i);
                OutputBuffers.Add(DrawBuffersEnum.ColorAttachment0 + i);
            }
            CreateFrameBuffer();
        }
        #endregion
        private void CreateFrameBuffer()
        {
            GenBuffer();
            BindBuffer();
            RenderBuffer.GenBuffer();
            RenderBuffer.Storage(RenderbufferStorage.DepthComponent32,Width,Height);

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, RenderBuffer.ID);
            
            for(int i = 0; i < TextureList.Count; i++)
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, Attachment[i], TextureTarget.Texture2D, TextureList[i].ID, 0);
            }

            RenderBuffer.UnBindBuffer();
            UnBindBuffer();
            Output.GLLog(Output.LogLevel.Error);
        }

        public void SizeChanged(int width,int height)
        {
            this.Height = height;
            this.Width = width;
        
            RenderBuffer.SizeChanged(Width, Height);
            foreach (var texture in TextureList)
            {
                texture.TextureBuffer.SizeChanged(Width, Height);
            }
        }
        public void ClearBuffer()
        {
            if (ID != -1)
            {
                BindBuffer();
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                UnBindBuffer();
            }
        }
        public override void PreBindBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
            GL.DrawBuffers(OutputBuffers.Count, OutputBuffers.ToArray());
        }
        public override void PreUnBindBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.DrawBuffer(DefaultOutBuffer);
        }
        public override void PreGenBuffer()
        {
            ID = GL.GenFramebuffer();
        }
        public override void PreDispose()
        {
            GL.DeleteFramebuffer(ID);
            RenderBuffer.Dispose();
            foreach(var texture in TextureList)
            {
                texture.Dispose();
            }
        }
        public override string ToString()
        {
            return this.Name;
        }


    }
}
