using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Utility;
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
        Voronoi
    }

    /// <summary>
    /// 頂点クラス
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// 法線
        /// </summary>
        private Vector3 normal = Vector3.Zero;

        /// <summary>
        /// temporaryEdgeforopposite
        /// </summary>
        private List<HalfEdge> aroundEdge = new List<HalfEdge>();

        /// <summary>
        /// パラメータ
        /// </summary>
        private Dictionary<VertexParam, object> parameter;

        /// <summary>
        /// 頂点
        /// </summary>
        /// <param name="pos">座標</param>
        /// <param name="index">要素番号</param>
        public Vertex(Vector3 pos, int index = -1)
        {
            parameter = new Dictionary<VertexParam, object>();
            Position = pos;
            Index = index;
        }

        /// <summary>
        /// 座標
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// HalfEdgeでもつm_VertexのIndex番号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// テンポラリ計算用フラグ
        /// </summary>
        public object CalcFlag { get; set; }

        /// <summary>
        /// 削除フラグ。Updateが走ると必ず削除するべきもの
        /// </summary>
        public bool DeleteFlag { get; set; }

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
        public IEnumerable<Mesh> AroundMesh
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
        public IEnumerable<Vertex> AroundVertex
        {
            get
            {
                foreach (var edge in AroundEdge)
                {
                    yield return edge.End;
                }
            }
        }

        /// <summary>
        /// 法線
        /// </summary>
        public Vector3 Normal
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

                    Vector3 value = Vector3.Zero;

                    foreach (var edge in AroundEdge)
                    {
                        HalfEdge opposite = edge.Opposite;
                        float alpha = edge.Next.Next.Radian;
                        float beta = opposite.Next.Next.Radian;
                        alpha = (float)(Math.Cos(alpha) / Math.Sin(alpha));
                        beta = (float)(Math.Cos(beta) / Math.Sin(beta));

                        value += (alpha + beta) * (edge.Start - edge.End);
                    }

                    normal = value.Normalized();
                }

                return normal;
            }
        }

        #region [operator]
        public static Vector3 operator +(Vertex v1, Vertex v2)
        {
            return new Vector3(v1.Position + v2.Position);
        }

        public static Vector3 operator -(Vertex v1, Vertex v2)
        {
            return new Vector3(v1.Position - v2.Position);
        }

        public static Vector3 operator *(Vertex v1, Vertex v2)
        {
            return new Vector3(v1.Position * v2.Position);
        }

        public static bool operator ==(Vertex v1, Vertex v2)
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

        public static bool operator !=(Vertex v1, Vertex v2)
        {
            return !(v1 == v2);
        }
        #endregion

        /// <summary>
        /// パラメータの追加
        /// </summary>
        /// <param name="param">パラメータのタイプ</param>
        /// <param name="value">値</param>
        public void AddParameter(VertexParam param, float value)
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
