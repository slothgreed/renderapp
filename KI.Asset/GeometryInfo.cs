using System.Collections.Generic;
using KI.Gfx.GLUtil;
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
    /// 頂点情報
    /// </summary>
    public class GeometryInfo
    {
        /// <summary>
        /// 頂点リスト
        /// </summary>
        private List<Vector3> position = new List<Vector3>();

        /// <summary>
        /// 法線リスト
        /// </summary>
        private List<Vector3> normal = new List<Vector3>();

        /// <summary>
        /// 色リスト
        /// </summary>
        private List<Vector3> color = new List<Vector3>();

        /// <summary>
        /// テクスチャ座標リスト
        /// </summary>
        private List<Vector2> texcoord = new List<Vector2>();

        /// <summary>
        /// 頂点インデックスリスト
        /// </summary>
        private List<int> index = new List<int>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="nor">法線</param>
        /// <param name="col">色</param>
        /// <param name="tex">テクスチャ座標</param>
        /// <param name="idx">頂点Index</param>
        /// <param name="type">形状タイプ</param>
        public GeometryInfo(List<Vector3> pos, List<Vector3> nor, List<Vector3> col, List<Vector2> tex, List<int> idx, GeometryType type)
        {
            Update(pos, nor, col, tex, idx, type);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="nor">法線</param>
        /// <param name="col">色</param>
        /// <param name="tex">テクスチャ座標</param>
        /// <param name="idx">頂点Index</param>
        /// <param name="type">形状タイプ</param>
        public GeometryInfo(List<Vector3> pos, List<Vector3> nor, Vector3 col, List<Vector2> tex, List<int> idx, GeometryType type)
        {
            Update(pos, nor, col, tex, idx, type);
        }

        /// <summary>
        /// 形状種類
        /// </summary>
        public GeometryType GeometryType { get; set; }

        /// <summary>
        /// 頂点リスト
        /// </summary>
        public List<Vector3> Position
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// 法線リスト
        /// </summary>
        public List<Vector3> Normal
        {
            get
            {
                return normal;
            }
        }

        /// <summary>
        /// 色リスト
        /// </summary>
        public List<Vector3> Color
        {
            get
            {
                return color;
            }
        }

        /// <summary>
        /// テクスチャ座標リスト
        /// </summary>
        public List<Vector2> TexCoord
        {
            get
            {
                return texcoord;
            }
        }

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

        /// <summary>
        /// 三角形の数
        /// </summary>
        public int TriangleNum
        {
            get
            {
                if (Index.Count == 0)
                {
                    return Position.Count / 3;
                }
                else
                {
                    return Index.Count / 3;
                }
            }
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            position.Clear();
            color.Clear();
            normal.Clear();
            texcoord.Clear();
            index.Clear();
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
        public void Update(List<Vector3> pos, List<Vector3> nor, List<Vector3> col, List<Vector2> tex, List<int> idx, GeometryType type)
        {
            if (pos != null)
            {
                position = pos;
            }

            if (nor != null)
            {
                normal = nor;
            }

            if (col != null)
            {
                color = col;
            }

            if (tex != null)
            {
                texcoord = tex;
            }

            if (idx != null)
            {
                index = idx;
            }

            GeometryType = type;
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
        public void Update(List<Vector3> pos, List<Vector3> nor, Vector3 col, List<Vector2> tex, List<int> idx, GeometryType type)
        {
            if (pos != null)
            {
                position = pos;
            }

            if (nor != null)
            {
                normal = nor;
            }

            if (col != null)
            {
                color.Clear();
                for (int i = 0; i < Position.Count; i++)
                {
                    color.Add(col);
                }
            }

            if (tex != null)
            {
                texcoord = tex;
            }

            if (idx != null)
            {
                index = idx;
            }

            GeometryType = type;
        }

        #region [convert mesh]
        /// <summary>
        /// Triangle毎に変換
        /// </summary>
        protected void ConvertPerTriangle()
        {
            if (Index.Count == 0)
                return;

            if (Position.Count == 0)
                return;

            bool texArray = false;
            bool colorArray = false;
            bool normalArray = false;
            if (TexCoord.Count == Position.Count)
                texArray = true;

            if (Normal.Count == Position.Count)
                normalArray = true;

            if (Color.Count == Position.Count)
                colorArray = true;

            var newPosition = new List<Vector3>();
            var newTexcoord = new List<Vector2>();
            var newColor = new List<Vector3>();
            var newNormal = new List<Vector3>();

            for (int i = 0; i < Index.Count; i += 3)
            {
                newPosition.Add(Position[Index[i]]);
                newPosition.Add(Position[Index[i + 1]]);
                newPosition.Add(Position[Index[i + 2]]);
                if (texArray)
                {
                    newTexcoord.Add(TexCoord[Index[i]]);
                    newTexcoord.Add(TexCoord[Index[i + 1]]);
                    newTexcoord.Add(TexCoord[Index[i + 2]]);
                }

                if (colorArray)
                {
                    newColor.Add(Color[Index[i]]);
                    newColor.Add(Color[Index[i + 1]]);
                    newColor.Add(Color[Index[i + 2]]);
                }

                if (normalArray)
                {
                    newNormal.Add(Normal[Index[i]]);
                    newNormal.Add(Normal[Index[i + 1]]);
                    newNormal.Add(Normal[Index[i + 2]]);
                }
            }

            position = newPosition;
            normal = newNormal;
            texcoord = newTexcoord;
            color = newColor;
            index.Clear();
        }

        /// <summary>
        /// 頂点配列に変換
        /// </summary>
        public void ConvertVertexArray()
        {
            if (Index.Count != 0)
                return;

            if (Position.Count == 0)
                return;

            bool texArray = false;
            bool colorArray = false;
            bool normalArray = false;
            if (TexCoord.Count == Position.Count)
                texArray = true;

            if (Normal.Count == Position.Count)
                normalArray = true;

            if (Color.Count == Position.Count)
                colorArray = true;

            var newPosition = new List<Vector3>();
            var newTexcoord = new List<Vector2>();
            var newColor = new List<Vector3>();
            var newNormal = new List<Vector3>();
            bool isExist = false;
            for (int i = 0; i < Position.Count; i++)
            {
                isExist = false;
                for (int j = 0; j < newPosition.Count; j++)
                {
                    if (newPosition[j] == Position[i])
                    {
                        isExist = true;
                        index.Add(j);
                        break;
                    }
                }

                if (!isExist)
                {
                    newPosition.Add(Position[i]);
                    index.Add(newPosition.Count - 1);

                    if (texArray)
                    {
                        newTexcoord.Add(TexCoord[i]);
                    }

                    if (colorArray)
                    {
                        newColor.Add(Color[i]);
                    }

                    if (normalArray)
                    {
                        newNormal.Add(Normal[i]);
                    }
                }
            }

            position = newPosition;
            texcoord = newTexcoord;
            color = newColor;
            normal = newNormal;
        }
        #endregion
    }
}
