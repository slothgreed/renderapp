using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil
{
    /// <summary>
    /// 形状の種類
    /// </summary>
    public enum GeometryType
    {
        None,
        Point,
        Line,
        Triangle,
        Quad,
        Patch,
        Mix
    }

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
        public int Width { get; set; }

        /// <summary>
        /// 縦
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void Initialize(int width, int height)
        {
            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.TextureCubeMap);
            GL.PointSize(2.0f);
            GL.PolygonOffset(1.0f, 1.0f);
            GL.FrontFace(FrontFaceDirection.Ccw);

            SizeChanged(width, height);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="type">形状タイプ</param>
        /// <param name="first">開始番号</param>
        /// <param name="count">数</param>
        public void DrawArrays(GeometryType type, int first, int count)
        {
            GL.DrawArrays(ConvertToPrimitiveType(type), first, count);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="type">形状タイプ</param>
        /// <param name="count">数</param>
        /// <param name="elementType">要素の型</param>
        /// <param name="indices">ポインタの場所</param>
        public void DrawElements(GeometryType type, int count, DrawElementsType elementType, int indices)
        {
            GL.DrawElements(ConvertToPrimitiveType(type), count, elementType, indices);
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

        /// <summary>
        /// 描画種類をプリミティブ種類に変換します。
        /// </summary>
        /// <param name="type">描画種類</param>
        /// <returns>プリミティブ種類</returns>
        private PrimitiveType ConvertToPrimitiveType(GeometryType type)
        {
            switch (type)
            {
                case GeometryType.None:
                case GeometryType.Point:
                    return PrimitiveType.Points;
                case GeometryType.Line:
                    return PrimitiveType.Lines;
                case GeometryType.Triangle:
                    return PrimitiveType.Triangles;
                case GeometryType.Quad:
                    return PrimitiveType.Quads;
                case GeometryType.Mix:
                    break;
                case GeometryType.Patch:
                    return PrimitiveType.Patches;
                default:
                    break;
            }

            return PrimitiveType.Points;
        }
    }
}
