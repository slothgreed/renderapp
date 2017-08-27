using System;
using System.IO;
using System.Linq;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// ハーフエッジの入出力
    /// </summary>
    public static class HalfEdgeIO
    {
        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="inputFile">入力</param>
        /// <param name="halfEdge">ハーフエッジインスタンス</param>
        /// <returns>成功</returns>
        public static bool ReadFile(string inputFile, HalfEdgeDS halfEdge)
        {
            if (!File.Exists(inputFile))
            {
                return false;
            }

            if (Path.GetExtension(inputFile).ToLower() != ".half")
            {
                return false;
            }

            try
            {
                halfEdge.Vertexs.Clear();
                halfEdge.Edges.Clear();
                halfEdge.Vertexs.Clear();

                string[] fileData = File.ReadAllLines(inputFile, System.Text.Encoding.GetEncoding("Shift_JIS"));
                ReadHalfEdgeData(fileData, halfEdge);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("読み込み失敗 : " + inputFile);
                return false;
            }
        }

        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="outputFile">出力ファイル</param>
        /// <param name="halfEdge">ハーフエッジインスタンス</param>
        public static void WriteFile(string outputFile, HalfEdgeDS halfEdge)
        {
            StreamWriter write = new StreamWriter(outputFile);

            write.WriteLine("HalfEdge Data Structure");
            write.WriteLine("Vertex : Position");
            foreach (var vertex in halfEdge.Vertexs)
            {
                write.WriteLine("v" + " " + vertex.Position.X + " " + vertex.Position.Y + " " + vertex.Position.Z);
            }

            write.WriteLine("Edge : Start Vetex Index, End Vertex Index");
            foreach (var edge in halfEdge.Edges)
            {
                write.WriteLine("e" + " " + edge.Start.Index + " " + edge.End.Index);
            }

            write.WriteLine("Mesh : Vertex Index");
            foreach (var mesh in halfEdge.Meshs)
            {
                string edgeIdx = string.Empty;
                foreach (var edge in mesh.AroundEdge)
                {
                    if (edge == mesh.AroundEdge.Last())
                    {
                        edgeIdx += edge.Index.ToString();
                    }
                    else
                    {
                        edgeIdx += edge.Index.ToString() + " ";
                    }
                }

                write.WriteLine("m" + " " + edgeIdx);
            }

            write.WriteLine("Edge Info : Next Edge Index,Before Edge, Opposite Edge Index, Incident Face ");
            foreach (var edge in halfEdge.Edges)
            {
                write.WriteLine("ei" + " " + edge.Next.Index + " " + edge.Before.Index + " " + edge.Opposite.Index + " " + edge.Mesh.Index);
            }

            write.WriteLine("end");
            write.Close();
        }

        /// <summary>
        /// データの読み込み
        /// </summary>
        /// <param name="fileData">ファイルデータ</param>
        /// <param name="halfEdge">ハーフエッジインスタンス</param>
        private static void ReadHalfEdgeData(string[] fileData, HalfEdgeDS halfEdge)
        {
            int lineNumber = 0;
            string line;
            int edgeInfoCounter = 0;
            while (fileData.Length != lineNumber)
            {
                line = fileData[lineNumber];
                lineNumber++;
                if (line.Contains("HalfEdge Data Strucure")) continue;
                if (line.Contains("Vertex :")) continue;
                if (line.Contains("Edge :")) continue;
                if (line.Contains("Mesh :")) continue;
                if (line.Contains("Edge Info :")) continue;

                string[] lineInfos = line.Split(' ');
                lineInfos = lineInfos.Where(p => !(string.IsNullOrWhiteSpace(p) || string.IsNullOrEmpty(p))).ToArray();

                if (lineInfos[0] == "v")
                {
                    var position = new Vector3(float.Parse(lineInfos[1]), float.Parse(lineInfos[2]), float.Parse(lineInfos[3]));
                    var vertex = new HalfEdgeVertex(position, halfEdge.Vertexs.Count);
                    halfEdge.Vertexs.Add(vertex);
                }

                if (lineInfos[0] == "e")
                {
                    int startIndex = int.Parse(lineInfos[1]);
                    int endIndex = int.Parse(lineInfos[2]);
                    var edge = new HalfEdge(halfEdge.Vertexs[startIndex], halfEdge.Vertexs[endIndex], halfEdge.Edges.Count);
                    halfEdge.Edges.Add(edge);
                }

                if (lineInfos[0] == "m")
                {
                    int edge1 = int.Parse(lineInfos[1]);
                    int edge2 = int.Parse(lineInfos[2]);
                    int edge3 = int.Parse(lineInfos[3]);
                    var mesh = new Mesh(halfEdge.Meshs.Count);
                    mesh.SetEdge(halfEdge.Edges[edge1], halfEdge.Edges[edge2], halfEdge.Edges[edge3]);
                    halfEdge.Meshs.Add(mesh);
                }

                if (lineInfos[0] == "ei")
                {
                    int nextIndex = int.Parse(lineInfos[1]);
                    int beforeIndex = int.Parse(lineInfos[2]);
                    int oppositeIndex = int.Parse(lineInfos[3]);
                    int meshIndex = int.Parse(lineInfos[4]);
                    var edge = halfEdge.Edges[edgeInfoCounter];

                    edge.Next = halfEdge.Edges[nextIndex];
                    edge.Before = halfEdge.Edges[beforeIndex];
                    edge.Opposite = halfEdge.Edges[oppositeIndex];
                    edge.Mesh = halfEdge.Meshs[meshIndex];
                    edgeInfoCounter++;
                }
            }

            int count = halfEdge.Edges.Count;

            //for(int i = 0; i < 3;i++)
            //{
            //    halfEdge.EdgeFlips(halfEdge.m_Edge[i]);
            //}

            //for (int i = 0; i < 1; i++)
            //{
            //    halfEdge.VertexDecimation(halfEdge.m_Edge[i]);
            //}

            //int counter = 0;
            //try
            //{
            //    for (int i = 0; i < 3; i++)
            //    {
            //        halfEdge.EdgeFlips(halfEdge.m_Edge[i]);
            //        counter++;
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //halfEdge.HasError();
        }
    }
}
