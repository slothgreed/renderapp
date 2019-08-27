using KI.Asset;
using KI.Asset.Primitive;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.KIShader;
using OpenTK;
using System.Linq;

namespace KI.Renderer
{
    /// <summary>
    /// ポリオゴン作成のユーティリティ
    /// </summary>
    public static class PolygonUtility
    {
        /// <summary>
        /// ポリゴンのセットアップ
        /// </summary>
        /// <returns>描画オブジェクト</returns>
        public static void Setup(Polygon polygon, Material material = null)
        {
            string vert = ShaderCreater.Instance.GetVertexShader(polygon.Type, material);
            string frag = ShaderCreater.Instance.GetFragShaderFilePath(polygon.Type, material);
            var shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);

            if (material == null)
            {
                material = new Material(shader);
            }
            else
            {
                material.Shader = shader;
            }

            polygon.Material = material;
        }

        /// <summary>
        /// ポリゴンの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>描画オブジェクト</returns>
        public static Polygon CreatePolygon(string name, PrimitiveBase primitive, Material material = null)
        {
            Polygon polygon = new Polygon(name, primitive.Vertexs.ToList(), primitive.Index.ToList(), primitive.Type);
            Setup(polygon, material);
            return polygon;
        }

        /// <summary>
        /// ポリゴンの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>描画オブジェクト</returns>
        public static Polygon CreatePolygon(string name, Vector3[] position, Vector3[] color, int[] index, KIPrimitiveType type, Material material = null)
        {
            Vertex[] vertex = new Vertex[position.Length];
            for (int i = 0; i < position.Length; i++)
            {
                vertex[i] = new Vertex(i, position[i], color[i]);
            }

            Polygon polygon = new Polygon(name, vertex.ToList(), index.ToList(), type);
            Setup(polygon, material);
            return polygon;
        }
    }
}
