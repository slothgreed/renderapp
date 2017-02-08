using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Command;
using KI.Gfx;

namespace KI.Gfx.Analyzer
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
    public class Dijkstra : ICommand, IAnalyzer
    {
        public GeometryInfo Geometry
        {
            get;
            set;
        }
        public int StartIndex
        {
            get;
            set;
        }
        public int EndIndex
        {
            get;
            set;
        }
        private float _distance;
        public float Distance
        {
            get
            {
                return _distance;
            }
            set
            {
                _distance = value;
            }
        }
        private HalfEdge _halfEdge = null;
        public Dijkstra()
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

            return true;
        }

        public bool CanExecute()
        {
            if (Geometry == null)
                return false;

            if (StartIndex < 0 ||
                EndIndex < 0 ||
                StartIndex > _halfEdge.m_Vertex.Count ||
                EndIndex > _halfEdge.m_Vertex.Count)
            {
                return false;
            }
            return true;
        }
        public bool Execute()
        {
            //var analyze = Geometry.FindAnalyze(HalfEdge.ToString());
            //if (analyze != null)
            //{
            //    _halfEdge = analyze as HalfEdge;
            //}
            //else
            //{
            //    HalfEdge half = new HalfEdge(Geometry);
            //    Geometry.AddAnalayzer(half);
            //    _halfEdge = half;
            //}
            //return DistanceDijkstra(StartIndex, EndIndex);
            return true;
        }

        private void CalcDijkstra(Node[] nodes,int start,int end)
        {
            //initialize
            Node current = nodes[start];
            current.done = true;
            current.cost = 0;

            //calc start
            foreach(var aroundVertex in nodes[start].vertex.GetAroundVertex())
            {
                Node around = nodes[aroundVertex.Index];

                if(around.done)
                {
                    float newCost = around.cost;
                    newCost += (current.vertex - around.vertex).Length;
                    if(around.cost > newCost)
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
            Geometry = null;
            Distance = -1;
            return true;
        }

        public bool Undo()
        {
            return false;
        }
    }
}
