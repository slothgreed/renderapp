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

        /// <summary>
        /// バッファにデータの設定
        /// </summary>
        /// <param name="polygon">ポリゴン</param>
        /// <param name="type">形状種類</param>
        /// <param name="color">頂点カラーの設定</param>
        public void SetupBuffer(Polygon polygon)
        {
            int[] indexBuffer = null;
            Vector3[] position = null;
            Vector3[] normal = null;
            Vector3[] color = null;
            Vector2[] texCoord = null;
            if (polygon.Index.Count != 0)
            {
                indexBuffer = polygon.Index.ToArray();
                position = polygon.Vertexs.Select(p => p.Position).ToArray();
                normal = polygon.Vertexs.Select(p => p.Normal).ToArray();
                color = polygon.Vertexs.Select(p => p.Color).ToArray();
                texCoord = polygon.Vertexs.Select(p => p.TexCoord).ToArray();
            }
            else if (polygon.Type == PrimitiveType.Points)
            {
                position = polygon.Vertexs.Select(p => p.Position).ToArray();
                normal = polygon.Vertexs.Select(p => p.Normal).ToArray();
                color = polygon.Vertexs.Select(p => p.Color).ToArray();
                texCoord = polygon.Vertexs.Select(p => p.TexCoord).ToArray();
            }
            else if (polygon.Type == PrimitiveType.Lines)
            {
                var vertexs = new List<Vertex>();
                foreach (var line in polygon.Lines)
                {
                    vertexs.Add(line.Start);
                    vertexs.Add(line.End);
                }

                position = vertexs.Select(p => p.Position).ToArray();
                normal = vertexs.Select(p => p.Normal).ToArray();
                color = vertexs.Select(p => p.Color).ToArray();
                texCoord = vertexs.Select(p => p.TexCoord).ToArray();
            }
            else
            {
                var vertexs = new List<Vertex>();
                var normals = new List<Vector3>();

                if (polygon.Type == PrimitiveType.Triangles)
                {
                    foreach (var mesh in polygon.Meshs)
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
                    foreach (var mesh in polygon.Meshs)
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
