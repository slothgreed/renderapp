using System;
using System.Collections.Generic;
using System.Linq;
using KI.Gfx.Geometry;
using KI.Mathmatics;
using OpenTK;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// 等値線のアルゴリズム(Z)だけ対応
    /// </summary>
    public class IsoLineAlgorithm
    {
        /// <summary>
        /// ハーフエッジデータ構造
        /// </summary>
        private HalfEdgeDS halfEdgeDS;

        /// <summary>
        /// 等値線空間
        /// </summary>
        public IsoLineSpace[] IsoSpace;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="halfEdgeDS">ハーフエッジデータ構造</param>
        public IsoLineAlgorithm(HalfEdgeDS halfEdgeDS)
        {
            this.halfEdgeDS = halfEdgeDS;
        }

        /// <summary>
        /// 計算処理
        /// </summary>
        /// <param name="space">間隔</param>
        public void Calculate(float space)
        {
            PreProcess(space);
            Process();
        }

        /// <summary>
        /// 平面の間隔毎にポリゴンを振り分ける
        /// </summary>
        /// <param name="space">空間</param>
        private void PreProcess(float space)
        {
            BDB bdb = new BDB(halfEdgeDS.Vertexs.Select(p => p.Position).ToList());
            int spaceNum = (int)((bdb.Max.Z - bdb.Min.Z) / space) + 1;

            IsoLineSpace[] isoLines = new IsoLineSpace[spaceNum];

            // 空間の計算
            for(int i = 0; i < isoLines.Length; i++)
            {
                float height = bdb.Min.Z + i * space;
                isoLines[i] = new IsoLineSpace(height);
            }

            //空間内のエッジの計算
            foreach (var isoLineSpace in isoLines)
            {
                foreach (var mesh in halfEdgeDS.Meshs)
                {
                    if (isoLineSpace.InSpace(mesh))
                    {
                        isoLineSpace.Meshs.Add((HalfEdgeMesh)mesh);
                    }
                }
            }

            IsoSpace = isoLines;
        }

        /// <summary>
        /// 平面の間隔毎にポリゴンを振り分ける
        /// </summary>
        private void Process()
        {
            List<Line> crossLines = new List<Line>();
            foreach (var space in IsoSpace)
            {

                Vector3 result = Vector3.Zero;
                foreach (var mesh in space.Meshs)
                {
                    Vector3 start = Vector3.Zero;
                    Vector3 end = Vector3.Zero;
                    foreach (var edge in mesh.AroundEdge)
                    {

                        if (Interaction.PlaneToLine(edge.Start.Position, edge.End.Position, space.PlaneFormula, out result) == true)
                        {
                            if (start == Vector3.Zero)
                            {
                                start = new Vector3(result);
                            }
                            else if (end == Vector3.Zero)
                            {
                                end = new Vector3(result);
                            }
                            else
                            {
                                if (IsoLineSpace.PositionEquals(start, end))
                                {
                                    end = result;
                                }
                            }
                        }
                    }

                    if (start != Vector3.Zero && end != Vector3.Zero)
                    {
                        crossLines.Add(new Line(start, end));
                    }
                }

                space.SetIsoLines(crossLines);
            }
        }

        public class IsoLine
        {
            /// <summary>
            /// 等値線
            /// </summary>
            public List<Line> Lines = new List<Line>();

            /// <summary>
            /// 等値線の総長
            /// </summary>
            public float Length
            {
                get
                {
                    float length = 0;
                    foreach (var line in Lines)
                    {
                        length += (line.Start.Position - line.End.Position).Length;
                    }

                    return length;
                }
            }
        }

        /// <summary>
        /// 等値線の空間
        /// </summary>
        public class IsoLineSpace
        {
            /// <summary>
            /// 等値線上の面リスト
            /// </summary>
            public List<HalfEdgeMesh> Meshs = new List<HalfEdgeMesh>();

            /// <summary>
            /// 等値線リスト
            /// </summary>
            public List<IsoLine> IsoLines = new List<IsoLine>();

            /// <summary>
            /// 等値線を構成する平面の公式
            /// </summary>
            public Vector4 PlaneFormula;

            /// <summary>
            /// 高さ
            /// </summary>
            public float Height;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="min">最小値</param>
            /// <param name="max">最大値</param>
            public IsoLineSpace(float height)
            {
                Height = height;
                PlaneFormula = Plane.Formula(new Vector3(0, 0, Height), Vector3.UnitZ);
            }

            /// <summary>
            /// 面が空間内にあるか
            /// </summary>
            /// <param name="mesh">メッシュ</param>
            /// <returns>bool</returns>
            public bool InSpace(Mesh mesh)
            {
                bool downFlag = false;
                bool upFlag = false;
                foreach (var vertex in mesh.Vertexs)
                {
                    if (vertex.Position.Z < Height)
                    {
                        downFlag = true;
                    }

                    if(vertex.Position.Z > Height)
                    {
                        upFlag = true;
                    }
                    
                }

                // 全て上 ではない or  全て下ではないならfalse
                if (downFlag == true && upFlag == false ||
                    upFlag == true && downFlag == false)
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 線分が空間内にあるか
            /// </summary>
            /// <param name="start">始点</param>
            /// <param name="end">終点</param>
            /// <returns>bool</returns>
            public bool InSpace(Vector3 start, Vector3 end)
            {
                if (start.Z < Height &&
                    end.Z < Height)
                {
                    return false;
                }

                if (start.Z > Height &&
                    end.Z > Height)
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 等値線のセッタ
            /// </summary>
            /// <param name="crossLine">等値線</param>
            public void SetIsoLines(List<Line> crossLine)
            {
                if(crossLine.Count == 0)
                {
                    return;
                }

                var isoLine = new IsoLine();
                var searchLine = FindSearchLine(crossLine);
                isoLine.Lines.Add(searchLine);
                crossLine.Remove(searchLine);
                while (crossLine.Count != 0)
                {
                    int addIndex = -1;
                    bool reverse = false;
                    var last = isoLine.Lines.Last();
                    for (int i = 0; i < crossLine.Count; i++)
                    {
                        if (PositionEquals(last.End.Position, crossLine[i].Start.Position))
                        {
                            addIndex = i;
                            reverse = false;
                            break;
                        }
                        else if (PositionEquals(last.End.Position, crossLine[i].End.Position))
                        {
                            addIndex = i;
                            reverse = true;
                            break;
                        }
                    }

                    if (addIndex != -1)
                    {
                        if (reverse)
                        {
                            isoLine.Lines.Add(new Line(crossLine[addIndex].End, crossLine[addIndex].Start));
                        }
                        else
                        {
                            isoLine.Lines.Add(crossLine[addIndex]);
                        }

                        crossLine.RemoveAt(addIndex);
                    }
                    else
                    {
                        IsoLines.Add(isoLine);
                        SetIsoLines(crossLine);
                    }
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="crossLine"></param>
            /// <returns></returns>
            private Line FindSearchLine(List<Line> crossLine)
            {
                float minLength =
                    crossLine[0].Start.Position.X +
                    crossLine[0].Start.Position.Y +
                    crossLine[0].Start.Position.Z;

                int index = 0;
                for (int i = 1; i < crossLine.Count; i++)
                {
                    var length =
                    crossLine[i].Start.Position.X +
                    crossLine[i].Start.Position.Y +
                    crossLine[i].Start.Position.Z;

                    if (minLength > length)
                    {
                        index = i;
                        minLength = length;
                    }

                    length =
                    crossLine[i].End.Position.X +
                    crossLine[i].End.Position.Y +
                    crossLine[i].End.Position.Z;

                    if (minLength > length)
                    {
                        index = i;
                        minLength = length;
                    }
                }

                var dotVec = Vector3.Normalize(Vector3.UnitX + Vector3.UnitZ);
                var pos1 =
                    crossLine[index].Start.Position.X +
                    crossLine[index].Start.Position.Y +
                    crossLine[index].Start.Position.Z;

                var pos2 =
                    crossLine[index].End.Position.X +
                    crossLine[index].End.Position.Y +
                    crossLine[index].End.Position.Z;

                if (Vector3.Dot(dotVec, crossLine[index].Start.Position - crossLine[index].End.Position) <
                    Vector3.Dot(dotVec, crossLine[index].End.Position - crossLine[index].Start.Position))
                {
                    return crossLine[index];
                }
                else
                {
                    var tmp = crossLine[index].Start;
                    crossLine[index].Start = crossLine[index].End;
                    crossLine[index].End = tmp;
                    return crossLine[index];
                }
            }

            public static bool PositionEquals(Vector3 v1, Vector3 v2)
            {
                if (Math.Abs(v1.X - v2.X) > Calculator.THRESHOLD05 * 10)
                {
                    return false;
                }

                if (Math.Abs(v1.Y - v2.Y) > Calculator.THRESHOLD05 * 10)
                {
                    return false;
                }

                if (Math.Abs(v1.Z - v2.Z) > Calculator.THRESHOLD05 * 10)
                {
                    return false;
                }

                return true;
            }
        }

    }
}
