using KI.Asset.Primitive;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.Gizmo
{
    /// <summary>
    /// 回転用ギズモ
    /// </summary>
    public class RotateGizmo : GizmoBase
    {
        public RotateGizmo()
        {
            Create();
        }

        private void Create()
        {
            Circle circleXY = new Circle(1, Vector3.Zero, Vector3.UnitZ, 10);
            Circle circleYZ = new Circle(1, Vector3.Zero, Vector3.UnitX, 10);
            Circle circleXZ = new Circle(1, Vector3.Zero, Vector3.UnitY, 10);


        }
    }
}
