using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using OpenCvSharp;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// 頂点パラメータ
    /// </summary>
    public enum VertexParam
    {
        MinCurvature,
        MaxCurvature,
        MeanCurvature,
        GaussCurvature,
        Voronoi,
        MaxVector,
        MinVector
    }

    /// <summary>
    /// 頂点クラス
    /// </summary>
    public class HalfEdgeVertex : Vertex
    {
        /// <summary>
        /// 選択色
        /// </summary>
        public readonly Vector3 SELECT_COLOR = Vector3.UnitY;

        /// <summary>
        /// 選択中か
        /// </summary>
        private bool isSelect = false;
        
        /// <summary>
        /// 法線
        /// </summary>
        private Vector3 normal = Vector3.Zero;

        /// <summary>
        /// temporaryEdgeforopposite
        /// </summary>
        private List<HalfEdge> aroundEdge = new List<HalfEdge>();

        /// <summary>
        /// ボロノイ領域
        /// </summary>
        private float voronoi = 0;

        /// <summary>
        /// 平均曲率
        /// </summary>
        private float meanCurvature = 0;

        /// <summary>
        /// ガウス曲率
        /// </summary>
        private float gaussCurvature = 0;

        /// <summary>
        /// 最小曲率
        /// </summary>
        private float minCurvature = 0;

        /// <summary>
        /// 最大曲率
        /// </summary>
        private float maxCurvature = 0;

        /// <summary>
        /// 最小主曲率方向
        /// </summary>
        private Vector3 minDirection = Vector3.Zero;

        /// <summary>
        /// 最大主曲率方向
        /// </summary>
        private Vector3 maxDirection = Vector3.Zero;

        /// <summary>
        /// パラメータ
        /// </summary>
        private Dictionary<VertexParam, object> parameter;

        /// <summary>
        /// 頂点
        /// </summary>
        /// <param name="pos">座標</param>
        /// <param name="index">要素番号</param>
        public HalfEdgeVertex(Vector3 pos, int index)
            : base(index, pos, Vector3.Zero, Vector3.Zero, Vector2.Zero)
        {
            parameter = new Dictionary<VertexParam, object>();
            Position = pos;
            Color = new Vector3(0.8f);
        }

        /// <summary>
        /// テンポラリ計算用変数
        /// </summary>
        public object TmpParameter { get; set; }

        /// <summary>
        /// 削除フラグ。Updateが走ると必ず削除するべきもの
        /// </summary>
        public bool DeleteFlag { get; set; }

        public override Vector3 Position
        {
            get
            {
                return base.Position;
            }

            set
            {
                base.Position = value;
                Modified();
            }
        }

        /// <summary>
        /// 選択中か
        /// </summary>
        public bool IsSelect
        {
            get
            {
                return isSelect;
            }

            set
            {
                isSelect = value;
            }
        }

        public override Vector3 Color
        {
            get
            {
                if (IsSelect)
                {
                    return SELECT_COLOR;
                }
                else
                {
                    return base.Color;
                }
            }
        }

        /// <summary>
        /// 頂点に不正があるか
        /// </summary>
        public bool ErrorVertex
        {
            get
            {
                return AroundEdge.Any(p => p.DeleteFlag) || DeleteFlag;
            }
        }

        /// <summary>
        /// 周辺エッジ
        /// </summary>
        public IEnumerable<HalfEdge> AroundEdge
        {
            get
            {
                //opposite計算後は削除されている。
                if (aroundEdge != null)
                {
                    foreach (var edge in aroundEdge)
                    {
                        yield return edge;
                    }
                }
            }
        }

        /// <summary>
        /// 周辺メッシュ
        /// </summary>
        public IEnumerable<HalfEdgeMesh> AroundMesh
        {
            get
            {
                foreach (var edge in AroundEdge)
                {
                    yield return edge.Mesh;
                }
            }
        }

        /// <summary>
        /// 周辺頂点
        /// </summary>
        public IEnumerable<HalfEdgeVertex> AroundVertex
        {
            get
            {
                foreach (var edge in AroundEdge)
                {
                    yield return edge.End;
                }
            }
        }

        #region [parameter]
        /// <summary>
        /// 法線
        /// </summary>
        public override Vector3 Normal
        {
            get
            {
                if (normal == Vector3.Zero)
                {
                    if (AroundMesh.Count() != 0)
                    {
                        Vector3 sum = Vector3.Zero;
                        int count = AroundMesh.Count();
                        foreach (var mesh in AroundMesh)
                        {
                            sum += mesh.Normal;
                        }

                        sum.X /= count;
                        sum.Y /= count;
                        sum.Z /= count;
                        normal = sum.Normalized();
                    }

                    //Vector3 value = Vector3.Zero;

                    //foreach (var edge in AroundEdge)
                    //{
                    //    HalfEdge opposite = edge.Opposite;
                    //    float alpha = edge.Next.Next.Radian;
                    //    float beta = opposite.Next.Next.Radian;
                    //    alpha = (float)(Math.Cos(alpha) / Math.Sin(alpha));
                    //    beta = (float)(Math.Cos(beta) / Math.Sin(beta));

                    //    value += (alpha + beta) * (edge.Start - edge.End);
                    //}

                    //normal = value.Normalized();
                }

                return normal;
            }
        }

        /// <summary>
        /// ボロノイ領域
        /// </summary>
        public float Voronoi
        {
            get
            {
                if (voronoi == 0)
                {
                    foreach (var edge in AroundEdge)
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

                        var area1 = KICalc.Area(midPoint1, edge.Start.Position, (edge.Start.Position + edge.End.Position) / 2);
                        var area2 = KICalc.Area(midPoint2, edge.Start.Position, (edge.Start.Position + edge.End.Position) / 2);

                        voronoi += area1 + area2;
                    }
                }

                return voronoi;
            }
        }

        /// <summary>
        /// 平均曲率
        /// </summary>
        public float MeanCurvature
        {
            get
            {
                if (meanCurvature == 0)
                {
                    foreach (var edge in AroundEdge)
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
                        meanCurvature += angle * kapper;
                    }

                    meanCurvature /= Voronoi;
                }

                return meanCurvature;
            }
        }

        /// <summary>
        /// ガウス曲率
        /// </summary>
        public float GaussCurvature
        {
            get
            {
                if (gaussCurvature == 0)
                {
                    float angle = 0;
                    foreach (var edge in AroundEdge)
                    {
                        angle += edge.Radian;
                    }

                    gaussCurvature = (2 * MathHelper.Pi - angle) / Voronoi;
                }

                return gaussCurvature;
            }
        }

        /// <summary>
        /// 最大曲率
        /// </summary>
        public float MaxCurvature
        {
            get
            {
                if (maxCurvature == 0)
                {
                    float mean = MeanCurvature;
                    float gauss = GaussCurvature;

                    float delta = (mean * mean) - gauss;
                    if (delta > 0)
                    {
                        delta = (float)Math.Sqrt(delta);
                    }
                    else
                    {
                        delta = 0;
                    }

                    maxCurvature = mean + delta;
                    minCurvature = mean - delta;
                }

                return maxCurvature;
            }
        }

        /// <summary>
        /// 最小曲率
        /// </summary>
        public float MinCurvature
        {
            get
            {
                if (minCurvature == 0)
                {
                    float mean = MeanCurvature;
                    float gauss = GaussCurvature;

                    float delta = (mean * mean) - gauss;
                    if (delta > 0)
                    {
                        delta = (float)Math.Sqrt(delta);
                    }
                    else
                    {
                        delta = 0;
                    }

                    maxCurvature = mean + delta;
                    minCurvature = mean - delta;
                }

                return minCurvature;
            }
        }
        
        /// <summary>
        /// 最大主曲率方向
        /// </summary>
        public Vector3 MaxDirection
        {
            get
            {
                if (maxDirection == Vector3.Zero)
                {
                    SetCurvatureDirection();
                }

                return maxDirection;
            }
        }

        /// <summary>
        /// 最小主曲率方向
        /// </summary>
        public Vector3 MinDirection
        {
            get
            {
                if (minDirection == Vector3.Zero)
                {
                    SetCurvatureDirection();
                }

                return minDirection;
            }
        }

        /// <summary>
        /// 曲率方向の算出
        /// </summary>
        private void SetCurvatureDirection()
        {
            Vector3 edge = AroundEdge.First().End.Position - AroundEdge.First().Start.Position;
            Vector3 numer = edge - (Vector3.Dot(edge, Normal) * Normal);
            Vector3 denom = new Vector3(numer.Length);
            Vector3 tangent;
            Vector2 tangentUV;
            Matrix3 ellipse = new Matrix3();
            Vector3 kapper = new Vector3();
            float edge_Kapper;
            //基底Z方向ベクトル
            Vector3 normal = Normal;
            //基底U方向ベクトル
            Vector3 baseU = new Vector3(numer.X / denom.X, numer.Y / denom.Y, numer.Z / denom.Z);
            baseU.Normalize();

            //基底V方向ベクトル
            Vector3 baseV = Vector3.Cross(baseU, normal);
            baseV.Normalize();

            foreach (var around in AroundEdge)
            {
                edge = around.End.Position - around.Start.Position;
                numer = edge - (Vector3.Dot(edge, normal) * normal);
                denom = new Vector3(numer.Length);
                tangent = new Vector3(numer.X / denom.X, numer.Y / denom.Y, numer.Z / denom.Z);
                tangentUV = new Vector2(Vector3.Dot(baseU, tangent), Vector3.Dot(baseV, tangent));
                var edgeSE = around.Start.Position - around.End.Position;
                edge_Kapper = 2 * Vector3.Dot(edgeSE, normal) / (edgeSE.Length * edgeSE.Length);

                ellipse.M11 += tangentUV.X * tangentUV.X * tangentUV.X * tangentUV.X;
                ellipse.M12 += 2 * tangentUV.X * tangentUV.X * tangentUV.X * tangentUV.Y;
                ellipse.M13 += tangentUV.X * tangentUV.X * tangentUV.Y * tangentUV.Y;

                ellipse.M21 += 2 * tangentUV.X * tangentUV.X * tangentUV.X * tangentUV.Y;
                ellipse.M22 += 4 * tangentUV.X * tangentUV.X * tangentUV.Y * tangentUV.Y;
                ellipse.M23 += 2 * tangentUV.X * tangentUV.Y * tangentUV.Y * tangentUV.Y;

                ellipse.M31 += tangentUV.X * tangentUV.X * tangentUV.Y * tangentUV.Y;
                ellipse.M32 += 2 * tangentUV.X * tangentUV.Y * tangentUV.Y * tangentUV.Y;
                ellipse.M33 += tangentUV.Y * tangentUV.Y * tangentUV.Y * tangentUV.Y;

                kapper.X += edge_Kapper * tangentUV.X * tangentUV.X;
                kapper.Y += 2 * edge_Kapper * tangentUV.X * tangentUV.Y;
                kapper.Z += edge_Kapper * tangentUV.Y * tangentUV.Y;
            }

            ellipse.Invert();
            Vector3 result = KICalc.Multiply(ellipse, kapper);
            float a = result.X;
            float b = result.Y;
            float c = result.Z;

            CvMat eigenVector;
            CvMat eigenValue;
            CvMat matEplise = Cv.CreateMat(2, 2, MatrixType.F32C1);
            matEplise.Set2D(0, 0, a);
            matEplise.Set2D(0, 1, b);
            matEplise.Set2D(1, 0, b);
            matEplise.Set2D(1, 1, c);
            eigenVector = Cv.CreateMat(2, 2, MatrixType.F32C1);
            eigenValue = Cv.CreateMat(1, 2, MatrixType.F32C1);
            Cv.Zero(eigenVector);
            Cv.Zero(eigenValue);
            Cv.EigenVV(matEplise, eigenVector, eigenValue);

            float max1 = (float)eigenVector.Get2D(0, 0).Val0;
            float max2 = (float)eigenVector.Get2D(0, 1).Val0;
            float min1 = (float)eigenVector.Get2D(1, 0).Val0;
            float min2 = (float)eigenVector.Get2D(1, 1).Val0;

            maxDirection = new Vector3(
                baseU.X * max1 + baseV.X * max2,
                baseU.Y * max1 + baseV.Y * max2,
                baseU.Z * max1 + baseV.Z * max2).Normalized();

            minDirection = new Vector3(
               baseU.X * min1 + baseV.X * min2,
               baseU.Y * min1 + baseV.Y * min2,
               baseU.Z * min1 + baseV.Z * min2).Normalized();
        }
        #endregion

        /// <summary>
        /// クラスタ番号
        /// </summary>
        public int Cluster { get; set; }


        #region [operator]
        public static Vector3 operator +(HalfEdgeVertex v1, HalfEdgeVertex v2)
        {
            return new Vector3(v1.Position + v2.Position);
        }

        public static Vector3 operator -(HalfEdgeVertex v1, HalfEdgeVertex v2)
        {
            return new Vector3(v1.Position - v2.Position);
        }

        public static Vector3 operator *(HalfEdgeVertex v1, HalfEdgeVertex v2)
        {
            return new Vector3(v1.Position * v2.Position);
        }

        public static bool operator ==(HalfEdgeVertex v1, HalfEdgeVertex v2)
        {
            if (object.ReferenceEquals(v1, v2))
            {
                return true;
            }

            if ((object)v1 == null || (object)v2 == null)
            {
                return false;
            }

            if (Math.Abs(v1.Position.X - v2.Position.X) > KICalc.THRESHOLD05)
            {
                return false;
            }

            if (Math.Abs(v1.Position.Y - v2.Position.Y) > KICalc.THRESHOLD05)
            {
                return false;
            }

            if (Math.Abs(v1.Position.Z - v2.Position.Z) > KICalc.THRESHOLD05)
            {
                return false;
            }

            return true;
        }

        public static bool operator !=(HalfEdgeVertex v1, HalfEdgeVertex v2)
        {
            return !(v1 == v2);
        }
        #endregion

        /// <summary>
        /// 編集したときに呼ぶ
        /// </summary>
        public override void Modified()
        {
            if (AroundEdge != null)
            {
                foreach (var around in AroundEdge)
                {
                    around.Modified();
                }
            }

            normal = Vector3.Zero;
            voronoi = 0;
            gaussCurvature = 0;
            meanCurvature = 0;
            minCurvature = 0;
            maxCurvature = 0;
            minDirection = Vector3.Zero;
            maxDirection = Vector3.Zero;
        }

        /// <summary>
        /// エッジを含んでいるか確認
        /// </summary>
        /// <param name="halfEdge">ハーフエッジ</param>
        /// <returns>含む</returns>
        public bool ContainsEdge(HalfEdge halfEdge)
        {
            return AroundEdge.Contains(halfEdge);
        }

        /// <summary>
        /// パラメータの追加
        /// </summary>
        /// <param name="param">パラメータのタイプ</param>
        /// <param name="value">値</param>
        public void AddParameter(VertexParam param, object value)
        {
            parameter.Add(param, value);
        }

        /// <summary>
        /// パラメータの取得
        /// </summary>
        /// <param name="param">パラメータのタイプ</param>
        /// <returns>値</returns>
        public object GetParameter(VertexParam param)
        {
            return parameter[param];
        }

        /// <summary>
        /// エッジのセッタ
        /// </summary>
        /// <param name="edge">エッジ</param>
        public void AddEdge(HalfEdge edge)
        {
            if (!aroundEdge.Contains(edge))
            {
                aroundEdge.Add(edge);
                Modified();
            }
        }

        /// <summary>
        /// 周辺エッジの削除
        /// </summary>
        /// <param name="edge">エッジ</param>
        public void RemoveAroundEdge(HalfEdge edge)
        {
            aroundEdge.Remove(edge);
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            DeleteFlag = true;
            aroundEdge = null;
        }
    }
}
