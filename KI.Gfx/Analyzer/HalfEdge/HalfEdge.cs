using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using KI.Foundation.Utility;
using KI.Gfx.KIAsset;
using System.IO;
namespace KI.Gfx.Analyzer
{
    public class HalfEdge
    {
        public List<Mesh> m_Mesh = new List<Mesh>();
        public List<Edge> m_Edge = new List<Edge>();
        public List<Vertex> m_Vertex = new List<Vertex>();
        public List<int> m_Index = new List<int>();
        public Parameter GaussCurvature = new Parameter();
        public Parameter MeanCurvature = new Parameter();
        public Parameter MaxCurvature = new Parameter();
        public Parameter MinCurvature = new Parameter();
        public Parameter Voronoi = new Parameter();
        public Parameter Saliency = new Parameter();

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

            SetVertexParameter();

            SetNormalizeParameter();

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
                if(vertex.ErrorVertex())
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="inputFile"></param>
        public bool ReadFile(string inputFile)
        {
            if (!File.Exists(inputFile))
            {
                return false;
            }
            if (Path.GetExtension(inputFile).ToLower() != ".half")
            {
                return false;
            }

            try
            {
                m_Vertex.Clear();
                m_Edge.Clear();
                m_Mesh.Clear();

                String[] fileData = File.ReadAllLines(inputFile, System.Text.Encoding.GetEncoding("Shift_JIS"));
                ReadHalfEdgeData(fileData);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("読み込み失敗 : " + inputFile);
                return false;
            }
                    
        }

        private void ReadHalfEdgeData(String[] fileData)
        {
            int lineNumber = 0;
            String line;
            int EdgeInfoCounter = 0;
            while (fileData.Length != lineNumber)
            {
                line = fileData[lineNumber];
                lineNumber++;
                if (line.Contains("HalfEdge Data Strucure")) continue;
                if (line.Contains("Vertex :")) continue;
                if (line.Contains("Edge :")) continue;
                if (line.Contains("Mesh :")) continue;
                if (line.Contains("Edge Info :")) continue;

                string[] lineInfos = line.Split(' ');
                lineInfos = lineInfos.Where(p => !(String.IsNullOrWhiteSpace(p) || String.IsNullOrEmpty(p))).ToArray();

                if (lineInfos[0] == "v")
                {
                    var position = new Vector3(float.Parse(lineInfos[1]), float.Parse(lineInfos[2]), float.Parse(lineInfos[3]));
                    var vertex = new Vertex(position, m_Vertex.Count);
                    m_Vertex.Add(vertex);
                }

                if (lineInfos[0] == "e")
                {
                    int startIndex = int.Parse(lineInfos[1]);
                    int endIndex = int.Parse(lineInfos[2]);
                    var edge = new Edge(m_Vertex[startIndex], m_Vertex[endIndex], m_Edge.Count);
                    m_Edge.Add(edge);
                    m_Vertex[startIndex].AddEdge(edge);
                }

                if (lineInfos[0] == "m")
                {
                    int edge1 = int.Parse(lineInfos[1]);
                    int edge2 = int.Parse(lineInfos[2]);
                    int edge3 = int.Parse(lineInfos[3]);
                    var mesh = new Mesh(m_Mesh.Count);
                    mesh.SetEdge(m_Edge[edge1], m_Edge[edge2], m_Edge[edge3]);
                    m_Mesh.Add(mesh);
                }

                if (lineInfos[0] == "ei")
                {
                    int nextIndex       = int.Parse(lineInfos[1]);
                    int beforeIndex     = int.Parse(lineInfos[2]);
                    int oppositeIndex   = int.Parse(lineInfos[3]);
                    int meshIndex       = int.Parse(lineInfos[4]);
                    var edge = m_Edge[EdgeInfoCounter];

                    edge.Next       = m_Edge[nextIndex];
                    edge.Before     = m_Edge[beforeIndex];
                    edge.Opposite   = m_Edge[oppositeIndex];
                    edge.Mesh       = m_Mesh[meshIndex];
                    EdgeInfoCounter++;
                }
            }
        }
        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="outputFile"></param>
        public void WriteFile(string outputFile)
        {
            StreamWriter write = new StreamWriter(outputFile);

            write.WriteLine("HalfEdge Data Structure");
            write.WriteLine("Vertex : Position");
            foreach (var vertex in m_Vertex)
            {
                write.WriteLine("v" + " " + vertex.Position.X + " " + vertex.Position.Y + " " + vertex.Position.Z);
            }
            write.WriteLine("Edge : Start Vetex Index, End Vertex Index");
            foreach (var edge in m_Edge)
            {
                write.WriteLine("e" + " " + edge.Start.Index + " " + edge.End.Index);
            }

            write.WriteLine("Mesh : Vertex Index");
            foreach (var mesh in m_Mesh)
            {
                string edgeIdx = "";
                foreach (var edge in mesh.AroundEdge)
                {
                    if (edge == mesh.AroundEdge.Last())
                    {
                        edgeIdx += edge.Index.ToString();
                    }
                    else
                    {
                        edgeIdx += edge.Index.ToString() + " ";
                    }
                }
                write.WriteLine("m" + " " + edgeIdx);
            }

            write.WriteLine("Edge Info : Next Edge Index,Before Edge, Opposite Edge Index, Incident Face ");
            foreach (var edge in m_Edge)
            {
                write.WriteLine("ei" + " " + edge.Next.Index + " " + edge.Before.Index + " " + edge.Opposite.Index + " " + edge.Mesh.Index);
            }
            write.WriteLine("end");
            write.Close();
        }

