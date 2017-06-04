using OpenTK;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace KI.Gfx.Analyzer
{
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
                halfEdge.m_Vertex.Clear();
                halfEdge.m_Edge.Clear();
                halfEdge.m_Vertex.Clear();

                String[] fileData = File.ReadAllLines(inputFile, System.Text.Encoding.GetEncoding("Shift_JIS"));
                ReadHalfEdgeData(fileData,halfEdge);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("読み込み失敗 : " + inputFile);
                return false;
            }
        }
        private static void ReadHalfEdgeData(String[] fileData, HalfEdge halfEdge)
        {
            int lineNumber = 0;
            String line;
            int EdgeInfoCounter = 0;
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
                lineInfos = lineInfos.Where(p => !(String.IsNullOrWhiteSpace(p) || String.IsNullOrEmpty(p))).ToArray();

                if (lineInfos[0] == "v")
                {
                    var position = new Vector3(float.Parse(lineInfos[1]), float.Parse(lineInfos[2]), float.Parse(lineInfos[3]));
                    var vertex = new Vertex(position, halfEdge.m_Vertex.Count);
                    halfEdge.m_Vertex.Add(vertex);
                }

                if (lineInfos[0] == "e")
                {
                    int startIndex = int.Parse(lineInfos[1]);
                    int endIndex = int.Parse(lineInfos[2]);
                    var edge = new Edge(halfEdge.m_Vertex[startIndex], halfEdge.m_Vertex[endIndex], halfEdge.m_Edge.Count);
                    halfEdge.m_Edge.Add(edge);
                }

                if (lineInfos[0] == "m")
                {
                    int edge1 = int.Parse(lineInfos[1]);
                    int edge2 = int.Parse(lineInfos[2]);
                    int edge3 = int.Parse(lineInfos[3]);
                    var mesh = new Mesh(halfEdge.m_Mesh.Count);
                    mesh.SetEdge(halfEdge.m_Edge[edge1], halfEdge.m_Edge[edge2], halfEdge.m_Edge[edge3]);
                    halfEdge.m_Mesh.Add(mesh);
                }

                if (lineInfos[0] == "ei")
                {
                    int nextIndex = int.Parse(lineInfos[1]);
                    int beforeIndex = int.Parse(lineInfos[2]);
                    int oppositeIndex = int.Parse(lineInfos[3]);
                    int meshIndex = int.Parse(lineInfos[4]);
                    var edge = halfEdge.m_Edge[EdgeInfoCounter];

                    edge.Next = halfEdge.m_Edge[nextIndex];
                    edge.Before = halfEdge.m_Edge[beforeIndex];
                    edge.Opposite = halfEdge.m_Edge[oppositeIndex];
                    edge.Mesh = halfEdge.m_Mesh[meshIndex];
                    EdgeInfoCounter++;
                }
            }
            int count = halfEdge.m_Edge.Count;

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
            foreach (var vertex in halfEdge.m_Vertex)
            {
                write.WriteLine("v" + " " + vertex.Position.X + " " + vertex.Position.Y + " " + vertex.Position.Z);
            }
            write.WriteLine("Edge : Start Vetex Index, End Vertex Index");
            foreach (var edge in halfEdge.m_Edge)
            {
                write.WriteLine("e" + " " + edge.Start.Index + " " + edge.End.Index);
            }

            write.WriteLine("Mesh : Vertex Index");
            foreach (var mesh in halfEdge.m_Mesh)
            {
                string edgeIdx = "";
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
            foreach (var edge in halfEdge.m_Edge)
            {
                write.WriteLine("ei" + " " + edge.Next.Index + " " + edge.Before.Index + " " + edge.Opposite.Index + " " + edge.Mesh.Index);
            }
            write.WriteLine("end");
            write.Close();
        }
    }
}
