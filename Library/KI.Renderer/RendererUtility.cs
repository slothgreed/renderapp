using System.Drawing;
using KI.Foundation.Core;
using KI.Gfx.KITexture;
using KI.Gfx.Render;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer
{
    public static class RendererUtility
    {
        /// <summary>
        /// スクリーンショット(フレームバッファ)
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <param name="extension">拡張子(.bmp,.jpg etc)</param>
        /// <param name="renderTarget">レンダーターゲット</param>
        public static void ScreenShot(string filename, string extension, RenderTarget renderTarget)
        {
            for (int k = 0; k < renderTarget.RenderTexture.Length; k++)
            {
                ImageInfo imageInfo = new ImageInfo(filename + k.ToString() + extension);

                if (renderTarget.GetPixelData(imageInfo, renderTarget.Width, renderTarget.Height, k) == true)
                {
                    
                    imageInfo.BmpImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    imageInfo.BmpImage.Save(imageInfo.Name);
                }

                imageInfo.Dispose();

            }
        }

        /// <summary>
        /// スクリーンショット(最終出力)
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <param name="extension">拡張子(.bmp,.jpg etc)</param>
        /// <param name="renderTarget">レンダーターゲット</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        public static void ScreenShot(string filename, string extension, int width, int height)
        {
            ImageInfo imageInfo = new ImageInfo(filename + extension);

            GL.ReadBuffer(ReadBufferMode.Back);

            imageInfo.LoadRenderImage(width, height);

            imageInfo.BmpImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
            imageInfo.BmpImage.Save(imageInfo.Name);

            imageInfo.Dispose();

            Logger.GLLog(Logger.LogLevel.Error);
        }
    }
}
