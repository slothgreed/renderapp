using System;
using System.Linq;
using KI.Mathmatics;
using OpenTK;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// 頂点曲率のアルゴリズム
    /// </summary>
    public class VertexCurvatureAlgorithm
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="half">ハーフエッジ</param>
        public VertexCurvatureAlgorithm(HalfEdgeDS half)
        {
            Calculate(half);
        }

        /// <summary>
        /// 計算処理
        /// </summary>
        /// <param name="half">ハーフエッジ</param>
        private void Calculate(HalfEdgeDS half)
        {
            foreach (var vertex in half.HalfEdgeVertexs)
            {
                SetVoronoiRagion(vertex);
                SetGaussianParameter(vertex);
                SetMeanCurvature(vertex);
                SetMaxMinCurvature(vertex);
                SetPrincipalCurvature(vertex);
            }
        }

        /// <summary>
        /// ボロノイ領域
        /// </summary>
        /// <param name="vertex">頂点</param>
        private void SetVoronoiRagion(HalfEdgeVertex vertex)
        {
            float voronoi = 0;
            foreach (var edge in vertex.AroundEdge)
            {
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

                var area1 = Geometry.Area(midPoint1, edge.Start.Position, (edge.Start.Position + edge.End.Position) / 2);
                var area2 = Geometry.Area(midPoint2, edge.Start.Position, (edge.Start.Position + edge.End.Position) / 2);

                voronoi += area1 + area2;
            }

            // TODO: not 1
            //voronoi = 1;
            vertex.AddParameter(VertexParam.Voronoi, voronoi);
        }

        /// <summary>
        /// ガウス曲率
        /// </summary>
        /// <param name="vertex">頂点</param>
        private void SetGaussianParameter(HalfEdgeVertex vertex)
        {
            float angle = 0;
            foreach (var edge in vertex.AroundEdge)
            {
                angle += edge.Radian;
            }

            float value = (2 * MathHelper.Pi - angle) / (float)vertex.GetParameter(VertexParam.Voronoi);
            vertex.AddParameter(VertexParam.GaussCurvature, value);
        }

        /// <summary>
        /// 平均曲率
        /// </summary>
        /// <param name="vertex">頂点</param>
        private void SetMeanCurvature(HalfEdgeVertex vertex)
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

                float angle = (alpha + beta) * length / 8;
                float kapper = 2 * Vector3.Dot(edge.Start - edge.End, edge.Start.Normal) / length;
                value += angle * kapper;
            }

            value /= (float)vertex.GetParameter(VertexParam.Voronoi);
            vertex.AddParameter(VertexParam.MeanCurvature, value);
        }

        /// <summary>
        /// 最大・最小主曲率
        /// </summary>
        /// <param name="vertex">頂点</param>
        private void SetMaxMinCurvature(HalfEdgeVertex vertex)
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

            vertex.AddParameter(VertexParam.MaxCurvature, mean + delta);
            vertex.AddParameter(VertexParam.MinCurvature, mean - delta);
        }

        /// <summary>
        /// 接平面に投影した時のベクトル
        /// </summary>
        /// <param name="vertex">頂点</param>
        private void SetPrincipalCurvature(HalfEdgeVertex vertex)
        {
            Vector3 edge = vertex.AroundEdge.First().End.Position - vertex.AroundEdge.First().Start.Position;
            Vector3 numer = edge - (Vector3.Dot(edge, vertex.Normal) * vertex.Normal);
            Vector3 denom = new Vector3(numer.Length);
            Vector3 tangent;
            Vector2 tangentUV;
            Matrix3 ellipse = new Matrix3();
            Vector3 kapper = new Vector3();
            float edge_Kapper;
            //基底Z方向ベクトル
            Vector3 normal = vertex.Normal;
            //基底U方向ベクトル
            Vector3 baseU = new Vector3(numer.X / denom.X, numer.Y / denom.Y, numer.Z / denom.Z);
            baseU.Normalize();

            float inner = Vector3.Dot(baseU, normal);
            //基底V方向ベクトル
            Vector3 baseV = Vector3.Cross(baseU, normal);
            baseV.Normalize();

            foreach (var around in vertex.AroundEdge)
            {
                edge = around.End.Position - around.Start.Position;
                numer = edge - (Vector3.Dot(edge, normal) * normal);
                denom = new Vector3(numer.Length);
                tangent = new Vector3(numer.X / denom.X, numer.Y / denom.Y, numer.Z / denom.Z);
                tangentUV = new Vector2(Vector3.Dot(baseU, tangent), Vector3.Dot(baseV, tangent));
                edge_Kapper = 2 * Vector3.Dot(-edge, normal) / edge.Length * edge.Length;

                ellipse.M11 += tangentUV.X * tangentUV.X * tangentUV.X * tangentUV.X;
                ellipse.M12 += tangentUV.X * tangentUV.X * tangentUV.X * tangentUV.Y;
                ellipse.M13 += tangentUV.X * tangentUV.X * tangentUV.Y * tangentUV.Y;

                ellipse.M21 += tangentUV.X * tangentUV.X * tangentUV.X * tangentUV.Y;
                ellipse.M22 += tangentUV.X * tangentUV.X * tangentUV.Y * tangentUV.Y;
                ellipse.M23 += tangentUV.X * tangentUV.Y * tangentUV.Y * tangentUV.Y;

                ellipse.M31 += tangentUV.X * tangentUV.X * tangentUV.Y * tangentUV.Y;
                ellipse.M32 += tangentUV.X * tangentUV.Y * tangentUV.Y * tangentUV.Y;
                ellipse.M33 += tangentUV.Y * tangentUV.Y * tangentUV.Y * tangentUV.Y;

                kapper.X += edge_Kapper * tangentUV.X * tangentUV.X;
                kapper.Y += edge_Kapper * tangentUV.X * tangentUV.Y;
                kapper.Z += edge_Kapper * tangentUV.Y * tangentUV.Y;
            }

            ellipse.Invert();
            Vector3 result = Calculator.Multiply(ellipse, kapper);
            float a = result.X;
            float b = result.Y / 2;
            float c = result.Z;


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

            var matrix = new float[,] { { a, b }, { b, c } };
            float[,] eigenVector;
            float[] eigenValue;
            LinearAlgebra.EigenValue(matrix, out eigenVector, out eigenValue);

            float max1 = eigenVector[0, 0];
            float max2 = eigenVector[0, 1];
            float min1 = eigenVector[1, 0];
            float min2 = eigenVector[1, 1];

            var maxVector = new Vector3(
                baseU.X * max1 + baseV.X * max2,
                baseU.Y * max1 + baseV.Y * max2,
                baseU.Z * max1 + baseV.Z * max2).Normalized();

            var minVector = new Vector3(
               baseU.X * min1 + baseV.X * min2,
               baseU.Y * min1 + baseV.Y * min2,
               baseU.Z * min1 + baseV.Z * min2).Normalized();

            vertex.AddParameter(VertexParam.MaxVector, maxVector);
            vertex.AddParameter(VertexParam.MinVector, minVector);
            //float in1 = Vector3.Dot(vertex.Normal, vertex.MaxVector);
            //float in2 = Vector3.Dot(vertex.Normal, vertex.MinVector);
        }
    }
}
