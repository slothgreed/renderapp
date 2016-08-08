using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.Analyzer
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
    class Dijkstra
    {
        private HalfEdge halfEdge;
        public Dijkstra(HalfEdge halfedge)
        {
            halfEdge = halfedge;
        }

        public float DistanceDijkstra(int index1, int index2)
        {
            if (index1 < 0 ||
                index2 < 0 ||
                index1 > halfEdge.m_Vertex.Count ||
                index2 > halfEdge.m_Vertex.Count)
            {
                return 0;
            }

            Node[] nodeArray = new Node[halfEdge.m_Vertex.Count];

            for (int i = 0; i < nodeArray.Length; i++)
            {
                nodeArray[i] = new Node(false, -1, halfEdge.m_Vertex[i]);
            }

            return 0;
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
     
        


    }
}
