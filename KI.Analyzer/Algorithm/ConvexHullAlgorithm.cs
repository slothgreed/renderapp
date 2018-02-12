using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using KI.Mathmatics;
using OpenTK;

namespace KI.Analyzer.Algorithm
{
    /// <summary>
    /// 凸包の作成
    /// </summary>
    public class ConvexHullAlgorithm
    {
        /// <summary>
        /// メッシュリスト
        /// </summary>
        private List<HalfEdgeMesh> meshList;

        /// <summary>
        /// 点群
        /// </summary>
        private List<Vector3> pointList;

        /// <summary>
        /// 頂点の数
        /// </summary>
        private int vertexCount = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="position">座標</param>
        public ConvexHullAlgorithm(List<Vertex> position)
        {
            pointList = new List<Vector3>();
            meshList = new List<HalfEdgeMesh>();

            pointList.AddRange(position.Select(p => p.Position));

            QuickHullAlgorithm();

            Logger.Log(Logger.LogLevel.Allway, "MeshList :" + meshList.Count.ToString());
            Logger.Log(Logger.LogLevel.Allway, "Point :" + pointList.Count.ToString());
        }

        /// <summary>
        /// メッシュ
        /// </summary>
        public List<HalfEdgeMesh> Meshs
        {
            get
            {
                return meshList;
            }
        }

        /// <summary>
        /// 座標
        /// </summary>
        public List<Vector3> Points
        {
            get
            {
                return pointList;
            }
        }

        /// <summary>
        /// クイックハル
        /// </summary>
        private void QuickHullAlgorithm()
        {
            if (pointList.Count < 3)
            {
                return;
            }

            CreateInitMesh();
            QuickHullCore();
            DeleteInsideVertex();
        }

        /// <summary>
        /// クイックハルアルゴリズム
        /// </summary>
        private void QuickHullCore()
        {
            while (true)
            {
                HalfEdgeMesh calcMesh = meshList.FirstOrDefault(p => (bool)p.TmpParameter == false);

                //すべて計算し終わったら終了
                if (calcMesh == null)
                {
                    return;
                }

                Vector3 farPoint;
                if (FindFarPoint(calcMesh, out farPoint))
                {
                    CreateQuickHullMesh(farPoint);
                    meshList.RemoveAll(mesh => mesh.DeleteFlag);
                    pointList.Remove(farPoint);

                    //簡単な高速化この処理が重い。
                    if (pointList.Count > 5000)
                    {
                        DeleteInsideVertex();
                    }
                }
                else
                {
                    //最も遠い頂点が見つからない場合は終了
                    calcMesh.TmpParameter = true;
                }
            }
        }

        /// <summary>
        /// QuickHullMeshの作成
        /// </summary>
        /// <param name="farPoint">最も遠い点</param>
        private void CreateQuickHullMesh(Vector3 farPoint)
        {
            var visibleMesh = FindVisibleMesh(meshList, farPoint);
            var boundaryList = FindBoundaryEdge(visibleMesh);
            CreateMesh(boundaryList, new HalfEdgeVertex(farPoint, vertexCount));
            vertexCount++;
            visibleMesh.All(p => { p.Dispose(); return true; });
        }

