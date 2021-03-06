﻿using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// ハーフエッジを編集するクラス
    /// </summary>
    public class HalfEdgeDSEditor
    {
        /// <summary>
        /// 削除するエッジ
        /// </summary>
        private List<HalfEdge> deleteEdges = new List<HalfEdge>();

        /// <summary>
        /// 削除する面
        /// </summary>
        private List<HalfEdgeMesh> deleteMeshs = new List<HalfEdgeMesh>();

        /// <summary>
        /// 削除する頂点
        /// </summary>
        private List<HalfEdgeVertex> deleteVertexs = new List<HalfEdgeVertex>();

        /// <summary>
        /// 編集中かどうか
        /// </summary>
        private bool nowEdit = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="half">ハーフエッジ</param>
        public HalfEdgeDSEditor(HalfEdgeDS half)
        {
            HalfEdge = half;
        }

        /// <summary>
        /// ハーフエッジ
        /// </summary>
        public HalfEdgeDS HalfEdge { get; private set; }

        /// <summary>
        /// 編集中かどうか
        /// </summary>
        public bool NowEdit
        {
            get
            {
                return nowEdit;
            }

            private set
            {
                nowEdit = value;
            }
        }

        #region [edit method]
        
        /// <summary>
        /// 編集開始
        /// </summary>
        public void StartEdit()
        {
            nowEdit = true;
        }

        /// <summary>
        /// 編集終了
        /// </summary>
        public void EndEdit()
        {
            var vertexs = HalfEdge.HalfEdgeVertexs.Where(p => !p.DeleteFlag);
            var edges = HalfEdge.HalfEdges.Where(p => !p.DeleteFlag);
            var meshs = HalfEdge.HalfEdgeMeshs.Where(p => !p.DeleteFlag);

            HalfEdge.Setup(vertexs, edges, meshs);

            int counter = 0;
            foreach (var mesh in HalfEdge.HalfEdgeMeshs)
            {
                mesh.Index = counter;
                counter++;
            }

            counter = 0;
            foreach (var edge in HalfEdge.HalfEdges)
            {
                edge.Index = counter;
                counter++;
            }

            counter = 0;
            foreach (var vertex in HalfEdge.HalfEdgeVertexs)
            {
                vertex.Index = counter;
                counter++;
            }

            HalfEdge.Index = new List<int>();
            foreach(var mesh in HalfEdge.HalfEdgeMeshs)
            {
                HalfEdge.Index.AddRange(mesh.AroundVertex.Select(p => p.Index));
            }
            

            nowEdit = false;

        }
        #region [vertex decimation]

        /// <summary>
        /// マージによる頂点削除削除後、頂点位置移動
        /// </summary>
        /// <param name="edge">削除するエッジ</param>
        /// <param name="newPosition">新しい頂点位置</param>
        /// <returns>残した頂点</returns>
        public HalfEdgeVertex EdgeCollapse(HalfEdge edge, Vector3 newPosition)
        {
            if (!NowEdit)
            {
                Logger.Log(Logger.LogLevel.Warning, "Call StartEdit");
                return null;
            }

            if (!CanEdgeCollapse(edge))
            {
                return null;
            }

            HalfEdgeVertex delV = edge.Start;
            HalfEdgeVertex remV = edge.End;

            #region [create delete list]
            //削除するメッシュ
            var delMesh = new List<HalfEdgeMesh>();
            //削除するエッジ
            var delEdge = new List<HalfEdge>();
            //削除する頂点
            var delVertex = new List<HalfEdgeVertex>();

            delVertex.Add(delV);

            //頂点とエッジの格納
            delEdge.Add(edge);
            delEdge.Add(edge.Next);
            delEdge.Add(edge.Next.Next);
            delMesh.Add(edge.Mesh);
            //頂点とエッジの格納
            delEdge.Add(edge.Opposite);
            delEdge.Add(edge.Opposite.Next);
            delEdge.Add(edge.Opposite.Next.Next);
            delMesh.Add(edge.Opposite.Mesh);
            #endregion

            //頂点が削除対象なら、残す方に移動
            foreach (var around in delV.AroundEdge)
            {
                if (around.Start == delV)
                {
                    around.Start = remV;
                    around.Start.AddEdge(around);
                }

                if (around.Before.End == delV)
                {
                    around.Before.End = remV;
                }
            }

            remV.Position = newPosition;
            //エッジ情報の切り替え
            Analyzer.HalfEdge.SetupOpposite(edge.Next.Opposite, edge.Before.Opposite);
            Analyzer.HalfEdge.SetupOpposite(edge.Opposite.Next.Opposite, edge.Opposite.Before.Opposite);

            foreach (var dedge in delEdge)
            {
                dedge.Dispose();
            }

            foreach (var dmesh in delMesh)
            {
                dmesh.Dispose();
            }

            foreach (var dvertex in delVertex)
            {
                dvertex.Dispose();
            }

            deleteVertexs.AddRange(delVertex);
            deleteEdges.AddRange(delEdge);
            deleteMeshs.AddRange(delMesh);

            return remV;
        }
        #endregion

        /// <summary>
        /// エッジの間に頂点の追加
        /// 既存のエッジは削除フラグを立てておく。
        /// 削除はEndEditで行う。
        /// </summary>
        /// <param name="edge">エッジ</param>
        /// <param name="position">追加する点の位置</param>
        /// <param name="newSplitedEdge">分割したエッジ</param>
        /// <param name="newCreateEdge">作成したエッジ</param>
        public void EdgeSplit(HalfEdge edge, Vector3 position, out HalfEdge[] newSplitedEdge, out HalfEdge[] newCreateEdge)
        {
            if (!NowEdit)
            {
                Logger.Log(Logger.LogLevel.Warning, "Call StartEdit");
                newSplitedEdge = null;
                newCreateEdge = null;
                return;
            }

            var opposite = edge.Opposite;
            var delMesh1 = edge.Mesh;
            var delMesh2 = opposite.Mesh;

            var vertex = new HalfEdgeVertex(position, HalfEdge.Vertexs.Count);

            // 分割するエッジ
            var right = new HalfEdge(vertex, edge.End, HalfEdge.HalfEdges.Count);
            var oppoRight = new HalfEdge(edge.End, vertex, HalfEdge.HalfEdges.Count + 1);
            Analyzer.HalfEdge.SetupOpposite(right, oppoRight);

            var left = new HalfEdge(edge.Start, vertex, HalfEdge.HalfEdges.Count + 2);
            var oppoLeft = new HalfEdge(vertex, edge.Start, HalfEdge.HalfEdges.Count + 3);
            Analyzer.HalfEdge.SetupOpposite(left, oppoLeft);

            // 新規に作成するエッジ
            var up = new HalfEdge(vertex, edge.Next.End, HalfEdge.HalfEdges.Count + 4);
            var oppoup = new HalfEdge(edge.Next.End, vertex, HalfEdge.HalfEdges.Count + 5);
            Analyzer.HalfEdge.SetupOpposite(up, oppoup);

            var down = new HalfEdge(vertex, opposite.Next.End, edge.Index);
            var oppodown = new HalfEdge(opposite.Next.End, vertex, opposite.Index);
            Analyzer.HalfEdge.SetupOpposite(down, oppodown);

            var rightUp = new HalfEdgeMesh(right, edge.Next, oppoup, delMesh1.Index);
            var leftUp = new HalfEdgeMesh(up, edge.Before, left, delMesh2.Index);
            var rightDown = new HalfEdgeMesh(down, opposite.Before, oppoRight, HalfEdge.HalfEdgeMeshs.Count);
            var leftDown = new HalfEdgeMesh(oppoLeft, opposite.Next, oppodown, HalfEdge.HalfEdgeMeshs.Count + 1);

            HalfEdge.Vertexs.Add(vertex);

            HalfEdge.HalfEdges.Add(right);  HalfEdge.HalfEdges.Add(oppoRight);
            HalfEdge.HalfEdges.Add(left);   HalfEdge.HalfEdges.Add(oppoLeft);
            HalfEdge.HalfEdges.Add(up);     HalfEdge.HalfEdges.Add(oppoup);
            HalfEdge.HalfEdges.Add(down);   HalfEdge.HalfEdges.Add(oppodown);

            HalfEdge.HalfEdgeMeshs.Add(rightUp);
            HalfEdge.HalfEdgeMeshs.Add(leftUp);
            HalfEdge.HalfEdgeMeshs.Add(rightDown);
            HalfEdge.HalfEdgeMeshs.Add(leftDown);

            edge.Dispose(); opposite.Dispose();
            deleteEdges.Add(edge); deleteEdges.Add(opposite);

            delMesh1.Dispose(); delMesh2.Dispose();
            deleteMeshs.Add(delMesh1); deleteMeshs.Add(delMesh2);

            newSplitedEdge = new HalfEdge[] { right, oppoRight, left, oppoLeft };
            newCreateEdge = new HalfEdge[] { up, oppoup, down, oppodown };

            //HasError();
        }

        /// <summary>
        /// エッジの入れ替え
        /// </summary>
        /// <param name="edge">入れ替えるエッジ</param>
        public void EdgeFlips(HalfEdge edge)
        {
            if (!NowEdit)
            {
                Logger.Log(Logger.LogLevel.Warning, "Call StartEdit");
                return;
            }

            if (!CanEdgeFlips(edge))
            {
                return;
            }
            // delete edge & mesh
            var opposite = edge.Opposite;
            var delMesh1 = edge.Mesh;
            var delMesh2 = opposite.Mesh;

            var startPos = edge.Next.End;
            var endPos = opposite.Next.End;

            var createEdge = new HalfEdge(startPos, endPos, edge.Index);
            var createEdgeOpposite = new HalfEdge(endPos, startPos, opposite.Index);
            var createMesh = new HalfEdgeMesh(createEdge, opposite.Before, edge.Next, delMesh1.Index);
            var createMeshOpposite = new HalfEdgeMesh(createEdgeOpposite, edge.Before, opposite.Next, delMesh2.Index);
            Analyzer.HalfEdge.SetupOpposite(createEdge, createEdgeOpposite);

            HalfEdge.HalfEdgeMeshs.Add(createMesh);
            HalfEdge.HalfEdgeMeshs.Add(createMeshOpposite);
            HalfEdge.HalfEdges.Add(createEdge);
            HalfEdge.HalfEdges.Add(createEdgeOpposite);

            edge.Dispose(); opposite.Dispose();
            deleteEdges.Add(edge); deleteEdges.Add(opposite);

            delMesh1.Dispose(); delMesh2.Dispose();
            deleteMeshs.Add(delMesh1); deleteMeshs.Add(delMesh2);
            //HasError();
        }
        #endregion

        /// <summary>
        /// EdgeFlipsできるか
        /// すでに同一エッジがある場合はできない。(cgjems情報 ifdef false内)
        /// (判定条件足りなさそうなので以下方法で)
        /// 交換前のエッジの両端点と、交換後のエッジの中点のベクトルが
        /// 両方内向きならできる。片方外向きならできない。
        /// 両方外向きにはなりえないからできない（90度はありうるのでtrue）
        /// 単点と中点のベクトル2つの内積が正ならできる
        /// </summary>
        /// <param name="edge">交換するエッジ</param>
        /// <returns>できる</returns>
        private bool CanEdgeFlips(HalfEdge edge)
        {
#if false
            var createStart = edge.Next.End;
            var createEnd = edge.Opposite.Next.End;

            foreach (var vertex in edge.Start.AroundVertex)
            {
                foreach (var arouond in vertex.AroundEdge)
                {
                    if (arouond.Start == createStart && arouond.End == createEnd)
                    {
                        return false;
                    }

                    if (arouond.Start == createEnd && arouond.End == createStart)
                    {
                        return false;
                    }
                }
            }

            return true;

#endif
            var createStar = edge.Next.End;
            var createEnd = edge.Opposite.Next.End;
            var middle = (createStar.Position + createEnd.Position) / 2;
            if (Vector3.Dot(middle - edge.Start.Position, middle - edge.End.Position) < 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// エッジを削除できるか
        /// edge.Startと、edge.Endを持つ頂点が3つ以上あったらfalse
        /// </summary>
        /// <param name="edge">エッジ</param>
        /// <returns>できる</returns>
        private bool CanEdgeCollapse(HalfEdge edge)
        {
            int commonNum = 0;
            foreach (var neightVertex in edge.Start.AroundVertex)
            {
                bool existStart = false;
                bool existEnd = false;
                //隣接する頂点の周囲のエッジ
                foreach (var around in neightVertex.AroundEdge)
                {
                    if (around.End == edge.Start)
                    {
                        existStart = true;
                    }

                    if (around.End == edge.End)
                    {
                        existEnd = true;
                    }
                }

                if (existStart && existEnd)
                {
                    commonNum++;
                }
            }

            if (commonNum > 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

#region [delete object]

        /// <summary>
        /// エラーを持っているかどうか
        /// </summary>
        /// <param name="vertex">頂点</param>
        /// <returns>エラーがあるかどうか</returns>
        private bool HasError(HalfEdgeVertex vertex)
        {
            foreach (var edge in HalfEdge.HalfEdges)
            {
                if (edge.HasVertex(vertex))
                {
                    Logger.Log(Logger.LogLevel.Error, "error");
                }
            }

            foreach (var mesh in HalfEdge.HalfEdgeMeshs)
            {
                if (mesh.HasVertex(vertex))
                {
                    Logger.Log(Logger.LogLevel.Error, "error");
                }
            }

            return false;
        }

        /// <summary>
        /// エラーがあるか
        /// </summary>
        /// <returns>ある</returns>
        public bool HasError()
        {
            int errorCounter = 0;
            foreach (var edge in HalfEdge.HalfEdges)
            {
                if (edge.ErrorEdge)
                {
                    Logger.Log(Logger.LogLevel.Error, "Edge : HasError");
                    errorCounter++;
                }
            }

            foreach (var mesh in HalfEdge.HalfEdgeMeshs)
            {
                if (mesh.ErrorMesh)
                {
                    Logger.Log(Logger.LogLevel.Error, "Mesh : HasError");
                    errorCounter++;
                }
            }

            foreach (var vertex in HalfEdge.HalfEdgeVertexs)
            {
                if (vertex.ErrorVertex)
                {
                    Logger.Log(Logger.LogLevel.Error, "Vertex : HasError");
                    errorCounter++;
                }
            }

            if (errorCounter == 0)
            {
                return false;
            }
            else
            {
                Logger.Log(Logger.LogLevel.Error, "ErrorCount " + errorCounter.ToString());
                return true;
            }
        }
#endregion
    }
}
