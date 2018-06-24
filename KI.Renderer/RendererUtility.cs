using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.GLUtil;
using KI.Gfx.Render;

namespace KI.Renderer
{
    public static class RendererUtility
    {
        public static void ScreenShot(string filename, RenderTarget renderTarget, int width, int height)
        {
            for (int k = 0; k < renderTarget.RenderTexture.Length; k++)
            {
                var pixelData = renderTarget.GetPixelData(DeviceContext.Instance.Width, DeviceContext.Instance.Height, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, k);
                if (pixelData == null)
                {
                    return;
                }

                using (Bitmap bmp = new Bitmap(width, height))
                {
                    for (int i = 0; i < bmp.Width; i++)
                    {
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            bmp.SetPixel(i, j, Color.FromArgb(
                                (int)(pixelData[i, j, 2]),
                                (int)(pixelData[i, j, 1]),
                                (int)(pixelData[i, j, 0])));
                        }
                    }

                    bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    bmp.Save(filename + k.ToString() + ".bmp");
                }
            }
        }
    }
}
