using System.Collections.Generic;
using KI.Mathmatics;
using OpenTK;
using KI.Foundation.Core;
using KI.Asset.Primitive;
using System.Linq;

namespace CADApp.Model
{
    public class BezierAssembly : CurveAssembly
    {
        public BezierAssembly(string name)
            : base(name)
        {

        }

        protected override void UpdateControlPoint()
        {
            ClearVertex();

            if (ControlPoint.Count > 3)
            {
                //　仮で面データを作成
                UNum = ControlPoint.Count;
                VNum = ControlPoint.Count;
                {
                    Vector3 firstPoint = ControlPoint[0].Position;

                    for (int j = 1; j < UNum; j++)
                    {
                        for (int i = 0; i < VNum; i++)
                        {
                            Vector3 heightPoint = new Vector3(ControlPoint[i].Position);
                            heightPoint.Y += j * 0.1f;
                            AddControlPoint(heightPoint);
                        }
                    }
                }

                Bezier surface = new Bezier(ControlPoint.Select(v => v.Position).ToArray(), UNum, VNum, 100);
                SetVertex(surface.Position);
                SetTriangleIndex(surface.Index);
            }
            else
            {
                Bezier bezierLine = new Bezier(ControlPoint.Select(v => v.Position).ToArray(), ControlPoint.Count, 1, 100);
                SetVertex(bezierLine.Position);
                SetLineIndex(bezierLine.Index.ToList());
            }
        }
    }
}
