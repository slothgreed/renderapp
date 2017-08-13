using System.Collections.Generic;
using KI.Foundation.Utility;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// ポリへドロン
    /// </summary>
    public class Polyhedron
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Polyhedron()
        {
        }

        public List<Mesh> Meshs { get; set; } = new List<Mesh>();
        public List<HalfEdge> Edges { get; set; } = new List<HalfEdge>();
        public List<Vertex> Vertexs { get; set; } = new List<Vertex>();
        public List<int> Indexs { get; set; } = new List<int>();

        /// <summary>
        /// メッシュのインデックスと、頂点を受け取る
        /// </summary>
        /// <param name="position">頂点座標リスト</param>
        /// <param name="polyIndex">「三角形を構成する頂点番号を格納したVector3」のリスト</param>
        public Polyhedron(List<Vector3> position, List<int> polyIndex = null)
        {
            Initialize(position, polyIndex);
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

        /// <summary>
        /// エッジの取得
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        /// <returns>エッジ</returns>
        public HalfEdge GetEdge(Vertex start, Vertex end)
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
        /// 面の作成
        /// </summary>
        /// <param name="v1">頂点1</param>
        /// <param name="v2">頂点2</param>
        /// <param name="v3">頂点3</param>
        private void CreateMesh(Vertex v1, Vertex v2, Vertex v3)
        {
            Mesh mesh = new Mesh(Meshs.Count);
            HalfEdge edge1 = new HalfEdge(mesh, v1, v2, Edges.Count);
            HalfEdge edge2 = new HalfEdge(mesh, v2, v3, Edges.Count + 1);
            HalfEdge edge3 = new HalfEdge(mesh, v3, v1, Edges.Count + 2);

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
        private void SetOppositeEdge2(HalfEdge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;
            //反対の頂点のエッジループ
            foreach (var opposite in end.AroundEdge)
            {
                if (opposite.Start == end && opposite.End == start)
                {
                    HalfEdge.SetupOpposite(opposite, edge);
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
            HalfEdge opposite;
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
