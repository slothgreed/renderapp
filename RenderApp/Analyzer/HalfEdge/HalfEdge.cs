using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RenderApp.Utility;
//using OpenCvSharp;
namespace RenderApp.Analyzer
{
    public class HalfEdge : IAnalyzer
    {
        private List<Mesh> m_Mesh = new List<Mesh>();
        private List<Edge> m_Edge = new List<Edge>();
        private List<Vertex> m_Vertex = new List<Vertex>();
        public Parameter GaussCurvature = new Parameter();
        public Parameter MeanCurvature = new Parameter();
        public Parameter MaxCurvature = new Parameter();
        public Parameter MinCurvature = new Parameter();
        public Parameter Voronoi = new Parameter();
        public Parameter Saliency = new Parameter();
        /// <summary>
        /// メッシュのインデックスと、頂点を受け取る
        /// </summary>
        /// <param name="position">頂点座標リスト</param>
        /// <param name="mesh">「三角形を構成する頂点番号を格納したVector3」のリスト</param>
        public HalfEdge(List<Vector3> position, List<int> poly_Index)
        {
            MakeVertexList(position);
            MakeEdgeList(poly_Index);
            SetOppositeEdge();

            SetMeshParameter();
            SetVertexParameter();

            SetNormalizeParameter();

            if (CheckOppositeEdge())
            {
                Console.WriteLine("OK!");
            }
            else
            {
                Console.WriteLine("NG!");
            }
        }
        #region [make halfedge data structure]
        /// <summary>
        /// ハーフエッジ用のリストに頂点を格納
        /// </summary>
        /// <param name="vertex_List"></param>
        private void MakeVertexList(List<Vector3> vertex_List)
        {
            for(int i = 0; i < vertex_List.Count; i++)
            {
                Vertex vertex = new Vertex(vertex_List[i],i);
                m_Vertex.Add(vertex);
            }
        }
        /// <summary>
        /// 頂点番号を持つエッジを生成
        /// </summary>
        private void MakeEdgeList(List<int> poly_Index)
        {
            int poly_Num = poly_Index.Count / 3;
            for (int num = 0; num < poly_Num; num++)
            {
                Mesh mesh = new Mesh();

                Vertex v1 = m_Vertex[poly_Index[3 * num]];
                Vertex v2 = m_Vertex[poly_Index[3 * num + 1]];
                Vertex v3 = m_Vertex[poly_Index[3 * num + 2]];
                Edge edge1 = new Edge(mesh, v1, v2);
                Edge edge2 = new Edge(mesh, v2, v3);
                Edge edge3 = new Edge(mesh, v3, v1);
                
                //次のエッジの格納
                edge1.Next = edge2;
                edge2.Next = edge3;
                edge3.Next = edge1;

                //前のエッジの格納
                edge1.Before = edge3;
                edge2.Before = edge1;
                edge3.Before = edge2;

                //頂点にエッジを持たせる
                v1.AddEdge(edge1);
                v2.AddEdge(edge2);
                v3.AddEdge(edge3);

                //エッジをリストに格納
                m_Edge.Add(edge1);
                m_Edge.Add(edge2);
                m_Edge.Add(edge3);

                //メッシュにエッジを格納
                mesh.SetEdge(edge1, edge2, edge3);
                //メッシュに頂点を格納
                mesh.SetVertex(v1, v2, v3);
                //メッシュをリストに格納
                m_Mesh.Add(mesh);
                
            }
        }
        /// <summary>
        /// 反対エッジのセット
        /// </summary>
        private void SetOppositeEdge()
        {
            foreach(var vertex in m_Vertex)
            {
                foreach(var edge in vertex.GetAroundEdge())
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
            foreach (var opposite in end.GetAroundEdge())
            {
                if (opposite.Start == end && opposite.End == start)
                {
                    opposite.Opposite = edge;
                    edge.Opposite = opposite;
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
            foreach(var edge in m_Edge)
            {
                opposite = edge.Opposite;
                if (edge.Start == opposite.End
                    && edge.End == opposite.Start)
                {
                    ok_flag++;
                }
            }
            if (ok_flag == m_Edge.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region [getter method]


        /// <summary>
        /// 隣接エッジの取得
        /// </summary>
        /// <param name="edge_Index"></param>
        /// <returns></returns>
        public Edge GetNeightEdge(Edge edge)
        {
            return edge.Opposite.Next;            
        }
        /// <summary>
        /// 頂点のindex（頂点配列用の）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetNormal(int index)
        {
            return m_Vertex[index].Normal;
        }
        /// <summary>
        /// 頂点周りのエッジ取得
        /// </summary>
        public List<Edge> GetAroundEdge(int vertex_Index)
        {
            List<Edge> edge_List = m_Vertex[vertex_Index].GetAroundEdgeList();
            return edge_List;
        }
        /// <summary>
        /// 1-ringの周辺頂点インデックス
        /// </summary>
        public IEnumerable<int> GetAroundVertexIndex(int vertex_Index)
        {
            foreach(var edge in m_Vertex[vertex_Index].GetAroundEdge())
            {
                yield return edge.End.Number;
            }
        }

        /// <summary>
        /// 頂点周りのメッシュの取得
        /// </summary>
        /// <param name="vert_Index"></param>
        /// <returns></returns>
        public IEnumerable<Mesh> GetAroundMesh(int vert_Index)
        {
            foreach(var edge in GetAroundEdge(vert_Index))
            {
                yield return edge.Mesh;
            }
        }
        /// <summary>
        /// 一定距離内の頂点の取得
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public List<Vertex> GetAroundVertex(Vertex vertex, float distance)
        {
            List<Vertex> vertex_list = new List<Vertex>();
            vertex.calcFrag = true;
            vertex_list.Add(vertex);
            RecursiveAroundPosition(vertex_list, vertex, vertex, distance);
            for (int i = 0; i < vertex_list.Count; i++)
            {
                vertex_list[i].calcFrag = false;
            }
            return vertex_list;
        }
        
        /// <summary>
        /// 一定距離内の頂点の取得を行う再起関数
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="distance"></param>
        private void RecursiveAroundPosition(List<Vertex> vertex_list, Vertex vertex,Vertex startVertex,float distance)
        {

            float length;
            foreach (var aroundEdge in vertex.GetAroundEdge())
            {
                length = (startVertex - aroundEdge.End).Length; 
                if ( length < distance && aroundEdge.End.calcFrag == false)
                {
                    aroundEdge.End.calcFrag = true;
                    vertex_list.Add(aroundEdge.End);
                    RecursiveAroundPosition(vertex_list,aroundEdge.End,startVertex, distance);
                }
            }
        }
        #endregion
        #region [パラメータの格納]
        /// <summary>
        /// 頂点から計算されるパラメータの格納
        /// </summary>
        private void SetVertexParameter()
        {
            for (int i = 0; i < m_Vertex.Count; i++)
            {
                SetVoronoiRagion(i);
                SetGaussianParameter(i);
                SetMeanCurvature(i);
                SetMaxMinCurvature(i);
                
            }
            for (int i = 0; i < m_Vertex.Count; i++)
            {
                SetPrincipalCurvature(i);
            }
        }

        /// <summary>
        /// 曲率パラメータ格納
        /// </summary>
        private void SetNormalizeParameter()
        {
            Voronoi.SetMaxMinMean();
            Voronoi.SetValiance();


            GaussCurvature.SetMaxMinMean();
            GaussCurvature.SetValiance();

            MeanCurvature.SetMaxMinMean();
            MeanCurvature.SetValiance();

            MaxCurvature.SetMaxMinMean();
            MaxCurvature.SetValiance();

            MinCurvature.SetMaxMinMean();
            MinCurvature.SetValiance();
        }
        
        #endregion
        #region [パラメータの計算]
        #region [頂点パラメータ]
        /// <summary>
        /// ガウス曲率
        /// </summary>
        /// <param name="v_index"></param>
        private void SetGaussianParameter(int v_index)
        {
            float angle;
            List<Edge> edge_list;

            angle = 0;
            edge_list = GetAroundEdge(v_index);
            for (int j = 0; j < edge_list.Count; j++)
            {
                angle += edge_list[j].Angle;
            }
            
            m_Vertex[v_index].GaussCurvature = (2 * MathHelper.Pi - angle) / m_Vertex[v_index].VoronoiRagion;
            GaussCurvature.AddValue(m_Vertex[v_index].GaussCurvature);
        }
        /// <summary>
        /// 平均曲率
        /// </summary>
        /// <param name="v_index"></param>
        private void SetMeanCurvature(int v_index)
        {
            float angle;
            Edge opposite;
            float alpha;
            float beta;
            float length;
            angle = 0;
            foreach(var edge in GetAroundEdge(v_index))
            {

                length = edge.Length;
                opposite = edge.Opposite;
                alpha = edge.Next.Next.Angle;
                beta = opposite.Next.Next.Angle;
                angle += (float)((1 / Math.Cos(alpha)) + (1 / Math.Cos(beta))) * length;
            }
            m_Vertex[v_index].MeanCurvature =  angle / ( m_Vertex[v_index].VoronoiRagion * 2);
            MeanCurvature.AddValue(m_Vertex[v_index].MeanCurvature);

        }
        /// <summary>
        /// ボロノイ領域
        /// </summary>
        /// <param name="v_index"></param>
        private void SetVoronoiRagion(int v_index)
        {
            float angle;
            Edge opposite;
            float alpha;
            float beta;
            float length;
            angle = 0;

            foreach (var edge in GetAroundEdge(v_index))
            {
                length = edge.Length;
                opposite = edge.Opposite;
                alpha = edge.Next.Next.Angle;
                beta = opposite.Next.Next.Angle;
                angle += (float)((1 / Math.Cos(alpha)) + (1 / Math.Cos(beta))) * length;

            }
            m_Vertex[v_index].VoronoiRagion = angle/8;
            Voronoi.AddValue(m_Vertex[v_index].VoronoiRagion);

        }
        /// <summary>
        /// 最小主曲率
        /// </summary>
        /// <param name="v_index"></param>
        private void SetMaxMinCurvature(int v_index)
        {
            Vertex vertex = m_Vertex[v_index];
            
            float delta = (vertex.MeanCurvature * vertex.MeanCurvature) - vertex.GaussCurvature;
            if(delta > 0)
            {
                delta = (float)Math.Sqrt((double)delta);
            }else{
                delta = 0;
            }
            vertex.MaxCurvature = vertex.MeanCurvature + delta;
            vertex.MinCurvature = vertex.MeanCurvature - delta;
            MaxCurvature.AddValue(vertex.MaxCurvature);
            MinCurvature.AddValue(vertex.MinCurvature);

        }
        #endregion
        #region[メッシュパラメータ]
        /// <summary>
        /// 法線の格納
        /// </summary>
        private void SetMeshParameter()
        {
            Vector3 normal = new Vector3();
            List<Vertex> v_Index;
            for (int i = 0; i < m_Mesh.Count; i++)
            {
                v_Index = m_Mesh[i].GetAroundVertex();
                normal = CCalc.Normal(v_Index[1].GetPosition() - v_Index[0].GetPosition(), v_Index[2].GetPosition() - v_Index[0].GetPosition());
                m_Mesh[i].Normal = normal.Normalized();

            }
        }
        #endregion
        #region [エッジパラメータ]
        /// <summary>
        /// 接平面に投影した時のベクトル
        /// </summary>
        /// <param name="v_index"></param>
        private void SetPrincipalCurvature(int v_index)
        {
            List<Edge> around = GetAroundEdge(v_index);
            Vector3 edge;
            Vector3 numer;
            Vector3 denom;
            Vector3 tangent;
            Vector2 TangentUV;
            Matrix3 ellipse = new Matrix3();
            Vector3 kapper = new Vector3();
            edge = around[0].End.GetPosition() - around[0].Start.GetPosition();
            numer = edge - (Vector3.Dot(edge, m_Vertex[v_index].Normal) * m_Vertex[v_index].Normal);
            denom = new Vector3(numer.Length);
            float edge_Kapper;
            //基底Z方向ベクトル
            Vector3 Normal = m_Vertex[v_index].Normal;
            //基底U方向ベクトル
            Vector3 baseU = new Vector3(numer.X / denom.X, numer.Y / denom.Y, numer.Z / denom.Z);
            baseU.Normalize();

            float inner = Vector3.Dot(baseU, Normal);
            //基底V方向ベクトル
            Vector3 baseV = Vector3.Cross(baseU, Normal);
            baseV.Normalize();


            for (int i = 0; i < around.Count; i++)
            {
                edge = around[i].End.GetPosition() - around[i].Start.GetPosition();
                numer = edge - (Vector3.Dot(edge, Normal) * Normal);
                denom = new Vector3(numer.Length);
                tangent = new Vector3(numer.X / denom.X, numer.Y / denom.Y, numer.Z / denom.Z);
                TangentUV = new Vector2(Vector3.Dot(baseU, tangent), Vector3.Dot(baseV, tangent));
                edge_Kapper = 2 * Vector3.Dot(-edge, Normal) / (edge).Length * (edge).Length;
               

                ellipse.M11 += (TangentUV.X * TangentUV.X * TangentUV.X * TangentUV.X);
                ellipse.M12 += (TangentUV.X * TangentUV.X * TangentUV.X * TangentUV.Y);
                ellipse.M13 += (TangentUV.X * TangentUV.X * TangentUV.Y * TangentUV.Y);

                ellipse.M21 += (TangentUV.X * TangentUV.X * TangentUV.X * TangentUV.Y);
                ellipse.M22 += (TangentUV.X * TangentUV.X * TangentUV.Y * TangentUV.Y);
                ellipse.M23 += (TangentUV.X * TangentUV.Y * TangentUV.Y * TangentUV.Y);

                ellipse.M31 += (TangentUV.X * TangentUV.X * TangentUV.Y * TangentUV.Y);
                ellipse.M32 += (TangentUV.X * TangentUV.Y * TangentUV.Y * TangentUV.Y);
                ellipse.M33 += (TangentUV.Y * TangentUV.Y * TangentUV.Y * TangentUV.Y);

                kapper.X += edge_Kapper * TangentUV.X * TangentUV.X;
                kapper.Y += edge_Kapper * TangentUV.X * TangentUV.Y;
                kapper.Z += edge_Kapper * TangentUV.Y * TangentUV.Y;

            }

            ellipse.Invert();
            Vector3 result = CCalc.Multiply(ellipse, kapper);
            float a = result.X;
            float b = result.Y / 2;
            float c = result.Z;
            //CvMat eigenVector;
            //CvMat eigenValue;
            //CvMat matEplise = Cv.CreateMat(2, 2, MatrixType.F32C1);
            //matEplise.Set2D(0, 0, a);
            //matEplise.Set2D(0, 1, b);
            //matEplise.Set2D(1, 0, b);
            //matEplise.Set2D(1, 1, c);
            //eigenVector = Cv.CreateMat(2, 2, MatrixType.F32C1);
            //eigenValue = Cv.CreateMat(1, 2, MatrixType.F32C1);
            //Cv.Zero(eigenVector);
            //Cv.Zero(eigenValue);
            //Cv.EigenVV(matEplise, eigenVector, eigenValue);


            //float max1 = (float)eigenVector.Get2D(0, 0).Val0;
            //float max2 = (float)eigenVector.Get2D(0, 1).Val0;
            //float min1 = (float)eigenVector.Get2D(1, 0).Val0;
            //float min2 = (float)eigenVector.Get2D(1, 1).Val0;
            

            //m_Vertex[v_index].MaxVector = new Vector3(
            //    baseU.X * max1 + baseV.X * max2,
            //    baseU.Y * max1 + baseV.Y * max2,
            //    baseU.Z * max1 + baseV.Z * max2
            //    ).Normalized();
            //m_Vertex[v_index].MinVector = new Vector3(
            //   baseU.X * min1 + baseV.X * min2,
            //   baseU.Y * min1 + baseV.Y * min2,
            //   baseU.Z * min1 + baseV.Z * min2
            //   ).Normalized();
            //float in1 = Vector3.Dot(m_Vertex[v_index].Normal, m_Vertex[v_index].MaxVector);
            //float in2 = Vector3.Dot(m_Vertex[v_index].Normal, m_Vertex[v_index].MaxVector);
            
        }
        #endregion

        #endregion

        public override string ToString()
        {
            return "HalfEdge";
        }

    }
}
