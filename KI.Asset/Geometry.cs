using System;
using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Foundation.Core;
using KI.Foundation.KIMath;
using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;
using OpenTK;

namespace KI.Asset
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

    /// <summary>
    /// 形状
    /// </summary>
    public class Geometry : KIObject
    {
        #region Propety

        /// <summary>
        /// 形状情報更新イベント
        /// </summary>
        public EventHandler GeometryUpdate { get; set; }

        /// <summary>
        /// 頂点リスト
        /// </summary>
        private List<Vertex> vertexs = new List<Vertex>();

        /// <summary>
        /// 頂点インデックスリスト
        /// </summary>
        private List<int> index = new List<int>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="pos">頂点</param>
        /// <param name="nor">法線</param>
        /// <param name="col">色</param>
        /// <param name="tex">テクスチャ座標</param>
        /// <param name="idx">頂点Index</param>
        /// <param name="type">形状タイプ</param>
        public Geometry(string name, List<Vector3> pos, List<Vector3> nor, List<Vector3> col, List<Vector2> tex, List<int> idx, GeometryType type)
            : base(name)
        {
            Vector3 normal = Vector3.Zero;
            Vector3 color = Vector3.Zero;
            Vector2 texcoord = Vector2.Zero;
            for (int i = 0; i < pos.Count; i++)
            {
                if (nor != null)
                {
                    normal = nor[i];
                }
                if (col != null)
                {
                    color = col[i];
                }
                if (tex != null)
                {
                    texcoord = tex[i];
                }

                vertexs.Add(new Vertex(pos[i], normal, color, texcoord, i));
            }

            Update(vertexs, idx, type);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="pos">頂点</param>
        /// <param name="nor">法線</param>
        /// <param name="col">色</param>
        /// <param name="tex">テクスチャ座標</param>
        /// <param name="idx">頂点Index</param>
        /// <param name="type">形状タイプ</param>
        public Geometry(string name, List<Vector3> pos, List<Vector3> nor, Vector3 col, List<Vector2> tex, List<int> idx, GeometryType type)
            : base(name)
        {
            Vector3 normal = Vector3.Zero;
            Vector3 color = Vector3.Zero;
            Vector2 texcoord = Vector2.Zero;
            for (int i = 0; i < pos.Count; i++)
            {
                if (nor != null)
                {
                    normal = nor[i];
                }

                if (tex != null)
                {
                    texcoord = tex[i];
                }

                vertexs.Add(new Vertex(pos[i], normal, col, texcoord, i));
            }

            Update(vertexs, idx, type);
        }

        /// <summary>
        /// 形状ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// ハーフエッジ
        /// </summary>
        public HalfEdgeDS HalfEdgeDS { get; set; }

        /// <summary>
        /// 形状種類
        /// </summary>
        public GeometryType GeometryType { get; set; }

        /// <summary>
        /// 頂点インデックスリスト
        /// </summary>
        public List<int> Index
        {
            get
            {
                return index;
            }
        }

        public List<Vertex> Vertexs
        {
            get
            {
                return vertexs;
            }
        }


        /// <summary>
        /// テクスチャ
        /// </summary>
        public Dictionary<TextureKind, Texture> Textures { get; private set; } = new Dictionary<TextureKind, Texture>();

        /// <summary>
        /// テクスチャ枚数
        /// </summary>
        public int TextureNum
        {
            get
            {
                return Textures.Count;
            }
        }

        public void UpdateHalfEdge()
        {
            if (HalfEdgeDS == null)
            {
                return;
            }

            index.Clear();

            vertexs = HalfEdgeDS.Vertexs.OfType<Vertex>().ToList();

            foreach (var mesh in HalfEdgeDS.Meshs)
            {
                index.AddRange(mesh.AroundVertex.Select(p => p.Index));
            }

            OnUpdate();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="nor">法線</param>
        /// <param name="col">色</param>
        /// <param name="tex">テクスチャ座標</param>
        /// <param name="idx">頂点Index</param>
        /// <param name="type">形状タイプ</param>
        public void Update(List<Vertex> vert, List<int> idx, GeometryType type)
        {
            vertexs = vert;

            if (idx != null)
            {
                index = idx;
            }

            GeometryType = type;

            OnUpdate();
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
        ///// 法線の算出
        ///// </summary>
        //public void CalcNormal()
        //{
        //    Normal.Clear();

        //    switch (GeometryType)
        //    {
        //        case GeometryType.None:
        //        case GeometryType.Point:
        //        case GeometryType.Line:
        //        case GeometryType.Mix:
        //            return;
        //        case GeometryType.Triangle:
        //            for (int i = 0; i < Position.Count; i += 3)
        //            {
        //                Vector3 normal = Vector3.Cross(Position[i + 2] - Position[i + 1], Position[i] - Position[i + 1]).Normalized();
        //                Normal.Add(normal);
        //                Normal.Add(normal);
        //                Normal.Add(normal);
        //            }

        //            break;
        //        case GeometryType.Quad:
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
        private void OnUpdate()
        {
            GeometryUpdate?.Invoke(this, EventArgs.Empty);
        }
    }
}
