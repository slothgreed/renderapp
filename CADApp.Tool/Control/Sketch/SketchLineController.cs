using System.Windows.Forms;
using KI.Tool.Control;

namespace CADApp.Tool.Control
{
    class SketchLineController : IController
    {
        private float zPosition = 0;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mouse"></param>
        /// <returns></returns>
        public override bool Move(MouseEventArgs mouse)
        {

            return true;
        }
    }
}
