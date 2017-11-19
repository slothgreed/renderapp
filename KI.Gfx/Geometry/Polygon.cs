using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using KI.Foundation.Parameter;
using KI.Gfx.KITexture;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.Geometry
{
    /// <summary>
    /// 頂点格納種類
    /// </summary>
    public enum VertexStoreType
    {
        None,       //入っていない
        Normal,     //普通に順番に入っている
        VertexArray //VertexArray状態で入っている
    }

    public enum VertexColor
    {
        Default,
        WireFrame,
        Voronoi,
        MeanCurvature,
        GaussCurvature,
        MinCurvature,
        MaxCurvature,
        TmpParameter,
    }


    /// <summary>
    /// 更新イベント
    /// </summary>
    public class UpdatePolygonEventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">更新した形状種類</param>
        public UpdatePolygonEventArgs(PrimitiveType type)
        {
            Type = type;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">更新した形状種類</param>
        /// <param name="type">更新した色情報</param>
        public UpdatePolygonEventArgs(PrimitiveType type, List<Vector3> color)
        {
            Type = type;
            Color = color;
        }

        /// <summary>
        /// 色情報
        /// </summary>
        public List<Vector3> Color { get; private set; }

        /// <summary>
        /// 更新した形状種類
        /// </summary>
        public PrimitiveType Type { get; private set; }
    }

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
        /// メッシュリスト
        /// </summary>
        private List<Mesh> meshs = new List<Mesh>();

        /// <summary>
        /// 頂点インデックスリスト
        /// </summary>
        private Dictionary<PrimitiveType, List<int>> index = new Dictionary<PrimitiveType, List<int>>();

        /// <summary>
        /// 頂点カラーリスト
        /// </summary>
        private Dictionary<VertexColor, List<Vector3>> vertexColor = new Dictionary<VertexColor, List<Vector3>>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public Polygon(string name)
           : base(name)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="vertex">頂点</param>
        public Polygon(string name, List<Vertex> vertex)
            : base(name)
        {
            vertexs = vertex;
            Type = PrimitiveType.Points;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="line">線分リスト</param>
        public Polygon(string name, List<Line> line)
            : base(name)
        {
            lines = line;
            Type = PrimitiveType.Lines;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="mesh">メッシュ</param>
        /// <param name="type">種類</param>
        public Polygon(string name, List<Mesh> mesh, PrimitiveType type)
            : base(name)
        {
            meshs = mesh;
            Type = type;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="vertex">頂点リスト</param>
        /// <param name="indexList">頂点バッファリスト</param>
        /// <param name="type">種類</param>
        public Polygon(string name, List<Vertex> vertex, List<int> indexList, PrimitiveType type)
            : base(name)
        {
            vertexs = vertex;
            index[type] = indexList;
            Type = type;
        }

        /// <summary>
        /// 形状情報更新イベント
        /// </summary>
        public EventHandler<UpdatePolygonEventArgs> PolygonUpdated { get; set; }

        /// <summary>
        /// 形状ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 形状種類
        /// </summary>
        public PrimitiveType Type { get; set; }

        /// <summary>
        /// 頂点インデックスリスト
        /// </summary>
        public Dictionary<PrimitiveType, List<int>> Index
        {
            get
            {
                return index;
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
        /// 面
        /// </summary>
        public List<Mesh> Meshs
        {
            get
            {
                return meshs;
            }

            protected set
            {
                meshs = value;
            }
        }

        /// <summary>
        /// テクスチャ
        /// </summary>
        public Dictionary<TextureKind, Texture> Textures { get; private set; } = new Dictionary<TextureKind, Texture>();

        /// <summary>
        /// 頂点バッファの更新
        /// </summary>
        /// <param name="type">形状タイプ</param>
        public virtual void UpdateVertexArray(PrimitiveType type)
        {
            if (type == PrimitiveType.Lines &&
                Index.ContainsKey(PrimitiveType.Lines))
            {
                Index[PrimitiveType.Lines].Clear();

                if (Lines != null)
                {
                    foreach (var line in Lines)
                    {
                        Index[PrimitiveType.Lines].Add(line.Start.Index);
                        Index[PrimitiveType.Lines].Add(line.End.Index);
                    }
                }
            }

            if (type == PrimitiveType.Triangles &&
                Index.ContainsKey(PrimitiveType.Triangles))
            {
                Index[PrimitiveType.Triangles].Clear();

                foreach (var mesh in Meshs)
                {
                    Index[PrimitiveType.Triangles].AddRange(mesh.Vertexs.Select(p => p.Index));
                }
            }

            OnUpdate(new UpdatePolygonEventArgs(PrimitiveType.Lines));
            OnUpdate(new UpdatePolygonEventArgs(PrimitiveType.Triangles));
        }


        /// <summary>
        /// 頂点インデックスの更新
        /// </summary>
        /// <param name="vert">頂点</param>
        /// <param name="idx">頂点Index</param>
        /// <param name="type">形状タイプ</param>
        public void UpdateIndexBuffer(List<Vertex> vert, List<int> idx, PrimitiveType type)
        {
            vertexs = vert;

            if (idx != null)
            {
                index[type] = idx;
            }

            Type = type;

            OnUpdate(new UpdatePolygonEventArgs(type));
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
        /// テクスチャの追加
        /// </summary>
        /// <param name="kind">種類</param>
        /// <param name="texture">テクスチャ</param>
        public void AddTexture(TextureKind kind, Texture texture)
        {
            Textures[kind] = texture;
        }

        /// <summary>
        /// テクスチャのゲッタ
        /// </summary>
        /// <param name="kind">種類</param>
        /// <returns>テクスチャ</returns>
        public Texture GetTexture(TextureKind kind)
        {
            if (Textures.ContainsKey(kind))
            {
                return Textures[kind];
            }
            else
            {
                return null;
            }
        }

        ///// <summary>
        ///// ワイヤフレームの作成
        ///// </summary>
        ///// <param name="color">ワイヤフレームの色</param>
        //public void CreateWireFrame(Vector3 color)
        //{
        //    List<int> lineIndex = new List<int>();
        //    List<Vector3> wireFrameColor = new List<Vector3>();
        //    foreach (var mesh in Meshs)
        //    {
        //        for (int j = 0; j < mesh.Vertexs.Count - 1; j++)
        //        {
        //            lineIndex.Add(mesh.Vertexs[j].Index);
        //            lineIndex.Add(mesh.Vertexs[j + 1].Index);
        //        }

        //        lineIndex.Add(mesh.Vertexs[mesh.Vertexs.Count - 1].Index);
        //        lineIndex.Add(mesh.Vertexs[0].Index);
        //        wireFrameColor.Add(color);
        //        wireFrameColor.Add(color);
        //    }

        //    Index[PrimitiveType.Lines] = lineIndex;
        //    vertexColor[VertexColor.WireFrame] = wireFrameColor;

        //    OnUpdate(new UpdatePolygonEventArgs(PrimitiveType.Lines, wireFrameColor));
        //}

        ///// <summary>
        ///// 法線の算出
        ///// </summary>
        //public void CalcNormal()
        //{
        //    Normal.Clear();

        //    switch (PrimitiveType)
        //    {
        //        case PrimitiveType.None:
        //        case PrimitiveType.Points:
        //        case PrimitiveType.Lines:
        //        case PrimitiveType.Mix:
        //            return;
        //        case PrimitiveType.Triangles:
        //            for (int i = 0; i < Position.Count; i += 3)
        //            {
        //                Vector3 normal = Vector3.Cross(Position[i + 2] - Position[i + 1], Position[i] - Position[i + 1]).Normalized();
        //                Normal.Add(normal);
        //                Normal.Add(normal);
        //                Normal.Add(normal);
        //            }

        //            break;
        //        case PrimitiveType.Quads:
        //            for (int i = 0; i < position.Count; i += 4)
        //            {
        //                Vector3 normal = Vector3.Cross(Position[i + 2] - Position[i + 1], Position[i] - Position[i + 1]).Normalized();
        //                Normal.Add(normal);
        //                Normal.Add(normal);
        //                Normal.Add(normal);
        //                Normal.Add(normal);
        //            }

        //            break;
        //        default:
        //            break;
        //    }
        //}

        /// <summary>
        /// 形状情報更新イベント
        /// </summary>
        /// <param name="type">形状種類</param>
        protected void OnUpdate(UpdatePolygonEventArgs e)
        {
            PolygonUpdated?.Invoke(this, e);
        }
    }
}