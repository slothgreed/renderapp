using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Asset.Loader.Model;
using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset.Loader.Converter
{
    /// <summary>
    /// STLのローダ現在テキストファイルのみ
    /// </summary>
    public class SSLICTestConverter : IPolygon
    {
        /// <summary>
        /// stlファイルのローダ
        /// </summary>
        private SSLICTestLoader licData;

        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public SSLICTestConverter(string filePath)
        {
            licData = new SSLICTestLoader(filePath);
            licData.Load();
            CreatePolygon();
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Polygon[] Polygons
        {
            get;
            private set;
        }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreatePolygon()
        {
            var mesh = new List<Mesh>();

            for (int i = 0; i < licData.Position.Count; i += 3)
            {
                mesh.Add(
                    new Mesh(
                        new Vertex(i, licData.Position[i], licData.Vector[i], Vector3.One, licData.Texcoord[i]),
                        new Vertex(i + 1, licData.Position[i + 1], licData.Vector[i + 1], Vector3.One, licData.Texcoord[i + 1]),
                        new Vertex(i + 2, licData.Position[i + 2], licData.Vector[i + 2], Vector3.One, licData.Texcoord[i + 2])));
            }

            Polygon info = new Polygon(licData.FileName, mesh, PolygonType.Triangles);
            Polygons = new Polygon[] { info };
        }
    }
}
