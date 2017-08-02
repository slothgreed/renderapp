using System;
using OpenTK;
using KI.Foundation.Core;

namespace KI.Asset
{
    /// <summary>
    /// カメラ
    /// </summary>
    public class Camera : KIFile
    {
        #region [property method]
        /// <summary>
        /// カメラからの距離
        /// </summary>
        private float ZoomLength;

        /// <summary>
        /// ズームイン倍率
        /// </summary>
        private float ZoomInRatio = 1.1f;

        /// <summary>
        /// ズームアウト倍率
        /// </summary>
        private float ZoomOutRatio = 0.9f;

        /// <summary>
        /// ズームの最小値
        /// </summary>
        private float ZoomMin = 1;

        /// <summary>
        /// ズームの最大値
        /// </summary>
        private float ZoomMax = 600;
        /// <summary>
        /// 注視点の平行移動量
        /// </summary>
        private Vector3 Pan = new Vector3(0.0f, 5.0f, 10.0f);

        /// <summary>
        /// 平行移動の最小値
        /// </summary>
        private Vector3 PanMin = new Vector3(-100, -100, -100);

        /// <summary>
        /// 平行移動の最大値
        /// </summary>
        private Vector3 PanMax = new Vector3(100, 100, 100);

        /// <summary>
        /// 平行移動量の倍率設定
        /// </summary>
        private float PanRatio = 10.0f;

        /// <summary>
        /// projectionのnear
        /// </summary>
        private float Near { get; set; } = 1;

        /// <summary>
        /// 回転倍率
        /// </summary>
        private float RotateRatio;

        /// <summary>
        /// 球面座標の角度
        /// </summary>
        private float phi;

        /// <summary>
        /// 球面座標の角度
        /// </summary>
        private float theta;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Camera(string name)
            : base(name)
        {
            InitCamera();
            SetProjMatrix(1.0f);
        }

        public Matrix4 ProjMatrix { get; set; }

        public Matrix4 Matrix { get; private set; }

        public Vector3 Position { get; private set; }

        public Vector3 Direction { get; private set; }

        public Vector3 LookAt { get; private set; }

        public Vector3 Up { get; private set; }

        public Matrix4 CameraProjMatrix
        {
            get
            {
                return Matrix * ProjMatrix;
            }
        }

        public Matrix4 UnProject
        {
            get
            {
                return Matrix * ProjMatrix;
            }
        }

        #endregion
        public float Far { get; set; } = 1000;

        #region [回転関連]

        /// <summary>
        /// カメラ回転のPHI
        /// </summary>
        public float Phi
        {
            get
            {
                return phi;
            }

            set
            {
                if (value >= 90.0f)
                {
                    phi = 89.9999f;
                }
                else if (value <= -90.0f)
                {
                    phi = -89.9999f;
                }
                else
                {
                    phi = value;
                }

            }
        }

        /// <summary>
        /// カメラ回転のTHETA
        /// </summary>
        public float Theta
        {
            get
            {
                return theta;
            }

            set
            {
                if (value >= 360.0f)
                {
                    theta = 0;
                }
                else if (value <= -360.0f)
                {
                    theta = 0;
                }
                else
                {
                    theta = value;
                }
            }
        }
        #endregion

        /// <summary>
        /// 初期のカメラ位置
        /// </summary>
        public void InitCamera()
        {
            Pan = new Vector3(0.0f, 22.5f, 0.0f);
            LookAt = Vector3.Zero;
            Up = Vector3.UnitY;
            ZoomLength = ZoomMax;
            Theta = 90.0f;
            Phi = 0.0f;
            UpdateCamera();
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
        }

        public void SetProjMatrix(float aspect)
        {
            ProjMatrix = Matrix4.CreatePerspectiveFieldOfView((float)System.Math.PI / 4.0f, aspect, Near, Far);
        }
        #region [カメラの平行移動]

