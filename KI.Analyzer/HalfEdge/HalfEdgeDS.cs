using System;
using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Parameter;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Analyzer
{
    /// <summary>
    /// ハーフエッジデータ構造
    /// </summary>
    public class HalfEdgeDS : Polygon
    {
        /// <summary>
        /// パラメータ
        /// </summary>
        private Dictionary<string, IParameter> parameters = new Dictionary<string, IParameter>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HalfEdgeDS(string name)
            : base(name)
        {
            Editor = new HalfEdgeDSEditor(this);
            Type = PrimitiveType.Triangles;
        }

        /// <summary>
        /// メッシュのインデックスと、頂点を受け取る
        /// </summary>
        /// <param name="position">頂点座標リスト</param>
        /// <param name="polyIndex">「三角形を構成する頂点番号を格納したVector3」のリスト</param>
        public HalfEdgeDS(string name, List<Vector3> position, List<int> polyIndex = null)
            : this(name)
        {
            Initialize(position, polyIndex);
        }

        /// <summary>
        /// パラメータ
        /// </summary>
        public Dictionary<string, IParameter> Parameter
        {
            get
            {
                return parameters;
            }
        }

        /// <summary>
        /// パラメータの追加
        /// </summary>
        /// <param name="parameter"></param>
        public void AddParameter(IParameter parameter)
        {
            parameters.Add(parameter.Name, parameter);
        }

        /// <summary>
        /// ハーフエッジのエディタ
        /// </summary>
        public HalfEdgeDSEditor Editor { get; private set; }

        /// <summary>
        /// エッジの取得
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        /// <returns>エッジ</returns>
        public HalfEdge GetEdge(HalfEdgeVertex start, HalfEdgeVertex end)
        {
            return start.AroundEdge.Where(p => p.Start == start && p.End == end).FirstOrDefault();
        }

        /// <summary>
        /// 一定距離内の頂点の取得
        /// </summary>
        /// <param name="vertex">起点</param>
        /// <param name="distance">距離</param>
        /// <returns>範囲内の頂点</returns>
        public List<HalfEdgeVertex> GetAroundVertex(HalfEdgeVertex vertex, float distance)
        {
            List<HalfEdgeVertex> vertex_list = new List<HalfEdgeVertex>();
            vertex.TmpParameter = true;
            vertex_list.Add(vertex);
            RecursiveAroundPosition(vertex_list, vertex, vertex, distance);
            for (int i = 0; i < vertex_list.Count; i++)
            {
                vertex_list[i].TmpParameter = false;
            }

            return vertex_list;
        }

        /// <summary>
        /// 選択頂点を削除
        /// </summary>
        public void ClearSelection()
        {
            foreach (var vertex in HalfEdgeVertexs)
            {
                vertex.IsSelect = false;
            }
        }


        /// <summary>
        /// ハーフエッジ頂点の取得
        /// </summary>
        public IEnumerable<HalfEdgeVertex> HalfEdgeVertexs
        {
            get
            {
                return Vertexs.OfType<HalfEdgeVertex>();
            }
        }

        /// <summary>
        /// ハーフエッジメッシュの取得
        /// </summary>
        public IEnumerable<HalfEdgeMesh> HalfEdgeMeshs
        {
            get
            {
                return Meshs.OfType<HalfEdgeMesh>();
            }
        }

        /// <summary>
        /// ハーフエッジの取得
        /// </summary>
        public IEnumerable<HalfEdge> HalfEdges
        {
            get
            {
                return Lines.OfType<HalfEdge>();
            }
        }

        #region [make halfedge data structure]

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
            }
            else
            {
                MakeVertexListByVertexIndex(position);
                MakeEdgeListByVertexIndex(poly_Index);
            }

            SetOppositeEdge();

            if (CheckOppositeEdge())
            {
                Logger.Log(Logger.LogLevel.Debug, "Create Half Edge OK!");
            }
        }

        /// <summary>
        /// ハーフエッジ用のリストに頂点を格納
        /// </summary>
        /// <param name="vertexList">頂点リスト</param>
        private void MakeVertexListByVertexIndex(List<Vector3> vertexList)
        {
            for (int i = 0; i < vertexList.Count; i++)
            {
                HalfEdgeVertex vertex = new HalfEdgeVertex(vertexList[i], i);
                Vertexs.Add(vertex);
            }
        }

        /// <summary>
        /// ハーフエッジデータ構造のために、重複する頂点情報を一つに.メッシュ情報も生成
        /// </summary>
        /// <param name="vertexlist">三角形の頂点リスト</param>
        private void CreateHalfEdgeData(List<Vector3> vertexlist)
        {
            HalfEdgeVertex v1 = null, v2 = null, v3 = null;
            //最終的にポリゴンに格納する頂点
            for (int i = 0; i < vertexlist.Count; i++)
            {
                //ないVertexを調査
                var vertex = Vertexs.Find(p => p.Position == vertexlist[i]);
                if (vertex == null)
                {
                    vertex = new HalfEdgeVertex(vertexlist[i], Vertexs.Count);
                    Vertexs.Add(vertex);
                }

                if (v1 == null)
                {
                    v1 = vertex as HalfEdgeVertex;
                }
                else if (v2 == null)
                {
                    v2 = vertex as HalfEdgeVertex;
                }
                else
                {
                    v3 = vertex as HalfEdgeVertex;
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
        private void CreateMesh(HalfEdgeVertex v1, HalfEdgeVertex v2, HalfEdgeVertex v3)
        {
            HalfEdgeMesh mesh = new HalfEdgeMesh(Meshs.Count);
            HalfEdge edge1 = new HalfEdge(mesh, v1, v2, Lines.Count);
            HalfEdge edge2 = new HalfEdge(mesh, v2, v3, Lines.Count + 1);
            HalfEdge edge3 = new HalfEdge(mesh, v3, v1, Lines.Count + 2);

            //メッシュにエッジを格納
            mesh.SetEdge(edge1, edge2, edge3);
            Meshs.Add(mesh);

            Lines.Add(edge1);
            Lines.Add(edge2);
            Lines.Add(edge3);
        }

        /// <summary>
        /// 頂点カラーの更新
        /// </summary>
        public void UpdateVertexColor(VertexColor colorType, float minValue, float maxValue)
        {
            IEnumerable<Vector3> color = null;
            switch (colorType)
            {
                case VertexColor.Default:
                    color = HalfEdgeVertexs.Select(p => p.Color);
                    break;
                case VertexColor.WireFrame:
                    color = HalfEdgeVertexs.Select(p => Vector3.Zero);
                    break;
                case VertexColor.Voronoi:
                    color = HalfEdgeVertexs.Select(p => KICalc.GetPseudoColor(p.Voronoi, minValue, maxValue));
                    break;
                case VertexColor.MeanCurvature:
                    color = HalfEdgeVertexs.Select(p => KICalc.GetPseudoColor(p.MeanCurvature, minValue, maxValue));
                    break;
                case VertexColor.GaussCurvature:
                    color = HalfEdgeVertexs.Select(p => KICalc.GetPseudoColor(p.GaussCurvature, minValue, maxValue));
                    break;
                case VertexColor.MinCurvature:
                    color = HalfEdgeVertexs.Select(p => KICalc.GetPseudoColor(p.MinCurvature, minValue, maxValue));
                    break;
                case VertexColor.MaxCurvature:
                    color = HalfEdgeVertexs.Select(p => KICalc.GetPseudoColor(p.MaxCurvature, minValue, maxValue));
                    break;
                case VertexColor.TmpParameter:
                    if (HalfEdgeVertexs.First().TmpParameter is IVertexColorParameter)
                    {
                        color = HalfEdgeVertexs.Select(p => KICalc.GetPseudoColor(((IVertexColorParameter)p.TmpParameter).Value, minValue, maxValue));
                    }
                    break;
                default:
                    break;
            }

            if (color != null)
            {
                OnUpdate(new UpdatePolygonEventArgs(PrimitiveType.Triangles, color.ToList()));
            }
        }

        public void Setup(IEnumerable<HalfEdgeVertex> vertexs, IEnumerable<HalfEdge> edges, IEnumerable<HalfEdgeMesh> meshs)
        {
            Vertexs = vertexs.OfType<Vertex>().ToList();
            Lines = edges.OfType<Line>().ToList();
            Meshs = meshs.OfType<Mesh>().ToList();
        }

        /// <summary>
        /// 頂点番号を持つエッジと面を生成
        /// </summary>
        /// <param name="polyIndex">頂点配列</param>
        private void MakeEdgeListByVertexIndex(List<int> polyIndex)
        {
            int poly_Num = polyIndex.Count / 3;
            for (int num = 0; num < poly_Num; num++)
            {
                HalfEdgeVertex v1 = Vertexs[polyIndex[3 * num]] as HalfEdgeVertex;
                HalfEdgeVertex v2 = Vertexs[polyIndex[3 * num + 1]] as HalfEdgeVertex;
                HalfEdgeVertex v3 = Vertexs[polyIndex[3 * num + 2]] as HalfEdgeVertex;
                CreateMesh(v1, v2, v3);
            }
        }

        /// <summary>
        /// 反対エッジのセット
        /// </summary>
        private void SetOppositeEdge()
        {
            foreach (var vertex in HalfEdgeVertexs)
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
            HalfEdgeVertex start = edge.Start;
            HalfEdgeVertex end = edge.End;
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
            foreach (var edge in HalfEdges)
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
        private void RecursiveAroundPosition(List<HalfEdgeVertex> vertex_list, HalfEdgeVertex vertex, HalfEdgeVertex startVertex, float distance)
        {
            float length;
            foreach (var aroundEdge in vertex.AroundEdge)
            {
                length = (startVertex - aroundEdge.End).Length;
                if (length < distance && (bool)aroundEdge.End.TmpParameter == false)
                {
                    aroundEdge.End.TmpParameter = true;
                    vertex_list.Add(aroundEdge.End);
                    RecursiveAroundPosition(vertex_list, aroundEdge.End, startVertex, distance);
                }
            }
        }
        #endregion
    }
}