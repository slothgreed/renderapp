using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CADApp.Model
{
    public abstract class CurveAssembly : Assembly
    {
        public CurveAssembly(string name)
            : base(name)
        {

        }

        /// <summary>
        /// U の数
        /// </summary>
        protected int UNum { get; set; } = 1;

        /// <summary>
        /// V の数
        /// </summary>
        protected int VNum { get; set; } = 1;

        protected abstract void UpdateControlPoint();

        public override void EndEdit()
        {
            UpdateControlPoint();
            base.EndEdit();
        }
    }
}
