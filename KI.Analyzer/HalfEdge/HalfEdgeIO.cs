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
        /// <param name="inputFile"></param>
        public static bool ReadFile(string inputFile, HalfEdge halfEdge)
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
                halfEdge.vertexs.Clear();
                halfEdge.edges.Clear();
                halfEdge.vertexs.Clear();

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

        private static void ReadHalfEdgeData(string[] fileData, HalfEdge halfEdge)
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
                    var vertex = new Vertex(position, halfEdge.vertexs.Count);
                    halfEdge.vertexs.Add(vertex);
                }

                if (lineInfos[0] == "e")
                {
                    int startIndex = int.Parse(lineInfos[1]);
                    int endIndex = int.Parse(lineInfos[2]);
                    var edge = new Edge(halfEdge.vertexs[startIndex], halfEdge.vertexs[endIndex], halfEdge.edges.Count);
                    halfEdge.edges.Add(edge);
                }

                if (lineInfos[0] == "m")
                {
                    int edge1 = int.Parse(lineInfos[1]);
                    int edge2 = int.Parse(lineInfos[2]);
                    int edge3 = int.Parse(lineInfos[3]);
                    var mesh = new Mesh(halfEdge.meshs.Count);
                    mesh.SetEdge(halfEdge.edges[edge1], halfEdge.edges[edge2], halfEdge.edges[edge3]);
                    halfEdge.meshs.Add(mesh);
                }

                if (lineInfos[0] == "ei")
                {
                    int nextIndex = int.Parse(lineInfos[1]);
                    int beforeIndex = int.Parse(lineInfos[2]);
                    int oppositeIndex = int.Parse(lineInfos[3]);
                    int meshIndex = int.Parse(lineInfos[4]);
                    var edge = halfEdge.edges[edgeInfoCounter];

                    edge.Next = halfEdge.edges[nextIndex];
                    edge.Before = halfEdge.edges[beforeIndex];
                    edge.Opposite = halfEdge.edges[oppositeIndex];
                    edge.Mesh = halfEdge.meshs[meshIndex];
                    edgeInfoCounter++;
                }
            }

            int count = halfEdge.edges.Count;

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

        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="outputFile"></param>
        public static void WriteFile(string outputFile, HalfEdge halfEdge)
        {
            StreamWriter write = new StreamWriter(outputFile);

            write.WriteLine("HalfEdge Data Structure");
            write.WriteLine("Vertex : Position");
            foreach (var vertex in halfEdge.vertexs)
            {
                write.WriteLine("v" + " " + vertex.Position.X + " " + vertex.Position.Y + " " + vertex.Position.Z);
            }

            write.WriteLine("Edge : Start Vetex Index, End Vertex Index");
            foreach (var edge in halfEdge.edges)
            {
                write.WriteLine("e" + " " + edge.Start.Index + " " + edge.End.Index);
            }

            write.WriteLine("Mesh : Vertex Index");
            foreach (var mesh in halfEdge.meshs)
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
            foreach (var edge in halfEdge.edges)
            {
                write.WriteLine("ei" + " " + edge.Next.Index + " " + edge.Before.Index + " " + edge.Opposite.Index + " " + edge.Mesh.Index);
            }

            write.WriteLine("end");
            write.Close();
        }
    }
}
