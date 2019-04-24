using System.Collections.Generic;
using KI.Mathmatics;
using OpenTK;

namespace CADApp.Model
{
    public class BezierCurvature : Assembly
    {
        public BezierCurvature(string name)
            : base(name)
        {

        }

        /// <summary>
        /// ベジエ曲線上の点
        /// </summary>
        /// <param name="t">媒介変数</param>
        /// <returns>位置</returns>
        public Vector3 BezierPoint(float t)
        {
            Vector3 point = Vector3.Zero;
            for (int i = 0; i < ControlPoint.Count; i++)
            {
                float bernstein = Calculator.Bernstein(ControlPoint.Count - 1, i, t);
                point += ControlPoint[i].Position * bernstein;
                
            }

            return point;
        }

        private void UpdateVertexList()
        {
            ClearVertex();

            for (float i = 0; i < 1; i += 0.01f)
            {
                AddVertex(BezierPoint(i));
            }


            if (Vertex.Count > 0)
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

        public override void EndEdit()
        {
            UpdateVertexList();
            base.EndEdit();
        }
    }
}
