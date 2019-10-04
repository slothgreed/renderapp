using System.Linq;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KITexture;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.Render
{
    /// <summary>
    /// レンダーターゲット
    /// </summary>
    public class RenderTarget : KIObject
    {
        /// <summary>
        /// 幅
        /// </summary>
        public int Width
        {
            get;
            private set;
        }

        /// <summary>
        /// 高さ
        /// </summary>
        public int Height
        {
            get;
            private set;
        }

        public bool UseDepthTexture { get; private set; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        /// <param name="num">出力バッファ数</param>
        internal RenderTarget(string name, int width, int height)
            : base(name)
        {
            FrameBuffer = BufferFactory.Instance.CreateFrameBuffer(name);
            Initialize(width, height);
        }

        /// <summary>
        /// フレームバッファ
        /// </summary>
        public FrameBuffer FrameBuffer { get; private set; }

        /// <summary>
        /// デプスのレンダリングテクスチャ
        /// </summary>
        public RenderBuffer DepthBuffer { get; private set; }
        
        /// <summary>
        /// デプステクスチャ
        /// </summary>
        public RenderTexture DepthTexture { get; private set; }
        
        /// <summary>
        /// レンダリング用テクスチャ
        /// </summary>
        public RenderTexture[] RenderTexture { get; private set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        private void Initialize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            FrameBuffer.GenBuffer();
            if (/*useDepthTexture*/ true)
            {

                SetupRenderBufferUseDepthTexture(Width, Height);
            }
            else
            {
                SetupRenderBuffer(Width, Height);
            }
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SizeChanged(int width, int height)
        {
            if (width == this.Width && this.Height == height)
            {
                return;
            }

            this.Height = height;
            this.Width = width;

            foreach (var texture in RenderTexture)
            {
                texture.SizeChanged(width, height);
            }

            if (DepthBuffer != null)
            {
                DepthBuffer.SizeChanged(width, height);
            }

            if (DepthTexture != null)
            {
                DepthTexture.SizeChanged(width, height);
            }

        }

        /// <summary>
        /// バッファのクリア
        /// </summary>
        public void ClearBuffer()
        {
            FrameBuffer.BindBuffer();
            DeviceContext.Instance.Clear();
            FrameBuffer.UnBindBuffer();
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {

            if (DepthBuffer != null)
            {
                DepthBuffer.Dispose();
            }

            if (DepthTexture != null)
            {
                DepthTexture.Dispose();
            }

            BufferFactory.Instance.RemoveByValue(FrameBuffer);
        }


        /// <summary>
        /// レンダーバッファの設定
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        public void SetupRenderBuffer(int width, int height)
        {
            FrameBuffer.BindBuffer();
            DepthBuffer = BufferFactory.Instance.CreateRenderBuffer();
            DepthBuffer.GenBuffer();
            DepthBuffer.Storage(RenderbufferStorage.DepthComponent, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, DepthBuffer.DeviceID);
            DepthBuffer.UnBindBuffer();
            FrameBuffer.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// レンダーバッファの設定(Depthバッファもテクスチャを利用する)
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        public void SetupRenderBufferUseDepthTexture(int width, int height)
        {
            FrameBuffer.BindBuffer();
            DepthTexture = new RenderTexture("Depth Texture", width, height, PixelFormat.DepthComponent);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, DepthTexture.DeviceID, 0);
            FrameBuffer.UnBindBuffer();

        }


        /// <summary>
        /// 出力バッファのバインド
        /// </summary>
        /// <param name="outputs">出力バッファ</param>
        public void BindRenderTarget()
        {
            FrameBuffer.BindBuffer();
            for (int i = 0; i < RenderTexture.Length; i++)
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, RenderTexture[i].Attachment, TextureTarget.Texture2D, RenderTexture[i].DeviceID, 0);
            }

            GL.DrawBuffers(RenderTexture.Length, RenderTexture.Select(p => p.DrawBuffers).ToArray());
        }

        /// <summary>
        /// レンダリングテクスチャの設定
        /// </summary>
        /// <param name="texture">テクスチャ</param>
        public void SetRenderTexture(RenderTexture[] texture)
        {
            RenderTexture = new RenderTexture[texture.Length];
            for (int i = 0; i < texture.Length; i++)
            {
                RenderTexture[i] = texture[i];
                RenderTexture[i].Attachment = (FramebufferAttachment.ColorAttachment0 + i);
                RenderTexture[i].DrawBuffers = (DrawBuffersEnum.ColorAttachment0 + i);
            }
        }

        /// <summary>
        /// 出力バッファのバインド解除
        /// </summary>
        public void UnBindRenderTarget()
        {
            FrameBuffer.UnBindBuffer();
            GL.DrawBuffer(DrawBufferMode.Back);
        }

        /// <summary>
        /// ピクセルデータの取得
        /// </summary>
        /// <param name="image">ピクセルデータの入った画像情報</param>
        /// <param name="index">MRTのインデクス</param>
        /// <returns>成功か</returns>
        public bool GetPixelData(ImageInfo image, int width, int height, int index)
        {
            if (index > RenderTexture.Length)
            {
                Logger.Log(Logger.LogLevel.Error, "Not RenderTargetNum");
                return false;
            }

            FrameBuffer.BindBuffer();
            GL.ReadBuffer((ReadBufferMode)RenderTexture[index].Attachment);
            image.LoadRenderImage(width, height);
            FrameBuffer.UnBindBuffer();

            return true;
        }
    }
}
