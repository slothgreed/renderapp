using System.Windows.Forms;
using KI.Tool.Control;

namespace CADApp.Tool.Control
{
    class SketchLineController : IController
    {
        /// <summary>
        /// 配置するZ位置
        /// </summary>
        private float zPosition = 0;

        public override bool Move(MouseEventArgs mouse)
        {

            return true;
        }
    }
}
