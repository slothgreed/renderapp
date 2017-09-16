using System.Collections.Generic;
using System.Linq;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer
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
        /// 描画する数
        /// </summary>
        public int Num { get; private set; }

        /// <summary>
        /// 頂点Indexバッファ
        /// </summary>
        public Dictionary<PrimitiveType, ArrayBuffer> IndexBuffer { get; private set; } = new Dictionary<PrimitiveType, ArrayBuffer>();

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            BufferFactory.Instance.RemoveByValue(PositionBuffer);
            BufferFactory.Instance.RemoveByValue(ColorBuffer);
            BufferFactory.Instance.RemoveByValue(TexCoordBuffer);
            BufferFactory.Instance.RemoveByValue(NormalBuffer);
        }

        /// <summary>
        /// バッファの作成
        /// </summary>
        /// <param name="polygon">ポリゴン</param>
        /// <param name="type">形状種類</param>
        public void GenBuffer(Polygon polygon, PrimitiveType type)
        {
            PositionBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            PositionBuffer.GenBuffer();
            NormalBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            NormalBuffer.GenBuffer();
            ColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            ColorBuffer.GenBuffer();
            TexCoordBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            TexCoordBuffer.GenBuffer();
            if (polygon.Index.ContainsKey(type))
            {
                IndexBuffer[type] = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
                IndexBuffer[type].GenBuffer();
            }
        }

        /// <summary>
        /// バッファにデータの設定
        /// </summary>
        /// <param name="polygon">ポリゴン</param>
        /// <param name="type">形状種類</param>
        public void SetupBuffer(Polygon polygon, PrimitiveType type, List<Vector3> color)
        {
            if (polygon.Index.ContainsKey(type))
            {
                PositionBuffer.SetData(polygon.Vertexs.Select(p => p.Position).ToList(), EArrayType.Vec3Array);
                NormalBuffer.SetData(polygon.Vertexs.Select(p => p.Normal).ToList(), EArrayType.Vec3Array);
                if (color != null)
                {
                    ColorBuffer.SetData(color, EArrayType.Vec3Array);
                }
                else
                {
                    ColorBuffer.SetData(polygon.Vertexs.Select(p => p.Color).ToList(), EArrayType.Vec3Array);
                }

                TexCoordBuffer.SetData(polygon.Vertexs.Select(p => p.TexCoord).ToList(), EArrayType.Vec2Array);
                IndexBuffer[type].SetData(polygon.Index[type], EArrayType.IntArray);
                Num = polygon.Index[type].Count;
            }
            else if (type == PrimitiveType.Points)
            {
                PositionBuffer.SetData(polygon.Vertexs.Select(p => p.Position).ToList(), EArrayType.Vec3Array);
                NormalBuffer.SetData(polygon.Vertexs.Select(p => p.Normal).ToList(), EArrayType.Vec3Array);
                if (color != null)
                {
                    ColorBuffer.SetData(color, EArrayType.Vec3Array);
                }
                else
                {
                    ColorBuffer.SetData(polygon.Vertexs.Select(p => p.Color).ToList(), EArrayType.Vec3Array);
                }
                TexCoordBuffer.SetData(polygon.Vertexs.Select(p => p.TexCoord).ToList(), EArrayType.Vec2Array);
                Num = polygon.Vertexs.Count;
            }
            else if (type == PrimitiveType.Lines)
            {
                var vertexs = new List<Vertex>();
                foreach (var line in polygon.Lines)
                {
                    vertexs.Add(line.Start);
                    vertexs.Add(line.End);
                }

                PositionBuffer.SetData(vertexs.Select(p => p.Position).ToList(), EArrayType.Vec3Array);
                NormalBuffer.SetData(vertexs.Select(p => p.Normal).ToList(), EArrayType.Vec3Array);
                if (color != null)
                {
                    ColorBuffer.SetData(color, EArrayType.Vec3Array);
                }
                else
                {
                    ColorBuffer.SetData(polygon.Vertexs.Select(p => p.Color).ToList(), EArrayType.Vec3Array);
                }

                TexCoordBuffer.SetData(vertexs.Select(p => p.TexCoord).ToList(), EArrayType.Vec2Array);
                Num = vertexs.Count;
            }
            else
            {
                var vertexs = new List<Vertex>();
                foreach (var mesh in polygon.Meshs)
                {
                    vertexs.AddRange(mesh.Vertexs);
                }

                PositionBuffer.SetData(vertexs.Select(p => p.Position).ToList(), EArrayType.Vec3Array);
                NormalBuffer.SetData(vertexs.Select(p => p.Normal).ToList(), EArrayType.Vec3Array);
                if (color != null)
                {
                    ColorBuffer.SetData(color, EArrayType.Vec3Array);
                }
                else
                {
                    ColorBuffer.SetData(polygon.Vertexs.Select(p => p.Color).ToList(), EArrayType.Vec3Array);
                }

                TexCoordBuffer.SetData(vertexs.Select(p => p.TexCoord).ToList(), EArrayType.Vec2Array);
                Num = vertexs.Count;
            }
        }
    }
}
