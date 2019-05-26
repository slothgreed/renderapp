using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using KI.Mathmatics;
using OpenTK;

namespace CADApp.Model
{
    /// <summary>
    /// アセンブリ
    /// </summary>
    public class Assembly : KIObject
    {
        private List<Vertex> vertexs;
        private List<int> selectVertexs;

        private List<int> lineIndex;
        private List<int> selectLines;

        private List<int> triangleIndex;
        private List<int> selectTriangles;

        List<Vertex> controlPoint;
        private List<int> selectControlPoints;

        public bool CurrentEdit { get; private set; }


        /// <summary>
        /// 形状情報更新イベント
        /// </summary>
        public EventHandler AssemblyUpdated { get; set; }

        public List<Vertex> Vertex
        {
            get
            {
                return vertexs;
            }
        }

        public List<int> SelectVertexs
        {
            get
            {
                return selectVertexs;
            }
        }

        public List<int> LineIndex
        {
            get
            {
                return lineIndex;
            }
        }

        public List<int> SelectLines
        {
            get
            {
                return selectLines;
            }
        }

        public int LineNum
        {
            get { return lineIndex.Count / 2; }
        }

        public List<int> TriangleIndex
        {
            get
            {
                return triangleIndex;
            }
        }

        public int TriangleNum
        {
            get { return triangleIndex.Count / 3; }
        }


        public List<int> SelectTriangles
        {
            get
            {
                return selectTriangles;
            }
        }

        public List<Vertex> ControlPoint
        {
            get
            {
                return controlPoint;
            }
        }

        public List<int> SelectControlPoints
        {
            get
            {
                return selectControlPoints;
            }
        }


        public Assembly(string name)
            : base(name)
        {
            vertexs = new List<Vertex>();
            lineIndex = new List<int>();
            triangleIndex = new List<int>();
            controlPoint = new List<Vertex>();

            selectVertexs = new List<int>();
            selectLines = new List<int>();
            selectTriangles = new List<int>();
            selectControlPoints = new List<int>();
        }

        /// <summary>
        /// コントロールポイントの追加
        /// </summary>
        public void AddControlPoint(Vector3 position)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            controlPoint.Add(new Vertex(controlPoint.Count, position, Vector3.UnitY));
        }

        public void AddVertex(Vector3 position)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            vertexs.Add(new Vertex(vertexs.Count, position, Vector3.UnitX));
        }


        public void AddVertex(Vector3 position, Vector3 normal, Vector3 color)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            vertexs.Add(new Vertex(vertexs.Count, position, normal, color));
        }

        public void AddVertex(Vector3 position, Vector3 normal)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            vertexs.Add(new Vertex(vertexs.Count, position, normal, Vector3.UnitX));
        }

        public void SetVertex(Vector3[] positionList)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            ClearVertex();

            for (int i = 0; i < positionList.Length; i++)
            {
                AddVertex(positionList[i]);
            }
        }

        public void SetVertex(Vector3[] positionList, Vector3[] normalList)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            ClearVertex();

            for(int i = 0; i < positionList.Length; i++)
            {
                AddVertex(positionList[i], normalList[i]);
            }
        }

        public void SetVertex(int index, Vector3 position)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            vertexs[index].Position = position;
        }


        public void RemoveVertex(int index)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            vertexs.RemoveAt(index);
        }

        public Vertex GetVertex(int index)
        {
            return vertexs[index];
        }

        public void ClearVertex()
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            vertexs.Clear();
        }

        public void SetLineIndex(IEnumerable<int> index)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            lineIndex = index.ToList();
        }

        public void AddLineIndex(int vertexIndex)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            lineIndex.Add(vertexIndex);
        }

        public void ClearLineIndex()
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            lineIndex.Clear();
        }

        public void SetTriangleIndexFromLineIndex(Vector3 triangleOrient)
        {

            int[] linesIndex;
            int[] triangleIndexArray;
            Geometry.ConvertLinesToLineLoop(LineIndex.ToArray(), out linesIndex);
            Vector3[] linesArray = new Vector3[linesIndex.Length];
            for (int i = 0; i < linesArray.Length; i++)
            {
                linesArray[i] = GetVertex(linesIndex[i]).Position;
            }

            Geometry.GenPolygonFromLineLoop(linesArray, triangleOrient, out triangleIndexArray);
            triangleIndex.Clear();

            triangleIndex.AddRange(triangleIndexArray);
        }

        public void SetLineIndexFromTriangleIndex()
        {
            if (triangleIndex.Count == 0)
            {
                return;
            }

            lineIndex.Clear();
            for (int i = 0; i < triangleIndex.Count; i += 3)
            {
                lineIndex.Add(triangleIndex[i + 0]);
                lineIndex.Add(triangleIndex[i + 1]);
                lineIndex.Add(triangleIndex[i + 1]);
                lineIndex.Add(triangleIndex[i + 2]);
                lineIndex.Add(triangleIndex[i + 2]);
                lineIndex.Add(triangleIndex[i + 0]);
            }

            lineIndex.Add(triangleIndex[triangleIndex.Count - 1]);
        }

        public void SetTriangleIndex(IEnumerable<int> index)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            triangleIndex = index.ToList();
            SetLineIndexFromTriangleIndex();
        }

        public void GetLine(int index, out int start, out int end)
        {
            index *= 2;
            start = LineIndex[index];
            end = LineIndex[index + 1];
        }

        public void GetTriangle(int index, out int triangle0, out int triangle1, out int triangle2)
        {
            index *= 3;
            triangle0 = triangleIndex[index];
            triangle1 = triangleIndex[index + 1];
            triangle2 = triangleIndex[index + 2];
        }

        public void ClearTriangleIndex()
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            triangleIndex.Clear();
        }

        public void AddSelectVertex(int index)
        {
            SelectVertexs.Add(index);
        }

        public void AddSelectLine(int index)
        {
            SelectLines.Add(index);
        }

        public void AddSelectTriangle(int index)
        {
            selectTriangles.Add(index);
        }

        public void AddSelectControlPoint(int index)
        {
            selectControlPoints.Add(index);
        }

        public void ClearSelect()
        {
            SelectVertexs.Clear();
            SelectLines.Clear();
            SelectTriangles.Clear();
            SelectControlPoints.Clear();
        }

        public virtual void BeginEdit()
        {
            CurrentEdit = true;
        }

        public virtual void EndEdit()
        {
            CurrentEdit = false;
            OnUpdate();
        }

        /// <summary>
        /// 形状情報更新イベント
        /// </summary>
        private void OnUpdate()
        {
            AssemblyUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
