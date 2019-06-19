using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.Primitive
{
    public class SplineCurve
    {
        /// <summary>
        /// 位置情報(GL_LINES)
        /// </summary>
        public Vector3[] Position;

        /// <summary>
        /// コントロールポイント
        /// </summary>
        public Vector3[] ControlPoint;

        /// <summary>
        /// インデックス情報
        /// </summary>
        public int[] Index;

        /// <summary>
        /// Line全体の点の数
        /// </summary>
        private int Partition;

        /// <summary>
        /// スプライン曲線
        /// </summary>
        /// <param name="controlPoint">コントロールポイント</param>
        /// <param name="partition">分割数</param>
        public SplineCurve(Vector3[] controlPoint, int partition)
        {
            ControlPoint = controlPoint;
            Partition = partition;

            CreateModel();
        }

        /// <summary>
        /// 形状の作成 参考サイト
        /// http://www5d.biglobe.ne.jp/stssk/maze/spline.html
        /// </summary>
        private void CreateModel()
        {
            var a = new Vector3[ControlPoint.Length];
            var b = new Vector3[ControlPoint.Length];
            var c = new Vector3[ControlPoint.Length];
            var d = new Vector3[ControlPoint.Length];
            var w = new Vector3[ControlPoint.Length];

            // 公式の算出
            {
                for (int i = 0; i < ControlPoint.Length; i++)
                {
                    a[i] = ControlPoint[i];
                }

                c[0] = Vector3.Zero;
                c[ControlPoint.Length - 1] = Vector3.Zero;
                for (int i = 1; i < ControlPoint.Length - 1; i++)
                {
                    c[i] = 3 * (a[i - 1] - 2.0f * a[i] + a[i + 1]);
                }

                w[0] = Vector3.Zero;
                Vector3 tmp;
                for (int i = 1; i < ControlPoint.Length - 1; i++)
                {
                    tmp.X = 4.0f - w[i - 1].X;
                    tmp.Y = 4.0f - w[i - 1].Y;
                    tmp.Z = 4.0f - w[i - 1].Z;

                    c[i] = (c[i] - c[i - 1]);
                    c[i].X /= tmp.X;
                    c[i].Y /= tmp.Y;
                    c[i].Z /= tmp.Z;

                    w[i].X /= tmp.X;
                    w[i].Y /= tmp.Y;
                    w[i].Z /= tmp.Z;
                }

                for (int i = ControlPoint.Length - 2; i > 0; i--)
                {
                    c[i] = c[i] - c[i + 1] * w[i];
                }

                b[ControlPoint.Length - 1] = Vector3.Zero;
                d[ControlPoint.Length - 1] = Vector3.Zero;
                for (int i = 0; i < ControlPoint.Length - 1; i++)
                {
                    d[i] = (c[i + 1] - c[i]) / 3.0f;
                    b[i] = a[i + 1] - a[i] - c[i] - d[i];
                }


            }

            List<Vector3> vertex = new List<Vector3>();
            float num = 1.0f / Partition;
            for (float i = 0; i < ControlPoint.Length - 1; i += num)
            {
                int index = (int)Math.Floor(i);
                float dt = i - index;
                vertex.Add(new Vector3(a[index] + (b[index] + (c[index] + d[index] * dt) * dt) * dt));
            }

            Position = vertex.ToArray();

            if (vertex.Count > 0)
            {
                List<int> line = new List<int>();
                line.Add(0);
                for (int i = 1; i < vertex.Count - 1; i++)
                {
                    line.Add(i);
                    line.Add(i);
                }

                line.Add(vertex.Count - 1);
                Index = line.ToArray();
            }
        }
    }
}
