using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Asset;
using KI.Foundation.Core;

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
    }
}
