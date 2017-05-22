#define  CHECKHALFEDGE

using System.Collections.Generic;
using OpenTK;
using KI.Foundation.Utility;
using KI.Gfx.KIAsset;
namespace KI.Gfx.Analyzer
{
    public class HalfEdge
    {
        public List<Mesh> m_Mesh = new List<Mesh>();
        public List<Edge> m_Edge = new List<Edge>();
        public List<Vertex> m_Vertex = new List<Vertex>();
        public List<int> m_Index = new List<int>();
        
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

        public HalfEdge(GeometryInfo geometry)
        {
            Initialize(geometry.Position, geometry.Index);
        }
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

        /// <summary>
        /// ハーフエッジがエラーを持つかチェック
        /// </summary>
        /// <returns></returns>
        public bool ErrorHalfEdge()
        {
            foreach(var vertex in m_Vertex)
            {
                if(vertex.ErrorVertex)
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
        /// <param name="removeIndex">削除するほう</param>
        /// <param name="remainIndex">残すほう</param>
        public void VertexDecimation(Vertex delV, Vertex remV)
        {
            if (delV == null || remV == null)
            {
                return;
            }
            //2点を端点とするエッジ
            Edge commonEdge = GetEdge(delV, remV);
            if (commonEdge == null)
            {
                return;
            }

            #region [create delete list]
            //削除するメッシュ
            var deleteMesh = new List<Mesh>();
            //削除するエッジ
            var deleteEdge = new List<Edge>();
            //削除する頂点
            var deleteVertex = new List<Vertex>();

            deleteVertex.Add(delV);


            //頂点とエッジの格納
            deleteEdge.Add(commonEdge);
            deleteEdge.Add(commonEdge.Next);
            deleteEdge.Add(commonEdge.Next.Next);
            deleteMesh.Add(commonEdge.Mesh);
            //頂点とエッジの格納
            deleteEdge.Add(commonEdge.Opposite);
            deleteEdge.Add(commonEdge.Opposite.Next);
            deleteEdge.Add(commonEdge.Opposite.Next.Next);
            deleteMesh.Add(commonEdge.Opposite.Mesh);
            #endregion

            //頂点が削除対象なら、残す方に移動
            foreach (var edge in delV.AroundEdge)
            {
                if (edge.Start == delV)
                {
                    edge.Start = remV;
                    edge.Opposite.End = remV;
                }
            }

            //エッジ情報の切り替え
            commonEdge.Next.Opposite.Opposite = commonEdge.Before.Opposite;
            commonEdge.Before.Opposite.Opposite = commonEdge.Next.Opposite;

            commonEdge.Opposite.Next.Opposite.Opposite = commonEdge.Opposite.Before.Opposite;
            commonEdge.Opposite.Before.Opposite.Opposite = commonEdge.Opposite.Next.Opposite;

            DeleteMesh(deleteMesh);
            DeleteEdge(deleteEdge);
            DeleteVertex(deleteVertex);
        }
        #endregion

        public void EdgeSplit(Edge edge)
        {
            var opposite = edge.Opposite;
            var delMesh1 = edge.Mesh;
            var delMesh2 = opposite.Mesh;

            var vertex = new Vertex((edge.Start.Position + edge.End.Position) / 2, m_Vertex.Count);

            var right = new Edge(vertex, edge.End, m_Edge.Count);
            var oppoRight = new Edge(edge.End, vertex, m_Edge.Count + 1);
            Edge.SetupOpposite(right, oppoRight);

            var left = new Edge(edge.Start, vertex, m_Edge.Count + 2);
            var oppoLeft = new Edge(vertex, edge.Start, m_Edge.Count + 3);
            Edge.SetupOpposite(left, oppoLeft);

            var up = new Edge(vertex, edge.Next.End, m_Edge.Count + 4);
            var oppoup = new Edge(edge.Next.End, vertex, m_Edge.Count + 5);
            Edge.SetupOpposite(up, oppoup);

            var down = new Edge(vertex, opposite.Next.End, m_Edge.Count + 6);
            var oppodown = new Edge(opposite.Next.End, vertex, m_Edge.Count + 7);
            Edge.SetupOpposite(down, oppodown);

            var rightUp = new Mesh(right, edge.Next, oppoup, delMesh1.Index);
            var leftUp = new Mesh(up, edge.Before, left, delMesh2.Index);
            var rightDown = new Mesh(down, opposite.Before, oppoRight, m_Mesh.Count);
            var leftDown = new Mesh(oppoLeft, opposite.Next, oppodown, m_Mesh.Count + 1);

            m_Vertex.Add(vertex);

            m_Edge.Add(right); m_Edge.Add(oppoRight);
            m_Edge.Add(left); m_Edge.Add(oppoLeft);
            m_Edge.Add(up); m_Edge.Add(oppoup);
            m_Edge.Add(down); m_Edge.Add(oppodown);

            m_Mesh.Add(rightUp);
            m_Mesh.Add(leftUp);
            m_Mesh.Add(rightDown);
            m_Mesh.Add(leftDown);

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

            m_Mesh.Add(createMesh);
            m_Mesh.Add(createMeshOpposite);
            m_Edge.Add(createEdge);
            m_Edge.Add(createEdgeOpposite);

            DeleteEdge(new List<Edge>() { edge, opposite });
            DeleteMesh(new List<Mesh>() { delMesh1, delMesh2 });

# if CHECKHALFEDGE
            HasError();
#endif
        }
        #endregion
        public bool HasError()
        {
            foreach(var edge in m_Edge)
            {
                if(edge.ErrorEdge)
                {
                    Logger.Log(Logger.LogLevel.Error, "Edge : HasError");
                    return true;
                }
            }
            foreach (var mesh in m_Mesh)
            {
                if (mesh.ErrorMesh)
                {
                    Logger.Log(Logger.LogLevel.Error, "Mesh : HasError");
                    return true;
                }
            }
            foreach (var vertex in m_Vertex)
            {
                if (vertex.ErrorVertex)
                {
                    Logger.Log(Logger.LogLevel.Error, "Vertex : HasError");
                    return true;
                }
            }
            return false;
        }
        #region [delete object]
        private void DeleteMesh(List<Mesh> deleteMesh)
        {
            foreach (var mesh in deleteMesh)
            {
                mesh.Dispose();
                m_Mesh.Remove(mesh);
            }
        }
        private void DeleteEdge(List<Edge> deleteEdge)
        {
            foreach (var edge in deleteEdge)
            {
                edge.Dispose();
                m_Edge.Remove(edge);
            }
        }
        private void DeleteVertex(List<Vertex> deleteVertex)
        {
            //エッジ削除
            foreach (var vertex in deleteVertex)
            {
                vertex.Dispose();
                m_Vertex.Remove(vertex);
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
                m_Vertex.Add(vertex);
            }
        }

        public GeometryInfo CreateGeometryInfo()
        {
            var geometry = new GeometryInfo();
            var gray = new Vector3(0.8f);
            foreach (var vertex in m_Vertex)
            {
                geometry.Position.Add(vertex.Position);
                geometry.Normal.Add(vertex.Normal);
                geometry.Color.Add(gray);

            }
            foreach (var mesh in m_Mesh)
            {
                foreach (var vertex in mesh.AroundVertex)
                {
                    geometry.Index.Add(vertex.Index);
                }
            }
            geometry.GeometryType = GeometryType.Triangle;
            return geometry;

        }
        /// <summary>
        /// 頂点インデックスの作成
        /// </summary>
        private void CreateVertexIndex()
        {
            foreach (var mesh in m_Mesh)
            {
                foreach (var index in mesh.AroundVertex)
                {
                    m_Index.Add(index.Index);
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
                Vertex vertex = m_Vertex.Find(p => p.Position == vertex_List[i]);
                if (vertex == null)
                {
                    vertex = new Vertex(vertex_List[i], m_Vertex.Count);
                    m_Vertex.Add(vertex);
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
            Mesh mesh = new Mesh(m_Mesh.Count);
            Edge edge1 = new Edge(mesh, v1, v2, m_Edge.Count);
            Edge edge2 = new Edge(mesh, v2, v3, m_Edge.Count + 1);
            Edge edge3 = new Edge(mesh, v3, v1, m_Edge.Count + 2);

            //メッシュにエッジを格納
            mesh.SetEdge(edge1, edge2, edge3);
            m_Mesh.Add(mesh);

            m_Edge.Add(edge1);
            m_Edge.Add(edge2);
            m_Edge.Add(edge3);
        }
        /// <summary>
        /// 頂点番号を持つエッジと面を生成
        /// </summary>
        private void MakeEdgeListByVertexIndex(List<int> poly_Index)
        {
            int poly_Num = poly_Index.Count / 3;
            for (int num = 0; num < poly_Num; num++)
            {
                Vertex v1 = m_Vertex[poly_Index[3 * num]];
                Vertex v2 = m_Vertex[poly_Index[3 * num + 1]];
                Vertex v3 = m_Vertex[poly_Index[3 * num + 2]];
                CreateMesh(v1, v2, v3);
                
            }
        }
        /// <summary>
        /// 反対エッジのセット
        /// </summary>
        private void SetOppositeEdge()
        {
            foreach(var vertex in m_Vertex)
            {
                foreach(var edge in vertex.AroundEdge)
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
            foreach (var edge in m_Edge)
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
        /// <param name="distance"></param>
        /// <returns></returns>
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
