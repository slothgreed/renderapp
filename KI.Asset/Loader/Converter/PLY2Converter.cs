using System;
using System.Collections.Generic;
using System.IO;
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

            string filePath3 = @"E:\MyProgram\CrestCode\ravines.txt";
            string filePath4 = @"E:\MyProgram\CrestCode\ridges.txt";
            string[] fileStream = File.ReadAllLines(filePath3, System.Text.Encoding.GetEncoding("Shift_JIS"));
            Polygon info3 = ReadData(fileStream);
            string[] fileStream2 = File.ReadAllLines(filePath4, System.Text.Encoding.GetEncoding("Shift_JIS"));
            Polygon info4 = ReadData(fileStream2);

            Polygons = new Polygon[] { info, info2, info3, info4 };
        }

        private Polygon ReadData(string[] fileStream)
        {
            try
            {
                int vertexNum = 0;
                int faceNum = 0;
                int connectNum = 0;
                List<Vector4> vertexs = new List<Vector4>();
                float x = 0, y = 0, z = 0, w = 0;
                for (int i = 0; i < fileStream.Length; i++)
                {
                    var lineData = fileStream[i]
                        .Split(' ')
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToArray();
                    if (i == 0)
                    {
                        vertexNum = int.Parse(lineData[0]);
                        continue;
                    }
                    if (i == 1)
                    {
                        faceNum = int.Parse(lineData[0]);
                        continue;
                    }
                    if (i == 2)
                    {
                        connectNum = int.Parse(lineData[0]);
                        continue;
                    }
                    if (vertexs.Count != vertexNum)
                    {
                        x = float.Parse(lineData[0]);
                        y = float.Parse(lineData[1]);
                        z = float.Parse(lineData[2]);
                        w = float.Parse(lineData[3]);
                        vertexs.Add(new Vector4(x, y, z, w));
                        continue;
                    }
                }
                List<List<Vertex>> links = new List<List<Vertex>>();
                for (int i = 0; i < connectNum; i++)
                {
                    links.Add(new List<Vertex>());
                }

                for (int i = 0; i < vertexs.Count; i++)
                {
                    links[(int)vertexs[i].W].Add(new Vertex(0, new Vector3(vertexs[i].X, vertexs[i].Y, vertexs[i].Z), Vector3.Zero));
                }

                var lines = new List<Line>();
                foreach (var link in links)
                {
                    for (int i = 0; i < link.Count - 1; i++)
                    {
                        var line = new Line(link[i], link[i + 1]);
                        lines.Add(line);
                    }
                }

                return new Polygon(plyData.FileName, lines);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return null;
        }
    }
}
