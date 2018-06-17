using System.Drawing;
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
            GL.DeleteFramebuffer(DeviceID);
        }


        /// <summary>
        /// ピクセルデータの取得
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="pixelFormat">ピクセルフォーマット</param>
        /// <returns>ピクセルデータ</returns>
        public float[,,] GetPixelData(int width, int height, PixelFormat pixelFormat)
        {
            BindBuffer();
            float[,,] rgb = new float[width, height, 4];

            Bitmap bmp = new Bitmap(width, height);
            System.Drawing.Imaging.BitmapData data =
                bmp.LockBits(new Rectangle(0, 0, width, height), 
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.ReadPixels(0, 0, width, height, PixelFormat.Rgb, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);


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

            UnBindBuffer();

            return rgb;
        }
    }
}
