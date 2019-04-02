using System;
using System.Collections.Generic;
using KI.Analyzer;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;


namespace KI.Asset
{
    class Icosahedron : KIObject, ICreateModel
    {

        /// <summary>
        /// 半径
        /// </summary>
        private float radial;

        /// <summary>
        /// スムージング回数
        /// </summary>
        private int smoothNum;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="radial">半径</param>
        /// <param name="smoothNum">スムージング回数</param>
        public Icosahedron(string name, float radial, int smoothNum)
            : base(name)
        {
            this.radial = radial;
            this.smoothNum = smoothNum;
            CreateModel();
        }

        public Polygon Model
        {
            get;
            private set;
        }

        public void CreateModel()
        {
            var position = new List<Vector3>();
            var normal = new List<Vector3>();
            var index = new List<int>();
            var length = (float)(1 + Math.Sqrt(5)) / 2.0f; // 黄金比

            position.Add(new Vector3(0, length, 1));
            position.Add(new Vector3(0, length, -1));
            position.Add(new Vector3(0, -length, 1));
            position.Add(new Vector3(0, -length, -1));

            position.Add(new Vector3(length, 1, 0));
            position.Add(new Vector3(length, -1, 0));
            position.Add(new Vector3(-length, 1, 0));
            position.Add(new Vector3(-length, -1, 0));

            position.Add(new Vector3(1, 0, length));
            position.Add(new Vector3(-1, 0, length));
            position.Add(new Vector3(1, 0, -length));
            position.Add(new Vector3(-1, 0, -length));

            var indexArray = new int[60]
                {
                    1,0,4,0,1,6,2,3,5,3,2,7,
                    4,5,10,5,4,8,6,7,9,7,6,11,
                    8,9,2,9,8,0,10,11,1,11,10,3,
                    0,8,4,0,6,9,1,4,10,1,11,6,
                    2,5,8,2,9,7,3,10,5,3,7,11
                };

            index.AddRange(indexArray);
            var vertexs = new List<Vertex>();

            HalfEdgeDS halfEdgeDS = new HalfEdgeDS(this.Name, position, index);
            foreach (var vertex in halfEdgeDS.Vertexs)
            {
                vertexs.Add(vertex.Clone());
            }

            //for (int i = 0; i < position.Count; i++)
            //{
            //    vertexs.Add(new Vertex(i, position[i]));
            //}

            Model = new Polygon(this.Name, vertexs, index, PolygonType.Triangles);
        }
    }
}