        #region [edit method]
        /// <summary>
        /// マージによる頂点削除削除後、頂点位置移動
        /// </summary>
        /// <param name="removeIndex">削除するほう</param>
        /// <param name="remainIndex">残すほう</param>
        public void VertexDecimation(Vertex delV,Vertex remV)
        {
            if(delV == null || remV == null)
            {
                return;
            }
            //2点を端点とするエッジ
            Edge commonEdge = GetEdge(delV, remV);
            if(commonEdge == null)
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

            remV.m_Edge = commonEdge.Opposite.Before.Opposite;
            DeleteMesh(deleteMesh);
            DeleteEdge(deleteEdge);
            DeleteVertex(deleteVertex);
        }
        #endregion
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
            foreach(var edge in deleteEdge)
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
            foreach (var vertex in m_Vertex)
            {
                geometry.Position.Add(vertex.Position);
                geometry.Normal.Add(vertex.Normal);

            }
            foreach (var mesh in m_Mesh)
            {
                foreach (var vertex in mesh.AroundVertex)
                {
                    geometry.Index.Add(vertex.Index);
                }
            }
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

            //次のエッジの格納
            edge1.Next = edge2;
            edge2.Next = edge3;
            edge3.Next = edge1;


            edge1.Before = edge3;
            edge2.Before = edge1;
            edge3.Before = edge2;

            //頂点にエッジを持たせる
            v1.AddEdge(edge1);
            v2.AddEdge(edge2);
            v3.AddEdge(edge3);

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
        /// 頂点のindex（頂点配列用の）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetNormal(int index)
        {
            return m_Vertex[index].Normal;
        }

        public IEnumerable<Edge> GetAroundEdge(int vertex_Index)
        {
            foreach(var edge in m_Vertex[vertex_Index].AroundEdge)
            {
                yield return edge;
            }
        }

