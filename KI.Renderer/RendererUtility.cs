using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;
using KI.Gfx.Render;

namespace KI.Renderer
{
    public static class RendererUtility
    {
        public static void ScreenShot(string filename, RenderTarget renderTarget, int width, int height)
        {
            for (int k = 0; k < renderTarget.RenderTexture.Length; k++)
            {
                ImageInfo imageInfo = new ImageInfo(filename + k.ToString() + ".jpg");

                if (renderTarget.GetPixelData(imageInfo, DeviceContext.Instance.Width, DeviceContext.Instance.Height, k) == true)
                {
                    imageInfo.BmpImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    imageInfo.BmpImage.Save(imageInfo.Name);
                }

                imageInfo.Dispose();
            }
        }
    }
}
