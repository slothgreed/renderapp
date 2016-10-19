using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using RenderApp.AssetModel;
namespace RenderApp.AssetModel
{
    /// <summary>
    /// カメラ
    /// </summary>
    public class Camera : Asset
    {
        #region [property method]
        public Matrix4 ProjMatrix { get; set; }
        public Matrix4 Matrix { get;private set; }
        public Vector3 Position { get;private set; }
        public Vector3 Direction { get;private set; }
        public Vector3 LookAt { get;private set; }
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
        public void SetProjMatrix(float aspect)
        {
            ProjMatrix = Matrix4.CreatePerspectiveFieldOfView((float)System.Math.PI / 4.0f, aspect, Near, Far);
        }

        #endregion
        #region [Zoom関連]
        /// <summary>
        /// カメラからの距離
        /// </summary>
        private float m_ZoomLength;
        /// <summary>
        /// ズームイン倍率
        /// </summary>
        public float m_ZoomInRatio = 1.1f;
        /// <summary>
        /// ズームアウト倍率
        /// </summary>
        public float m_ZoomOutRatio = 0.9f;
        /// <summary>
        /// ズームの最小値
        /// </summary>
        public float m_ZoomMin = 1;
        /// <summary>
        /// ズームの最大値
        /// </summary>
        public float m_ZoomMax = 150;
        #endregion
        #region [平行移動関連]
        /// <summary>
        /// 注視点の平行移動量
        /// </summary>
        private Vector3 m_Pan = new Vector3(0.0f, 5.0f, 0.0f);
        /// <summary>
        /// 平行移動の最小値
        /// </summary>
        public Vector3 m_PanMin = new Vector3(-100, -100, -100);
        /// <summary>
        /// 平行移動の最大値
        /// </summary>
        public Vector3 m_PanMax = new Vector3(100, 100, 100);
        /// <summary>
        /// 平行移動量の倍率設定
        /// </summary>
        public float m_PanRatio = 1.0f;
        /// <summary>
        /// projectionのnear
        /// </summary>
        public float _near = 1;
        public float Near
        {
            get
            {
                return _near;
            }
            private set
            {
                _near = value;
            }
        }
        /// <summary>
        /// projectionのfar
        /// </summary>
        private float _far = 300;
        public float Far
        {
            get
            {
                return _far;
            }
            private set
            {
                _far = value;
            }
        }
        #endregion
        #region [回転関連]
        /// <summary>
        /// 回転倍率
        /// </summary>
        public float m_RotateRatio;
        /// <summary>
        /// 球面座標の角度
        /// </summary>
        private float PHI;
        /// <summary>
        /// 球面座標の角度
        /// </summary>
        private float THETA;
        /// <summary>
        /// カメラ回転のPHI
        /// </summary>
        public float m_phi
        {
            get
            {
                return PHI;
            }
            set
            {
                if (value >= 90.0f)
                {
                    PHI = 89.9999f;
                }
                else if (value <= -90.0f)
                {
                    PHI = -89.9999f;
                }
                else
                {
                    PHI = value;
                }

            }
        }
        /// <summary>
        /// カメラ回転のTHETA
        /// </summary>
        public float m_theta
        {
            get
            {
                return THETA;
            }
            set
            {
                if (value >= 360.0f)
                {
                    THETA = 0;
                }
                else if (value <= -360.0f)
                {
                    THETA = 0;
                }
                else
                {
                    THETA = value;
                }
            }
        }
        #endregion
        #region [コンストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Camera(string name)
            :base(name)
        {
            InitCamera();
            SetProjMatrix(1.0f);
        }
        public override void Dispose()
        {

        }
        /// <summary>
        /// 初期のカメラ位置
        /// </summary>
        public void InitCamera()
        {
            m_Pan = new Vector3(0.0f, 22.5f, 0.0f);
            LookAt = Vector3.Zero;
            Up = Vector3.UnitY;
            m_ZoomLength = m_ZoomMax;
            m_theta = 90.0f;
            m_phi = 0.0f;
            UpdateCamera();
        }
        #endregion
        #region [カメラの球面移動]
        private Vector3 GetSphericalMove()
        {
            Vector3 move = new Vector3();
            double theta = MathHelper.DegreesToRadians(m_theta);
            double phi = MathHelper.DegreesToRadians(m_phi);
            move.X = (float)(Math.Cos(theta) * Math.Cos(phi));
            move.Y = (float)(Math.Sin(phi));
            move.Z = (float)(Math.Sin(theta) * Math.Cos(phi));

            return move;
        }
        /// <summary>
        /// カメラの設定
        /// </summary>
        private void UpdateCamera()
        {
            Vector3 q_Move = GetSphericalMove();
            Position = m_Pan + (q_Move * m_ZoomLength);
            LookAt = m_Pan;
            Matrix = Matrix4.LookAt(Position, LookAt, Up);
        }
        #endregion
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
                m_Pan = move;
            }
            else
            {
                m_Pan += move;
            }
            //境界ないかチェック
            if (m_Pan.X > m_PanMax.X)
            {
                m_Pan.X = m_PanMax.X;
            }
            if (m_Pan.Y > m_PanMax.Y)
            {
                m_Pan.Y = m_PanMax.Y;
            }
            if (m_Pan.Z > m_PanMax.Z)
            {
                m_Pan.Z = m_PanMax.Z;
            }

            if (m_Pan.X < m_PanMin.X)
            {
                m_Pan.X = m_PanMin.X;
            }
            if (m_Pan.Y < m_PanMin.Y)
            {
                m_Pan.Y = m_PanMin.Y;
            }
            if (m_Pan.Z < m_PanMin.Z)
            {
                m_Pan.Z = m_PanMin.Z;
            }

            UpdateCamera();
        }

        public void Translate(float x,float y,float z)
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
                SetPan((vectorX + vectorY) * m_PanRatio);
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
            m_theta += move.X;
            m_phi += move.Y;
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
                ratio = m_ZoomInRatio;
            }
            else
            {
                ratio = m_ZoomOutRatio;
            }
            //拡大縮小
            m_ZoomLength *= ratio;
            if (m_ZoomLength < m_ZoomMin)
            {
                m_ZoomLength = m_ZoomMin;
            }
            else if (m_ZoomLength > m_ZoomMax)
            {
                m_ZoomLength = m_ZoomMax;
            }
            UpdateCamera();
        }
        #endregion

    }
}
