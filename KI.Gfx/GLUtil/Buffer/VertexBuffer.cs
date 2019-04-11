using System;
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
        /// 浅いコピーのオブジェクトかどうか
        /// </summary>
        public bool ShallowCopyObject { get; set; } = false;

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
                PositionBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            }

            if (NormalBuffer == null)
            {
                NormalBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            }

            if (ColorBuffer == null)
            {
                ColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            }

            if (TexCoordBuffer == null)
            {
                TexCoordBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            }

            if (IndexBuffer == null)
            {
                IndexBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
            }
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
            SetPosition(position);
            SetNormal(normal);
            SetColor(color);
            SetTextureCode(texCoord);

            if (indexBuffer == null)
            {
                Num = position.Length;
                EnableIndexBuffer = false;
            }
            else
            {
                SetIndexArray(indexBuffer);
            }
        }
        
        /// <summary>
        /// 位置情報の設定
        /// </summary>
        /// <param name="position">位置データ</param>
        public void SetPosition(Vector3[] position)
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
        public void SetColor(Vector3[] color)
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
        public void SetNormal(Vector3[] normal)
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
        public void SetTextureCode(Vector2[] texture)
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
        /// <param name="indexArray">頂点配列</param>
        public void SetIndexArray(int[] indexArray)
        {
            if (IndexBuffer == null)
            {
                Logger.Log(Logger.LogLevel.Error, "No Gen Buffer");
            }

            IndexBuffer.SetData(indexArray, EArrayType.IntArray);
            Num = indexArray.Length;
            EnableIndexBuffer = true;
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
        public void SetBuffer(Vertex[] vertex, int[] indexBuffer)
        {
            SetPosition(vertex.Select(p => p.Position).ToArray());
            SetNormal(vertex.Select(p => p.Normal).ToArray());
            SetColor(vertex.Select(p => p.Color).ToArray());
            SetTextureCode(vertex.Select(p => p.TexCoord).ToArray());

            if (indexBuffer == null)
            {
                Num = vertex.Length;
                EnableIndexBuffer = false;
            }
            else
            {
                SetIndexArray(indexBuffer);
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
            vertexBuffer.ShallowCopyObject = true;

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
        /// 頂点バッファの設定（点群）
        /// </summary>
        /// <param name="vertexSrc">頂点リスト</param>
        /// <param name="indexSrc">インデクサ</param>
        public void SetupPointBuffer(IEnumerable<Vertex> vertexSrc, List<int> indexSrc)
        {
            int[] indexBuffer = null;
            Vector3[] position = null;
            Vector3[] normal = null;
            Vector3[] color = null;
            Vector2[] texCoord = null;
            if (indexSrc != null && indexSrc.Count != 0)
            {
                indexBuffer = indexSrc.ToArray();
                position = vertexSrc.Select(p => p.Position).ToArray();
                normal = vertexSrc.Select(p => p.Normal).ToArray();
                color = vertexSrc.Select(p => p.Color).ToArray();
                texCoord = vertexSrc.Select(p => p.TexCoord).ToArray();
            }
            else
            {
                position = vertexSrc.Select(p => p.Position).ToArray();
                normal = vertexSrc.Select(p => p.Normal).ToArray();
                color = vertexSrc.Select(p => p.Color).ToArray();
                texCoord = vertexSrc.Select(p => p.TexCoord).ToArray();
            }

            SetBuffer(position, normal, color, texCoord, indexBuffer);
        }

        /// <summary>
        /// 頂点バッファの設定（線分）
        /// </summary>
        /// <param name="vertexSrc">頂点バッファ</param>
        /// <param name="indexSrc">インデックスバッファ</param>
        /// <param name="lines">線分</param>
        public void SetupLineBuffer(IEnumerable<Vertex> vertexSrc, List<int> indexSrc, List<Line> lineSrc)
        {
            int[] indexBuffer = null;
            Vector3[] position = null;
            Vector3[] normal = null;
            Vector3[] color = null;
            Vector2[] texCoord = null;
            if (indexSrc != null && indexSrc.Count != 0)
            {
                indexBuffer = indexSrc.ToArray();
                position = vertexSrc.Select(p => p.Position).ToArray();
                normal = vertexSrc.Select(p => p.Normal).ToArray();
                color = vertexSrc.Select(p => p.Color).ToArray();
                texCoord = vertexSrc.Select(p => p.TexCoord).ToArray();
            }
            else
            {
                var vertexs = new List<Vertex>();
                foreach (var line in lineSrc)
                {
                    vertexs.Add(line.Start);
                    vertexs.Add(line.End);
                }

                position = vertexs.Select(p => p.Position).ToArray();
                normal = vertexs.Select(p => p.Normal).ToArray();
                color = vertexs.Select(p => p.Color).ToArray();
                texCoord = vertexs.Select(p => p.TexCoord).ToArray();
            }

            SetBuffer(position, normal, color, texCoord, indexBuffer);
        }

        /// <summary>
        /// 頂点バッファの設定（メッシュ）
        /// </summary>
        /// <param name="vertexSrc">頂点バッファ</param>
        /// <param name="indexSrc">インデックスバッファ</param>
        /// <param name="meshSrc">メッシュリスト</param>
        public void SetupMeshBuffer(IEnumerable<Vertex> vertexSrc, List<int> indexSrc, List<Mesh> meshSrc, PolygonType type)
        {
            int[] indexBuffer = null;
            Vector3[] position = null;
            Vector3[] normal = null;
            Vector3[] color = null;
            Vector2[] texCoord = null;
            if (indexSrc != null && indexSrc.Count != 0)
            {
                indexBuffer = indexSrc.ToArray();
                position = vertexSrc.Select(p => p.Position).ToArray();
                normal = vertexSrc.Select(p => p.Normal).ToArray();
                color = vertexSrc.Select(p => p.Color).ToArray();
                texCoord = vertexSrc.Select(p => p.TexCoord).ToArray();
            }
            else
            {
                var vertexs = new List<Vertex>();
                var normals = new List<Vector3>();

                if (type == PolygonType.Triangles)
                {
                    foreach (var mesh in meshSrc)
                    {
                        vertexs.AddRange(mesh.Vertexs);

                        var meshNormal =
                            Mathmatics.Geometry.Normal(
                                    mesh.Lines[1].Start.Position - mesh.Lines[0].Start.Position,
                                    mesh.Lines[2].Start.Position - mesh.Lines[0].Start.Position);

                        normals.Add(meshNormal);
                        normals.Add(meshNormal);
                        normals.Add(meshNormal);
                    }
                }
                else
                {
                    foreach (var mesh in meshSrc)
                    {
                        vertexs.AddRange(mesh.Vertexs);
                        var meshNormal =
                            Mathmatics.Geometry.Normal(
                                    mesh.Lines[1].Start.Position - mesh.Lines[0].Start.Position,
                                    mesh.Lines[2].Start.Position - mesh.Lines[0].Start.Position);

                        normals.Add(meshNormal);
                        normals.Add(meshNormal);
                        normals.Add(meshNormal);
                        normals.Add(meshNormal);
                    }
                }

                position = vertexs.Select(p => p.Position).ToArray();
                normal = normals.ToArray();
                color = vertexs.Select(p => p.Color).ToArray();
                texCoord = vertexs.Select(p => p.TexCoord).ToArray();
            }

            SetBuffer(position, normal, color, texCoord, indexBuffer);
        }
    }
}
