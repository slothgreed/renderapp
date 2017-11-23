using System.Collections.Generic;
using System.Linq;
using KI.Asset.Loader.Loader;
using KI.Gfx.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace KI.Asset
{
    /// <summary>
    /// plyファイルデータを独自形式に変換
    /// </summary>
    public class PLY2Converter : IPolygon
    {
        /// <summary>
        /// plyファイルデータ
        /// </summary>
        private PLY2Loader plyData;

        /// <summary>
        /// plyファイルのローダ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public PLY2Converter(string filePath,string filePath2)
        {
            plyData = new PLY2Loader(filePath, filePath2);
            CreatePolygon();
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Polygon[] Polygons { get; private set; }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreatePolygon()
        {
            var vertexs = new List<Vertex>();
            var lines = new List<Line>();
            int vertexNum = plyData.Vertexs.Count;
            for (int i = 0; i < vertexNum; i++)
            {
                var position = plyData.Vertexs[i];
                var k1 = plyData.Propertys[i][0];
                var k2 = plyData.Propertys[i][1];
                var t1x = plyData.Propertys[i][2] * 0.1f;
                var t1y = plyData.Propertys[i][3] * 0.1f;
                var t1z = plyData.Propertys[i][4] * 0.1f;
                var t2x = plyData.Propertys[i][5] * 0.1f;
                var t2y = plyData.Propertys[i][6] * 0.1f;
                var t2z = plyData.Propertys[i][7] * 0.1f;
                var normalx = plyData.Propertys[i][8];
                var normaly = plyData.Propertys[i][9];
                var normalz = plyData.Propertys[i][10];

                var vertex = new Vertex(i, position, new Vector3(normalx, normaly, normalz), Vector3.UnitZ);
                vertexs.Add(vertex);

                var vertex1 = new Vertex(i, vertex);
                var vertex2 = new Vertex(i, vertex);
                vertex1.Color = Vector3.UnitX;
                vertex2.Color = Vector3.UnitY;
                vertex1.Position = position - new Vector3(t1x, t1y, t1z);
                vertex2.Position = position - new Vector3(t2x, t2y, t2z);
                var t1vec = new Vertex(i, position + new Vector3(t1x, t1y, t1z), new Vector3(normalx, normaly, normalz), Vector3.UnitX);
                var t2vec = new Vertex(i, position + new Vector3(t2x, t2y, t2z), new Vector3(normalx, normaly, normalz), Vector3.UnitY);

                lines.Add(new Line(vertex1, t1vec));
                lines.Add(new Line(vertex2, t2vec));

            }

            Polygon info = new Polygon(plyData.FileName, vertexs,plyData.FaceIndex,PrimitiveType.Triangles);
            Polygon info2 = new Polygon(plyData.FileName, lines);
            Polygons = new Polygon[] { info ,info2};
        }
    }
}
