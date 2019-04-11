using System;
using System.Collections.Generic;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using OpenTK;

namespace CADApp.Model.Assembly
{
    /// <summary>
    /// スケッチ
    /// </summary>
    public class Sketch : KIObject
    {
        private List<Vertex> vertexs;

        private List<int> lineIndex;

        private List<int> triangleIndex;

        public bool CurrentEdit { get; private set; }

        /// <summary>
        /// 形状情報更新イベント
        /// </summary>
        public EventHandler SketchUpdated { get; set; }

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

        public Sketch(string name)
            : base(name)
        {
            vertexs = new List<Vertex>();
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

        public void ClearLineIndex()
        {
            if (CurrentEdit == false)
            {
                Logger.Log(Logger.LogLevel.Error, "Call Begin Edit.");
            }

            lineIndex.Clear();
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

        public void BeginEdit()
        {
            CurrentEdit = true;
        }

        public void EndEdit()
        {
            CurrentEdit = false;
            OnUpdate();
        }

        /// <summary>
        /// 形状情報更新イベント
        /// </summary>
        private void OnUpdate()
        {
            SketchUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
