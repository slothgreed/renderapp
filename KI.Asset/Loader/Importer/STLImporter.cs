using System.Collections.Generic;
using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset.Loader.Importer
{
    /// <summary>
    /// STLのローダ現在テキストファイルのみ
    /// </summary>
    public class STLImporter : ICreateModel
    {
        /// <summary>
        /// stlファイルのローダ
        /// </summary>
        private STLLoader stlData;

        /// <summary>
        /// ファイルパス
        /// </summary>
        private string filePath;

        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public STLImporter(string filePath)
        {
            stlData = new STLLoader(filePath);
            filePath = this.filePath;
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
            var index = new List<int>();

            for (int i = 0; i < stlData.Position.Count; i += 3)
            {
                vertex.Add(new Vertex(i + 0, stlData.Position[i + 0], stlData.Normal[i + 0], Vector3.One));
                vertex.Add(new Vertex(i + 1, stlData.Position[i + 1], stlData.Normal[i + 1], Vector3.One));
                vertex.Add(new Vertex(i + 2, stlData.Position[i + 2], stlData.Normal[i + 2], Vector3.One));
                index.Add(i);
                index.Add(i + 1);
                index.Add(i + 2);
            }

            ImporterUtility.NormalizeObject(vertex);

            Model = new Polygon(filePath, vertex, index, KIPrimitiveType.Triangles);
        }
    }
}
