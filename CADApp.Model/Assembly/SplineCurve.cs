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

        /// <summary>
        /// 参考サイト
        /// http://www5d.biglobe.ne.jp/stssk/maze/spline.html
        /// </summary>
        protected override void UpdateControlPoint()
        {
            ClearVertex();

            if (ControlPoint.Count > 1)
            {
                var spline = new SplineCurve(ControlPoint.Select(v => v.Position).ToArray(), 100);
                SetVertex(spline.Position);
                SetLineIndex(spline.Index);
            }
        }
    }
}
