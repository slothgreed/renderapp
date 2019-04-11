using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Renderer;

namespace CADApp.Model.Node
{
    /// <summary>
    /// アプリケーションルートノード
    /// </summary>
    public class AppRootNode : SceneNode
    {
        public AppRootNode(string name)
            : base(name)
        {

        }



        public override void RenderCore(Scene scene)
        {

        }
    }
}
