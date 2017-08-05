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
    public abstract class Geometry : KIObject
    {
        #region Propety

        private Vector3 translate = Vector3.Zero;

        private Vector3 scale = Vector3.One;

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
        /// 形状ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 形状情報
        /// </summary>
        public GeometryInfo GeometryInfo { get; protected set; }

        /// <summary>
        /// 可視不可視
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// ハーフエッジ
        /// </summary>
        public HalfEdge HalfEdge { get; set; }

        /// <summary>
        /// モデルマトリックス
        /// </summary>
        public Matrix4 ModelMatrix { get; set; }


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

        public Shader Shader { get; set; }

        public Dictionary<TextureKind, Texture> TextureItem { get; private set; }
        
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

        public void AddTexture(TextureKind kind, Texture texture)
        {
            TextureItem[kind] = texture;
        }

        public int TextureNum()
        {
            if (TextureItem == null)
            {
                return 0;
            }

            return TextureItem.Count;
        }

        public Texture GetTexture(TextureKind kind)
        {
            if (TextureItem.ContainsKey(kind))
            {
                return TextureItem[kind];
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
        /// <param name="matrix">matrix</param>
        public bool Transformation(Matrix4 matrix)
        {
            ModelMatrix = Matrix4.Identity;
            ModelMatrix *= matrix;
            return true;
        }
        #endregion

        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            ModelMatrix = Matrix4.Identity;
            TextureItem = new Dictionary<TextureKind, Texture>();
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
    }
}
