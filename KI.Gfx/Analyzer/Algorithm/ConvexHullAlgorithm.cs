using KI.Foundation.Utility;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Gfx.Analyzer.Algorithm
{
    public class ConvexHullAlgorithm
    {
        /// <summary>
        /// メッシュリスト
        /// </summary>
        private List<Mesh> MeshList;
        public ReadOnlyCollection<Mesh> Meshs
        {
            get
            {
                return MeshList.AsReadOnly();
            }
        }
        /// <summary>
        /// 点群
        /// </summary>
        private List<Vector3> PointList;
        public List<Vector3> Points
        {
            get
            {
                return PointList;
            }
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public ConvexHullAlgorithm(List<Vector3> position)
        {
            PointList = new List<Vector3>();
            MeshList = new List<Mesh>();

            foreach (var pos in position)
            {
                PointList.Add(pos);
            }
            QuickHullAlgorithm();
            
            Logger.Log(Logger.LogLevel.Descript, "MeshList :" + MeshList.Count.ToString());
            Logger.Log(Logger.LogLevel.Descript, "Point :" + PointList.Count.ToString());
        }

        /// <summary>
        /// クイックハル
        /// </summary>
        /// <param name="position"></param>
        public void QuickHullAlgorithm()
        {
            if (PointList.Count < 3)
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
            while(true)
            {
                Mesh calcMesh = MeshList.FirstOrDefault(p => (bool)p.CalcFlag == false);

                //すべて計算し終わったら終了
                if (calcMesh == null)
                {
                    return;
                }
                Vector3 FarPoint;
                if (FindFarPoint(calcMesh, out FarPoint))
                {
                    CreateQuickHullMesh(FarPoint);
                    MeshList.RemoveAll(mesh => mesh.DeleteFlag);
                    PointList.Remove(FarPoint);

                    //簡単な高速化この処理が重い。
                    if (PointList.Count > 5000)
                    {
                        DeleteInsideVertex();
                    }
                }
                else
                {
                    //最も遠い頂点が見つからない場合は終了
                    calcMesh.CalcFlag = true;
                }
            }
        }
        /// <summary>
        /// QuickHullMeshの作成
        /// </summary>
        /// <param name="farPoint"></param>
        private void CreateQuickHullMesh(Vector3 farPoint)
        {
            var visibleMesh = FindVisibleMesh(MeshList, farPoint);
            var boundaryList = FindBoundaryEdge(visibleMesh);
            CreateMesh(boundaryList, new Vertex(farPoint));
            visibleMesh.All(p => { p.Dispose(); return true; });
        }

        /// <summary>
        /// 内側の点の削除
        /// </summary>
        private void DeleteInsideVertex()
        {
            for (int i = 0; i < PointList.Count; i++)
            {
                //1つでも負の値があるなら、凸包の中にないので残す
                if (!MeshList.Any(mesh => Vector3.Dot(mesh.Normal, mesh.Gravity - PointList[i]) < 0))
                {
                    PointList.RemoveAt(i);
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
        private List<Mesh> FindVisibleMesh(List<Mesh> meshList, Vector3 point)
        {
            return meshList.Where(mesh => Vector3.Dot(mesh.Normal, point - mesh.Gravity) > 0).ToList();
        }

        /// <summary>
        /// 可視面から、境界エッジの取得
        /// </summary>
        /// <param name="visibleMesh">可視面</param>
        /// <returns>境界エッジ</returns>
        private List<Edge> FindBoundaryEdge(List<Mesh> visibleMesh)
        {
            var boundaryList = new List<Edge>();
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

            //select sort.
            Edge tmpEdge = null;
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

            //境界エッジがループでできていないとエラー
            //check method
            //for (int i = 0; i < boundaryList.Count - 1; i++)
            //{
            //   if(boundaryList[i].End != boundaryList[i + 1].Start)
            //   {
            //       Console.WriteLine("Error");
            //   }
            //}
            return boundaryList;
        }

        /// <summary>
        /// メッシュの作成
        /// </summary>
        /// <param name="boundaryList">境界リスト</param>
        /// <param name="vertex">頂点</param>
        private void CreateMesh(List<Edge> boundaryList,Vertex vertex)
        {
            //反対エッジ作成用に
            var newMesh = new List<Mesh>();

            //境界エッジからポリゴンの作成
            foreach (var boundary in boundaryList)
            {
                Edge edge1 = new Edge(vertex, boundary.Start);
                Edge edge2 = new Edge(boundary.End, vertex);
                Mesh mesh = new Mesh(edge1, boundary, edge2);
                mesh.CalcFlag = false;
                newMesh.Add(mesh);
                MeshList.Add(mesh);
            }

            //反対エッジの設定
            for (int i = 0; i < newMesh.Count; i++)
            {
                if (i == 0)
                {
                    Edge.SetupOpposite(newMesh[newMesh.Count - 1].GetEdge(2), newMesh[i].GetEdge(0));
                    Edge.SetupOpposite(newMesh[i].GetEdge(2), newMesh[i + 1].GetEdge(0));
                    continue;
                }

                if (i == newMesh.Count - 1)
                {
                    Edge.SetupOpposite(newMesh[i - 1].GetEdge(2), newMesh[i].GetEdge(0));
                    Edge.SetupOpposite(newMesh[i].GetEdge(2), newMesh[0].GetEdge(0));
                    continue;
                }

                Edge.SetupOpposite(newMesh[i - 1].GetEdge(2), newMesh[i].GetEdge(0));
                Edge.SetupOpposite(newMesh[i].GetEdge(2), newMesh[i + 1].GetEdge(0));
            }
        }
        /// <summary>
        /// 最も遠い点の取得
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="posit"></param>
        /// <returns></returns>
        private bool FindFarPoint(Mesh mesh,out Vector3 posit)
        {
            float maxDist = float.MinValue;
            posit = Vector3.Zero;
            foreach(var pos in PointList)
            {
                float dist = KICalc.DistancePlane(mesh.Plane, pos);
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
            
            foreach (var pos in PointList)
            {
                if (max.X < pos.X && max.Y < pos.X && max.Z < pos.X)
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

            Vertex vertex1 = new Vertex(min);
            Vertex vertex2 = new Vertex(max);
            Vertex vertex3 = new Vertex(xyMinzMax);
            Edge edge1 = new Edge(vertex1, vertex2);
            Edge edge2 = new Edge(vertex2, vertex3);
            Edge edge3 = new Edge(vertex3, vertex1);

            Edge oppo1 = new Edge(vertex2, vertex1);
            Edge oppo2 = new Edge(vertex1, vertex3);
            Edge oppo3 = new Edge(vertex3, vertex2);

            Edge.SetupOpposite(edge1, oppo1);
            Edge.SetupOpposite(edge2, oppo3);
            Edge.SetupOpposite(edge3, oppo2);

            var mesh1 = new Mesh(edge1, edge2, edge3);
            var mesh2 = new Mesh(oppo1, oppo2, oppo3);
            mesh1.CalcFlag = false;
            mesh2.CalcFlag = false;
            MeshList.Add(mesh1);
            MeshList.Add(mesh2);

            //triangleを作った頂点の削除
            PointList.RemoveAll(p => (p == vertex1.Position || p == vertex2.Position || p == vertex3.Position));
        }
    }
}
