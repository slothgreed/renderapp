using KI.Foundation.Utility;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil.Buffer
{
    /// <summary>
    /// 頂点バッファ
    /// </summary>
    public class VertexBuffer
    {
        /// <summary>
        /// 頂点バッファ
        /// </summary>
        public ArrayBuffer PositionBuffer { get; private set; }

        /// <summary>
        /// 法線バッファ
        /// </summary>
        public ArrayBuffer NormalBuffer { get; private set; }
        
        /// <summary>
        /// カラーバッファ
        /// </summary>
        public ArrayBuffer ColorBuffer { get; private set; }

        /// <summary>
        /// テクスチャ座標バッファ
        /// </summary>
        public ArrayBuffer TexCoordBuffer { get; private set; }

        /// <summary>
        /// インデックスバッファ
        /// </summary>
        public ArrayBuffer IndexBuffer { get; private set; }
        
        /// <summary>
        /// インデックスバッファが有効か
        /// </summary>
        public bool EnableIndexBuffer { get; private set; }

        /// <summary>
        /// 頂点情報
        /// </summary>
        private Vector3[] PositionList
        {
            set
            {
                if (value.Length == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertex num error");
                }

                if(PositionBuffer == null)
                {
                    PositionBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
                }

                PositionBuffer.SetData(value, EArrayType.Vec3Array);
            }
        }

        /// <summary>
        /// 法線情報
        /// </summary>
        private Vector3[] NormalList
        {
            set
            {
                if (value.Length == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertex num error");
                }

                if (NormalBuffer == null)
                {
                    NormalBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
                }

                NormalBuffer.SetData(value, EArrayType.Vec3Array);
            }
        }

        /// <summary>
        /// カラー情報
        /// </summary>
        private Vector3[] ColorList
        {
            set
            {
                if (value.Length == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertex num error");
                }
                if (ColorBuffer == null)
                {
                    ColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
                }

                ColorBuffer.SetData(value, EArrayType.Vec3Array);
            }
        }

        /// <summary>
        /// テクスチャ座標情報
        /// </summary>
        private Vector2[] TexCoordList
        {
            set
            {
                if (value.Length == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertex num error");
                }
                if (TexCoordBuffer == null)
                {
                    TexCoordBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
                }

                TexCoordBuffer.SetData(value, EArrayType.Vec2Array);
            }
        }

        /// <summary>
        /// 頂点Indexバッファ
        /// </summary>
        private int[] IndexBufferList
        {
            set
            {
                if (value.Length == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "index buffer count 0");
                }

                if (IndexBuffer == null)
                {
                    IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
                }

                IndexBuffer.SetData(value, EArrayType.IntArray);
            }
        }

        /// <summary>
        /// 描画する数
        /// </summary>
        public int Num { get; private set; }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            BufferFactory.Instance.RemoveByValue(PositionBuffer);
            BufferFactory.Instance.RemoveByValue(NormalBuffer);
            BufferFactory.Instance.RemoveByValue(ColorBuffer);
            BufferFactory.Instance.RemoveByValue(TexCoordBuffer);
            PositionBuffer = null;
            NormalBuffer = null;
            ColorBuffer = null;
            TexCoordBuffer = null;
        }

        /// <summary>
        /// バッファの作成
        /// </summary>
        public void GenBuffer()
        {
            PositionBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            NormalBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            ColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            TexCoordBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
        }

        /// <summary>
        /// Index バッファの設定
        /// </summary>
        /// <param name="index">頂点インデックス</param>
        /// <param name="num">数</param>
        public void SetIndexBuffer(int[] index)
        {
            IndexBufferList = index;
            Num = index.Length;
            EnableIndexBuffer = true;
        }

        /// <summary>
        /// 頂点カラーの変更
        /// </summary>
        /// <param name="colorBuffer">カラーバッファ</param>
        public void ChangeVertexColor(ArrayBuffer colorBuffer)
        {
            ColorBuffer = colorBuffer;
        }

        /// <summary>
        /// 頂点カラーの変更
        /// </summary>
        /// <param name="indexBuffer">カラーバッファ</param>
        public void ChangeIndexBuffer(ArrayBuffer indexBuffer)
        {
            IndexBuffer = indexBuffer;
        }

        /// <summary>
        /// バッファの設定
        /// </summary>
        /// <param name="position">頂点</param>
        /// <param name="normal">法線</param>
        /// <param name="color">色</param>
        /// <param name="texCoord">テクスチャ</param>
        /// <param name="index">頂点インデックス</param>
        /// <param name="num">数</param>
        public void SetBuffer(Vector3[] position, Vector3[] normal, Vector3[] color, Vector2[] texCoord)
        {
            if (!EnableIndexBuffer)
            {
                Num = position.Length;
            }

            PositionList = position;
            NormalList = normal;
            ColorList = color;
            TexCoordList = texCoord;
        }
    }
}
