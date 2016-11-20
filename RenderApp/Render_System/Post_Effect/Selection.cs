using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil.ShaderModel;
namespace RenderApp.Render_System.Post_Effect
{
    partial class Selection : PostEffect
    {
        private int _uID;
        private int uID
        {
            get
            {
                return _uID;
            }
            set
            {
                SetValue<int>(ref _uID, value);
            }
        }
        public override void Initialize()
        {
            uID = -1;
        }
        public Selection(Shader shader)
            :base(shader)
        {

        }
    }
}
