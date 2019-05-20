using System.Collections.Generic;
using KI.Mathmatics;
using OpenTK;
using KI.Foundation.Core;

namespace CADApp.Model
{
    public class BezierCurve : CurveAssembly
    {
        public BezierCurve(string name)
            : base(name)
        {

        }

        /// <summary>
        /// ベジエ曲線上の点
        /// </summary>
        /// <param name="parameter">媒介変数</param>
        /// <returns>位置</returns>
        public Vector3 BezierPoint(float parameter)
        {
            Vector3 point = Vector3.Zero;
            for (int i = 0; i < ControlPoint.Count; i++)
            {
                float bernstein = Calculator.Bernstein(ControlPoint.Count - 1, i, parameter);
                point += ControlPoint[i].Position * bernstein;
            }

            return point;
        }

        protected override void UpdateControlPoint()
        {
            ClearVertex();

            if(Surface == true)
            {
                if (GenerateSurface() == true)
                {
                    return;
                }
            }

            for (float i = 0; i < 1; i += 0.1f)
            {
                AddVertex(BezierPoint(i));
            }


            if (Vertex.Count > 0)
            {
                List<int> line = new List<int>();
                line.Add(0);
                for (int i = 1; i < Vertex.Count - 1; i++)
                {
                    line.Add(i);
                    line.Add(i);
                }
                line.Add(Vertex.Count - 1);
                SetLineIndex(line);
            }
        }

        public bool Surface { get; set; }


        /// <summary>
        /// 曲面の頂点位置
        /// </summary>
        /// <param name="uparam">U座標</param>
        /// <param name="vparam">V座標</param>
        /// <returns></returns>
        public Vector3 BezierSurfacePoint(float uparam, float vparam)
        {
            Vector3 point = Vector3.Zero;
            for (int vIndex = 0; vIndex < VNum; vIndex++)
            {
                float vbernstein = Calculator.Bernstein(VNum - 1, vIndex, vparam);
                for (int uIndex = 0; uIndex < UNum; uIndex++)
                {
                    float ubernstein = Calculator.Bernstein(UNum - 1, uIndex, uparam);
                    int index = uIndex + vIndex * UNum;
                    point += ControlPoint[index].Position * ubernstein * vbernstein;
                }
            }

            return point;
        }

        /// <summary>
        /// 曲面の計算(仮置き)
        /// </summary>
        public bool GenerateSurface()
        {
            if (ControlPoint.Count != 4)
            {
                return false;
            }

            UNum = ControlPoint.Count;
            VNum = ControlPoint.Count;
            Vector3 firstPoint = ControlPoint[0].Position;

            for (int j = 1; j < UNum; j++)
            {
                for (int i = 0; i < VNum; i++)
                {
                    Vector3 heightPoint = new Vector3(ControlPoint[i].Position);
                    heightPoint.Y += j * 0.1f;
                    AddControlPoint(heightPoint);
                }
            }

            int pointNum = 100;
            float diff = 1.0f / pointNum;
            for (int vIndex = 0; vIndex <= pointNum; vIndex++)
            {
                float vParameter = vIndex * diff;
                for (int uIndex = 0; uIndex <= pointNum; uIndex++)
                {
                    float uParameter = uIndex * diff;
                    Vector3 point = BezierSurfacePoint(uParameter, vParameter);
                    Vector3 color = PseudoColor.GetColor(uParameter + vParameter, 0, 2);
                    AddVertex(point, Vector3.UnitX, color);
                }
            }

            //for (float vParameter = 0; vParameter < 1; vParameter += diff)
            //{
            //    for (float uParameter = 0; uParameter < 1; uParameter += diff)
            //    {
            //        Vector3 point = BezierSurfacePoint(uParameter, vParameter);
            //        Vector3 color = PseudoColor.GetColor(uParameter + vParameter, 0, 2);
            //        AddVertex(point, Vector3.UnitX, color);
            //    }
            //}

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

            SetTriangleIndex(triangleIndex);

            return true;
        }
    }
}
