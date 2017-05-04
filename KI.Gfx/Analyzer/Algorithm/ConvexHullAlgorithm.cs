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
            Console.WriteLine("MeshList" + MeshList.Count.ToString());
            Console.WriteLine("Point" + PointList.Count.ToString());
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
        }

        /// <summary>
        /// クイックハルアルゴリズム
        /// </summary>
        private void QuickHullCore()
        {
            Mesh calcMesh = null;
            for (int i = 0; i < MeshList.Count; i++)
            {
                if ((bool)MeshList[i].CalcFlag == false)
                {
                    calcMesh = MeshList[i];
                    break;
                }
            }
            //すべて計算し終わったら終了
            if (calcMesh == null)
            {
                return;
            }
            Vector3 FarPoint;
            if (FindFarPoint(calcMesh, out FarPoint))
            {
                CreateQuickHullMesh(FarPoint);
                RemoveMesh();
                PointList.Remove(FarPoint);
                DeleteInsideVertex();
            }
            else
            {
                //最も遠い頂点が見つからない場合は終了
                calcMesh.CalcFlag = true;
            }

            QuickHullCore();
        }
        /// <summary>
        /// 不要な点と面を削除
        /// </summary>
        private void RemoveMesh()
        {
            //削除フラグが立ったメッシュを削除
            for (int i = 0; i < MeshList.Count; i++)
            {
                if (MeshList[i].DeleteFlag)
                {
                    MeshList.RemoveAt(i);
                    i--;
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

            foreach (var mesh in visibleMesh)
            {
                mesh.Dispose();
            }

        }

        /// <summary>
        /// 内側の点の削除
        /// </summary>
        private void DeleteInsideVertex()
        {
            bool deleteFlag = false;
            for (int i = 0; i < PointList.Count; i++)
            {
                deleteFlag = true;
                foreach (var mesh in MeshList)
                {
                    //1つでも負の値があるなら、凸包の中にない
                    if (Vector3.Dot(mesh.Normal, mesh.Gravity - PointList[i]) < 0)
                    {
                        deleteFlag = false;
                        break;
                    }
                }

                if (deleteFlag)
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
            List<Mesh> visibleMesh = new List<Mesh>();
            foreach(var mesh in meshList)
            {
                if(Vector3.Dot(mesh.Normal,point - mesh.Gravity) > 0)
                {
                    visibleMesh.Add(mesh);
                }
            }
            return visibleMesh;
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
                    bool isExist = false;
                    foreach (var checkMesh in visibleMesh)
                    {
                        //反対側のメッシュが可視面でない場合は境界エッジ
                        if (edge.Opposite.Mesh == checkMesh)
                        {
                            isExist = true;
                        }
                    }
                    if (isExist == false)
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
            for (int i = 0; i < boundaryList.Count - 1; i++)
            {
               if(boundaryList[i].End != boundaryList[i + 1].Start)
               {
                   Console.WriteLine("Error");
               }
            }
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
                }

                if (min.X > pos.X && min.Y > pos.Y && min.Z > pos.Z)
                {
                    min = pos;
                }

                if (xyMinzMax.X > pos.X && xyMinzMax.Y > pos.Y && xyMinzMax.Z < pos.Z)
                {
                    xyMinzMax = pos;
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

            for (int i = 0; i < PointList.Count; i++)
            {
                if (vertex1.Position == PointList[i] ||
                   vertex2.Position == PointList[i] ||
                   vertex3.Position == PointList[i])
                {
                    PointList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Update()
        {
            QuickHullCore();
        }
    }
}
