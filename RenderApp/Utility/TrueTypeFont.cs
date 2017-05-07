using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;

namespace RenderApp.Utility
{
    /// <summary>
    /// 文字列描画用のクラス
    /// </summary>
    public class TrueTypeFont
    {
        /** Array that holds necessary information about the font characters */
        //各文字の位置と大きさを保存
        private IntObject[] charArray = new IntObject[256];

        /** Boolean flag on whether AntiAliasing is enabled or not */
        private bool antiAlias;

        /** Font's size */
        private int fontSize = 0;

        /** Font's height */
        private int fontHeight = 0;

        /** Texture used to cache the font 0-255 characters */
        private int fontTextureID;

        /** Default font texture width */
        private int textureWidth = 512;

        /** Default font texture height */
        private int textureHeight = 512;

        /** A reference to Java's AWT Font that we create our font texture from */
        private Font font;

        private int correctL = 9;

        public Vector3 color;

        /// <summary>
        /// テクスチャバッファ
        /// </summary>
        public static int m_TextureBuffer;

        private class IntObject
        {
            public IntObject()
            {
                this.storedX = this.storedY = this.width = this.height = 0;
            }

            /** Character's width */
            public int width;

            /** Character's height */
            public int height;

            /** Character's stored x position */
            public int storedX;

            /** Character's stored y position */
            public int storedY;
        }
        #region [コンストラクタから呼ばれる関数たち]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TrueTypeFont(Font font, bool antiAlias)
        {
            this.font = font;
            this.fontSize = (int)font.Size + 3;
            this.antiAlias = antiAlias;

            color = new Vector3(1, 1, 1);

            createSet();

            fontHeight -= 1;
            if (fontHeight <= 0) fontHeight = 1;
        }

        /// <summary>
        /// 各文字を取得して、テクスチャに書く
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private Bitmap getFontImage(char ch)
        {

            // Create a temporary image to extract the character's size
            Bitmap tempfontImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(tempfontImage);
            if (antiAlias == true)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }
            #region [1文字(ch)の長さを取得]
            SizeF dims = g.MeasureString(new string(new char[] { ch }), font);

            if (ch == ' ')
            {
                dims = g.MeasureString("l", font);
            }
            
            int charwidth = (int)dims.Width + 2;

            if (charwidth <= 0)
            {
                charwidth = 7;
            }
            int charheight = (int)dims.Height + 3;
            if (charheight <= 0)
            {
                charheight = fontSize;
            }
            #endregion
            #region [Contextの生成]
            // Create another image holding the character we are creating
            Bitmap fontImage = new Bitmap(charwidth, charheight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gt = Graphics.FromImage(fontImage);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            #endregion
            #region [Contextに文字を描画]
            int charx = 3;
            int chary = 1;
            gt.DrawString(new string(new char[] { ch }), font, new SolidBrush(Color.White), new PointF(charx, chary));
            #endregion


            return fontImage;

        }

        private void createSet()
        {


            // In any case this should be done in other way. Texture with size 512x512
            // can maintain only 256 characters with resolution of 32x32. The texture
            // size should be calculated dynamicaly by looking at character sizes.

            try
            {

                Bitmap imgTemp = new Bitmap(textureWidth, textureHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(imgTemp);

                //g.FillRectangle(new SolidBrush(Color.Yellow), 0, 0, textureWidth, textureHeight);

                int rowHeight = 0;
                int positionX = 0;
                int positionY = 0;

                for (int i = 0; i < 256; i++)
                {

                    // get 0-255 characters and then custom characters
                    char ch = (char)i;
                    //fontImageテクスチャに1文字(ch)を描画
                    Bitmap fontImage = getFontImage(ch);

                    IntObject newIntObject = new IntObject();
                    newIntObject.width = fontImage.Width;
                    newIntObject.height = fontImage.Height;

                    #region [文字を書き込む位置を調整]
                    //texture幅より長いと改行
                    if (positionX + newIntObject.width >= textureWidth)
                    {
                        positionX = 0;
                        positionY += rowHeight;
                        rowHeight = 0;
                    }

                    newIntObject.storedX = positionX;
                    newIntObject.storedY = positionY;

                    if (newIntObject.height > fontHeight)
                    {
                        fontHeight = newIntObject.height;
                    }

                    if (newIntObject.height > rowHeight)
                    {
                        rowHeight = newIntObject.height;
                    }
                    #endregion

                    // テクスチャに文字を書き込む
                    g.DrawImage(fontImage, positionX, positionY);
                    //書き込んだ文字分位置調整
                    positionX += newIntObject.width;

                    if (i < 256)
                    { // standard characters
                        charArray[i] = newIntObject;
                    }

                    fontImage = null;
                }

                //  imgTemp = gに書き込んだ全ての文字
                fontTextureID = loadImage(imgTemp);

            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to create font.");
                Console.WriteLine(e.StackTrace);
            }
        }


        /// <summary>
        ///　テクスチャをGLに紐付する
        /// </summary>
        /// <param name="bufferedImage">英数字がすべて書かれたテクスチャ</param>
        /// <returns></returns>
        public static int loadImage(Bitmap bufferedImage)
        {
            try
            {
                short width = (short)bufferedImage.Width;
                short height = (short)bufferedImage.Height;
                int bpp = bufferedImage.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb ? 32 : 24;

                BitmapData bData = bufferedImage.LockBits(new Rectangle(new Point(), bufferedImage.Size),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                int byteCount = bData.Stride * bufferedImage.Height;
                byte[] byteI = new byte[byteCount];
                Marshal.Copy(bData.Scan0, byteI, 0, byteCount);
                bufferedImage.UnlockBits(bData);

                GL.Enable(EnableCap.Texture2D);
                GL.GenTextures(1, out m_TextureBuffer);
                GL.BindTexture(TextureTarget.Texture2D, m_TextureBuffer);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

                GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Modulate);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, byteI);

                return m_TextureBuffer;

                //string filename = "bitmap.bmp";
                //if (String.IsNullOrEmpty(filename))
                //    throw new ArgumentException(filename);

                //int m_TextureBuffer = GL.GenTexture();
                //GL.BindTexture(TextureTarget.Texture2D, m_TextureBuffer);

                //// We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
                //// We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
                //// mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                //Bitmap bmp = new Bitmap(filename);
                //BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                //    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

                //bmp.UnlockBits(bmp_data);

                //return m_TextureBuffer;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Environment.Exit(-1);
            }

            return -1;
        }
        #endregion

