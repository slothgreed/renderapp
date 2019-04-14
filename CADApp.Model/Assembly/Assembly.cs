using System;
using System.Collections.Generic;
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

        private List<int> lineIndex;

        private List<int> triangleIndex;

        public bool CurrentEdit { get; private set; }

        List<Vertex> controlPoint;

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

        public List<int> LineIndex
        {
            get
            {
                return lineIndex;
            }
        }

        public List<int> TriangleIndex
        {
            get
            {
                return triangleIndex;
            }
        }

        public List<Vertex> ControlPoint
        {
            get
            {
                return controlPoint;
            }
        }

        public Assembly(string name)
            : base(name)
        {
            vertexs = new List<Vertex>();
            lineIndex = new List<int>();
            triangleIndex = new List<int>();
            controlPoint = new List<Vertex>();
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

        public void SetLineIndex(List<int> index)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            lineIndex = index;
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

        public void SetTriangleIndex(List<int> index)
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            triangleIndex = index;
        }

        public void ClearTriangleIndex()
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            triangleIndex.Clear();
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
