using System.Collections.Generic;
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
        private int width = 0;

        /// <summary>
        /// 高さ
        /// </summary>
        private int height = 0;

        /// <summary>
        /// テクスチャのアタッチメント
        /// </summary>
        private List<FramebufferAttachment> attachment = new List<FramebufferAttachment>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        /// <param name="num">出力バッファ数</param>
        internal RenderTarget(string name, int width, int height, int num)
            : base(name)
        {
            FrameBuffer = BufferFactory.Instance.CreateFrameBuffer(name);
            Initialize(width, height, num);
        }

        /// <summary>
        /// 描画場所
        /// </summary>
        public List<DrawBuffersEnum> OutputBuffers { get; private set; } = new List<DrawBuffersEnum>();

        /// <summary>
        /// フレームバッファ
        /// </summary>
        public FrameBuffer FrameBuffer { get; private set; }

        /// <summary>
        /// レンダーバッファID
        /// </summary>
        public RenderBuffer RenderBuffer { get; private set; }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SizeChanged(int width, int height)
        {
            if (width == this.width && this.height == height)
            {
                return;
            }

            this.height = height;
            this.width = width;
            RenderBuffer.SizeChanged(width, height);
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
            BufferFactory.Instance.RemoveByValue(RenderBuffer);
            BufferFactory.Instance.RemoveByValue(FrameBuffer);
        }

        /// <summary>
        /// 出力バッファのバインド
        /// </summary>
        /// <param name="output">出力バッファ</param>
        public void BindRenderTarget(Texture output)
        {
            BindRenderTarget(new Texture[] { output });
        }

        /// <summary>
        /// 出力バッファのバインド
        /// </summary>
        /// <param name="outputs">出力バッファ</param>
        public void BindRenderTarget(Texture[] outputs)
        {
            FrameBuffer.BindBuffer();
            for (int i = 0; i < outputs.Length; i++)
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment[i], TextureTarget.Texture2D, outputs[i].DeviceID, 0);

                if (outputs[i].Width != width || outputs[i].Height != height)
                {
                    outputs[i].TextureBuffer.SizeChanged(width, height);
                }
            }

            GL.DrawBuffers(OutputBuffers.Count, OutputBuffers.ToArray());
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
        /// 初期化
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        /// <param name="num">出力バッファ数</param>
        private void Initialize(int width, int height, int num)
        {
            RenderBuffer = new RenderBuffer();
            this.width = width;
            this.height = height;

            for (int i = 0; i < num; i++)
            {
                attachment.Add(FramebufferAttachment.ColorAttachment0 + i);
                OutputBuffers.Add(DrawBuffersEnum.ColorAttachment0 + i);
            }

            CreateFrameBuffer();
        }

        /// <summary>
        /// フレームバッファの作成
        /// </summary>
        private void CreateFrameBuffer()
        {
            FrameBuffer.GenBuffer();
            CreateRenderBuffer();
        }

        /// <summary>
        /// レンダーバッファの作成
        /// </summary>
        private void CreateRenderBuffer()
        {
            FrameBuffer.BindBuffer();
            RenderBuffer.GenBuffer();
            RenderBuffer.Storage(RenderbufferStorage.DepthComponent, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, RenderBuffer.DeviceID);
            RenderBuffer.UnBindBuffer();
            FrameBuffer.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }
    }
}
