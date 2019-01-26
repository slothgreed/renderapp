using System.Collections.Generic;
using KI.Asset.Loader;
using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset.Loader.Converter
{
    /// <summary>
    /// STLのローダ現在テキストファイルのみ
    /// </summary>
    public class STLConverter : ICreateModel
    {
        /// <summary>
        /// stlファイルのローダ
        /// </summary>
        private STLLoader stlData;

        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public STLConverter(string filePath)
        {
            stlData = new STLLoader(filePath);
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
            var mesh = new List<Mesh>();

            for (int i = 0; i < stlData.Position.Count; i += 3)
            {
                mesh.Add(
                    new Mesh(
                        new Vertex(i, stlData.Position[3 * i], stlData.Normal[3 * i], Vector3.One),
                        new Vertex(i + 1, stlData.Position[3 * i + 1], stlData.Normal[3 * i + 1], Vector3.One),
                        new Vertex(i + 2, stlData.Position[3 * i + 2], stlData.Normal[3 * i + 2], Vector3.One)));
            }

            Model = new Polygon(stlData.FileName, mesh, PolygonType.Triangles);
        }
    }
}
