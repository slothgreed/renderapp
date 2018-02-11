using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer.Attribute
{
    /// <summary>
    /// 標準アトリビュート
    /// </summary>
    public class PolygonAttribute : AttributeBase
    {
        /// <summary>
        /// ポリゴン情報
        /// </summary>
        private Polygon polygon;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="polygon">ポリゴン</param>
        /// <param name="shader">シェーダ</param>
        public PolygonAttribute(string name, Polygon polygon, Shader shader)
            : base(name, polygon.Type, shader)
        {
            this.polygon = polygon;
            SetupBuffer();
        }

        /// <summary>
        /// バッファにデータの設定
        /// </summary>
        /// <param name="polygon">ポリゴン</param>
        /// <param name="type">形状種類</param>
        /// <param name="color">頂点カラーの設定</param>
        public void SetupBuffer()
        {
            if (VertexBuffer != null)
            {
                VertexBuffer.Dispose();
            }

            VertexBuffer = new VertexBuffer();

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
            else if (Type == PrimitiveType.Points)
            {
                position = polygon.Vertexs.Select(p => p.Position).ToArray();
                normal = polygon.Vertexs.Select(p => p.Normal).ToArray();
                color = polygon.Vertexs.Select(p => p.Color).ToArray();
                texCoord = polygon.Vertexs.Select(p => p.TexCoord).ToArray();
            }
            else if (Type == PrimitiveType.Lines)
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

                if (Type == PrimitiveType.Triangles)
                {
                    foreach (var mesh in polygon.Meshs)
                    {
                        vertexs.AddRange(mesh.Vertexs);
                        normals.Add(mesh.Normal);
                        normals.Add(mesh.Normal);
                        normals.Add(mesh.Normal);
                    }
                }
                else
                {
                    foreach (var mesh in polygon.Meshs)
                    {
                        vertexs.AddRange(mesh.Vertexs);
                        normals.Add(mesh.Normal);
                        normals.Add(mesh.Normal);
                        normals.Add(mesh.Normal);
                        normals.Add(mesh.Normal);
                    }
                }

                position = vertexs.Select(p => p.Position).ToArray();
                normal = normals.ToArray();
                color = vertexs.Select(p => p.Color).ToArray();
                texCoord = vertexs.Select(p => p.TexCoord).ToArray();
            }

            VertexBuffer.SetBuffer(position, normal, color, texCoord, indexBuffer);
        }
    }
}
