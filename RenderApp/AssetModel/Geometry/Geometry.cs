using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using RenderApp.GLUtil;
using KI.Foundation.Utility;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.KIAsset;
using KI.Gfx.GLUtil;
using KI.Gfx.Analyzer;
using RenderApp.GLUtil.ShaderModel;
namespace RenderApp.AssetModel
{
    public abstract class Geometry : KIObject
    {
        #region Propety
        public int ID { get; private set; }
        public PrimitiveType RenderType { get; set; }
        public GeometryInfo GeometryInfo { get; set; }
        
        public List<int> Timer { get; protected set; }
        public Vector3 Min { get; protected set; }
        public Vector3 Max { get; protected set; }

        public Matrix4 ModelMatrix
        {
            get;
            set;
        }

        private Vector3 _translate = Vector3.Zero;
        public Vector3 Translate
        {
            get
            {
                return _translate;
            }
            set
            {
                _translate = value;
                CalcTranslate(_translate);
            }
        }
        private Vector3 _scale = Vector3.One;
        public Vector3 Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
                CalcScale(_scale);
            }
        }
        private Vector3 _rotate = Vector3.Zero;
        public Vector3 Rotate
        {
            get
            {
                return _rotate;
            }
            set
            {
                _rotate = value;
                RotateXYZ(_rotate.X, _rotate.Y, _rotate.Z);
            }
        }
        public Matrix3 NormalMatrix
        {
            get
            {
                Matrix4 norm = ModelMatrix.ClearScale();
                return new Matrix3(norm);
            }
        }
        private Shader _ShaderItem;
        public Shader Shader
        {
            get
            {
                return _ShaderItem;
            }
            set
            {
                _ShaderItem = value;
            }
        }

        public Dictionary<TextureKind, Texture> TextureItem
        {
            get;
            private set;
        }
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

        #endregion
        #region Initializer disposer
        public Geometry(string name, PrimitiveType renderType)
            : base(name)
        {
            Initialize(name, renderType);

        }
        public Geometry(string name)
            : base(name)
        {
            Initialize(name, PrimitiveType.Triangles);
        }

        private void Initialize(string name = null, PrimitiveType renderType = PrimitiveType.Triangles)
        {
            RenderType = renderType;
            GeometryInfo = new GeometryInfo();
            Timer = new List<int>();
            ModelMatrix = Matrix4.Identity;
            TextureItem = new Dictionary<TextureKind, Texture>();
            Shader = ShaderFactory.Instance.DefaultAnalyzeShader;
        }

        public override void Dispose()
        {
            GeometryInfo.Dispose();
            Timer.Clear();
            ModelMatrix = Matrix4.Identity;
            Translate = Vector3.Zero;
            Scale = Vector3.One;
            Rotate = Vector3.Zero;
        }
        #endregion
        #region render
        public virtual void Render()
        {
            Shader.InitializeState(this, TextureItem);
            Shader.BindBuffer(this.GeometryInfo);
            if (GeometryInfo.Index.Count == 0)
            {
                DeviceContext.Instance.DrawArrays(RenderType, 0, GeometryInfo.Position.Count);
            }
            else
            {
                DeviceContext.Instance.DrawElements(RenderType, GeometryInfo.Index.Count, DrawElementsType.UnsignedInt, 0);
            }
            Shader.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        #endregion
        #region [modelmatrix]
        #region [translate]
        /// <summary>
        ///　モデルビューに平行移動を適用
        /// </summary>
        /// <param name="move"></param>
        private void CalcTranslate(Vector3 move)
        {
            ModelMatrix = ModelMatrix.ClearTranslation();
            Matrix4 translate = Matrix4.CreateTranslation(move);
            ModelMatrix *= translate;
        }
        #endregion
        #region [scale]

        /// <summary>
        ///　モデルビューに拡大縮小を適用
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
        #endregion
        #region [rotate]
        /// <summary>
        ///　形状に回転を適用(初期の向きに対して)
        /// </summary>
        private bool SetModelViewRotateXYZ(Matrix4 quart, bool init)
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

            return true;
        }


        /// <summary>
        /// X軸で回転
        /// </summary>
        /// <param name="angle">Degree</param>
        /// <param name="init">初期形状に対してか否か</param>
        public virtual bool RotateX(float angle, bool init = false)
        {
            Matrix4 rotate = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angle));
            return SetModelViewRotateXYZ(rotate, init);
        }

        /// <summary>
        /// Y軸で回転
        /// </summary>
        /// <param name="angle">Degree</param>
        /// <param name="init">初期形状に対してか否か</param>
        public virtual bool RotateY(float angle, bool init = false)
        {
            Matrix4 rotate = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angle));
            return SetModelViewRotateXYZ(rotate, init);
        }
        /// <summary>
        /// Z軸で回転
        /// </summary>
        /// <param name="angle">Degree</param>
        /// <param name="init">初期形状に対してか否か</param>
        public virtual bool RotateZ(float angle, bool init = false)
        {
            Matrix4 rotate = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
            return SetModelViewRotateXYZ(rotate, init);
        }
        public bool RotateXYZ(float angleX, float angleY, float angleZ, bool init = false)
        {
            Matrix4 rotateX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angleX));
            Matrix4 rotateY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angleY));
            Matrix4 rotateZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angleZ));
            return SetModelViewRotateXYZ(rotateX * rotateY * rotateZ, init);
        }
        /// <summary>
        /// vector1をvector2に回転させる。
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <param name="init">初期形状に対してか否か</param>
        public virtual bool RotateQuaternion(Vector3 vector1, Vector3 vector2, bool init = false)
        {
            Vector3 Ex = Vector3.Cross(vector1, vector2);
            if (vector1.Z == -1.0f)
            {
                RotateY(180, init);
                return true;
            }
            else if (vector1.Z == 1.0f)
            {
                RotateY(0, init);
                return true;
            }
            if (Ex.Length != 0)
            {
                Ex.Normalize();
                float angle = KICalc.Angle(vector1, vector2, Ex);
                Matrix4 mat = Matrix4.CreateFromAxisAngle(Ex, angle);
                return SetModelViewRotateXYZ(mat, init);
            }
            return false;
        }
        #endregion
        #region [Transformation]
        /// <summary>
        /// 初期形状から一括変換する用
        /// </summary>
        public bool Transformation(Matrix4 matrix)
        {
            ModelMatrix = Matrix4.Identity;
            ModelMatrix *= matrix;
            return true;
        }
        #endregion
        #endregion

    }
}
