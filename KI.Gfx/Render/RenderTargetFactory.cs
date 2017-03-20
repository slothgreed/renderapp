using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KIAsset;
using KI.Foundation.Core;
namespace KI.Gfx.Render
{
    public class RenderTargetFactory : KIFactoryBase<RenderTarget>
    {
        private static RenderTargetFactory _instance = new RenderTargetFactory();
        public static RenderTargetFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public RenderTarget CreateRenderTarget(string name, int width, int height, int outputNum)
        {
            RenderTarget target = new RenderTarget(name, width, height, outputNum);
            AddItem(target);
            return target;
        }

        private RenderTarget _default = null;
        public RenderTarget Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new RenderTarget("DefaultBuffer", 1, 1, 1);
                }
                return _default;
            }
        }

    }

}
