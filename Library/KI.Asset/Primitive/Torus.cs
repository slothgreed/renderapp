using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.Primitive
{
    public class Torus : PrimitiveBase
    {
        /// <summary>
        /// 内径
        /// </summary>
        private float inner;

        /// <summary>
        /// 外形
        /// </summary>
        private float outer;

        /// <summary>
        /// ロンジチュードの分割数(輪)
        /// </summary>
        private int ringSubdiv;

        /// <summary>
        /// メリディアンの分割数(内径から外形にかけてできる輪)
        /// </summary>
        private int radialSubdiv;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="inner">内径</param>
        /// <param name="outer">外形</param>
        /// <param name="ringSubdiv">ロンジチュードの分割数(輪)</param>
        /// <param name="radialSubdiv">メリディアンの分割数(内径から外形にかけてできる輪)</param>
        public Torus(float _inner, float _outer, int _ringSubdiv, int _radialSubdiv)
        {
            inner = _inner;
            outer = _outer;
            ringSubdiv = _ringSubdiv;
            radialSubdiv = _radialSubdiv;
            CreateModel();
        }

        /// <summary>
        /// モデルの作成
        /// </summary>
        private void CreateModel()
        {
            int perRing = 360 / ringSubdiv;
            int perRad = 360 / radialSubdiv;

            List<Vertex> vertexList = new List<Vertex>();
            List<int> indexList = new List<int>();
            for (int i = 0; i < 360; i += perRing)
            {
                float t = MathHelper.DegreesToRadians(i);
                float tNext = 0;
                if (i + perRing >= 360)
                {
                    tNext = MathHelper.DegreesToRadians(0);
                }
                else
                {
                    tNext = MathHelper.DegreesToRadians(i + perRing);
                }

                for (int j = 0; j < 360; j += perRad)
                {
                    float p = MathHelper.DegreesToRadians(j);
                    float pNext = 0;
                    if (j + perRad >= 360)
                    {
                        pNext = MathHelper.DegreesToRadians(0);
                    }
                    else
                    {
                        pNext = MathHelper.DegreesToRadians(j + perRad);
                    }


                    Vector3 position1 = GetTorusPoint(inner, outer, t, p);
                    Vector3 normal1 = GetTorusNormal(inner, outer, t, p);

                    Vector3 position2 = GetTorusPoint(inner, outer, t, pNext);
                    Vector3 normal2 = GetTorusNormal(inner, outer, t, pNext);

                    Vector3 position3 = GetTorusPoint(inner, outer, tNext, p);
                    Vector3 normal3 = GetTorusNormal(inner, outer, tNext, p);

                    Vector3 position4 = GetTorusPoint(inner, outer, tNext, pNext);
                    Vector3 normal4 = GetTorusNormal(inner, outer, tNext, pNext);

                    vertexList.Add(new Vertex(vertexList.Count, position1, Vector3.UnitX, normal1));
                    vertexList.Add(new Vertex(vertexList.Count, position3, Vector3.UnitX, normal3));
                    vertexList.Add(new Vertex(vertexList.Count, position2, Vector3.UnitX, normal2));

                    vertexList.Add(new Vertex(vertexList.Count, position2, Vector3.UnitX, normal2));
                    vertexList.Add(new Vertex(vertexList.Count, position3, Vector3.UnitX, normal3));
                    vertexList.Add(new Vertex(vertexList.Count, position4, Vector3.UnitX, normal4));
                }
            }


            Vertexs = vertexList.ToArray();
            Index = indexList.ToArray();
            Type = KIPrimitiveType.Triangles;
        }

        /// <summary>
        /// トーラス位置の取得
        /// </summary>
        /// <param name="inner">内径</param>
        /// <param name="outer">外形</param>
        /// <param name="t">輪の媒介変数ラジアン</param>
        /// <param name="p">内径から外形にかけてできる輪の媒介変数ラジアン</param>
        /// <returns></returns>
        private Vector3 GetTorusPoint(float inner, float outer, float t, float p)
        {
            Vector3 position = new Vector3();
            float cost = (float)Math.Cos(t);
            float cosp = (float)Math.Cos(p);
            float sint = (float)Math.Sin(t);
            float sinp = (float)Math.Sin(p);

            position.X = outer * cost + inner * cosp * cost;
            position.Y = outer * sint + inner * cosp * sint;
            position.Z = inner * sinp;

            return position;
        }

        private Vector3 GetTorusNormal(float inner, float outer, float t, float p)
        {
            Vector3 normal = new Vector3();
            float cost = (float)Math.Cos(t);
            float cosp = (float)Math.Cos(p);
            float sint = (float)Math.Sin(t);
            float sinp = (float)Math.Sin(p);

            normal.X = cost * cosp;
            normal.Y = sint * cosp;
            normal.Z = sinp;

            return normal;
        }
    }
}
