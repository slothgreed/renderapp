using KI.Mathmatics;
using OpenTK;
using System.Collections.Generic;
using System;

namespace KI.Asset.Primitive
{
    /// <summary>
    /// ベジエ曲線
    /// </summary>
    public class Bezier : PrimitiveBase
    {
        /// <summary>
        /// コントロールポイント
        /// </summary>
        public Vector3[] ControlPoint;

        /// <summary>
        /// Line全体の点の数
        /// </summary>
        private int Partition;

        /// <summary>
        /// 横の数
        /// </summary>
        public int UNum { get; private set; }

        /// <summary>
        /// 縦の数(1以上の場合サーフェスになる)
        /// </summary>
        public int VNum { get; private set; }

        /// <summary>
        /// 面かどうか
        /// </summary>
        public bool Surface { get { return VNum > 1; } }

        /// <summary>
        /// ベジエ曲線
        /// </summary>
        /// <param name="controlPoint">コントロールポイント</param>
        /// <param name="partition">分割数</param>
        public Bezier(Vector3[] controlPoint, int unum, int vnum, int partition)
        {
            ControlPoint = controlPoint;
            UNum = unum;
            VNum = vnum;
            Partition = partition;
            if (VNum == 1)
            {
                CreateBezierLine();
            }
            else
            {
                CreateBezierSurface();
            }
        }

        /// <summary>
        /// ベジエ曲線上の点
        /// </summary>
        /// <param name="parameter">媒介変数</param>
        /// <returns>位置</returns>
        private Vector3 BezierLinePoint(float parameter)
        {
            Vector3 point = Vector3.Zero;
            for (int i = 0; i < ControlPoint.Length; i++)
            {
                float bernstein = Calculator.Bernstein(ControlPoint.Length - 1, i, parameter);
                point += ControlPoint[i] * bernstein;
            }

            return point;
        }

        /// <summary>
        /// ベジエ曲線作成
        /// </summary>
        private void CreateBezierLine()
        {
            List<Vector3> position = new List<Vector3>();
            float d = 1.0f / Partition;
            for (float i = 0; i < 1; i += d)
            {
                position.Add(BezierLinePoint(i));
            }

            Position = position.ToArray();

            if (position.Count > 0)
            {
                List<int> line = new List<int>();
                line.Add(0);
                for (int i = 1; i < position.Count - 1; i++)
                {
                    line.Add(i);
                    line.Add(i);
                }
                line.Add(position.Count - 1);

                Index = line.ToArray();
            }
        }


        /// <summary>
        /// 曲面の頂点位置
        /// </summary>
        /// <param name="uparam">U座標</param>
        /// <param name="vparam">V座標</param>
        /// <returns></returns>
        private Vector3 BezierSurfacePoint(float uparam, float vparam)
        {
            Vector3 point = Vector3.Zero;
            for (int vIndex = 0; vIndex < VNum; vIndex++)
            {
                float vbernstein = Calculator.Bernstein(VNum - 1, vIndex, vparam);
                for (int uIndex = 0; uIndex < UNum; uIndex++)
                {
                    float ubernstein = Calculator.Bernstein(UNum - 1, uIndex, uparam);
                    int index = uIndex + vIndex * UNum;
                    point += ControlPoint[index] * ubernstein * vbernstein;
                }
            }

            return point;
        }

        /// <summary>
        /// 曲面の計算
        /// </summary>
        private void CreateBezierSurface()
        {

            int pointNum = 100;
            float diff = 1.0f / pointNum;
            List<Vector3> position = new List<Vector3>();
            for (int vIndex = 0; vIndex <= pointNum; vIndex++)
            {
                float vParameter = vIndex * diff;
                for (int uIndex = 0; uIndex <= pointNum; uIndex++)
                {
                    float uParameter = uIndex * diff;
                    Vector3 point = BezierSurfacePoint(uParameter, vParameter);
                    position.Add(point);
                }
            }

            Position = position.ToArray();

            // 描画に利用するU,V方向の頂点数
            List<int> triangleIndex = new List<int>();
            for (int v = 0; v < pointNum; v++)
            {
                for (int u = 0; u < pointNum; u++)
                {
                    int quad0 = u + v * (pointNum + 1);             // 違う列の最初の位置はpointNum + 1番目
                    int quad1 = (u + 1) + v * (pointNum + 1);
                    int quad2 = (u + 1) + (v + 1) * (pointNum + 1);
                    int quad3 = u + (v + 1) * (pointNum + 1);

                    triangleIndex.Add(quad0);
                    triangleIndex.Add(quad1);
                    triangleIndex.Add(quad2);

                    triangleIndex.Add(quad0);
                    triangleIndex.Add(quad2);
                    triangleIndex.Add(quad3);
                }
            }

            Index = triangleIndex.ToArray();
        }
    }
}
