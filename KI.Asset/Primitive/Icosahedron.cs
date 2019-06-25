using System;
using System.Collections.Generic;
using OpenTK;


namespace KI.Asset.Primitive
{
    public class Icosahedron : PrimitiveBase
    {
        /// <summary>
        /// 半径
        /// </summary>
        private float radial;

        /// <summary>
        /// 細分割回数
        /// </summary>
        private int smoothNum;

        /// <summary>
        /// 中心位置
        /// </summary>
        private Vector3 centerPos;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="radial">半径</param>
        /// <param name="subdivNum">細分割回数</param>
        /// <param name="centerPos">中心位置</param>
        public Icosahedron(float radial, int subdivNum, Vector3 centerPos)
        {
            this.radial = radial;
            this.smoothNum = subdivNum;
            this.centerPos = centerPos;
            CreateModel();
        }

        public void CreateModel()
        {
            var position = new List<Vector3>();
            var normal = new List<Vector3>();
            var index = new List<int>();
            var ratio = (float)(1 + Math.Sqrt(5)) / 2.0f; // 黄金比
            ratio *= radial;

            position.Add(new Vector3(0, ratio, radial));
            position.Add(new Vector3(0, ratio, -radial));
            position.Add(new Vector3(0, -ratio, radial));
            position.Add(new Vector3(0, -ratio, -radial));

            position.Add(new Vector3(ratio, radial, 0));
            position.Add(new Vector3(ratio, -radial, 0));
            position.Add(new Vector3(-ratio, radial, 0));
            position.Add(new Vector3(-ratio, -radial, 0));

            position.Add(new Vector3(radial, 0, ratio));
            position.Add(new Vector3(-radial, 0, ratio));
            position.Add(new Vector3(radial, 0, -ratio));
            position.Add(new Vector3(-radial, 0, -ratio));

            for (int i = 0; i < position.Count; i++)
            {
                position[i] += centerPos;
            }

            foreach (var pos in position)
            {
                normal.Add(pos.Normalized());
            }

            Position = position.ToArray();
            Normal = normal.ToArray();
            Index = new int[60]
                {
                    1,0,4,0,1,6,2,3,5,3,2,7,
                    4,5,10,5,4,8,6,7,9,7,6,11,
                    8,9,2,9,8,0,10,11,1,11,10,3,
                    0,8,4,0,6,9,1,4,10,1,11,6,
                    2,5,8,2,9,7,3,10,5,3,7,11
                };
        }
    }
}
