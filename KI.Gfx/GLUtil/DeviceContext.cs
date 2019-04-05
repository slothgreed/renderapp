using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil
{
    /// <summary>
    /// デバイスコンテキスト
    /// </summary>
    public class DeviceContext
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private DeviceContext()
        {
        }

        /// <summary>
        /// インスタンス
        /// </summary>
        public static DeviceContext Instance { get; } = new DeviceContext();

        /// <summary>
        /// 横
        /// </summary>
        public int Width
        {
            get { return viewportRect[2]; }
            set { viewportRect[2] = value; }
        }

        /// <summary>
        /// 縦
        /// </summary>
        public int Height
        {
            get { return viewportRect[3]; }
            set { viewportRect[3] = value; }
        }

        private int[] viewportRect;
        public int[] ViewportRect
        {
            get
            {
                return viewportRect;
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void Initialize(int width, int height)
        {
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.TextureCubeMap);
            GL.PointSize(10.0f);
            GL.LineWidth(1);
            GL.PolygonOffset(1.0f, 1.0f);
            GL.FrontFace(FrontFaceDirection.Ccw);

            viewportRect = new int[] { 0, 0, 0, 0 };
            SizeChanged(width, height);
        }

        /// <summary>
        /// クリアカラーのセッタ
        /// </summary>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="a">アルファ</param>
        public void SetClearColor(float r,float g, float b, float a)
        {
            GL.ClearColor(r, g, b, a);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="type">形状タイプ</param>
        /// <param name="first">開始番号</param>
        /// <param name="count">数</param>
        public void DrawArrays(PolygonType type, int first, int count)
        {
            PrimitiveType primitiveType = TypeUtility.ConvertPolygonType(type);
            GL.DrawArrays(primitiveType, first, count);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="type">形状タイプ</param>
        /// <param name="count">数</param>
        /// <param name="elementType">要素の型</param>
        /// <param name="indices">ポインタの場所</param>
        public void DrawElements(PolygonType type, int count, DrawElementsType elementType, int indices)
        {
            PrimitiveType primitiveType = TypeUtility.ConvertPolygonType(type);
            GL.DrawElements(primitiveType, count, elementType, indices);
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SizeChanged(int width, int height)
        {
            Width = width;
            Height = height;
            GL.Viewport(0, 0, Width, Height);
        }

        /// <summary>
        /// ピクセルデータの取得
        /// </summary>
        /// <param name="data">ビットマップデータ</param>
        public void ReadPixel(System.Drawing.Imaging.BitmapData data)
        {
            GL.ReadPixels(0, 0, Width, Height, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
        }

        /// <summary>
        /// バッファをクリアします。
        /// </summary>
        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}