        /// <summary>
        /// falseの時は+=、trueは=基本は＋＝
        /// </summary>
        /// <param name="move"></param>
        /// <param name="type">move分動かすのか,move位置に移動するのか</param>
        public void SetPan(Vector3 move, bool type = false)
        {
            if (type)
            {
                Pan = move;
            }
            else
            {
                Pan += move;
            }
            ////境界ないかチェック
            //if (m_Pan.X > m_PanMax.X)
            //{
            //    m_Pan.X = m_PanMax.X;
            //}
            //if (m_Pan.Y > m_PanMax.Y)
            //{
            //    m_Pan.Y = m_PanMax.Y;
            //}
            //if (m_Pan.Z > m_PanMax.Z)
            //{
            //    m_Pan.Z = m_PanMax.Z;
            //}

            //if (m_Pan.X < m_PanMin.X)
            //{
            //    m_Pan.X = m_PanMin.X;
            //}
            //if (m_Pan.Y < m_PanMin.Y)
            //{
            //    m_Pan.Y = m_PanMin.Y;
            //}
            //if (m_Pan.Z < m_PanMin.Z)
            //{
            //    m_Pan.Z = m_PanMin.Z;
            //}
            UpdateCamera();
        }

        public void Translate(float x, float y, float z)
        {
            Translate(new Vector3(x, y, z));
        }

        /// <summary>
        /// Viewの平行移動
        /// </summary>
        /// <param name="Middle"></param>
        public void Translate(Vector3 move)
        {
            Vector3 vectorX = new Vector3();
            Vector3 vectorY = new Vector3();
            Vector3 orient = new Vector3(move);
            orient.Normalize();
            if (move.Length != 0)
            {
                vectorX = new Vector3(Matrix.Column0);
                vectorY = new Vector3(Matrix.Column1);
                vectorX *= orient.X;
                vectorY *= orient.Y;
                SetPan((vectorX + vectorY) * PanRatio);
            }
        }
        #endregion
        #region [カメラの回転]
        public void Rotate(float x, float y, float z)
        {
            Rotate(new Vector3(x, y, z));
        }

        /// <summary>
        /// Viewの回転
        /// </summary>
        /// <param name="Right"></param>
        public void Rotate(Vector3 move)
        {
            Theta += move.X;
            Phi += move.Y;
            UpdateCamera();
        }
        #endregion
        #region [カメラの拡大縮小]
        /// <summary>
        /// Viewのズーム[詳:m_ZoomMin,m_ZoomMax,m_ZoomInRatio,m_ZoomOutRatio]
        /// </summary>
        /// <param name="Delta">正のときズームイン負のときズームアウト</param>
        public void Zoom(int Delta)
        {
            float ratio;
            //倍率決め
            if (Delta > 0)
            {
                ratio = ZoomInRatio;
            }
            else
            {
                ratio = ZoomOutRatio;
            }
            //拡大縮小
            ZoomLength *= ratio;
            //if (m_ZoomLength < m_ZoomMin)
            //{
            //    m_ZoomLength = m_ZoomMin;
            //}
            //else if (m_ZoomLength > m_ZoomMax)
            //{
            //    m_ZoomLength = m_ZoomMax;
            //}
            UpdateCamera();
        }
        #endregion

        /// <summary>
        /// カメラの設定
        /// </summary>
        private void UpdateCamera()
        {
            Vector3 q_Move = GetSphericalMove();
            Position = Pan + (q_Move * ZoomLength);
            LookAt = Pan;
            Matrix = Matrix4.LookAt(Position, LookAt, Up);
        }

        /// <summary>
        /// カメラの球面移動
        /// </summary>
        /// <returns></returns>
        private Vector3 GetSphericalMove()
        {
            Vector3 move = new Vector3();
            double theta = MathHelper.DegreesToRadians(Theta);
            double phi = MathHelper.DegreesToRadians(Phi);
            move.X = (float)(Math.Cos(theta) * Math.Cos(phi));
            move.Y = (float)(Math.Sin(phi));
            move.Z = (float)(Math.Sin(theta) * Math.Cos(phi));

            return move;
        }
    }
}
