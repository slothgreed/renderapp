using System.Collections.Generic;
using KI.Analyzer;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.GLUtil;
using KI.Gfx.KIShader;
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
        /// 平行移動
        /// </summary>
        private Vector3 translate = Vector3.Zero;

        /// <summary>
        /// スケール
        /// </summary>
        private Vector3 scale = Vector3.One;

        /// <summary>
        /// 回転
        /// </summary>
        private Vector3 rotate = Vector3.Zero;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Geometry()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public Geometry(string name)
            : base(name)
        {
            Initialize();
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
        public Geometry(List<Vector3> pos, List<Vector3> nor, List<Vector3> col, List<Vector2> tex, List<int> idx, GeometryType type)
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
        public Geometry(List<Vector3> pos, List<Vector3> nor, Vector3 col, List<Vector2> tex, List<int> idx, GeometryType type)
        {
            Update(pos, nor, col, tex, idx, type);
        }

        /// <summary>
        /// 形状ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Geometry GeometryInfo { get; protected set; }

        /// <summary>
        /// ハーフエッジ
        /// </summary>
        public HalfEdge HalfEdge { get; set; }

        /// <summary>
        /// モデルマトリックス
        /// </summary>
        public Matrix4 ModelMatrix { get; set; }

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
        /// 移動情報
        /// </summary>
        public Vector3 Translate
        {
            get
            {
                return translate;
            }

            set
            {
                translate = value;
                CalcTranslate(translate);
            }
        }

        /// <summary>
        /// スケール情報
        /// </summary>
        public Vector3 Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
                CalcScale(scale);
            }
        }

        /// <summary>
        /// 回転情報
        /// </summary>
        public Vector3 Rotate
        {
            get
            {
                return rotate;
            }

            set
            {
                rotate = value;
                RotateXYZ(rotate.X, rotate.Y, rotate.Z);
            }
        }

        /// <summary>
        /// 法線行列
        /// </summary>
        public Matrix3 NormalMatrix
        {
            get
            {
                Matrix4 norm = ModelMatrix.ClearScale();
                return new Matrix3(norm);
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
        public void ConvertPerTriangle()
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

        #endregion

        #region [Initializer disposer]

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            GeometryInfo.Dispose();
            ModelMatrix = Matrix4.Identity;
            Translate = Vector3.Zero;
            Scale = Vector3.One;
            Rotate = Vector3.Zero;
        }
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

        #region [modelmatrix]

        /// <summary>
        /// X軸で回転
        /// </summary>
        /// <param name="angle">Degree</param>
        /// <param name="init">初期形状に対してか否か</param>
        public virtual void RotateX(float angle, bool init = false)
        {
            Matrix4 rotate = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angle));
            SetModelViewRotateXYZ(rotate, init);
        }

        /// <summary>
        /// Y軸で回転
        /// </summary>
        /// <param name="angle">Degree</param>
        /// <param name="init">初期形状に対してか否か</param>
        public virtual void RotateY(float angle, bool init = false)
        {
            Matrix4 rotate = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angle));
            SetModelViewRotateXYZ(rotate, init);
        }

        /// <summary>
        /// Z軸で回転
        /// </summary>
        /// <param name="angle">Degree</param>
        /// <param name="init">初期形状に対してか否か</param>
        public virtual void RotateZ(float angle, bool init = false)
        {
            Matrix4 rotate = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
            SetModelViewRotateXYZ(rotate, init);
        }

        /// <summary>
        /// xyzの順で回転
        /// </summary>
        /// <param name="angleX">X角度</param>
        /// <param name="angleY">Y角度</param>
        /// <param name="angleZ">Z角度</param>
        /// <param name="init">初期角度に対してか</param>
        public void RotateXYZ(float angleX, float angleY, float angleZ, bool init = false)
        {
            Matrix4 rotateX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angleX));
            Matrix4 rotateY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angleY));
            Matrix4 rotateZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angleZ));
            SetModelViewRotateXYZ(rotateX * rotateY * rotateZ, init);
        }

        /// <summary>
        /// vector1をvector2に回転させる。
        /// </summary>
        /// <param name="vector1">vector1</param>
        /// <param name="vector2">vector2</param>
        /// <param name="init">初期形状に対してか否か</param>
        public void RotateQuaternion(Vector3 vector1, Vector3 vector2, bool init = false)
        {
            Vector3 exterior = Vector3.Cross(vector1, vector2);
            if (vector1.Z == -1.0f)
            {
                RotateY(180, init);
                return;
            }
            else if (vector1.Z == 1.0f)
            {
                RotateY(0, init);
                return;
            }

            if (exterior.Length != 0)
            {
                exterior.Normalize();
                float angle = KICalc.Angle(vector1, vector2, exterior);
                Matrix4 mat = Matrix4.CreateFromAxisAngle(exterior, angle);
                SetModelViewRotateXYZ(mat, init);
                return;
            }
        }

        /// <summary>
        /// 初期形状から一括変換する用
        /// </summary>
        /// <param name="matrix">適用行列</param>
        public void Transformation(Matrix4 matrix)
        {
            ModelMatrix = Matrix4.Identity;
            ModelMatrix *= matrix;
        }
        #endregion

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            ModelMatrix = Matrix4.Identity;
            Textures = new Dictionary<TextureKind, Texture>();
            //Shader = ShaderCreater.Instance.DefaultShader;
        }

        /// <summary>
        /// モデルビューに平行移動を適用
        /// </summary>
        /// <param name="move">移動量</param>
        private void CalcTranslate(Vector3 move)
        {
            ModelMatrix = ModelMatrix.ClearTranslation();
            Matrix4 translate = Matrix4.CreateTranslation(move);
            ModelMatrix *= translate;
        }

        /// <summary>
        /// モデルビューに拡大縮小を適用
        /// </summary>
        /// <param name="scale">スケール</param>
        private void CalcScale(Vector3 scale)
        {
            ModelMatrix = ModelMatrix.ClearScale();
            Vector3 translate = ModelMatrix.ExtractTranslation();
            Quaternion quart = ModelMatrix.ExtractRotation();
            ModelMatrix = Matrix4.Identity;

            ModelMatrix *= Matrix4.CreateScale(scale);
            Matrix4 quartMat = Matrix4.CreateFromQuaternion(quart);
            ModelMatrix *= quartMat;
            ModelMatrix = ModelMatrix.ClearTranslation();
            ModelMatrix *= Matrix4.CreateTranslation(translate);
        }

        /// <summary>
        /// 形状に回転を適用(初期の向きに対して)
        /// </summary>
        private void SetModelViewRotateXYZ(Matrix4 quart, bool init)
        {
            //移動量を消して、回転後移動
            Vector3 translate = ModelMatrix.ExtractTranslation();
            //初期形状に対しての場合は、回転量も削除
            if (init)
            {
                ModelMatrix = ModelMatrix.ClearRotation();
            }

            ModelMatrix = ModelMatrix.ClearTranslation();

            ModelMatrix *= quart;
            //基点分元に戻す
            ModelMatrix = ModelMatrix.ClearTranslation();
            ModelMatrix *= Matrix4.CreateTranslation(translate);
        }

        /// <summary>
        /// 法線の算出
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="type">種類</param>
        private void CalcNormal(List<Vector3> position, GeometryType type)
        {
            switch (type)
            {
                case GeometryType.None:
                case GeometryType.Point:
                case GeometryType.Line:
                case GeometryType.Mix:
                    return;
                case GeometryType.Triangle:
                    for (int i = 0; i < position.Count; i += 3)
                    {
                        Vector3 normal = Vector3.Cross(position[i + 2] - position[i + 1], position[i] - position[i + 1]).Normalized();
                        GeometryInfo.Normal.Add(normal);
                        GeometryInfo.Normal.Add(normal);
                        GeometryInfo.Normal.Add(normal);
                    }

                    break;
                case GeometryType.Quad:
                    for (int i = 0; i < position.Count; i += 4)
                    {
                        Vector3 normal = Vector3.Cross(position[i + 2] - position[i + 1], position[i] - position[i + 1]).Normalized();
                        GeometryInfo.Normal.Add(normal);
                        GeometryInfo.Normal.Add(normal);
                        GeometryInfo.Normal.Add(normal);
                        GeometryInfo.Normal.Add(normal);
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
