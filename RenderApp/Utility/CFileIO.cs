using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace RenderApp.Utility
{

    /// <summary>
    /// ファイルの保存
    /// </summary>
    public class CFileIO
    {
        #region [ファイルの保存]

        /// <summary>
        /// スクリーンショット
        /// </summary>
        /// <returns></returns>
        private static Bitmap GrabScreenshot(GLControl glControl)
        {
            if (GraphicsContext.CurrentContext == null)
                throw new GraphicsContextMissingException();

            System.Drawing.Rectangle r = new System.Drawing.Rectangle(glControl.Location, glControl.Size);
            Bitmap bmp = new Bitmap(glControl.Width, glControl.Height);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(r, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.ReadPixels(0, 0, glControl.Width, glControl.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }
        /// <summary>
        /// GL画面の保存
        /// </summary>
        /// <param name="glControl"></param>
        public static void OutputGLImage(GLControl glControl)
        {
            string path = System.Windows.Forms.Application.StartupPath;

            System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.InitialDirectory = path;
            dlg.Filter = "BMPファイル(*.bmp)|*.bmp;|すべてのファイル(*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.Title = "保存するファイルを選択してください。";


            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap img = GrabScreenshot(glControl);
                img.Save(dlg.FileName);
                img.Dispose();
            }

        }
        #endregion

    }
}
