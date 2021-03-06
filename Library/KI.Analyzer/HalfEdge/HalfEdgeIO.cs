﻿using System;
using System.Collections.Generic;
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
                string[] fileData = File.ReadAllLines(inputFile, System.Text.Encoding.GetEncoding("Shift_JIS"));
                if (fileData[0].Contains("V2"))
                {
                    ReadHalfEdgeDataVer2(fileData, halfEdge);
                }
                else
                {
                    ReadHalfEdgeData(fileData, halfEdge);
                }
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
        /// <param name="version">バージョン</param>
        public static void WriteFile(string outputFile, HalfEdgeDS halfEdge, int version)
        {
            if (version == 0)
            {
                WriteFile(outputFile, halfEdge);
            }
            else
            {
                WriteFileVer2(outputFile, halfEdge);
            }
        }

        /// <summary>
        /// 書き込みVersion2
        /// </summary>
        /// <param name="outputFile">出力ファイル</param>
        /// <param name="halfEdge">ハーフエッジインスタンス</param>
        public static void WriteFileVer2(string outputFile, HalfEdgeDS halfEdge)
        {
            StreamWriter write = new StreamWriter(outputFile);

            write.WriteLine("HalfEdge Data Structure V2");
            write.WriteLine(halfEdge.HalfEdgeVertexs.Count() + " " + halfEdge.HalfEdges.Count() + " " + halfEdge.HalfEdgeMeshs.Count());
            foreach (var vertex in halfEdge.HalfEdgeVertexs)
            {
                write.WriteLine("v" + " " + vertex.Position.X + " " + vertex.Position.Y + " " + vertex.Position.Z);
            }

            foreach (var edge in halfEdge.HalfEdges)
            {
                write.WriteLine("e" + " " + edge.Start.Index + " " + edge.End.Index);
            }

            foreach (var mesh in halfEdge.HalfEdgeMeshs)
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

            foreach (var edge in halfEdge.HalfEdges)
            {
                write.WriteLine("ei" + " " + edge.Next.Index + " " + edge.Before.Index + " " + edge.Opposite.Index + " " + edge.Mesh.Index);
            }

            write.WriteLine("end");
            write.Close();
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
            foreach (var vertex in halfEdge.HalfEdgeVertexs)
            {
                write.WriteLine("v" + " " + vertex.Position.X + " " + vertex.Position.Y + " " + vertex.Position.Z);
            }

            write.WriteLine("Edge : Start Vetex Index, End Vertex Index");
            foreach (var edge in halfEdge.HalfEdges)
            {
                write.WriteLine("e" + " " + edge.Start.Index + " " + edge.End.Index);
            }

            write.WriteLine("Mesh : Vertex Index");
            foreach (var mesh in halfEdge.HalfEdgeMeshs)
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
            foreach (var edge in halfEdge.HalfEdges)
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
            var halfEdges = new List<HalfEdge>();
            var halfMeshs = new List<HalfEdgeMesh>();
            var halfVertexs = new List<HalfEdgeVertex>();

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
                    var vertex = new HalfEdgeVertex(position, halfVertexs.Count);
                    halfVertexs.Add(vertex);
                }

                if (lineInfos[0] == "e")
                {
                    int startIndex = int.Parse(lineInfos[1]);
                    int endIndex = int.Parse(lineInfos[2]);
                    var edge = new HalfEdge(halfVertexs[startIndex], halfVertexs[endIndex], halfEdges.Count);
                    halfEdges.Add(edge);
                }

                if (lineInfos[0] == "m")
                {
                    int edge1 = int.Parse(lineInfos[1]);
                    int edge2 = int.Parse(lineInfos[2]);
                    int edge3 = int.Parse(lineInfos[3]);
                    var mesh = new HalfEdgeMesh(halfMeshs.Count);
                    mesh.SetEdge(halfEdges[edge1], halfEdges[edge2], halfEdges[edge3]);
                    halfMeshs.Add(mesh);
                }

                if (lineInfos[0] == "ei")
                {
                    int nextIndex = int.Parse(lineInfos[1]);
                    int beforeIndex = int.Parse(lineInfos[2]);
                    int oppositeIndex = int.Parse(lineInfos[3]);
                    int meshIndex = int.Parse(lineInfos[4]);
                    var edge = halfEdges[edgeInfoCounter];

                    edge.Next = halfEdges[nextIndex];
                    edge.Before = halfEdges[beforeIndex];
                    edge.Opposite = halfEdges[oppositeIndex];
                    edge.Mesh = halfMeshs[meshIndex];
                    edgeInfoCounter++;
                }
            }

            halfEdge.Setup(halfVertexs, halfEdges, halfMeshs);
        }

        /// <summary>
        /// データの読み込み
        /// </summary>
        /// <param name="fileData">ファイルデータ</param>
        /// <param name="halfEdge">ハーフエッジインスタンス</param>
        private static void ReadHalfEdgeDataVer2(string[] fileData, HalfEdgeDS halfEdge)
        {
            int lineNumber = 1;
            string line;
            int edgeInfoCounter = 0;
            var halfEdges = new List<HalfEdge>();
            var halfMeshs = new List<HalfEdgeMesh>();
            var halfVertexs = new List<HalfEdgeVertex>();

            while (fileData.Length != lineNumber)
            {
                line = fileData[lineNumber];
                lineNumber++;

                string[] lineInfos = line.Split(' ');
                lineInfos = lineInfos.Where(p => !(string.IsNullOrWhiteSpace(p) || string.IsNullOrEmpty(p))).ToArray();

                if (lineInfos[0] == "v")
                {
                    var position = new Vector3(float.Parse(lineInfos[1]), float.Parse(lineInfos[2]), float.Parse(lineInfos[3]));
                    var vertex = new HalfEdgeVertex(position, halfVertexs.Count);
                    halfVertexs.Add(vertex);
                }

                if (lineInfos[0] == "e")
                {
                    int startIndex = int.Parse(lineInfos[1]);
                    int endIndex = int.Parse(lineInfos[2]);
                    var edge = new HalfEdge(halfVertexs[startIndex], halfVertexs[endIndex], halfEdges.Count);
                    halfEdges.Add(edge);
                }

                if (lineInfos[0] == "m")
                {
                    int edge1 = int.Parse(lineInfos[1]);
                    int edge2 = int.Parse(lineInfos[2]);
                    int edge3 = int.Parse(lineInfos[3]);
                    var mesh = new HalfEdgeMesh(halfMeshs.Count);
                    mesh.SetEdge(halfEdges[edge1], halfEdges[edge2], halfEdges[edge3]);
                    halfMeshs.Add(mesh);
                }

                if (lineInfos[0] == "ei")
                {
                    int nextIndex = int.Parse(lineInfos[1]);
                    int beforeIndex = int.Parse(lineInfos[2]);
                    int oppositeIndex = int.Parse(lineInfos[3]);
                    int meshIndex = int.Parse(lineInfos[4]);
                    var edge = halfEdges[edgeInfoCounter];

                    edge.Next = halfEdges[nextIndex];
                    edge.Before = halfEdges[beforeIndex];
                    edge.Opposite = halfEdges[oppositeIndex];
                    edge.Mesh = halfMeshs[meshIndex];
                    edgeInfoCounter++;
                }
            }

            halfEdge.Setup(halfVertexs, halfEdges, halfMeshs);
        }
    }
}
