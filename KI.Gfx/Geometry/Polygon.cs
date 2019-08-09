using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;

namespace KI.Gfx.Geometry
{
    /// <summary>
    /// 形状
    /// </summary>
    public class Polygon : KIObject
    {
        #region Propety

        /// <summary>
        /// 頂点リスト
        /// </summary>
        private List<Vertex> vertexs = new List<Vertex>();

        /// <summary>
        /// ワイヤフレーム
        /// </summary>
        private List<Line> lines = new List<Line>();

        /// <summary>
        /// 頂点インデックスリスト
        /// </summary>
        private List<int> index = new List<int>();

        /// <summary>
        /// マテリアル
        /// </summary>
        public Material Material { get; set; } = new Material();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public Polygon(string name, KIPrimitiveType type = KIPrimitiveType.Points)
           : base(name)
        {
            Type = type;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="vertex">頂点</param>
        public Polygon(string name, List<Vertex> vertex, KIPrimitiveType type = KIPrimitiveType.Points)
            : base(name)
        {
            vertexs = vertex;
            Type = type;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="line">線分リスト</param>
        public Polygon(string name, List<Line> line, KIPrimitiveType type = KIPrimitiveType.Lines)
            : base(name)
        {
            lines = line;
            Type = type;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="vertex">頂点リスト</param>
        /// <param name="indexList">頂点バッファリスト</param>
        /// <param name="type">種類</param>
        public Polygon(string name, List<Vertex> vertex, List<int> indexList, KIPrimitiveType type)
            : base(name)
        {
            vertexs = vertex;
            index = indexList;
            Type = type;
        }

        /// <summary>
        /// 形状情報更新イベント
        /// </summary>
        public EventHandler PolygonUpdated { get; set; }

        /// <summary>
        /// 形状種類
        /// </summary>
        public KIPrimitiveType Type { get; set; }

        /// <summary>
        /// 頂点インデックスリスト
        /// </summary>
        public List<int> Index
        {
            get
            {
                return index;
            }
            
            set
            {
                index = value;
            }
        }

        /// <summary>
        /// 頂点リスト
        /// </summary>
        public virtual List<Vertex> Vertexs
        {
            get
            {
                return vertexs;
            }

            protected set
            {
                vertexs = value;
            }
        }

        /// <summary>
        /// 線分
        /// </summary>
        public List<Line> Lines
        {
            get
            {
                return lines;
            }

            protected set
            {
                lines = value;
            }
        }

        /// <summary>
        /// 頂点バッファの更新
        /// </summary>
        /// <param name="type">形状タイプ</param>
        public virtual void UpdateVertexArray()
        {
            if (Type == KIPrimitiveType.Points &&
                Index.Count != 0)
            {
                Index.Clear();

                if (Vertexs != null)
                {
                    foreach (var vertex in Vertexs)
                    {
                        Index.Add(vertex.Index);
                    }
                }
            }

            if (Type == KIPrimitiveType.Lines &&
                Index.Count != 0)
            {
                Index.Clear();

                if (Lines != null)
                {
                    foreach (var line in Lines)
                    {
                        Index.Add(line.Start.Index);
                        Index.Add(line.End.Index);
                    }
                }

                OnUpdate();
            }
        }

        #region [convert mesh]
        ///// <summary>
        ///// Triangle毎に変換
        ///// </summary>
        //public void ConvertPerTriangle()
        //{
        //    if (Index.Count == 0)
        //        return;

        //    if (Position.Count == 0)
        //        return;

        //    bool texArray = false;
        //    bool colorArray = false;
        //    bool normalArray = false;
        //    if (TexCoord.Count == Position.Count)
        //        texArray = true;

        //    if (Normal.Count == Position.Count)
        //        normalArray = true;

        //    if (Color.Count == Position.Count)
        //        colorArray = true;

        //    var newPosition = new List<Vector3>();
        //    var newTexcoord = new List<Vector2>();
        //    var newColor = new List<Vector3>();
        //    var newNormal = new List<Vector3>();

        //    for (int i = 0; i < Index.Count; i += 3)
        //    {
        //        newPosition.Add(Position[Index[i]]);
        //        newPosition.Add(Position[Index[i + 1]]);
        //        newPosition.Add(Position[Index[i + 2]]);
        //        if (texArray)
        //        {
        //            newTexcoord.Add(TexCoord[Index[i]]);
        //            newTexcoord.Add(TexCoord[Index[i + 1]]);
        //            newTexcoord.Add(TexCoord[Index[i + 2]]);
        //        }

        //        if (colorArray)
        //        {
        //            newColor.Add(Color[Index[i]]);
        //            newColor.Add(Color[Index[i + 1]]);
        //            newColor.Add(Color[Index[i + 2]]);
        //        }

        //        if (normalArray)
        //        {
        //            newNormal.Add(Normal[Index[i]]);
        //            newNormal.Add(Normal[Index[i + 1]]);
        //            newNormal.Add(Normal[Index[i + 2]]);
        //        }
        //    }

        //    position = newPosition;
        //    normal = newNormal;
        //    texcoord = newTexcoord;
        //    color = newColor;
        //    index.Clear();
        //}

        ///// <summary>
        ///// 頂点配列に変換
        ///// </summary>
        //public void ConvertVertexArray()
        //{
        //    if (Index.Count != 0)
        //        return;

        //    if (Position.Count == 0)
        //        return;

        //    bool texArray = false;
        //    bool colorArray = false;
        //    bool normalArray = false;
        //    if (TexCoord.Count == Position.Count)
        //        texArray = true;

        //    if (Normal.Count == Position.Count)
        //        normalArray = true;

        //    if (Color.Count == Position.Count)
        //        colorArray = true;

        //    var newPosition = new List<Vector3>();
        //    var newTexcoord = new List<Vector2>();
        //    var newColor = new List<Vector3>();
        //    var newNormal = new List<Vector3>();
        //    bool isExist = false;
        //    for (int i = 0; i < Position.Count; i++)
        //    {
        //        isExist = false;
        //        for (int j = 0; j < newPosition.Count; j++)
        //        {
        //            if (newPosition[j] == Position[i])
        //            {
        //                isExist = true;
        //                index.Add(j);
        //                break;
        //            }
        //        }

        //        if (!isExist)
        //        {
        //            newPosition.Add(Position[i]);
        //            index.Add(newPosition.Count - 1);

        //            if (texArray)
        //            {
        //                newTexcoord.Add(TexCoord[i]);
        //            }

        //            if (colorArray)
        //            {
        //                newColor.Add(Color[i]);
        //            }

        //            if (normalArray)
        //            {
        //                newNormal.Add(Normal[i]);
        //            }
        //        }
        //    }

        //    position = newPosition;
        //    texcoord = newTexcoord;
        //    color = newColor;
        //    normal = newNormal;
        //}
        #endregion

        #endregion

        /// <summary>
        /// 形状情報更新イベント
        /// </summary>
        private void OnUpdate()
        {
            PolygonUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}