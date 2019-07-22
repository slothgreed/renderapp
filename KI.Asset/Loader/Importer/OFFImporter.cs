using KI.Asset.Loader.Model;
using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.Loader.Importer
{
    class OFFImporter : ICreateModel
    {
        /// <summary>
        /// offファイルのローダ
        /// </summary>
        private OFFLoader offData;

        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public OFFImporter(string filePath)
        {
            offData = new OFFLoader(filePath);
            CreateModel();
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Polygon Model
        {
            get;
            private set;
        }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateModel()
        {
            var vertex = new List<Vertex>();
            var posArray = offData.Position.ToArray();
            var indexArray = offData.Index.ToArray();
            for (int i = 0; i < offData.Position.Count; i++)
            {
                vertex.Add(new Vertex(i + 0, offData.Position[i], offData.Color[i].Xyz / 255.0f));
            }

            Model = new Polygon(offData.FileName, vertex, offData.Index, KIPrimitiveType.Triangles);
        }
    }
}
