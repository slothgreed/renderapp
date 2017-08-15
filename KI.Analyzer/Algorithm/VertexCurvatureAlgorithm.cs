using OpenTK;
using System;
using KI.Foundation.Utility;
using System.Linq;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// 頂点曲率のアルゴリズム
    /// </summary>
    public class VertexCurvatureAlgorithm : IAnalyzer
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="half">ハーフエッジ</param>
        public VertexCurvatureAlgorithm(HalfEdgeDS half)
        {
            ScalarParameter param = new ScalarParameter();
            Parameters.Add(VertexParam.Voronoi, new ScalarParameter());
            Parameters.Add(VertexParam.GaussCurvature, new ScalarParameter());
            Parameters.Add(VertexParam.MeanCurvature, new ScalarParameter());
            Parameters.Add(VertexParam.MaxCurvature, new ScalarParameter());
            Parameters.Add(VertexParam.MinCurvature, new ScalarParameter());
            Calculate(half);
        }

        /// <summary>
        /// 計算処理
        /// </summary>
        /// <param name="half">ハーフエッジ</param>
        private void Calculate(HalfEdgeDS half)
        {
            foreach (var vertex in half.Vertexs)
            {
                SetVoronoiRagion(vertex);
                SetGaussianParameter(vertex);
                SetMeanCurvature(vertex);
                SetMaxMinCurvature(vertex);
            }
        }

        /// <summary>
        /// ボロノイ領域
        /// </summary>
        /// <param name="vertex">頂点</param>
        private void SetVoronoiRagion(Vertex vertex)
        {
            float voronoi = 0;
            foreach (var edge in vertex.AroundEdge)
            {
                //float length = edge.Length;
                //HalfEdge opposite = edge.Opposite;
                //float alpha = edge.Next.Next.Radian;
                //float beta = opposite.Next.Next.Radian;
                //if (!edge.Mesh.IsObtuse)
                //{
                //    alpha = (float)(Math.Cos(alpha) / Math.Sin(alpha));
                //    beta = (float)(Math.Cos(beta) / Math.Sin(beta));

                //    voronoi += ((float)(alpha + beta) * length * length) / 8;
                //}
                //else
                //{
                //    if (edge.Radian > MathHelper.PiOver2)
                //    {
                //        voronoi += edge.Mesh.Area / 2;
                //    }
                //    else
                //    {
                //        voronoi += edge.Mesh.Area / 4;
                //    }
                //}

                Vector3 midPoint1;
                Vector3 midPoint2;
                if (edge.Radian < MathHelper.PiOver2)
                {
                    midPoint1 = edge.Mesh.Gravity;
                }
                else
                {
                    midPoint1 = (edge.End.Position + edge.Next.End.Position) / 2;
                }

                var area2Edge = edge.Opposite.Next;
                if (area2Edge.Radian < MathHelper.PiOver2)
                {
                    midPoint2 = area2Edge.Mesh.Gravity;
                }
                else
                {
                    midPoint2 = (area2Edge.End.Position + area2Edge.Next.End.Position) / 2;
                }

                var area1 = KICalc.Area(midPoint1, edge.Start.Position, (edge.Start.Position + edge.End.Position) / 2);
                var area2 = KICalc.Area(midPoint2, edge.Start.Position, (edge.Start.Position + edge.End.Position) / 2);

                voronoi += area1 + area2;
            }

            Parameters[VertexParam.Voronoi].AddValue(voronoi);
            vertex.AddParameter(VertexParam.Voronoi, voronoi);
        }

        /// <summary>
        /// ガウス曲率
        /// </summary>
        /// <param name="vertex">頂点</param>
        private void SetGaussianParameter(Vertex vertex)
        {
            float angle = 0;
            foreach (var edge in vertex.AroundEdge)
            {
                angle += edge.Radian;
            }

            float value = (2 * MathHelper.Pi - angle) / (float)vertex.GetParameter(VertexParam.Voronoi);
            Parameters[VertexParam.GaussCurvature].AddValue(value);
            vertex.AddParameter(VertexParam.GaussCurvature, value);
        }

        /// <summary>
        /// 平均曲率
        /// </summary>
        /// <param name="vertex">頂点</param>
        private void SetMeanCurvature(Vertex vertex)
        {
            float value = 0;
            foreach (var edge in vertex.AroundEdge)
            {
                float length = edge.Length;
                length = length * length;
                HalfEdge opposite = edge.Opposite;
                float alpha = edge.Next.Next.Radian;
                float beta = opposite.Next.Next.Radian;
                alpha = (float)(Math.Cos(alpha) / Math.Sin(alpha));
                beta = (float)(Math.Cos(beta) / Math.Sin(beta));

                float angle =  (float)(alpha + beta) * length / 8;
                float kapper = 2 * Vector3.Dot((edge.Start - edge.End), edge.Start.Normal) / length;
                value += angle * kapper;
            }

            value /= (float)vertex.GetParameter(VertexParam.Voronoi);
            Parameters[VertexParam.MeanCurvature].AddValue(value);
            vertex.AddParameter(VertexParam.MeanCurvature, value);
        }

        /// <summary>
        /// 最大・最小主曲率
        /// </summary>
        /// <param name="vertex"></param>
        private void SetMaxMinCurvature(Vertex vertex)
        {
            float mean = (float)vertex.GetParameter(VertexParam.MeanCurvature);
            float gauss = (float)vertex.GetParameter(VertexParam.GaussCurvature);

            float delta = (mean * mean) - gauss;
            if (delta > 0)
            {
                delta = (float)Math.Sqrt(delta);
            }
            else
            {
                delta = 0;
            }

            Parameters[VertexParam.MaxCurvature].AddValue(mean + delta);
            Parameters[VertexParam.MinCurvature].AddValue(mean - delta);
            vertex.AddParameter(VertexParam.MaxCurvature, mean + delta);
            vertex.AddParameter(VertexParam.MinCurvature, mean - delta);
        }

        /// <summary>
        /// 接平面に投影した時のベクトル
        /// </summary>
        /// <param name="v_index"></param>
        private void SetPrincipalCurvature(int v_index)
        {
            //List<Edge> around = GetAroundEdge(v_index);
            //Vector3 edge;
            //Vector3 numer;
            //Vector3 denom;
            //Vector3 tangent;
            //Vector2 TangentUV;
            //Matrix3 ellipse = new Matrix3();
            //Vector3 kapper = new Vector3();
            //edge = around[0].End.GetPosition() - around[0].Start.GetPosition();
            //numer = edge - (Vector3.Dot(edge, m_Vertex[v_index].Normal) * m_Vertex[v_index].Normal);
            //denom = new Vector3(numer.Length);
            //float edge_Kapper;
            ////基底Z方向ベクトル
            //Vector3 Normal = m_Vertex[v_index].Normal;
            ////基底U方向ベクトル
            //Vector3 baseU = new Vector3(numer.X / denom.X, numer.Y / denom.Y, numer.Z / denom.Z);
            //baseU.Normalize();

            //float inner = Vector3.Dot(baseU, Normal);
            ////基底V方向ベクトル
            //Vector3 baseV = Vector3.Cross(baseU, Normal);
            //baseV.Normalize();

            //for (int i = 0; i < around.Count; i++)
            //{
            //    edge = around[i].End.GetPosition() - around[i].Start.GetPosition();
            //    numer = edge - (Vector3.Dot(edge, Normal) * Normal);
            //    denom = new Vector3(numer.Length);
            //    tangent = new Vector3(numer.X / denom.X, numer.Y / denom.Y, numer.Z / denom.Z);
            //    TangentUV = new Vector2(Vector3.Dot(baseU, tangent), Vector3.Dot(baseV, tangent));
            //    edge_Kapper = 2 * Vector3.Dot(-edge, Normal) / (edge).Length * (edge).Length;

            //    ellipse.M11 += (TangentUV.X * TangentUV.X * TangentUV.X * TangentUV.X);
            //    ellipse.M12 += (TangentUV.X * TangentUV.X * TangentUV.X * TangentUV.Y);
            //    ellipse.M13 += (TangentUV.X * TangentUV.X * TangentUV.Y * TangentUV.Y);

            //    ellipse.M21 += (TangentUV.X * TangentUV.X * TangentUV.X * TangentUV.Y);
            //    ellipse.M22 += (TangentUV.X * TangentUV.X * TangentUV.Y * TangentUV.Y);
            //    ellipse.M23 += (TangentUV.X * TangentUV.Y * TangentUV.Y * TangentUV.Y);

            //    ellipse.M31 += (TangentUV.X * TangentUV.X * TangentUV.Y * TangentUV.Y);
            //    ellipse.M32 += (TangentUV.X * TangentUV.Y * TangentUV.Y * TangentUV.Y);
            //    ellipse.M33 += (TangentUV.Y * TangentUV.Y * TangentUV.Y * TangentUV.Y);

            //    kapper.X += edge_Kapper * TangentUV.X * TangentUV.X;
            //    kapper.Y += edge_Kapper * TangentUV.X * TangentUV.Y;
            //    kapper.Z += edge_Kapper * TangentUV.Y * TangentUV.Y;

            //}

            //ellipse.Invert();
            //Vector3 result = CCalc.Multiply(ellipse, kapper);
            //float a = result.X;
            //float b = result.Y / 2;
            //float c = result.Z;
            //CvMat eigenVector;
            //CvMat eigenValue;
            //CvMat matEplise = Cv.CreateMat(2, 2, MatrixType.F32C1);
            //matEplise.Set2D(0, 0, a);
            //matEplise.Set2D(0, 1, b);
            //matEplise.Set2D(1, 0, b);
            //matEplise.Set2D(1, 1, c);
            //eigenVector = Cv.CreateMat(2, 2, MatrixType.F32C1);
            //eigenValue = Cv.CreateMat(1, 2, MatrixType.F32C1);
            //Cv.Zero(eigenVector);
            //Cv.Zero(eigenValue);
            //Cv.EigenVV(matEplise, eigenVector, eigenValue);

            //float max1 = (float)eigenVector.Get2D(0, 0).Val0;
            //float max2 = (float)eigenVector.Get2D(0, 1).Val0;
            //float min1 = (float)eigenVector.Get2D(1, 0).Val0;
            //float min2 = (float)eigenVector.Get2D(1, 1).Val0;

            //m_Vertex[v_index].MaxVector = new Vector3(
            //    baseU.X * max1 + baseV.X * max2,
            //    baseU.Y * max1 + baseV.Y * max2,
            //    baseU.Z * max1 + baseV.Z * max2
            //    ).Normalized();
            //m_Vertex[v_index].MinVector = new Vector3(
            //   baseU.X * min1 + baseV.X * min2,
            //   baseU.Y * min1 + baseV.Y * min2,
            //   baseU.Z * min1 + baseV.Z * min2
            //   ).Normalized();
            //float in1 = Vector3.Dot(m_Vertex[v_index].Normal, m_Vertex[v_index].MaxVector);
            //float in2 = Vector3.Dot(m_Vertex[v_index].Normal, m_Vertex[v_index].MaxVector);
        }
    }
}
