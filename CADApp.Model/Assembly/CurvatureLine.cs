using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CADApp.Model
{
    public abstract class CurvatureLine : Assembly
    {
        public CurvatureLine(string name)
            : base(name)
        {

        }

        protected abstract void UpdateControlPoint();

        public override void EndEdit()
        {
            UpdateControlPoint();
            base.EndEdit();
        }
    }
}
