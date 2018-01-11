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
        public ArrayBuffer PositionBuffer { get; set; }

        /// <summary>
        /// 法線バッファ
        /// </summary>
        public ArrayBuffer NormalBuffer { get; set; }
        
        /// <summary>
        /// カラーバッファ
        /// </summary>
        public ArrayBuffer ColorBuffer { get; set; }

        /// <summary>
        /// テクスチャ座標バッファ
        /// </summary>
        public ArrayBuffer TexCoordBuffer { get; set; }

        /// <summary>
        /// インデックスバッファ
        /// </summary>
        public ArrayBuffer IndexBuffer { get; set; }
        
        /// <summary>
        /// インデックスバッファが有効か
        /// </summary>
        public bool EnableIndexBuffer { get; set; }

        /// <summary>
        /// 描画する数
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// バッファの作成
        /// </summary>
        private void GenBuffer()
        {
            Dispose();
            PositionBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            NormalBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            ColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            TexCoordBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
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
        public void SetBuffer(Vector3[] position, Vector3[] normal, Vector3[] color, Vector2[] texCoord, int[] indexBuffer)
        {
            GenBuffer();

            PositionBuffer.SetData(position, EArrayType.Vec3Array);
            NormalBuffer.SetData(normal, EArrayType.Vec3Array);
            ColorBuffer.SetData(color, EArrayType.Vec3Array);
            TexCoordBuffer.SetData(texCoord, EArrayType.Vec2Array);

            if (indexBuffer == null)
            {
                Num = position.Length;
                EnableIndexBuffer = false;
            }
            else
            {
                IndexBuffer.SetData(indexBuffer, EArrayType.IntArray);
                Num = indexBuffer.Length;
                EnableIndexBuffer = true;
            }
        }

        /// <summary>
        /// 複製(BufferのID情報のみ・データ自体は複製しない)
        /// ここで生成したものは解放不要
        /// </summary>
        /// <returns>vertexbuffer</returns>
        public VertexBuffer ShallowCopy()
        {
            var vertexBuffer = new VertexBuffer();
            vertexBuffer.PositionBuffer = PositionBuffer.ShallowCopy();
            vertexBuffer.ColorBuffer = ColorBuffer.ShallowCopy();
            vertexBuffer.NormalBuffer = NormalBuffer.ShallowCopy();
            vertexBuffer.TexCoordBuffer = TexCoordBuffer.ShallowCopy();
            vertexBuffer.IndexBuffer = IndexBuffer.ShallowCopy();
            vertexBuffer.EnableIndexBuffer = EnableIndexBuffer;
            vertexBuffer.Num = Num;

            return vertexBuffer;
        }

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
    }
}
