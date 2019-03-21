﻿using System;
using KI.Foundation.Core;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// カメラ
    /// </summary>
    public class Camera : KIFile
    {
        #region [property method]


        /// <summary>
        /// ズームの最小値
        /// </summary>
        private float zoomMin = 1;

        /// <summary>
        /// ズームの最大値
        /// </summary>
        private float zoomMax = 10;

        /// <summary>
        /// 注視点の平行移動量
        /// </summary>
        private Vector3 pan = new Vector3(0.0f, 5.0f, 10.0f);

        /// <summary>
        /// 平行移動の最小値
        /// </summary>
        private Vector3 panMin = new Vector3(-100, -100, -100);

        /// <summary>
        /// 平行移動の最大値
        /// </summary>
        private Vector3 panMax = new Vector3(100, 100, 100);

        /// <summary>
        /// 平行移動量の倍率設定
        /// </summary>
        private float panRatio = 0.1f;

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
        /// <param name="name">名前</param>
        public Camera(string name)
            : base(name)
        {
            InitCamera();
            SetProjMatrix(1.0f);
        }

        /// <summary>
        /// projectionのnear
        /// </summary>
        public float Near { get; set; } = 0.0001f;

        /// <summary>
        /// 投影行列
        /// </summary>
        public Matrix4 ProjMatrix { get; set; }

        /// <summary>
        /// マトリクス
        /// </summary>
        public Matrix4 Matrix { get; private set; }

        /// <summary>
        /// FOV
        /// </summary>
        public float FOV { get; private set; }

        /// <summary>
        /// FOVAspect
        /// </summary>
        public float FOVAspect { get; private set; }

        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// 向き
        /// </summary>
        public Vector3 Direction { get; private set; }

        /// <summary>
        /// 注視点
        /// </summary>
        public Vector3 LookAt { get; private set; }

        /// <summary>
        /// カメラからの距離
        /// </summary>
        public float LookAtDistance { get; private set; }

        /// <summary>
        /// 上方向
        /// </summary>
        public Vector3 Up { get; private set; }

        /// <summary>
        /// カメラ*投影マトリクス
        /// </summary>
        public Matrix4 CameraProjMatrix
        {
            get
            {
                return Matrix * ProjMatrix;
            }
        }

        /// <summary>
        /// 投影*カメラマトリクス
        /// </summary>
        public Matrix4 UnProject
        {
            get
            {
                return (Matrix * ProjMatrix).Inverted();
            }
        }

        #endregion

        /// <summary>
        /// 遠景
        /// </summary>
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
            pan = new Vector3(0.0f, 0, 0.0f);
            LookAt = Vector3.Zero;
            Up = Vector3.UnitY;
            LookAtDistance = zoomMax;
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

        /// <summary>
        /// 投影マトリクスの設定
        /// </summary>
        /// <param name="aspect">比率</param>
        public void SetProjMatrix(float aspect)
        {
            FOV = (float)Math.PI / 4.0f;
            FOVAspect = aspect;
            ProjMatrix = Matrix4.CreatePerspectiveFieldOfView(FOV, aspect, Near, Far);
        }

        #region [カメラの平行移動]

        /// <summary>
        /// falseの時は+=、trueは=基本は＋＝
        /// </summary>
        /// <param name="move">移動量</param>
        /// <param name="type">move分動かすのか,move位置に移動するのか</param>
        public void SetPan(Vector3 move, bool type = false)
        {
            if (type)
            {
                pan = move;
            }
            else
            {
                pan += move;
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

        /// <summary>
        /// 平行移動
        /// </summary>
        /// <param name="x">x座標</param>
        /// <param name="y">y座標</param>
        /// <param name="z">z座標</param>
        public void Translate(float x, float y, float z)
        {
            Translate(new Vector3(x, y, z));
        }

        /// <summary>
        /// Viewの平行移動
        /// </summary>
        /// <param name="move">移動量</param>
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
                SetPan((vectorX + vectorY) * panRatio);
            }
        }
        #endregion
        #region [カメラの回転]

        /// <summary>
        /// 回転
        /// </summary>
        /// <param name="x">x座標</param>
        /// <param name="y">y座標</param>
        /// <param name="z">z座標</param>
        public void Rotate(float x, float y, float z)
        {
            Rotate(new Vector3(x, y, z));
        }

        /// <summary>
        /// Viewの回転
        /// </summary>
        /// <param name="delta">移動量</param>
        public void Rotate(Vector3 delta)
        {
            Theta += delta.X;
            Phi += delta.Y;
            UpdateCamera();
        }
        #endregion
        #region [カメラの拡大縮小]
        /// <summary>
        /// Viewのズーム[詳:m_ZoomMin,m_ZoomMax,m_ZoomInRatio,m_ZoomOutRatio]
        /// </summary>
        /// <param name="lookAtDistance">LookAtまでの距離</param>
        public void Zoom(float lookAtDistance)
        {
            //拡大縮小
            LookAtDistance = lookAtDistance;

            UpdateCamera();
        }

        #endregion

        /// <summary>
        /// カメラの設定
        /// </summary>
        private void UpdateCamera()
        {
            Vector3 q_Move = GetSphericalMove();
            Position = pan + (q_Move * LookAtDistance);
            LookAt = pan;
            Matrix = Matrix4.LookAt(Position, LookAt, Up);
        }

        /// <summary>
        /// カメラの球面移動
        /// </summary>
        /// <returns>移動後の値</returns>
        private Vector3 GetSphericalMove()
        {
            Vector3 move = new Vector3();
            double theta = MathHelper.DegreesToRadians(Theta);
            double phi = MathHelper.DegreesToRadians(Phi);
            move.X = (float)(Math.Cos(theta) * Math.Cos(phi));
            move.Y = (float)Math.Sin(phi);
            move.Z = (float)(Math.Sin(theta) * Math.Cos(phi));

            return move;
        }
    }
}
