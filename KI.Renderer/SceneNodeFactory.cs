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
        public PolygonNode CreatePolygonNode(string name, Polygon polygon)
        {
            string vert = ShaderCreater.Instance.GetVertexShader(polygon);
            string frag = ShaderCreater.Instance.GetFragShader(polygon);
            var shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);

            var polygonNode = new PolygonNode(name, polygon, shader);
            AddItem(polygonNode);
            return polygonNode;
        }

        /// <summary>
        /// ジオメトリの描画オブジェクトの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="primitive">ジオメトリ</param>
        /// <returns>描画オブジェクト</returns>
        public PolygonNode CreatePolygonNode(string name, ICreateModel primitive)
        {
            return CreatePolygonNode(name, primitive.Model);
        }

        /// <summary>
        /// ライトの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// 
        /// <returns>ライト</returns>
        public Light CreatePointLight(string name, Vector3 lightPos)
        {
            return new PointLight(name, lightPos);
        }

        /// <summary>
        /// ライトの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="lightPos">位置</param>
        /// <param name="direction">向き</param>
        /// <returns>ライト</returns>
        public Light CreateDirectionLight(string name, Vector3 lightPos, Vector3 direction)
        {
            return new DirectionLight(name, lightPos, direction);
        }

    }
}
