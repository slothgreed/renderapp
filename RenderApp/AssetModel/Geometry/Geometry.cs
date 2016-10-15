using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using RenderApp.AssetModel;
using RenderApp.GLUtil;
using RenderApp.Utility;
namespace RenderApp.AssetModel
{
    public abstract class Geometry : Asset
    {
        #region [static property]
        private static int GeometryIDCounter = 0;
        #endregion
        #region Propety
        public int ID { get; private set; }
        public PrimitiveType RenderType { get; set; }
        public List<Vector3> Position { get; protected set; }
        public List<Vector3> Normal { get; protected set; }
        public List<Vector3> Color { get; protected set; }
        public List<Vector2> TexCoord { get; protected set; }
        public List<int> Timer { get; protected set; }
        public List<int> Index { get; protected set; }
        public Vector3 Min { get; protected set; }
        public Vector3 Max { get; protected set; }
        public Matrix4 ModelMatrix
        {
            get;
            private set;
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
        public Material MaterialItem
        {
            get;
            set;
        }

        #endregion
        #region Initializer disposer
        private void Initialize(string name = null, PrimitiveType renderType = PrimitiveType.Triangles)
        {
            RenderType = renderType;
            Position = new List<Vector3>();
            Normal = new List<Vector3>();
            Color = new List<Vector3>();
            TexCoord = new List<Vector2>();
            Index = new List<int>();
            Timer = new List<int>();
            ModelMatrix = Matrix4.Identity;
            MaterialItem = Material.Default;
            if(GeometryIDCounter > 255)
            {
                Output.Error("ToManyObject");
            }
            GeometryIDCounter++;
            ID = GeometryIDCounter;
        }


        public Geometry(string name, PrimitiveType renderType = PrimitiveType.Triangles)
            : base(name)
        {
            Initialize(name, renderType);
        }
        public Geometry(string name)
            : base(name)
        {
            Initialize(name, PrimitiveType.Triangles);
        }
        public override void Dispose()
        {

        }
        #endregion
        #region calculator

        #endregion

        #region render
        public virtual void Render()
        {
            MaterialItem.InitializeState(this);
            MaterialItem.BindShader(this);
            if (Index.Count == 0)
            {
                GL.DrawArrays(RenderType, 0, Position.Count);
            }
            else
            {
                GL.DrawElements(RenderType, Index.Count, DrawElementsType.UnsignedInt, 0);
            }
            MaterialItem.UnBindShader();
            Output.GLError();
        }

        #endregion

        #region [modelmatrix]
        //基本的に各関数では、変換処理ができない場合形状位置を変えないようにしています。
        //段階を踏んで変換を行いたい場合は、各変換行列を生成し、掛け合わせた値を
        //Transformation()関数で変換しましょう
        //なお、Transformation関数でも、変換できない場合は、変換しません。
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
                float angle = -CCalc.Angle(vector1, vector2, Ex);
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


            for(int i = 0; i < Index.Count; i+=3)
            {
                newPosition.Add(Position[Index[i]]);
                newPosition.Add(Position[Index[i + 1]]);
                newPosition.Add(Position[Index[i + 2]]);
                if(texArray)
                {
                    newTexcoord.Add(TexCoord[Index[i]]);
                    newTexcoord.Add(TexCoord[Index[i + 1]]);
                    newTexcoord.Add(TexCoord[Index[i + 2]]);
                }
                if(colorArray)
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

            Position = newPosition;
            Normal = newNormal;
            TexCoord = newTexcoord;
            Color = newColor;
            Index.Clear();
        }
        /// <summary>
        /// 頂点配列に変換
        /// </summary>
        protected void ConvertVertexArray()
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
            for(int i = 0; i< Position.Count;i++)
            {
                isExist = false;
                for (int j = 0; j < newPosition.Count; j++ )
                {
                    if (newPosition[j] == Position[i])
                    {
                        isExist = true;
                        Index.Add(j);
                        break;
                    }
                }
                if (!isExist)
                {
                    newPosition.Add(Position[i]);
                    Index.Add(newPosition.Count -1 );


                    if(texArray)
                    {
                        newTexcoord.Add(TexCoord[i]);
                    }
                    if(colorArray)
                    {
                        newColor.Add(Color[i]);
                    }
                    if(normalArray)
                    {
                        newNormal.Add(Normal[i]);
                    }
                }
            }
            Position = newPosition;
            TexCoord = newTexcoord;
            Color = newColor;
            Normal = newNormal;

        }
    }
}
