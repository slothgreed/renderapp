using KI.Foundation.Core;
using KI.Mathmatics;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// シーンノード
    /// </summary>
    public abstract class SceneNode : KIObject
    {
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
        /// <param name="name">ノード名</param>
        public SceneNode(string name)
            : base(name)
        {
            ModelMatrix = Matrix4.Identity;
        }

        /// <summary>
        /// モデルマトリックス
        /// </summary>
        public Matrix4 ModelMatrix { get; set; } = Matrix4.Identity;

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
        /// 可視不可視
        /// </summary>
        public bool Visible { get; set; } = true;

        #region [modelmatrix]

        /// <summary>
        /// レンダリングコア
        /// </summary>
        /// <param name="scene">シーン</param>
        public abstract void RenderCore(Scene scene);

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public void Render(Scene scene)
        {
            if (Visible == true)
            {
                RenderCore(scene);
            }
        }

        /// <summary>
        /// X軸で回転
        /// </summary>
        /// <param name="angle">Degree</param>
        public virtual void RotateX(float angle)
        {
            Matrix4 rotate = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angle));
            SetModelViewRotateXYZ(rotate);
        }

        /// <summary>
        /// Y軸で回転
        /// </summary>
        /// <param name="angle">Degree</param>
        public virtual void RotateY(float angle)
        {
            Matrix4 rotate = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angle));
            SetModelViewRotateXYZ(rotate);
        }

        /// <summary>
        /// Z軸で回転
        /// </summary>
        /// <param name="angle">Degree</param>
        public virtual void RotateZ(float angle)
        {
            Matrix4 rotate = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
            SetModelViewRotateXYZ(rotate);
        }

        /// <summary>
        /// xyzの順で回転
        /// </summary>
        /// <param name="angleX">X角度</param>
        /// <param name="angleY">Y角度</param>
        /// <param name="angleZ">Z角度</param>
        public void RotateXYZ(float angleX, float angleY, float angleZ)
        {
            Matrix4 rotateX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angleX));
            Matrix4 rotateY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angleY));
            Matrix4 rotateZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angleZ));
            SetModelViewRotateXYZ(rotateX * rotateY * rotateZ);
        }

        /// <summary>
        /// vector1をvector2に回転させる。
        /// </summary>
        /// <param name="vector1">vector1</param>
        /// <param name="vector2">vector2</param>
        public void RotateQuaternion(Vector3 vector1, Vector3 vector2)
        {
            Vector3 exterior = Vector3.Cross(vector1, vector2);
            if (vector1.Z == -1.0f)
            {
                RotateY(180);
                return;
            }
            else if (vector1.Z == 1.0f)
            {
                RotateY(0);
                return;
            }

            if (exterior.Length != 0)
            {
                exterior.Normalize();
                float angle = Calculator.Radian(vector1, vector2, exterior);
                Matrix4 mat = Matrix4.CreateFromAxisAngle(exterior, angle);
                SetModelViewRotateXYZ(mat);
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
        /// <param name="quart">クオータニオン</param>
        private void SetModelViewRotateXYZ(Matrix4 quart)
        {
            //移動量を消して、回転後移動
            Vector3 translate = ModelMatrix.ExtractTranslation();
            ModelMatrix = ModelMatrix.ClearRotation();

            ModelMatrix = ModelMatrix.ClearTranslation();

            ModelMatrix *= quart;
            //基点分元に戻す
            ModelMatrix = ModelMatrix.ClearTranslation();
            ModelMatrix *= Matrix4.CreateTranslation(translate);
        }
    }
}
