#define  CHECKHALFEDGE

using System.Collections.Generic;
using KI.Foundation.Utility;
using OpenTK;

namespace KI.Analyzer
{
    public class HalfEdge
    {
        public List<Mesh> meshs = new List<Mesh>();
        public List<Edge> edges = new List<Edge>();
        public List<Vertex> vertexs = new List<Vertex>();
        public List<int> indexs = new List<int>();

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
            foreach (var vertex in vertexs)
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

            for (int i = 0; i < meshs.Count; i++)
            {
                meshs[i].Index = i;
            }

            for (int i = 0; i < edges.Count; i++)
            {
                edges[i].Index = i;
            }

            for (int i = 0; i < vertexs.Count; i++)
            {
                vertexs[i].Index = i;
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

            var vertex = new Vertex((edge.Start.Position + edge.End.Position) / 2, vertexs.Count);

            var right = new Edge(vertex, edge.End, edges.Count);
            var oppoRight = new Edge(edge.End, vertex, edges.Count + 1);
            Edge.SetupOpposite(right, oppoRight);

            var left = new Edge(edge.Start, vertex, edges.Count + 2);
            var oppoLeft = new Edge(vertex, edge.Start, edges.Count + 3);
            Edge.SetupOpposite(left, oppoLeft);

            var up = new Edge(vertex, edge.Next.End, edges.Count + 4);
            var oppoup = new Edge(edge.Next.End, vertex, edges.Count + 5);
            Edge.SetupOpposite(up, oppoup);

            var down = new Edge(vertex, opposite.Next.End, edge.Index);
            var oppodown = new Edge(opposite.Next.End, vertex, opposite.Index);
            Edge.SetupOpposite(down, oppodown);

            var rightUp = new Mesh(right, edge.Next, oppoup, delMesh1.Index);
            var leftUp = new Mesh(up, edge.Before, left, delMesh2.Index);
            var rightDown = new Mesh(down, opposite.Before, oppoRight, meshs.Count);
            var leftDown = new Mesh(oppoLeft, opposite.Next, oppodown, meshs.Count + 1);

            vertexs.Add(vertex);

            edges.Add(right); edges.Add(oppoRight);
            edges.Add(left); edges.Add(oppoLeft);
            edges.Add(up); edges.Add(oppoup);
            edges.Add(down); edges.Add(oppodown);

            meshs.Add(rightUp);
            meshs.Add(leftUp);
            meshs.Add(rightDown);
            meshs.Add(leftDown);

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

            meshs.Add(createMesh);
            meshs.Add(createMeshOpposite);
            edges.Add(createEdge);
            edges.Add(createEdgeOpposite);

            DeleteEdge(new List<Edge>() { edge, opposite });
            DeleteMesh(new List<Mesh>() { delMesh1, delMesh2 });

# if CHECKHALFEDGE
            HasError();
#endif
        }
        #endregion
        public bool HasError()
        {
            foreach (var edge in edges)
            {
                if (edge.ErrorEdge)
                {
                    Logger.Log(Logger.LogLevel.Error, "Edge : HasError");
                    return true;
                }
            }

            foreach (var mesh in meshs)
            {
                if (mesh.ErrorMesh)
                {
                    Logger.Log(Logger.LogLevel.Error, "Mesh : HasError");
                    return true;
                }
            }

            foreach (var vertex in vertexs)
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
                meshs.Remove(mesh);
            }
        }

        private void DeleteEdge(List<Edge> deleteEdge)
        {
            foreach (var edge in deleteEdge)
            {
                edge.Dispose();
                edges.Remove(edge);
            }
        }

        private void DeleteVertex(List<Vertex> deleteVertex)
        {
            //エッジ削除
            foreach (var vertex in deleteVertex)
            {
                vertex.Dispose();
                vertexs.Remove(vertex);
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
                vertexs.Add(vertex);
            }
        }

        /// <summary>
        /// 頂点インデックスの作成
        /// </summary>
        private void CreateVertexIndex()
        {
            foreach (var mesh in meshs)
            {
                foreach (var index in mesh.AroundVertex)
                {
                    indexs.Add(index.Index);
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
                Vertex vertex = vertexs.Find(p => p.Position == vertex_List[i]);
                if (vertex == null)
                {
                    vertex = new Vertex(vertex_List[i], vertexs.Count);
                    vertexs.Add(vertex);
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

        private void CreateMesh(Vertex v1, Vertex v2, Vertex v3)
        {
            Mesh mesh = new Mesh(meshs.Count);
            Edge edge1 = new Edge(mesh, v1, v2, edges.Count);
            Edge edge2 = new Edge(mesh, v2, v3, edges.Count + 1);
            Edge edge3 = new Edge(mesh, v3, v1, edges.Count + 2);

            //メッシュにエッジを格納
            mesh.SetEdge(edge1, edge2, edge3);
            meshs.Add(mesh);

            edges.Add(edge1);
            edges.Add(edge2);
            edges.Add(edge3);
        }

        /// <summary>
        /// 頂点番号を持つエッジと面を生成
        /// </summary>
        private void MakeEdgeListByVertexIndex(List<int> poly_Index)
        {
            int poly_Num = poly_Index.Count / 3;
            for (int num = 0; num < poly_Num; num++)
            {
                Vertex v1 = vertexs[poly_Index[3 * num]];
                Vertex v2 = vertexs[poly_Index[3 * num + 1]];
                Vertex v3 = vertexs[poly_Index[3 * num + 2]];
                CreateMesh(v1, v2, v3);
            }
        }

        /// <summary>
        /// 反対エッジのセット
        /// </summary>
        private void SetOppositeEdge()
        {
            foreach (var vertex in vertexs)
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
        /// <param name="index">元となるエッジ</param>
        /// <returns></returns>
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
        /// <returns></returns>
        private bool CheckOppositeEdge()
        {
            int ok_flag = 0;
            Edge opposite;
            foreach (var edge in edges)
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
