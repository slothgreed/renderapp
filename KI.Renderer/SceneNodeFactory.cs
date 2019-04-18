using KI.Asset;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.KIShader;
using OpenTK;

namespace KI.Renderer
{
    /// <summary>
    /// 描画オブジェクト作成ファクトリ
    /// </summary>
    public class SceneNodeFactory : KIFactoryBase<PolygonNode>
    {
        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        public static SceneNodeFactory Instance { get; } = new SceneNodeFactory();

        /// <summary>
        /// 空オブジェクトの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>描画オブジェクト</returns>
        public PolygonNode CreatePolygonNode(string name, Polygon polygon, Material material = null)
        {
            string vert = ShaderCreater.Instance.GetVertexShader(polygon);
            string frag = ShaderCreater.Instance.GetFragShader(polygon);
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

            var polygonNode = new PolygonNode(name, polygon);
            AddItem(polygonNode);
            return polygonNode;
        }

        /// <summary>
        /// 空オブジェクトの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>描画オブジェクト</returns>
        public PolygonNode CreatePolygonNode(string name, Vector3[] position, Vector3[] normal, Vector2[] texcoord, int[] index, PolygonType type)
        {
            Vertex[] vertex = new Vertex[position.Length];
            for (int i = 0; i < position.Length; i++)
            {
                vertex[i] = new Vertex(i, position[i], normal[i], texcoord[i]);
            }

            Polygon polygon = new Polygon(name, vertex, index, type);
            string vert = ShaderCreater.Instance.GetVertexShader(polygon);
            string frag = ShaderCreater.Instance.GetFragShader(polygon);
            var shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);
            Material material = new Material(shader);
            polygon.Material = material;

            var polygonNode = new PolygonNode(name, polygon);
            AddItem(polygonNode);
            return polygonNode;
        }

        /// <summary>
        /// 空オブジェクトの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>描画オブジェクト</returns>
        public PolygonNode CreatePolygonNode(string name, Vector3[] position, Vector3[] color, int[] index, PolygonType type)
        {
            Vertex[] vertex = new Vertex[position.Length];
            for (int i = 0; i < position.Length; i++)
            {
                vertex[i] = new Vertex(i, position[i], color[i]);
            }

            Polygon polygon = new Polygon(name, vertex, index, type);
            string vert = ShaderCreater.Instance.GetVertexShader(polygon);
            string frag = ShaderCreater.Instance.GetFragShader(polygon);
            var shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);
            Material material = new Material(shader);
            polygon.Material = material;

            var polygonNode = new PolygonNode(name, polygon);
            AddItem(polygonNode);
            return polygonNode;
        }

        /// <summary>
        /// ジオメトリの描画オブジェクトの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="primitive">ジオメトリ</param>
        /// <returns>描画オブジェクト</returns>
        public PolygonNode CreatePolygonNode(string name, ICreateModel primitive, Material material)
        {
            return CreatePolygonNode(name, primitive.Model, material);
        }
    }
}
