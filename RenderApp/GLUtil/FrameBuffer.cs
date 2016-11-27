using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using RenderApp.Utility;
namespace RenderApp.GLUtil
{
    public class FrameBuffer
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
        /// フレームバッファID
        /// </summary>
        public int FrameId { get; private set; }
        /// <summary>
        /// レンダーバッファID
        /// </summary>
        public int RenderId { get; private set; }
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

            Width = width;
            Height = height;
            FramebufferAttachment[] attachment = new FramebufferAttachment[textureName.Length];
            if(textureName.Length == 1)
            {
                attachment[0] = FramebufferAttachment.ColorAttachment0;
            }
            else
            {
                for (int i = 0; i < textureName.Length; i++)
                {
                    attachment[i] = FramebufferAttachment.ColorAttachment0 + i;
                }

            }


            for(int i = 0; i < textureName.Length; i++)
            {
                TextureList.Add(TextureFactory.Instance.CreateTexture(textureName[i],width,height));
                Attachment.Add(attachment[i]);
                OutputBuffers.Add((DrawBuffersEnum)attachment[i]);
            }
            CreateFrameBuffer(attachment);
        }
        #endregion
        private void CreateFrameBuffer(FramebufferAttachment[] attachment)
        {
            FrameId = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameId);
            this.RenderId = GL.GenRenderbuffer();

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RenderId);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32, Width, Height);

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, RenderId);
            
            for(int i = 0; i < TextureList.Count; i++)
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment[i], TextureTarget.Texture2D, TextureList[i].ID, 0);
            }

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            Output.GLLog(Output.LogLevel.Error);
        }

        private void CreateFrameBuffer(int textureNum, out int[] textureId, FramebufferAttachment[] attachment)
        {
            FrameId = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameId);
            //レンダーバッファ
            this.RenderId = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, RenderId);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32, Width, Height);
            //テクスチャの割り当て
            textureId = new int[textureNum];
            //デプスバッファ
            for (int i = 0; i < textureNum; i++)
            {
                textureId[i] = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, textureId[i]);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, Width, Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Byte, IntPtr.Zero);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            }

            //割り当て
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, RenderId);
            for (int i = 0; i < textureNum; i++)
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment[i], TextureTarget.Texture2D, textureId[i], 0);
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            Output.GLLog(Output.LogLevel.Error);
        }
        public void Dispose()
        {
            GL.DeleteFramebuffer(FrameId);
            GL.DeleteRenderbuffer(RenderId);
        }
        public void SizeChanged(int width,int height)
        {
            this.Height = height;
            this.Width = width;
            Dispose();
            int[] textureId = new int[TextureList.Count];
            CreateFrameBuffer(TextureList.Count,out textureId,Attachment.ToArray());
            
            for(int i = 0; i < TextureList.Count; i++)
            {
                TextureList[i].ID = textureId[i];
            }
        }
        public void ClearBuffer()
        {
            if (FrameId != 0)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameId);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            }
        }
        public void BindBuffer()
        {
            if(FrameId != 0)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, FrameId);
                GL.DrawBuffers(OutputBuffers.Count,OutputBuffers.ToArray());
            }
        }
        public void UnBindBuffer()
        {
            if (FrameId != 0)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                GL.DrawBuffer(DefaultOutBuffer);
            }
        }
        public override string ToString()
        {
            return this.Name;
        }

    }
}
