﻿using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Utility;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// メッシュ
    /// </summary>
    public class Mesh
    {
        /// <summary>
        /// エッジ
        /// </summary>
        private List<HalfEdge> edges = new List<HalfEdge>();

        /// <summary>
        /// 法線
        /// </summary>
        private Vector3 normal = Vector3.Zero;

        /// <summary>
        /// 平面の公式
        /// </summary>
        private Vector4 plane = Vector4.Zero;

        /// <summary>
        /// 重心
        /// </summary>
        private Vector3 gravity = Vector3.Zero;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="index">要素番号</param>
        public Mesh(int index = -1)
        {
            Index = index;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="edge1">エッジ1</param>
        /// <param name="edge2">エッジ2</param>
        /// <param name="edge3">エッジ3</param>
        /// <param name="index">要素番号</param>
        public Mesh(HalfEdge edge1, HalfEdge edge2, HalfEdge edge3, int index = -1)
        {
            SetEdge(edge1, edge2, edge3);
            Index = index;
        }

        /// <summary>
        /// 要素番号
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
        /// 法線
        /// </summary>
        public Vector3 Normal
        {
            get
            {
                if (normal == Vector3.Zero)
                {
                    normal = KICalc.Normal(
                        edges[1].Start.Position - edges[0].Start.Position,
                        edges[2].Start.Position - edges[0].Start.Position);
                }

                return normal;
            }
        }

        /// <summary>
        /// 平面の公式
        /// </summary>
        public Vector4 Plane
        {
            get
            {
                if (plane == Vector4.Zero)
                {
                    Vector3 pos1 = Vector3.Zero;
                    Vector3 pos2 = Vector3.Zero;
                    Vector3 pos3 = Vector3.Zero;
                    foreach (var position in AroundVertex)
                    {
                        if (pos1 == Vector3.Zero)
                        {
                            pos1 = position.Position;
                            continue;
                        }

                        if (pos2 == Vector3.Zero)
                        {
                            pos2 = position.Position;
                            continue;
                        }

                        if (pos3 == Vector3.Zero)
                        {
                            pos3 = position.Position;
                            break;
                        }
                    }

                    plane = KICalc.GetPlaneFormula(pos1, pos2, pos3);
                }

                return plane;
            }
        }

        /// <summary>
        /// 重心
        /// </summary>
        public Vector3 Gravity
        {
            get
            {
                if (gravity == Vector3.Zero)
                {
                    foreach (var vertex in AroundVertex)
                    {
                        gravity += vertex.Position;
                    }

                    gravity /= AroundVertex.Count();
                }

                return gravity;
            }
        }

        /// <summary>
        /// 周囲のエッジ
        /// </summary>
        public IEnumerable<HalfEdge> AroundEdge
        {
            get
            {
                return edges;
            }
        }

        /// <summary>
        /// 頂点
        /// </summary>
        public IEnumerable<Vertex> AroundVertex
        {
            get
            {
                foreach (var edge in edges)
                {
                    yield return edge.Start;
                }
            }
        }

        /// <summary>
        /// エラーメッシュ
        /// </summary>
        public bool ErrorMesh
        {
            get
            {
                return AroundEdge.Any(p => p.DeleteFlag) || DeleteFlag;
            }
        }

        /// <summary>
        /// エッジのセッタ
        /// </summary>
        /// <param name="edge1">エッジ1</param>
        /// <param name="edge2">エッジ2</param>
        /// <param name="edge3">エッジ3</param>
        public void SetEdge(HalfEdge edge1, HalfEdge edge2, HalfEdge edge3)
        {
            edges.Add(edge1);
            edges.Add(edge2);
            edges.Add(edge3);

            HalfEdge.SetupNextBefore(edge1, edge2, edge3);
            edge1.Mesh = this;
            edge2.Mesh = this;
            edge3.Mesh = this;
        }

        /// <summary>
        /// エッジのゲッタ
        /// </summary>
        /// <param name="index">番号</param>
        /// <returns>エッジ</returns>
        public HalfEdge GetEdge(int index)
        {
            if (edges.Count < index)
            {
                return null;
            }

            return edges[index];
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            DeleteFlag = true;
            edges.Clear();
            edges = null;
        }
    }
}
