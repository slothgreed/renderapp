using KI.Foundation.Command;

namespace KI.Analyzer
{
    public struct Node
    {
        public bool Done;
        public float Cost;
        public Vertex Vertex;

        public Node(bool done, int cost, Vertex vertex)
        {
            Done = done;
            Cost = cost;
            Vertex = vertex;
        }
    }

    public class DijkstraAlgorithm : ICommand
    {
        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public float Distance { get; set; }

        private HalfEdge halfEdge = null;

        public DijkstraAlgorithm()
        {
            Reset();
        }

        public string CanExecute(string commandArg = null)
        {
            if (StartIndex < 0 ||
                EndIndex < 0 ||
                StartIndex > halfEdge.vertexs.Count ||
                EndIndex > halfEdge.vertexs.Count)
            {
                return "init value error";
            }

            return string.Empty;
        }

        public void SetGeometry(HalfEdge halfEdge)
        {
            this.halfEdge = halfEdge;
        }

        public string Execute(string commandArg = null)
        {
            if (halfEdge == null)
            {
                return string.Empty;
            }

            DistanceDijkstra(StartIndex, EndIndex);
            return string.Empty;
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

        private bool DistanceDijkstra(int index1, int index2)
        {
            Node[] nodeArray = new Node[halfEdge.vertexs.Count];

            for (int i = 0; i < nodeArray.Length; i++)
            {
                nodeArray[i] = new Node(false, -1, halfEdge.vertexs[i]);
            }

            CalcDijkstra(nodeArray, index1, index2);
            return true;
        }

        private void CalcDijkstra(Node[] nodes, int start, int end)
        {
            //initialize
            Node current = nodes[start];
            current.Done = true;
            current.Cost = 0;

            //calc start
            foreach (var aroundVertex in nodes[start].Vertex.AroundVertex)
            {
                Node around = nodes[aroundVertex.Index];

                if (around.Done)
                {
                    float newCost = around.Cost;
                    newCost += (current.Vertex - around.Vertex).Length;
                    if (around.Cost > newCost)
                    {
                        around.Cost = newCost;
                    }
                }
            }
        }
    }
}
