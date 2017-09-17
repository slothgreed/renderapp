using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Utility;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// ハーフエッジを編集するクラス
    /// </summary>
    public class HalfEdgeDSEditor
    {
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

        #region [edit method]
        #region [vertex decimation]

        /// <summary>
        /// マージによる頂点削除削除後、頂点位置移動
        /// </summary>
        /// <param name="edge">削除するエッジ</param>
        public void EdgeCollapse(HalfEdge edge)
        {
            if (!CanEdgeCollapse(edge))
            {
                return;
            }

            HalfEdgeVertex delV = edge.Start;
            HalfEdgeVertex remV = edge.End;

            #region [create delete list]
            //削除するメッシュ
            var deleteMesh = new List<HalfEdgeMesh>();
            //削除するエッジ
            var deleteEdge = new List<HalfEdge>();
            //削除する頂点
            var deleteVertex = new List<HalfEdgeVertex>();

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
                    around.Start.AddEdge(around);
                }

                if(around.Before.End == delV)
                {
                    around.Before.End = remV;
                }
            }

            remV.Position = (remV.Position + delV.Position) / 2;
            //エッジ情報の切り替え
            Analyzer.HalfEdge.SetupOpposite(edge.Next.Opposite, edge.Before.Opposite);
            Analyzer.HalfEdge.SetupOpposite(edge.Opposite.Next.Opposite, edge.Opposite.Before.Opposite);

            DeleteMesh(deleteMesh);
            DeleteEdge(deleteEdge);
            DeleteVertex(deleteVertex);

            //HasError(delV);
            //HasError();

            //for (int i = 0; i < HalfEdge.Meshs.Count; i++)
            //{
            //    ((HalfEdgeMesh)HalfEdge.Meshs[i]).Index = i;
            //}

            //for (int i = 0; i < HalfEdge.Lines.Count; i++)
            //{
            //    ((HalfEdge)HalfEdge.Lines[i]).Index = i;
            //}

            for (int i = 0; i < HalfEdge.Vertexs.Count; i++)
            {
                HalfEdge.Vertexs[i].Index = i;
            }

        }
        #endregion

        /// <summary>
        /// エッジの中点に頂点の追加
        /// </summary>
        /// <param name="edge">エッジ</param>
        public void EdgeSplit(HalfEdge edge)
        {
            var opposite = edge.Opposite;
            var delMesh1 = edge.Mesh;
            var delMesh2 = opposite.Mesh;

            var vertex = new HalfEdgeVertex((edge.Start.Position + edge.End.Position) / 2, HalfEdge.Vertexs.Count);

            var right = new HalfEdge(vertex, edge.End, HalfEdge.Lines.Count);
            var oppoRight = new HalfEdge(edge.End, vertex, HalfEdge.Lines.Count + 1);
            Analyzer.HalfEdge.SetupOpposite(right, oppoRight);

            var left = new HalfEdge(edge.Start, vertex, HalfEdge.Lines.Count + 2);
            var oppoLeft = new HalfEdge(vertex, edge.Start, HalfEdge.Lines.Count + 3);
            Analyzer.HalfEdge.SetupOpposite(left, oppoLeft);

            var up = new HalfEdge(vertex, edge.Next.End, HalfEdge.Lines.Count + 4);
            var oppoup = new HalfEdge(edge.Next.End, vertex, HalfEdge.Lines.Count + 5);
            Analyzer.HalfEdge.SetupOpposite(up, oppoup);

            var down = new HalfEdge(vertex, opposite.Next.End, edge.Index);
            var oppodown = new HalfEdge(opposite.Next.End, vertex, opposite.Index);
            Analyzer.HalfEdge.SetupOpposite(down, oppodown);

            var rightUp = new HalfEdgeMesh(right, edge.Next, oppoup, delMesh1.Index);
            var leftUp = new HalfEdgeMesh(up, edge.Before, left, delMesh2.Index);
            var rightDown = new HalfEdgeMesh(down, opposite.Before, oppoRight, HalfEdge.Meshs.Count);
            var leftDown = new HalfEdgeMesh(oppoLeft, opposite.Next, oppodown, HalfEdge.Meshs.Count + 1);

            HalfEdge.Vertexs.Add(vertex);

            HalfEdge.Lines.Add(right); HalfEdge.Lines.Add(oppoRight);
            HalfEdge.Lines.Add(left); HalfEdge.Lines.Add(oppoLeft);
            HalfEdge.Lines.Add(up); HalfEdge.Lines.Add(oppoup);
            HalfEdge.Lines.Add(down); HalfEdge.Lines.Add(oppodown);

            HalfEdge.Meshs.Add(rightUp);
            HalfEdge.Meshs.Add(leftUp);
            HalfEdge.Meshs.Add(rightDown);
            HalfEdge.Meshs.Add(leftDown);

            DeleteEdge(new List<HalfEdge>() { edge, opposite });
            DeleteMesh(new List<HalfEdgeMesh>() { delMesh1, delMesh2 });

            HasError();
        }

        /// <summary>
        /// エッジの入れ替え
        /// </summary>
        /// <param name="edge">入れ替えるエッジ</param>
        public void EdgeFlips(HalfEdge edge)
        {
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

            HalfEdge.Meshs.Add(createMesh);
            HalfEdge.Meshs.Add(createMeshOpposite);
            HalfEdge.Lines.Add(createEdge);
            HalfEdge.Lines.Add(createEdgeOpposite);

            DeleteEdge(new List<HalfEdge>() { edge, opposite });
            DeleteMesh(new List<HalfEdgeMesh>() { delMesh1, delMesh2 });

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
            if(Vector3.Dot(middle - edge.Start.Position, middle - edge.End.Position) < 0)
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
        /// メッシュ削除
        /// </summary>
        /// <param name="deleteMesh">削除するメッシュ</param>
        private void DeleteMesh(List<HalfEdgeMesh> deleteMesh)
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
        /// <param name="deleteEdge">削除するエッジ</param>
        private void DeleteEdge(List<HalfEdge> deleteEdge)
        {
            foreach (var edge in deleteEdge)
            {
                edge.Dispose();
                HalfEdge.Lines.Remove(edge);
            }
        }

        /// <summary>
        /// 頂点削除
        /// </summary>
        /// <param name="deleteVertex">削除する頂点</param>
        private void DeleteVertex(List<HalfEdgeVertex> deleteVertex)
        {
            //エッジ削除
            foreach (var vertex in deleteVertex)
            {
                vertex.Dispose();
                HalfEdge.Vertexs.Remove(vertex);
            }
        }


        private bool HasError(HalfEdgeVertex vertex)
        {
            foreach (var edge in HalfEdge.HalfEdges)
            {
                if(edge.HasVertex(vertex))
                {
                    Console.WriteLine("error");
                }
            }

            foreach (var mesh in HalfEdge.HalfEdgeMeshs)
            {
                if (mesh.HasVertex(vertex))
                {
                    Console.WriteLine("error");
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
