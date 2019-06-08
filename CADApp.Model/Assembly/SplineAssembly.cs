using System;
using System.Collections.Generic;
using OpenTK;
using KI.Asset.Primitive;
using System.Linq;

namespace CADApp.Model
{
    /// <summary>
    /// スプライン
    /// </summary>
    public class SplineAssembly : CurveAssembly
    {
        public SplineAssembly(string name)
            : base(name)
        {
        }


        protected override void UpdateControlPoint()
        {
            ClearVertex();

            if (ControlPoint.Count == 4)
            {
                var spline = new BSpline(ControlPoint.Select(v => v.Position).ToArray(), 3);
                SetVertex(spline.Position);
                SetLineIndex(spline.Index);

            }
        }
    }
}
