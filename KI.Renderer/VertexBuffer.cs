using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Utility;
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
        /// 頂点情報
        /// </summary>
        public List<Vector3> PositionList
        {
            set
            {
                if (value.Count == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertex num error");
                }

                PositionBuffer.SetData(value, EArrayType.Vec3Array);
            }
        }

        /// <summary>
        /// 法線情報
        /// </summary>
        public List<Vector3> NormalList
        {
            set
            {
                if (value.Count == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertex num error");
                }

                NormalBuffer.SetData(value, EArrayType.Vec3Array);
            }
        }

        /// <summary>
        /// カラー情報
        /// </summary>
        public List<Vector3> ColorList
        {
            set
            {
                if (value.Count == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertex num error");
                }

                ColorBuffer.SetData(value, EArrayType.Vec3Array);
            }
        }

        /// <summary>
        /// テクスチャ座標情報
        /// </summary>
        public List<Vector2> TexCoordList
        {
            set
            {
                if (value.Count == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertex num error");
                }

                TexCoordBuffer.SetData(value, EArrayType.Vec2Array);
            }
        }

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
            NormalBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            ColorBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);
            TexCoordBuffer = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ArrayBuffer);

            if (polygon.Index.ContainsKey(type))
            {
                IndexBuffer[type] = BufferFactory.Instance.CreateArrayBuffer(BufferTarget.ElementArrayBuffer);
            }
        }

        /// <summary>
        /// バッファにデータの設定
        /// </summary>
        /// <param name="polygon">ポリゴン</param>
        /// <param name="type">形状種類</param>
        /// <param name="color">頂点カラーの設定</param>
        public void SetupBuffer(Polygon polygon, PrimitiveType type)
        {
            if (polygon.Index.ContainsKey(type))
            {
                IndexBuffer[type].SetData(polygon.Index[type], EArrayType.IntArray);

                Num = polygon.Index[type].Count;
                PositionList = polygon.Vertexs.Select(p => p.Position).ToList();
                NormalList = polygon.Vertexs.Select(p => p.Normal).ToList();
                ColorList = polygon.Vertexs.Select(p => p.Color).ToList();
                TexCoordList = polygon.Vertexs.Select(p => p.TexCoord).ToList();
            }
            else if (type == PrimitiveType.Points)
            {
                Num = polygon.Vertexs.Count;
                PositionList = polygon.Vertexs.Select(p => p.Position).ToList();
                NormalList = polygon.Vertexs.Select(p => p.Normal).ToList();
                ColorList = polygon.Vertexs.Select(p => p.Color).ToList();
                TexCoordList = polygon.Vertexs.Select(p => p.TexCoord).ToList();
            }
            else if (type == PrimitiveType.Lines)
            {
                var vertexs = new List<Vertex>();
                foreach (var line in polygon.Lines)
                {
                    vertexs.Add(line.Start);
                    vertexs.Add(line.End);
                }

                Num = vertexs.Count;
                PositionList = vertexs.Select(p => p.Position).ToList();
                NormalList = vertexs.Select(p => p.Normal).ToList();
                ColorList = vertexs.Select(p => p.Color).ToList();
                TexCoordList = vertexs.Select(p => p.TexCoord).ToList();
            }
            else
            {
                var vertexs = new List<Vertex>();
                var normals = new List<Vector3>();
                foreach (var mesh in polygon.Meshs)
                {
                    vertexs.AddRange(mesh.Vertexs);
                    normals.Add(mesh.Normal);
                    normals.Add(mesh.Normal);
                    normals.Add(mesh.Normal);
                }

                Num = vertexs.Count;
                PositionList = vertexs.Select(p => p.Position).ToList();
                NormalList =normals;
                ColorList = vertexs.Select(p => p.Color).ToList();
                TexCoordList = vertexs.Select(p => p.TexCoord).ToList();
            }
        }
    }
}
