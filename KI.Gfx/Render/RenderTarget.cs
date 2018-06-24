using System;
using System.Collections.Generic;
using System.Drawing;
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
        /// フレームバッファ
        /// </summary>
        public FrameBuffer FrameBuffer { get; private set; }

        /// <summary>
        /// レンダリング用テクスチャ
        /// </summary>
        public RenderTexture[] RenderTexture { get; private set; }

        
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        /// <param name="num">出力バッファ数</param>
        private void Initialize(int width, int height, int num)
        {
            this.Width = width;
            this.Height = height;
            FrameBuffer.GenBuffer();
            FrameBuffer.SetupRenderBuffer(Width, Height);
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
                texture.TextureBuffer.SizeChanged(width, height);
            }

            FrameBuffer.SizeChanged(width, height);
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
            BufferFactory.Instance.RemoveByValue(FrameBuffer);
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
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="pixelFormat">ピクセルフォーマット</param>
        /// <param name="index">MRTのインデクス</param>
        /// <returns>ピクセルデータ</returns>
        public float[,,] GetPixelData(int width, int height, PixelFormat pixelFormat, int index)
        {
            FrameBuffer.BindBuffer();
            float[,,] rgb = new float[width, height, 4];

            if (index > RenderTexture.Length)
            {
                Logger.Log(Logger.LogLevel.Error, "Not RenderTargetNum");
                return null;
            }

            GL.ReadBuffer((ReadBufferMode)RenderTexture[index].Attachment);
            using (Bitmap bmp = new Bitmap(width, height))
            {
                System.Drawing.Imaging.BitmapData data =
                    bmp.LockBits(new Rectangle(0, 0, width, height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                GL.ReadPixels(0, 0, width, height, PixelFormat.Rgb, PixelType.UnsignedByte, data.Scan0);
                bmp.UnlockBits(data);

                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        rgb[i, j, 0] = bmp.GetPixel(i, j).R;
                        rgb[i, j, 1] = bmp.GetPixel(i, j).G;
                        rgb[i, j, 2] = bmp.GetPixel(i, j).B;
                        rgb[i, j, 3] = bmp.GetPixel(i, j).A;
                    }
                }
            }

            FrameBuffer.UnBindBuffer();

            return rgb;
        }
    }
}
