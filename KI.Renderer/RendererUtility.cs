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
            Bitmap bmp = new Bitmap(width, height);

            var pixelData = renderTarget.FrameBuffer.GetPixelData(DeviceContext.Instance.Width, DeviceContext.Instance.Height, OpenTK.Graphics.OpenGL.PixelFormat.Rgba);

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

            bmp.Save(filename);
            bmp.Dispose();
        }
    }
}
