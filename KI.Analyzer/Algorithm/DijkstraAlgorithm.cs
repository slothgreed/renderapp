using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace KI.Analyzer
{
    /// <summary>
    /// ノード
    /// </summary>
    public class Node
    {
        /// <summary>
        /// ノード
        /// </summary>
        /// <param name="done">確定したか</param>
        /// <param name="cost">重み</param>
        /// <param name="vertex">頂点座標</param>
        public Node(bool done, int cost, Vertex vertex)
        {
            Done = done;
            Cost = cost;
            Vertex = vertex;
        }

        /// <summary>
        /// 確定したか
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        /// 最小経路を辿った一つ前のノード
        /// </summary>
        public Node MinRouteNode { get; set; }

        /// <summary>
        /// 重み
        /// </summary>
        public float Cost { get; set; }

        /// <summary>
        /// 頂点座標
        /// </summary>
        public Vertex Vertex { get; private set; }
    }

    /// <summary>
    /// ダイクストラアルゴリズム
    /// </summary>
    public class DijkstraAlgorithm
    {
        /// <summary>
        /// ハーフエッジ
        /// </summary>
        private HalfEdgeDS halfEdge;

        /// <summary>
        /// 開始位置
        /// </summary>
        private int startIndex;

        /// <summary>
        /// 終了位置
        /// </summary>
        private int endIndex;

        /// <summary>
        /// ノード配列
        /// </summary>
        private Node[] nodes;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="half">ハーフエッジ</param>
        /// <param name="start">開始位置</param>
        /// <param name="end">終了位置</param>
        public DijkstraAlgorithm(HalfEdgeDS half, Vertex start, Vertex end)
        {
            halfEdge = half;
            startIndex = start.Index;
            endIndex = end.Index; 
        }

        /// <summary>
        /// 総和距離
        /// </summary>
        public float Distance { get; private set; }

        /// <summary>
        /// 実行
        /// </summary>
        /// <returns>成功</returns>
        public bool Execute()
        {
            if (!CanExecute())
            {
                return false;
            }

            Initialize();
            List<Node> noConfirms = new List<Node>();
            CalculateCost(nodes[startIndex], noConfirms);

            return true;
        }

        /// <summary>
        /// ダイクストラの算出結果
        /// </summary>
        /// <returns>ダイクストラの線算出</returns>
        public List<Vector3> DijkstraLine()
        {
            List<Vector3> line = new List<Vector3>();
            Node current = nodes[endIndex];
            while (current != nodes[startIndex])
            {
                line.Add(current.Vertex.Position);
                line.Add(current.MinRouteNode.Vertex.Position);

                current = current.MinRouteNode;
            }

            return line;
        }

        /// <summary>
        /// ダイクストラの初期化処理
        /// </summary>
        private void Initialize()
        {
            nodes = new Node[halfEdge.Vertexs.Count];

            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new Node(false, -1, halfEdge.Vertexs[i]);
            }

            //initialize
            nodes[startIndex].Cost = 0;
        }

        /// <summary>
        /// 確定ノードの探索
        /// </summary>
        /// <param name="candidate">候補</param>
        /// <returns>確定ノード</returns>
        private Node FindConfirmNode(IEnumerable<Node> candidate)
        {
            float minValue = float.MaxValue;
            Node node = null;
            foreach (var item in candidate.Where(p => !p.Done))
            {
                if (minValue > item.Cost)
                {
                    minValue = item.Cost;
                    node = item;
                }
            }

            return node;
        }

        /// <summary>
        /// コストの計算
        /// </summary>
        /// <param name="current">コストを計算するノード</param>
        /// <param name="noConfirm">計算後の未確定ノード</param>
        private void CalculateCost(Node current, List<Node> noConfirm)
        {
            while (true)
            {
                if (current == null)
                {
                    return;
                }

                //calc start
                current.Done = true;

                if (current == nodes[endIndex])
                {
                    return;
                }

                float minValue = float.MaxValue;
                Node minNode = null;

                foreach (var aroundVertex in current.Vertex.AroundVertex)
                {
                    Node around = nodes[aroundVertex.Index];

                    if (!around.Done)
                    {
                        float newCost = 0;
                        newCost = current.Cost + (current.Vertex - around.Vertex).Length;

                        if (around.Cost > newCost || around.Cost == -1)
                        {
                            around.Cost = newCost;
                            around.MinRouteNode = current;

                            if (minValue > newCost)
                            {
                                minValue = newCost;
                                minNode = nodes[aroundVertex.Index];
                            }
                        }

                        if (!noConfirm.Contains(around))
                        {
                            noConfirm.Add(around);
                        }
                    }
                }

                current = FindConfirmNode(noConfirm);
            }
        }

        /// <summary>
        /// 実行できるか
        /// </summary>
        /// <returns>できる</returns>
        private bool CanExecute()
        {
            if (startIndex < 0 ||
                endIndex < 0 ||
                startIndex > halfEdge.Vertexs.Count ||
                endIndex > halfEdge.Vertexs.Count ||
                halfEdge == null)
            {
                return false;
            }

            return true;
        }
    }
}
