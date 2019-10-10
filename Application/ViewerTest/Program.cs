using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
#pragma warning disable
namespace ViewerTest
{
    class ViewerTest
    {
        [STAThread]
        static int Main()
        {
            using (Game window = new Game())
            {
                window.Run(30.0);
            }

            return 0;
        }
    }

    class Game : GameWindow
    {
        public PLYLoader ply;
        public List<Vector3> position;
        public List<Vector2> texcoord;
        public List<Vector3> vector;
        public List<Vector3> positionTest;
        public List<Vector2> texcoordTest;
        public List<Vector3> vectorTest;
        public byte[,,] frame;
        public byte[,,] noize;
        public int noizeId;
        public int frameId;
        public float tmax;  //荒さ
        public bool first = true;
        //800x600のウィンドウを作る。タイトルは「0-3:GameWindow」
        public Game() : base(800, 800, GraphicsMode.Default, "0-3:GameWindow")
        {
            VSync = VSyncMode.On;
        }

        //ウィンドウの起動時に実行される。
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            ply = new PLYLoader(@"E:\MyProgram\KIProject\renderapp\ViewerTest\sphere1.ply");

            int vertexNum = ply.Propertys[0].Count;
            position = new List<Vector3>();
            vector = new List<Vector3>();
            texcoord = new List<Vector2>();
            for (int i = 0; i < vertexNum; i++)
            {
                var x = ply.Propertys[0][i] / 1.732050f + 0.5f;
                var y = ply.Propertys[1][i] / 1.732050f + 0.5f;
                var z = ply.Propertys[2][i] / 1.732050f;
                var angle = ply.Propertys[3][i];
                var vectorX = ply.Propertys[4][i];
                var vectorY = ply.Propertys[5][i];
                var vectorZ = ply.Propertys[6][i];

                Vector3 tmp = new Vector3(x, y, z) - new Vector3(vectorX, vectorY, vectorZ);
                position.Add(new Vector3(x, y, z));
                vector.Add(new Vector3(vectorX, vectorY, vectorZ).Normalized());
                texcoord.Add(new Vector2(tmp.X, tmp.Y));
            }

            string filePath = @"E:\MyProgram\KIProject\renderapp\ViewerTest\vectorfield.txt";
            string[] fileStream = File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("Shift_JIS"));

            positionTest = new List<Vector3>();
            texcoordTest = new List<Vector2>();
            vectorTest = new List<Vector3>();
            for (int i = 0; i < fileStream.Length; i++)
            {
                string[] lineData = fileStream[i]
                    .Split(',')
                    .Where(p => !string.IsNullOrEmpty(p))
                    .ToArray();

                if (lineData.Length != 8)
                {
                    break;
                }

                float[] floatData = lineData
                    .Select(p => float.Parse(p))
                    .ToArray();

                positionTest.Add(new Vector3(floatData[0], floatData[1], floatData[2]));
                texcoordTest.Add(new Vector2(floatData[3], floatData[4]));
                vectorTest.Add(new Vector3(floatData[5], floatData[6], floatData[7]));

            }

            //テクスチャ用バッファの生成
            noizeId = GL.GenTexture();
            frameId = GL.GenTexture();

            CalcNoize();

            //テクスチャ用バッファのひもづけ
            GL.BindTexture(TextureTarget.Texture2D, noizeId);
            //テクスチャの設定
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.BindTexture(TextureTarget.Texture2D, 0);


