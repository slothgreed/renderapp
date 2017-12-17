using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Asset;
using KI.Foundation.Core;
using OpenTK;

namespace KI.Renderer
{
    /// <summary>
    /// 描画オブジェクト作成ファクトリ
    /// </summary>
    public class RenderObjectFactory : KIFactoryBase<RenderObject>
    {
        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        public static RenderObjectFactory Instance { get; } = new RenderObjectFactory();

        /// <summary>
        /// 空オブジェクトの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>描画オブジェクト</returns>
        public RenderObject CreateRenderObject(string name)
        {
            var renderObject = new RenderObject(name);
            AddItem(renderObject);
            return renderObject;
        }

        /// <summary>
        /// ジオメトリの描画オブジェクトの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="primitive">ジオメトリ</param>
        /// <returns>描画オブジェクト</returns>
        public RenderObject CreateRenderObject(string name, IPolygon primitive)
        {
            var renderObject = new RenderObject(name, primitive.Polygons.First());
            AddItem(renderObject);
            return renderObject;
        }

        /// <summary>
        /// ジオメトリの描画オブジェクトの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="primitive">ジオメトリ</param>
        /// <returns>描画オブジェクト</returns>
        public IEnumerable<RenderObject> CreateRenderObjects(string name, IPolygon primitive)
        {
            var list = new List<RenderObject>();
            foreach (var info in primitive.Polygons)
            {
                var renderObject = new RenderObject(name, info);
                list.Add(renderObject);
                AddItem(renderObject);
            }

            return list;
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
