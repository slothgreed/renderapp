using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.GLUtil.ShaderModel;
namespace RenderApp.Render_System.Post_Effect
{
    public partial class Selection : PostEffect
    {
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