            GL.BindTexture(TextureTarget.Texture2D, frameId);
            //テクスチャの設定
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            tmax = frame.GetLength(0) / (3.0f * noize.GetLength(0));
        }

        //ウィンドウのサイズが変更された場合に実行される。
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 glortho = Matrix4.CreateOrthographicOffCenter(0, 1, 0, 1, -1000, 4000);
            GL.LoadMatrix(ref glortho);
        }

        //画面更新で実行される。
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            //Escapeキーで終了
            if (Keyboard[Key.Escape])
            {
                this.Exit();
            }

            if (Keyboard[Key.Z])
            {
                CalcNoize();
            }

            if (Keyboard[Key.R])
            {
                SwapBuffers();
            }
        }

        private void CalcNoize()
        {
            Random rand = new Random();
            frame = new byte[800, 800, 4];
            noize = new byte[64, 64, 4];

            for (int i = 0; i < frame.GetLength(0); i++)
            {
                for (int j = 0; j < frame.GetLength(1); j++)
                {
                    frame[i, j, 0] = 0;
                    frame[i, j, 1] = 0;
                    frame[i, j, 2] = 0;
                    frame[i, j, 3] = 255;
                }
            }

            var lut_r = new byte[256];
            var lut_g = new byte[256];
            var lut_b = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                lut_r[i] = (byte)(1.22 * i < 127 ? 0 : 255);
                lut_g[i] = (byte)(1.3 * i < 127 ? 0 : 255);
                lut_b[i] = (byte)(1.1 * i < 127 ? 0 : 255);
            }

            for (int i = 0; i < noize.GetLength(0); i++)
            {
                for (int j = 0; j < noize.GetLength(1); j++)
                {
                    byte color = (byte)rand.Next(255);

                    noize[i, j, 0] = lut_r[rand.Next(255)];
                    noize[i, j, 1] = lut_g[rand.Next(255)];
                    noize[i, j, 2] = lut_b[rand.Next(255)];
                    noize[i, j, 3] = 15;

                    noize[i, j, 0] = color;
                    noize[i, j, 1] = color;
                    noize[i, j, 2] = color;
                    noize[i, j, 3] = 15;
                }
            }

            //テクスチャ用バッファのひもづけ
            GL.BindTexture(TextureTarget.Texture2D, noizeId);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, noize.GetLength(0), noize.GetLength(1), 0, PixelFormat.Rgba, PixelType.UnsignedByte, noize);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        //画面描画で実行される。
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            {
                GL.ClearColor(Color4.Black);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                GL.BindTexture(TextureTarget.Texture2D, frameId);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, frame.GetLength(0), frame.GetLength(1), 0, PixelFormat.Rgb, PixelType.UnsignedByte, frame);

                GL.PointSize(5);
                GL.Begin(BeginMode.Points);

                for (int i = 0; i < position.Count; i++)
                {
                    GL.Vertex3(position[i]);
                }

                GL.End();

                // direction の描画
                GL.Begin(BeginMode.Lines);

                for (int i = 0; i < position.Count; i++)
                {
                    GL.Vertex3(position[i]);
                    GL.Vertex3(position[i] + vector[i].Normalized() * 0.01f);
                }

                GL.End();

                // sphere の描画
                GL.Begin(BeginMode.Triangles);
                for (int i = 0; i < positionTest.Count; i++)
                {
                    GL.TexCoord2(texcoordTest[i]);
                    GL.Vertex3(positionTest[i]);
                }

                //for(int i = 0; i < position.Count; i++)
                //{
                //    GL.TexCoord2(texcoord[i]);
                //    GL.Vertex3(position[i]);
                //}

                GL.End();

                GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            {
                GL.BindTexture(TextureTarget.Texture2D, noizeId);
                GL.Enable(EnableCap.Blend);
                //色 = 前景 * (前景のアルファ値) + 背景 * (1.0-背景のアルファ値)
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0, 0); GL.Vertex3(0, 0, 1);
                GL.TexCoord2(0, tmax); GL.Vertex3(0, 1, 1);
                GL.TexCoord2(tmax, tmax); GL.Vertex3(1, 1, 1);
                GL.TexCoord2(tmax, 0); GL.Vertex3(1, 0, 1);
                GL.End();
                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.Disable(EnableCap.Blend);
            }

            GL.ReadPixels(0, 0, frame.GetLength(0), frame.GetLength(1), PixelFormat.Rgb, PixelType.UnsignedByte, frame);

            SwapBuffers();
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        protected override void Dispose(bool manual)
        {
            GL.DeleteTexture(noizeId);
            GL.DeleteTexture(frameId);
            base.Dispose(manual);
        }
    }
}