        /// <summary>
        /// 頂点周りのメッシュの取得
        /// </summary>
        /// <param name="vert_Index"></param>
        /// <returns></returns>
        public IEnumerable<Mesh> GetAroundMesh(int vertex_Index)
        {
            foreach (var mesh in m_Vertex[vertex_Index].AroundMesh)
            {
                yield return mesh;
            }
        }
        /// <summary>
        /// 1-ringの周辺頂点インデックス
        /// </summary>
        public IEnumerable<int> GetAroundVertexIndex(int vertex_Index)
        {
            foreach (var edge in m_Vertex[vertex_Index].AroundEdge)
            {
                yield return edge.End.Index;
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
        private void RecursiveAroundPosition(List<Vertex> vertex_list, Vertex vertex, Vertex startVertex, float distance)
        {
            float length;
            foreach (var aroundEdge in vertex.AroundEdge)
            {
                length = (startVertex - aroundEdge.End).Length;
                if (length < distance && aroundEdge.End.calcFrag == false)
                {
                    aroundEdge.End.calcFrag = true;
                    vertex_list.Add(aroundEdge.End);
                    RecursiveAroundPosition(vertex_list, aroundEdge.End, startVertex, distance);
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

            angle = 0;

            foreach(var edge in GetAroundEdge(v_index))
            {
                angle += edge.Angle;
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

        #region [エッジパラメータ]
        /// <summary>
        /// 接平面に投影した時のベクトル
        /// </summary>
        /// <param name="v_index"></param>
        private void SetPrincipalCurvature(int v_index)
        {
            //List<Edge> around = GetAroundEdge(v_index);
            //Vector3 edge;
            //Vector3 numer;
            //Vector3 denom;
            //Vector3 tangent;
            //Vector2 TangentUV;
            //Matrix3 ellipse = new Matrix3();
            //Vector3 kapper = new Vector3();
            //edge = around[0].End.GetPosition() - around[0].Start.GetPosition();
            //numer = edge - (Vector3.Dot(edge, m_Vertex[v_index].Normal) * m_Vertex[v_index].Normal);
            //denom = new Vector3(numer.Length);
            //float edge_Kapper;
            ////基底Z方向ベクトル
            //Vector3 Normal = m_Vertex[v_index].Normal;
            ////基底U方向ベクトル
            //Vector3 baseU = new Vector3(numer.X / denom.X, numer.Y / denom.Y, numer.Z / denom.Z);
            //baseU.Normalize();

            //float inner = Vector3.Dot(baseU, Normal);
            ////基底V方向ベクトル
            //Vector3 baseV = Vector3.Cross(baseU, Normal);
            //baseV.Normalize();


            //for (int i = 0; i < around.Count; i++)
            //{
            //    edge = around[i].End.GetPosition() - around[i].Start.GetPosition();
            //    numer = edge - (Vector3.Dot(edge, Normal) * Normal);
            //    denom = new Vector3(numer.Length);
            //    tangent = new Vector3(numer.X / denom.X, numer.Y / denom.Y, numer.Z / denom.Z);
            //    TangentUV = new Vector2(Vector3.Dot(baseU, tangent), Vector3.Dot(baseV, tangent));
            //    edge_Kapper = 2 * Vector3.Dot(-edge, Normal) / (edge).Length * (edge).Length;
               

            //    ellipse.M11 += (TangentUV.X * TangentUV.X * TangentUV.X * TangentUV.X);
            //    ellipse.M12 += (TangentUV.X * TangentUV.X * TangentUV.X * TangentUV.Y);
            //    ellipse.M13 += (TangentUV.X * TangentUV.X * TangentUV.Y * TangentUV.Y);

            //    ellipse.M21 += (TangentUV.X * TangentUV.X * TangentUV.X * TangentUV.Y);
            //    ellipse.M22 += (TangentUV.X * TangentUV.X * TangentUV.Y * TangentUV.Y);
            //    ellipse.M23 += (TangentUV.X * TangentUV.Y * TangentUV.Y * TangentUV.Y);

            //    ellipse.M31 += (TangentUV.X * TangentUV.X * TangentUV.Y * TangentUV.Y);
            //    ellipse.M32 += (TangentUV.X * TangentUV.Y * TangentUV.Y * TangentUV.Y);
            //    ellipse.M33 += (TangentUV.Y * TangentUV.Y * TangentUV.Y * TangentUV.Y);

            //    kapper.X += edge_Kapper * TangentUV.X * TangentUV.X;
            //    kapper.Y += edge_Kapper * TangentUV.X * TangentUV.Y;
            //    kapper.Z += edge_Kapper * TangentUV.Y * TangentUV.Y;

            //}

            //ellipse.Invert();
            //Vector3 result = CCalc.Multiply(ellipse, kapper);
            //float a = result.X;
            //float b = result.Y / 2;
            //float c = result.Z;
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
    }
}
