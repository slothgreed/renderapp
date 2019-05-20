using System;
using System.Collections.Generic;
using OpenTK;

namespace CADApp.Model
{
    /// <summary>
    /// スプライン
    /// </summary>
    public class SplineCurve : CurveAssembly
    {
        public SplineCurve(string name)
            : base(name)
        {
        }

        /// <summary>
        /// 参考サイト
        /// http://www5d.biglobe.ne.jp/stssk/maze/spline.html
        /// </summary>
        protected override void UpdateControlPoint()
        {
            var a = new Vector3[ControlPoint.Count];
            var b = new Vector3[ControlPoint.Count];
            var c = new Vector3[ControlPoint.Count];
            var d = new Vector3[ControlPoint.Count];
            var w = new Vector3[ControlPoint.Count];
            for (int i = 0; i < ControlPoint.Count; i++)
            {
                a[i] = ControlPoint[i].Position;
            }

            c[0] = Vector3.Zero;
            c[ControlPoint.Count - 1] = Vector3.Zero;
            for (int i = 1; i < ControlPoint.Count - 1; i++)
            {
                c[i] = 3 * (a[i - 1] - 2.0f * a[i] + a[i + 1]);
            }

            w[0] = Vector3.Zero;
            Vector3 tmp;
            for (int i = 1; i < ControlPoint.Count - 1; i++)
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

            for (int i = ControlPoint.Count - 2; i > 0; i--)
            {
                c[i] = c[i] - c[i + 1] * w[i];
            }

            b[ControlPoint.Count - 1] = Vector3.Zero;
            d[ControlPoint.Count - 1] = Vector3.Zero;
            for (int i = 0; i < ControlPoint.Count - 1; i++)
            {
                d[i] = (c[i + 1] - c[i]) / 3.0f;
                b[i] = a[i + 1] - a[i] - c[i] - d[i];
            }

            ClearVertex();

            for (float i = 0; i < ControlPoint.Count - 1; i += 0.01f)
            {
                int index = (int)Math.Floor(i);
                float dt = i - index;
                AddVertex(new Vector3(a[index] + (b[index] + (c[index] + d[index] * dt) * dt) * dt));
            }

            if(Vertex.Count > 0)
            {
                List<int> line = new List<int>();
                line.Add(0);
                for (int i = 1; i < Vertex.Count - 1; i++)
                {
                    line.Add(i);
                    line.Add(i);
                }
                line.Add(Vertex.Count - 1);
                SetLineIndex(line);
            }
        }
    }
}
