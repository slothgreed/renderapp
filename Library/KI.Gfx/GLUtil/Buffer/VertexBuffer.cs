using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil.Buffer
{
    /// <summary>
    /// 頂点バッファ
    /// </summary>
    public class VertexBuffer : KIObject
    {
        /// <summary>
        /// 頂点バッファ
        /// </summary>
        public ArrayBuffer PositionBuffer { get; set; }

        /// <summary>
        /// 法線バッファ
        /// </summary
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
        /// 浅いコピーのオブジェクトかどうか
        /// </summary>
        public bool ShallowCopyObject { get; private set; } = false;

        /// <summary>
        /// レンダリングの種類
        /// </summary>
        public KIPrimitiveType Type { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VertexBuffer()
        {
            GenBuffer();
        }

        /// <summary>
        /// バッファの作成
        /// </summary>
        private void GenBuffer()
        {
            // TODO : サイズが変わった時の挙動が分からない。
            if(PositionBuffer == null)
            {
                PositionBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer, EArrayType.Vec3Array);
            }

            if (NormalBuffer == null)
            {
                NormalBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer, EArrayType.Vec3Array);
            }

            if (ColorBuffer == null)
            {
                ColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer, EArrayType.Vec3Array);
            }

            if (TexCoordBuffer == null)
            {
                TexCoordBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer, EArrayType.Vec2Array);
            }

            if (IndexBuffer == null)
            {
                IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer, EArrayType.IntArray);
            }
        }

        /// <summary>
        /// バッファの設定
        /// </summary>
        /// <param name="type">ポリゴン種類</param>
        /// <param name="position">頂点</param>
        /// <param name="normal">法線</param>
        /// <param name="color">色</param>
        /// <param name="texCoord">テクスチャ</param>
        /// <param name="index">頂点インデックス</param>
        /// <param name="num">数</param>
        public void SetBuffer(KIPrimitiveType type, Vector3[] position, Vector3[] normal, Vector3[] color, Vector2[] texCoord, int[] indexBuffer)
        {
            Type = type;
            SetPosition(position);
            if (normal != null)
            {
                SetNormal(normal);
            }
            if (color != null)
            {
                SetColor(color);
            }

            if(texCoord != null)
            {
                SetTextureCode(texCoord);
            }

            if (indexBuffer == null)
            {
                Num = position.Length;
                EnableIndexBuffer = false;
            }
            else
            {
                SetIndexArray(type, indexBuffer);
            }
        }


        /// <summary>
        /// 頂点バッファの設定
        /// </summary>
        /// <param name="type">ポリゴン種類</param>
        /// <param name="vertexSrc">頂点リスト</param>
        /// <param name="indexSrc">インデクサ</param>
        public void SetBuffer(KIPrimitiveType type, IEnumerable<Vertex> vertexSrc, IEnumerable<int> indexSrc)
        {
            if (indexSrc != null && indexSrc.Count() != 0)
            {
                SetPosition(vertexSrc.Select(p => p.Position).ToArray());
                SetNormal(vertexSrc.Select(p => p.Normal).ToArray());
                SetColor(vertexSrc.Select(p => p.Color).ToArray());
                SetTextureCode(vertexSrc.Select(p => p.TexCoord).ToArray());
                SetIndexArray(type, indexSrc.ToArray());
            }
            else
            {
                SetPosition(vertexSrc.Select(p => p.Position).ToArray());
                SetNormal(vertexSrc.Select(p => p.Normal).ToArray());
                SetColor(vertexSrc.Select(p => p.Color).ToArray());
                SetTextureCode(vertexSrc.Select(p => p.TexCoord).ToArray());
                Num = vertexSrc.Count();
                EnableIndexBuffer = false;
            }

            Type = type;
        }

        /// <summary>
        /// 位置情報の設定
        /// </summary>
        /// <param name="position">位置データ</param>
        private void SetPosition(Vector3[] position)
        {
            if(PositionBuffer == null)
            {
                Logger.Log(Logger.LogLevel.Error, "No Gen Buffer");
            }

            PositionBuffer.SetData(position, EArrayType.Vec3Array);
        }

        /// <summary>
        /// カラー情報の設定
        /// </summary>
        /// <param name="color">カラーデータ</param>
        private void SetColor(Vector3[] color)
        {
            if (ColorBuffer == null)
            {
                Logger.Log(Logger.LogLevel.Error, "No Gen Buffer");
            }

            ColorBuffer.SetData(color, EArrayType.Vec3Array);
        }

        /// <summary>
        /// 法線情報の設定
        /// </summary>
        /// <param name="normal">法線データ</param>
        private void SetNormal(Vector3[] normal)
        {
            if (NormalBuffer == null)
            {
                Logger.Log(Logger.LogLevel.Error, "No Gen Buffer");
            }

            NormalBuffer.SetData(normal, EArrayType.Vec3Array);
        }

        /// <summary>
        /// 法線情報の設定
        /// </summary>
        /// <param name="textureCode">テクスチャ座標データ</param>
        private void SetTextureCode(Vector2[] texture)
        {
            if (TexCoordBuffer == null)
            {
                Logger.Log(Logger.LogLevel.Error, "No Gen Buffer");
            }

            TexCoordBuffer.SetData(texture, EArrayType.Vec2Array);
        }

        /// <summary>
        /// 頂点配列の設定
        /// </summary>
        /// <param name="type">ポリゴン種類</param>
        /// <param name="indexArray">頂点配列</param>
        public void SetIndexArray(KIPrimitiveType type, int[] indexArray)
        {
            if (IndexBuffer == null)
            {
                Logger.Log(Logger.LogLevel.Error, "No Gen Buffer");
            }


            IndexBuffer.SetData(indexArray, EArrayType.IntArray);
            Type = type;
            Num = indexArray.Length;
            EnableIndexBuffer = true;
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
            vertexBuffer.ShallowCopyObject = true;
            vertexBuffer.Type = Type;

            return vertexBuffer;
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
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
        /// 描画
        /// </summary>
        public void Render()
        {
            if (EnableIndexBuffer)
            {
                DeviceContext.Instance.DrawElements(Type, Num, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                DeviceContext.Instance.DrawArrays(Type, 0, Num);
            }
        }
    }
}
