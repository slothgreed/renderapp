using KI.Foundation.Command;

namespace KI.Analyzer
{
    public struct Node
    {
        public bool done;
        public float cost;
        public Vertex vertex;
        public Node(bool _done, int _Cost, Vertex _v)
        {
            done = _done;
            cost = _Cost;
            vertex = _v;
        }
    }

    public class DijkstraAlgorithm : ICommand
    {
        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public float Distance { get; set; }

        private HalfEdge _halfEdge = null;

        public DijkstraAlgorithm()
        {
            Reset();
        }

        private bool DistanceDijkstra(int index1, int index2)
        {
            Node[] nodeArray = new Node[_halfEdge.m_Vertex.Count];

            for (int i = 0; i < nodeArray.Length; i++)
            {
                nodeArray[i] = new Node(false, -1, _halfEdge.m_Vertex[i]);
            }

            CalcDijkstra(nodeArray, index1, index2);
            return true;
        }

        public string CanExecute(string commandArg = null)
        {
            if (StartIndex < 0 ||
                EndIndex < 0 ||
                StartIndex > _halfEdge.m_Vertex.Count ||
                EndIndex > _halfEdge.m_Vertex.Count)
            {
                return "init value error";
            }
            return string.Empty;
        }

        public void SetGeometry(HalfEdge halfEdge)
        {
            _halfEdge = halfEdge;
        }

        public string Execute(string commandArg = null)
        {
            if (_halfEdge == null)
            {
                return string.Empty;
            }
            DistanceDijkstra(StartIndex, EndIndex);
            return string.Empty;
        }

        private void CalcDijkstra(Node[] nodes, int start, int end)
        {
            //initialize
            Node current = nodes[start];
            current.done = true;
            current.cost = 0;

            //calc start
            foreach (var aroundVertex in nodes[start].vertex.AroundVertex)
            {
                Node around = nodes[aroundVertex.Index];

                if (around.done)
                {
                    float newCost = around.cost;
                    newCost += (current.vertex - around.vertex).Length;
                    if (around.cost > newCost)
                    {
                        around.cost = newCost;
                    }
                }
            }
        }

        public bool Reset()
        {
            StartIndex = -1;
            EndIndex = -1;
            Distance = -1;
            return true;
        }

        public string Undo(string commandArg = null)
        {
            return "Failed";
        }
    }
}
