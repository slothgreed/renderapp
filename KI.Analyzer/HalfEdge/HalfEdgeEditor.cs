using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Utility;

namespace KI.Analyzer
{
    /// <summary>
    /// ハーフエッジを編集するクラス
    /// </summary>
    public class HalfEdgeEditor
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="half">ハーフエッジ</param>
        public HalfEdgeEditor(HalfEdge half)
        {
            HalfEdge = half;
        }

        /// <summary>
        /// ハーフエッジ
        /// </summary>
        public HalfEdge HalfEdge { get; private set; }

        #region [edit method]
        #region [vertex decimation]
        /// <summary>
        /// マージによる頂点削除削除後、頂点位置移動
        /// </summary>
        /// <param name="edge">削除するエッジ</param>
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

            for (int i = 0; i < HalfEdge.Meshs.Count; i++)
            {
                HalfEdge.Meshs[i].Index = i;
            }

            for (int i = 0; i < HalfEdge.Edges.Count; i++)
            {
                HalfEdge.Edges[i].Index = i;
            }

            for (int i = 0; i < HalfEdge.Vertexs.Count; i++)
            {
                HalfEdge.Vertexs[i].Index = i;
            }

            //HasError();
        }
        #endregion

        public void EdgeSplit(Edge edge)
        {
            var opposite = edge.Opposite;
            var delMesh1 = edge.Mesh;
            var delMesh2 = opposite.Mesh;

            var vertex = new Vertex((edge.Start.Position + edge.End.Position) / 2, HalfEdge.Vertexs.Count);

            var right = new Edge(vertex, edge.End, HalfEdge.Edges.Count);
            var oppoRight = new Edge(edge.End, vertex, HalfEdge.Edges.Count + 1);
            Edge.SetupOpposite(right, oppoRight);

            var left = new Edge(edge.Start, vertex, HalfEdge.Edges.Count + 2);
            var oppoLeft = new Edge(vertex, edge.Start, HalfEdge.Edges.Count + 3);
            Edge.SetupOpposite(left, oppoLeft);

            var up = new Edge(vertex, edge.Next.End, HalfEdge.Edges.Count + 4);
            var oppoup = new Edge(edge.Next.End, vertex, HalfEdge.Edges.Count + 5);
            Edge.SetupOpposite(up, oppoup);

            var down = new Edge(vertex, opposite.Next.End, edge.Index);
            var oppodown = new Edge(opposite.Next.End, vertex, opposite.Index);
            Edge.SetupOpposite(down, oppodown);

            var rightUp = new Mesh(right, edge.Next, oppoup, delMesh1.Index);
            var leftUp = new Mesh(up, edge.Before, left, delMesh2.Index);
            var rightDown = new Mesh(down, opposite.Before, oppoRight, HalfEdge.Meshs.Count);
            var leftDown = new Mesh(oppoLeft, opposite.Next, oppodown, HalfEdge.Meshs.Count + 1);

            HalfEdge.Vertexs.Add(vertex);

            HalfEdge.Edges.Add(right); HalfEdge.Edges.Add(oppoRight);
            HalfEdge.Edges.Add(left); HalfEdge.Edges.Add(oppoLeft);
            HalfEdge.Edges.Add(up); HalfEdge.Edges.Add(oppoup);
            HalfEdge.Edges.Add(down); HalfEdge.Edges.Add(oppodown);

            HalfEdge.Meshs.Add(rightUp);
            HalfEdge.Meshs.Add(leftUp);
            HalfEdge.Meshs.Add(rightDown);
            HalfEdge.Meshs.Add(leftDown);

            DeleteEdge(new List<Edge>() { edge, opposite });
            DeleteMesh(new List<Mesh>() { delMesh1, delMesh2 });

            //HasError();
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

            HalfEdge.Meshs.Add(createMesh);
            HalfEdge.Meshs.Add(createMeshOpposite);
            HalfEdge.Edges.Add(createEdge);
            HalfEdge.Edges.Add(createEdgeOpposite);

            DeleteEdge(new List<Edge>() { edge, opposite });
            DeleteMesh(new List<Mesh>() { delMesh1, delMesh2 });

            //HasError();
        }
        #endregion

        #region [delete object]
        /// <summary>
        /// メッシュ削除
        /// </summary>
        /// <param name="deleteMesh">削除するメッシュ</param>
        private void DeleteMesh(List<Mesh> deleteMesh)
        {
            foreach (var mesh in deleteMesh)
            {
                mesh.Dispose();
                HalfEdge.Meshs.Remove(mesh);
            }
        }

        /// <summary>
        /// エッジ削除
        /// </summary>
        /// <param name="deleteMesh">削除するエッジ</param>
        private void DeleteEdge(List<Edge> deleteEdge)
        {
            foreach (var edge in deleteEdge)
            {
                edge.Dispose();
                HalfEdge.Edges.Remove(edge);
            }
        }

        /// <summary>
        /// 頂点削除
        /// </summary>
        /// <param name="deleteVertex">削除する頂点</param>
        private void DeleteVertex(List<Vertex> deleteVertex)
        {
            //エッジ削除
            foreach (var vertex in deleteVertex)
            {
                vertex.Dispose();
                HalfEdge.Vertexs.Remove(vertex);
            }
        }

        /// <summary>
        /// エラーがあるか
        /// </summary>
        /// <returns>ある</returns>
        private bool HasError()
        {
            foreach (var edge in HalfEdge.Edges)
            {
                if (edge.ErrorEdge)
                {
                    Logger.Log(Logger.LogLevel.Error, "Edge : HasError");
                    return true;
                }
            }

            foreach (var mesh in HalfEdge.Meshs)
            {
                if (mesh.ErrorMesh)
                {
                    Logger.Log(Logger.LogLevel.Error, "Mesh : HasError");
                    return true;
                }
            }

            foreach (var vertex in HalfEdge.Vertexs)
            {
                if (vertex.ErrorVertex)
                {
                    Logger.Log(Logger.LogLevel.Error, "Vertex : HasError");
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
