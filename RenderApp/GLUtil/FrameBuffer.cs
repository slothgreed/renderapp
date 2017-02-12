using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
using KI.Gfx.GLUtil;
using KI.Foundation.Utility;
using RenderApp.GLUtil;
using KI.Gfx.KIAsset;
namespace RenderApp.GLUtil
{
    public class FrameBuffer : BufferObject
    {
        /// <summary>
        /// 名前
        /// </summary>
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
                var texture = TextureFactory.Instance.CreateTexture(textureName[i], width, height);
                TextureList.Add(texture);
                RenderApp.Globals.Project.ActiveProject.AddChild(texture);
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

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, RenderBuffer.DeviceID);
            
            for(int i = 0; i < TextureList.Count; i++)
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, Attachment[i], TextureTarget.Texture2D, TextureList[i].DeviceID, 0);
            }

            RenderBuffer.UnBindBuffer();
            UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
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
            if (DeviceID != -1)
            {
                BindBuffer();
                DeviceContext.Instance.Clear();
                UnBindBuffer();
            }
        }
        public override void PreBindBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, DeviceID);
            GL.DrawBuffers(OutputBuffers.Count, OutputBuffers.ToArray());
        }
        public override void PreUnBindBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.DrawBuffer(DefaultOutBuffer);
        }
        public override void PreGenBuffer()
        {
            DeviceID = GL.GenFramebuffer();
        }
        public override void PreDispose()
        {
            GL.DeleteFramebuffer(DeviceID);
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
