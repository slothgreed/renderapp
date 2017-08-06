﻿#define  CHECKHALFEDGE

using System.Collections.Generic;
using KI.Foundation.Utility;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// ハーフエッジ
    /// </summary>
    public class HalfEdge
    {
        public List<Mesh> Meshs = new List<Mesh>();
        public List<Edge> Edges = new List<Edge>();
        public List<Vertex> Vertexs = new List<Vertex>();
        public List<int> Indexs = new List<int>();

        public HalfEdge()
        {
        }

        /// <summary>
        /// メッシュのインデックスと、頂点を受け取る
        /// </summary>
        /// <param name="position">頂点座標リスト</param>
        /// <param name="mesh">「三角形を構成する頂点番号を格納したVector3」のリスト</param>
        public HalfEdge(List<Vector3> position, List<int> poly_Index = null)
        {
            Initialize(position, poly_Index);
        }

        /// <summary>
        /// ハーフエッジがエラーを持つかチェック
        /// </summary>
        /// <returns></returns>
        public bool ErrorHalfEdge()
        {
            foreach (var vertex in Vertexs)
            {
                if (vertex.ErrorVertex)
                {
                    return true;
                }
            }

            return false;
        }

        #region [edit method]
        #region [vertex decimation]
        /// <summary>
        /// マージによる頂点削除削除後、頂点位置移動
        /// </summary>
        public void VertexDecimation(Edge edge)
        {
            Vertex delV = edge.Start;
            Vertex remV = edge.End;

            #region [create delete list]
            //削除するメッシュ
            var deleteMesh = new List<Mesh>();
            //削除するエッジ
            var deleteEdge = new List<Edge>();
            //削除する頂点
            var deleteVertex = new List<Vertex>();

            deleteVertex.Add(delV);

            //頂点とエッジの格納
            deleteEdge.Add(edge);
            deleteEdge.Add(edge.Next);
            deleteEdge.Add(edge.Next.Next);
            deleteMesh.Add(edge.Mesh);
            //頂点とエッジの格納
            deleteEdge.Add(edge.Opposite);
            deleteEdge.Add(edge.Opposite.Next);
            deleteEdge.Add(edge.Opposite.Next.Next);
            deleteMesh.Add(edge.Opposite.Mesh);
            #endregion

            //頂点が削除対象なら、残す方に移動
            foreach (var around in delV.AroundEdge)
            {
                if (around.Start == delV)
                {
                    around.Start = remV;
                    around.Opposite.End = remV;
                }
            }

            //remV.Position = (remV.Position + delV.Position) / 2;
            //エッジ情報の切り替え
            Edge.SetupOpposite(edge.Next.Opposite, edge.Before.Opposite);
            Edge.SetupOpposite(edge.Opposite.Next.Opposite, edge.Opposite.Before.Opposite);

            DeleteMesh(deleteMesh);
            DeleteEdge(deleteEdge);
            DeleteVertex(deleteVertex);

            for (int i = 0; i < Meshs.Count; i++)
            {
                Meshs[i].Index = i;
            }

            for (int i = 0; i < Edges.Count; i++)
            {
                Edges[i].Index = i;
            }

            for (int i = 0; i < Vertexs.Count; i++)
            {
                Vertexs[i].Index = i;
            }

# if CHECKHALFEDGE
            HasError();
#endif
        }
        #endregion

        public void EdgeSplit(Edge edge)
        {
            var opposite = edge.Opposite;
            var delMesh1 = edge.Mesh;
            var delMesh2 = opposite.Mesh;

            var vertex = new Vertex((edge.Start.Position + edge.End.Position) / 2, Vertexs.Count);

            var right = new Edge(vertex, edge.End, Edges.Count);
            var oppoRight = new Edge(edge.End, vertex, Edges.Count + 1);
            Edge.SetupOpposite(right, oppoRight);

            var left = new Edge(edge.Start, vertex, Edges.Count + 2);
            var oppoLeft = new Edge(vertex, edge.Start, Edges.Count + 3);
            Edge.SetupOpposite(left, oppoLeft);

            var up = new Edge(vertex, edge.Next.End, Edges.Count + 4);
            var oppoup = new Edge(edge.Next.End, vertex, Edges.Count + 5);
            Edge.SetupOpposite(up, oppoup);

            var down = new Edge(vertex, opposite.Next.End, edge.Index);
            var oppodown = new Edge(opposite.Next.End, vertex, opposite.Index);
            Edge.SetupOpposite(down, oppodown);

            var rightUp = new Mesh(right, edge.Next, oppoup, delMesh1.Index);
            var leftUp = new Mesh(up, edge.Before, left, delMesh2.Index);
            var rightDown = new Mesh(down, opposite.Before, oppoRight, Meshs.Count);
            var leftDown = new Mesh(oppoLeft, opposite.Next, oppodown, Meshs.Count + 1);

            Vertexs.Add(vertex);

            Edges.Add(right); Edges.Add(oppoRight);
            Edges.Add(left); Edges.Add(oppoLeft);
            Edges.Add(up); Edges.Add(oppoup);
            Edges.Add(down); Edges.Add(oppodown);

            Meshs.Add(rightUp);
            Meshs.Add(leftUp);
            Meshs.Add(rightDown);
            Meshs.Add(leftDown);

            DeleteEdge(new List<Edge>() { edge, opposite });
            DeleteMesh(new List<Mesh>() { delMesh1, delMesh2 });

# if CHECKHALFEDGE
            //HasError();
#endif
        }

        public void EdgeFlips(Edge edge)
        {
            // delete edge & mesh
            var opposite = edge.Opposite;
            var delMesh1 = edge.Mesh;
            var delMesh2 = opposite.Mesh;

            var startPos = edge.Next.End;
            var endPos = opposite.Next.End;

            var createEdge = new Edge(startPos, endPos, edge.Index);
            var createEdgeOpposite = new Edge(endPos, startPos, opposite.Index);
            var createMesh = new Mesh(createEdge, opposite.Before, edge.Next, delMesh1.Index);
            var createMeshOpposite = new Mesh(createEdgeOpposite, edge.Before, opposite.Next, delMesh2.Index);
            Edge.SetupOpposite(createEdge, createEdgeOpposite);

            Meshs.Add(createMesh);
            Meshs.Add(createMeshOpposite);
            Edges.Add(createEdge);
            Edges.Add(createEdgeOpposite);

            DeleteEdge(new List<Edge>() { edge, opposite });
            DeleteMesh(new List<Mesh>() { delMesh1, delMesh2 });

# if CHECKHALFEDGE
            HasError();
#endif
        }
        #endregion

        /// <summary>
        /// エッジの取得
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        /// <returns>エッジ</returns>
        public Edge GetEdge(Vertex start, Vertex end)
        {
            foreach (var edge in start.AroundEdge)
            {
                if (edge.Start == start && edge.End == end)
                {
                    return edge;
                }
            }

            return null;
        }

        /// <summary>
        /// 一定距離内の頂点の取得
        /// </summary>
        /// <param name="vertex">起点</param>
        /// <param name="distance">距離</param>
        /// <returns>範囲内の頂点</returns>
        public List<Vertex> GetAroundVertex(Vertex vertex, float distance)
        {
            List<Vertex> vertex_list = new List<Vertex>();
            vertex.CalcFlag = true;
            vertex_list.Add(vertex);
            RecursiveAroundPosition(vertex_list, vertex, vertex, distance);
            for (int i = 0; i < vertex_list.Count; i++)
            {
                vertex_list[i].CalcFlag = false;
            }

            return vertex_list;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="position">頂点</param>
        /// <param name="poly_Index">頂点Index</param>
        private void Initialize(List<Vector3> position, List<int> poly_Index = null)
        {
            if (poly_Index == null || poly_Index.Count == 0)
            {
                CreateHalfEdgeData(position);
                CreateVertexIndex();
            }
            else
            {
                MakeVertexListByVertexIndex(position);
                MakeEdgeListByVertexIndex(poly_Index);
            }

            SetOppositeEdge();

            //for (int i = 0; i < m_Vertex.Count; i++ )
            //{
            //    VertexDecimation(m_Vertex[i].m_Edge.Start, m_Vertex[i].m_Edge.End);
            //}

            if (CheckOppositeEdge())
            {
                Logger.Log(Logger.LogLevel.Debug, "Create Half Edge OK!");
            }
        }

        #region [delete object]
        private void DeleteMesh(List<Mesh> deleteMesh)
        {
            foreach (var mesh in deleteMesh)
            {
                mesh.Dispose();
                Meshs.Remove(mesh);
            }
        }

        private void DeleteEdge(List<Edge> deleteEdge)
        {
            foreach (var edge in deleteEdge)
            {
                edge.Dispose();
                Edges.Remove(edge);
            }
        }

        private void DeleteVertex(List<Vertex> deleteVertex)
        {
            //エッジ削除
            foreach (var vertex in deleteVertex)
            {
                vertex.Dispose();
                Vertexs.Remove(vertex);
            }
        }
        #endregion

        #region [make halfedge data structure]
        /// <summary>
        /// ハーフエッジ用のリストに頂点を格納
        /// </summary>
        /// <param name="vertex_List"></param>
        /// <param name="checkOverlap">重複チェック</param>
        private void MakeVertexListByVertexIndex(List<Vector3> vertex_List)
        {
            for (int i = 0; i < vertex_List.Count; i++)
            {
                Vertex vertex = new Vertex(vertex_List[i], i);
                Vertexs.Add(vertex);
            }
        }

        /// <summary>
        /// 頂点インデックスの作成
        /// </summary>
        private void CreateVertexIndex()
        {
            foreach (var mesh in Meshs)
            {
                foreach (var index in mesh.AroundVertex)
                {
                    Indexs.Add(index.Index);
                }
            }
        }

        /// <summary>
        /// ハーフエッジデータ構造のために、重複する頂点情報を一つに.メッシュ情報も生成
        /// </summary>
        /// <param name="vertex_List"></param>
        private void CreateHalfEdgeData(List<Vector3> vertex_List)
        {
            Vertex v1 = null, v2 = null, v3 = null;
            //最終的にポリゴンに格納する頂点
            for (int i = 0; i < vertex_List.Count; i++)
            {
                //ないVertexを調査
                Vertex vertex = Vertexs.Find(p => p.Position == vertex_List[i]);
                if (vertex == null)
                {
                    vertex = new Vertex(vertex_List[i], Vertexs.Count);
                    Vertexs.Add(vertex);
                }

                if (v1 == null)
                {
                    v1 = vertex;
                }
                else if (v2 == null)
                {
                    v2 = vertex;
                }
                else
                {
                    v3 = vertex;
                    CreateMesh(v1, v2, v3);
                    v1 = null;
                    v2 = null;
                    v3 = null;
                }
            }
        }

        /// <summary>
        /// エラーがあるか
        /// </summary>
        /// <returns>ある</returns>
        private bool HasError()
        {
            foreach (var edge in Edges)
            {
                if (edge.ErrorEdge)
                {
                    Logger.Log(Logger.LogLevel.Error, "Edge : HasError");
                    return true;
                }
            }

            foreach (var mesh in Meshs)
            {
                if (mesh.ErrorMesh)
                {
                    Logger.Log(Logger.LogLevel.Error, "Mesh : HasError");
                    return true;
                }
            }

            foreach (var vertex in Vertexs)
            {
                if (vertex.ErrorVertex)
                {
                    Logger.Log(Logger.LogLevel.Error, "Vertex : HasError");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 面の作成
        /// </summary>
        /// <param name="v1">頂点1</param>
        /// <param name="v2">頂点2</param>
        /// <param name="v3">頂点3</param>
        private void CreateMesh(Vertex v1, Vertex v2, Vertex v3)
        {
            Mesh mesh = new Mesh(Meshs.Count);
            Edge edge1 = new Edge(mesh, v1, v2, Edges.Count);
            Edge edge2 = new Edge(mesh, v2, v3, Edges.Count + 1);
            Edge edge3 = new Edge(mesh, v3, v1, Edges.Count + 2);

            //メッシュにエッジを格納
            mesh.SetEdge(edge1, edge2, edge3);
            Meshs.Add(mesh);

            Edges.Add(edge1);
            Edges.Add(edge2);
            Edges.Add(edge3);
        }

        /// <summary>
        /// 頂点番号を持つエッジと面を生成
        /// </summary>
        private void MakeEdgeListByVertexIndex(List<int> poly_Index)
        {
            int poly_Num = poly_Index.Count / 3;
            for (int num = 0; num < poly_Num; num++)
            {
                Vertex v1 = Vertexs[poly_Index[3 * num]];
                Vertex v2 = Vertexs[poly_Index[3 * num + 1]];
                Vertex v3 = Vertexs[poly_Index[3 * num + 2]];
                CreateMesh(v1, v2, v3);
            }
        }

        /// <summary>
        /// 反対エッジのセット
        /// </summary>
        private void SetOppositeEdge()
        {
            foreach (var vertex in Vertexs)
            {
                foreach (var edge in vertex.AroundEdge)
                {
                    SetOppositeEdge2(edge);
                }
            }
        }

        /// <summary>
        /// 反対エッジの取得
        /// edge      op_edge
        /// 
        ///    |      |
        /// <--・<--->・-->
        ///    |      |
        ///   
        ///       ↑
        ///  共有エッジの取得
        /// </summary>
        /// <param name="edge">元となるエッジ</param>
        private void SetOppositeEdge2(Edge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;
            //反対の頂点のエッジループ
            foreach (var opposite in end.AroundEdge)
            {
                if (opposite.Start == end && opposite.End == start)
                {
                    Edge.SetupOpposite(opposite, edge);
                    break;
                }
            }
        }

        /// <summary>
        /// 反対エッジがきちんとできているかチェック
        /// </summary>
        /// <returns>正常か</returns>
        private bool CheckOppositeEdge()
        {
            int ok_flag = 0;
            Edge opposite;
            foreach (var edge in Edges)
            {
                opposite = edge.Opposite;
                if (edge == opposite)
                {
                    ok_flag++;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
        #region [getter method]

        /// <summary>
        /// 一定距離内の頂点の取得を行う再起関数
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="distance"></param>
        private void RecursiveAroundPosition(List<Vertex> vertex_list, Vertex vertex, Vertex startVertex, float distance)
        {
            float length;
            foreach (var aroundEdge in vertex.AroundEdge)
            {
                length = (startVertex - aroundEdge.End).Length;
                if (length < distance && (bool)aroundEdge.End.CalcFlag == false)
                {
                    aroundEdge.End.CalcFlag = true;
                    vertex_list.Add(aroundEdge.End);
                    RecursiveAroundPosition(vertex_list, aroundEdge.End, startVertex, distance);
                }
            }
        }
        #endregion
    }
}