        /// <summary>
        /// 内側の点の削除
        /// </summary>
        private void DeleteInsideVertex()
        {
            for (int i = 0; i < pointList.Count; i++)
            {
                //1つでも負の値があるなら、凸包の中にないので残す
                if (!meshList.Any(mesh => Vector3.Dot(mesh.Normal, mesh.Gravity - pointList[i]) < 0))
                {
                    pointList.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// 可視面の取得
        /// </summary>
        /// <param name="meshList">検索面</param>
        /// <param name="point">この点から見える面</param>
        /// <returns>可視面</returns>
        private List<HalfEdgeMesh> FindVisibleMesh(List<HalfEdgeMesh> meshList, Vector3 point)
        {
            return meshList.Where(mesh => Vector3.Dot(mesh.Normal, point - mesh.Gravity) > 0).ToList();
        }

        /// <summary>
        /// 可視面から、境界エッジの取得
        /// </summary>
        /// <param name="visibleMesh">可視面</param>
        /// <returns>境界エッジ</returns>
        private List<HalfEdge> FindBoundaryEdge(List<HalfEdgeMesh> visibleMesh)
        {
            var boundaryList = new List<HalfEdge>();
            foreach (var mesh in visibleMesh)
            {
                foreach (var edge in mesh.AroundEdge)
                {
                    //エッジの反対面が、可視面に含まれていない場合は、境界面
                    if (visibleMesh.Any(checkMesh => checkMesh == edge.Opposite.Mesh) == false)
                    {
                        boundaryList.Add(edge);
                    }
                }
            }

            //境界線が一周するように並べ替え
            HalfEdge tmpEdge = null;
            for (int i = 0; i < boundaryList.Count; i++)
            {
                for (int j = i + 1; j < boundaryList.Count; j++)
                {
                    if (boundaryList[i].End == boundaryList[j].Start)
                    {
                        tmpEdge = boundaryList[i + 1];
                        boundaryList[i + 1] = boundaryList[j];
                        boundaryList[j] = tmpEdge;
                    }
                }
            }

            return boundaryList;
        }

        /// <summary>
        /// メッシュの作成
        /// </summary>
        /// <param name="boundaryList">境界リスト</param>
        /// <param name="vertex">頂点</param>
        private void CreateMesh(List<HalfEdge> boundaryList, HalfEdgeVertex vertex)
        {
            //反対エッジ作成用に
            var newMesh = new List<HalfEdgeMesh>();

            //境界エッジからポリゴンの作成
            foreach (var boundary in boundaryList)
            {
                HalfEdge edge1 = new HalfEdge(vertex, boundary.Start);
                HalfEdge edge2 = new HalfEdge(boundary.End, vertex);
                HalfEdgeMesh mesh = new HalfEdgeMesh(edge1, boundary, edge2);
                mesh.TmpParameter = false;
                newMesh.Add(mesh);
                meshList.Add(mesh);
            }

            //反対エッジの設定
            for (int i = 0; i < newMesh.Count; i++)
            {
                if (i == 0)
                {
                    HalfEdge.SetupOpposite(newMesh[newMesh.Count - 1].GetEdge(2), newMesh[i].GetEdge(0));
                    HalfEdge.SetupOpposite(newMesh[i].GetEdge(2), newMesh[i + 1].GetEdge(0));
                    continue;
                }

                if (i == newMesh.Count - 1)
                {
                    HalfEdge.SetupOpposite(newMesh[i - 1].GetEdge(2), newMesh[i].GetEdge(0));
                    HalfEdge.SetupOpposite(newMesh[i].GetEdge(2), newMesh[0].GetEdge(0));
                    continue;
                }

                HalfEdge.SetupOpposite(newMesh[i - 1].GetEdge(2), newMesh[i].GetEdge(0));
                HalfEdge.SetupOpposite(newMesh[i].GetEdge(2), newMesh[i + 1].GetEdge(0));
            }
        }

        /// <summary>
        /// 最も遠い点の取得
        /// </summary>
        /// <param name="mesh">面</param>
        /// <param name="posit">点</param>
        /// <returns>成功</returns>
        private bool FindFarPoint(HalfEdgeMesh mesh, out Vector3 posit)
        {
            float maxDist = float.MinValue;
            posit = Vector3.Zero;
            foreach (var pos in pointList)
            {
                float dist = Distance.PlaneToPoint(mesh.Plane, pos);
                if (Vector3.Dot(mesh.Normal, pos - mesh.Gravity) > 0)
                {
                    if (maxDist < dist)
                    {
                        maxDist = dist;
                        posit = pos;
                    }
                }
            }

            if (posit == Vector3.Zero)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 初期平面の作成
        /// </summary>
        private void CreateInitMesh()
        {
            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);
            Vector3 xyMinzMax = new Vector3(float.MaxValue, float.MaxValue, float.MinValue);

            foreach (var pos in pointList)
            {
                if (max.X < pos.X && max.Y < pos.Y && max.Z < pos.Z)
                {
                    max = pos;
                    continue;
                }

                if (min.X > pos.X && min.Y > pos.Y && min.Z > pos.Z)
                {
                    min = pos;
                    continue;
                }

                if (xyMinzMax.X > pos.X && xyMinzMax.Y > pos.Y && xyMinzMax.Z < pos.Z)
                {
                    xyMinzMax = pos;
                    continue;
                }
            }

            HalfEdgeVertex vertex1 = new HalfEdgeVertex(min, 0);
            HalfEdgeVertex vertex2 = new HalfEdgeVertex(max, 1);
            HalfEdgeVertex vertex3 = new HalfEdgeVertex(xyMinzMax, 2);
            vertexCount = 3;

            HalfEdge edge1 = new HalfEdge(vertex1, vertex2);
            HalfEdge edge2 = new HalfEdge(vertex2, vertex3);
            HalfEdge edge3 = new HalfEdge(vertex3, vertex1);

            HalfEdge oppo1 = new HalfEdge(vertex2, vertex1);
            HalfEdge oppo2 = new HalfEdge(vertex1, vertex3);
            HalfEdge oppo3 = new HalfEdge(vertex3, vertex2);

            HalfEdge.SetupOpposite(edge1, oppo1);
            HalfEdge.SetupOpposite(edge2, oppo3);
            HalfEdge.SetupOpposite(edge3, oppo2);

            var mesh1 = new HalfEdgeMesh(edge1, edge2, edge3);
            var mesh2 = new HalfEdgeMesh(oppo1, oppo2, oppo3);
            mesh1.TmpParameter = false;
            mesh2.TmpParameter = false;
            meshList.Add(mesh1);
            meshList.Add(mesh2);

            //triangleを作った頂点の削除
            pointList.RemoveAll(p => (p == vertex1.Position || p == vertex2.Position || p == vertex3.Position));
        }
    }
}
