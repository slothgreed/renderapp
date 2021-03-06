﻿using System;
using KI.Foundation.Tree;
using OpenTK;

namespace KI.Asset
{

    public enum CameraMode
    {
        Ortho,
        Perspective
    }

    /// <summary>
    /// カメラ
    /// </summary>
    public class Camera : KINode
    {

        #region [property method]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public Camera(string name)
            : base(name)
        {
            InitCamera(CameraMode.Perspective);
            SetProjMatrix(1.0f);
        }


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
        public Vector3 LookAtDirection { get; private set; }

        /// <summary>
        /// 注視点
        /// </summary>
        private Vector3 lookAt;

        /// <summary>
        /// 注視点
        /// </summary>
        public Vector3 LookAt
        {
            get { return lookAt; }
            set
            {
                lookAt = value;
                UpdateCamera();
            }
        }

        /// <summary>
        /// カメラからの距離
        /// </summary>
        private float lookAtDistance;

        /// <summary>
        /// カメラからの距離
        /// </summary>
        public float LookAtDistance
        {
            get { return lookAtDistance; }
            set
            {
                lookAtDistance = value;
                UpdateCamera();
            }
        }

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
        /// projectionのnear
        /// </summary>
        public float Near { get; set; } = 0.01f;

        /// <summary>
        /// 遠景
        /// </summary>
        public float Far { get; set; } = 1000;

        public CameraMode CameraMode { get; private set; }

        #region [回転関連]

        /// <summary>
        /// 球面座標の角度
        /// </summary>
        private float phi;

        /// <summary>
        /// 球面座標の角度
        /// </summary>
        private float theta;

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
        public void InitCamera(CameraMode mode)
        {
            CameraMode = mode;
            if (CameraMode == CameraMode.Perspective)
            {
                InitPerspective();
            }
            else
            {
                InitOrtho();
            }
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

        private void InitPerspective()
        {
            LookAt = Vector3.Zero;
            Up = Vector3.UnitY;
            Position = Vector3.UnitZ;
            LookAtDistance = (Position - LookAt).Length;
            Theta = 90.0f;
            Phi = 0.0f;
            UpdateCamera();
        }

        private void InitOrtho()
        {
            ProjMatrix = Matrix4.CreateOrthographicOffCenter(-1, 1, -1, 1, -1, 1);
            Matrix = Matrix4.Identity;
        }

        #region [カメラの回転]

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

        /// <summary>
        /// カメラの設定
        /// </summary>
        private void UpdateCamera()
        {
            Vector3 q_Move = GetSphericalMove();
            Position = LookAt + (q_Move * LookAtDistance);
            LookAtDirection = (LookAt - Position).Normalized();
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