        #region [描画に用いる関数たち]
        /// <summary>
        /// テクスチャ座標の指定1文字1Quads
        /// </summary>
        private void drawQuad(float drawX, float drawY, float drawX2, float drawY2,
                float srcX, float srcY, float srcX2, float srcY2,List<Vector3>position,List<Vector2>texcoord)
        {
            float DrawWidth = drawX2 - drawX;
            float DrawHeight = drawY2 - drawY;
            float TextureSrcX = srcX / textureWidth;
            float TextureSrcY = srcY / textureHeight;
            float SrcWidth = srcX2 - srcX;
            float SrcHeight = srcY2 - srcY;
            float RenderWidth = (SrcWidth / textureWidth);
            float RenderHeight = (SrcHeight / textureHeight);

            texcoord.Add(new Vector2(TextureSrcX, TextureSrcY));
            position.Add(new Vector3(drawX, drawY,0));
            texcoord.Add(new Vector2(TextureSrcX, TextureSrcY + RenderHeight));
            position.Add(new Vector3(drawX, drawY + DrawHeight,0));
            texcoord.Add(new Vector2(TextureSrcX + RenderWidth, TextureSrcY + RenderHeight));
            position.Add(new Vector3(drawX + DrawWidth, drawY + DrawHeight,0));
            texcoord.Add(new Vector2(TextureSrcX + RenderWidth, TextureSrcY));
            position.Add(new Vector3(drawX + DrawWidth, drawY,0));

        }

        /// <summary>
        /// 位置指定用
        /// </summary>
        /// <param name="x">Windowの右端から右へ＋</param>
        /// <param name="y">Windowの右端から下へ＋</param>
        /// <param name="whatchars">表示したい文字列</param>
        /// <param name="scaleX">大きさ</param>
        /// <param name="scaleY">大きさ</param>
        /// <param name="position">テクスチャを張る四角形の頂点</param>
        /// <param name="texcoord">テクスチャ座標</param>
        public void MakeTextTexture(float x, float y,
                string whatchars, float scaleX, float scaleY, List<Vector3> position, List<Vector2> texcoord)
        {
            MakeTextTexture(x, y, whatchars, 0, whatchars.Length - 1, scaleX, scaleY,position,texcoord);
        }


        private void MakeTextTexture(float x, float y,
                string whatchars, int startIndex, int endIndex,
                float scaleX, float scaleY,List<Vector3>position,List<Vector2>texcoord)
        {

            IntObject intObject = null;
            int charCurrent;


            int totalwidth = 0;
            int i = startIndex, loopd = 1, c = correctL;
            float startY = 0;

            //中央に移動Start
            for (int l = startIndex; l <= endIndex; l++)
            {
                charCurrent = whatchars[l];
                if (charCurrent == '\n') break;
                if (charCurrent < 256)
                {
                    intObject = charArray[charCurrent];
                }

                totalwidth += intObject.width - correctL;
            }
            totalwidth /= -2;
            //中央に移動Fin

            while (i >= startIndex && i <= endIndex)
            {

                charCurrent = whatchars[i];
                if (charCurrent < 256)
                {
                    intObject = charArray[charCurrent];
                }

                if (intObject != null)
                {
                    if (loopd < 0) totalwidth += (intObject.width - c) * loopd;
                    //改行のとき
                    if (charCurrent == '\n')
                    {
                        startY += fontHeight * loopd;
                        totalwidth = 0;

                        for (int l = i + 1; l <= endIndex; l++)
                        {
                            charCurrent = whatchars[l];
                            if (charCurrent == '\n') break;
                            if (charCurrent < 256)
                            {
                                intObject = charArray[charCurrent];
                            }

                            totalwidth += intObject.width - correctL;
                        }
                        totalwidth /= -2;

                    }
                    else
                    {
                        //文字のとき
                        drawQuad((totalwidth + intObject.width) * scaleX + x, startY * scaleY + y,
                            totalwidth * scaleX + x,
                            (startY + intObject.height) * scaleY + y, intObject.storedX + intObject.width,
                            intObject.storedY, intObject.storedX,
                            intObject.storedY + intObject.height,
                            position,texcoord);
                        if (loopd > 0) totalwidth += (intObject.width - c) * loopd;
                    }
                    i += loopd;

                }
            }
        }
        #endregion


        /// <summary>
        /// 解放するときに呼び出す。
        /// </summary>
        public void destroy()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.DeleteTexture(fontTextureID);
        }
    }
}