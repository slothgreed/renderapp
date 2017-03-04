using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KIAsset;

namespace KI.Gfx.Render
{
    public class RenderTargetFactory
    {
        private static RenderTargetFactory _instance = new RenderTargetFactory();
        public static RenderTargetFactory Instance
        {
            get
            {
                return _instance;
            }
        }
        private List<RenderTarget> renderTargetList = new List<RenderTarget>();


        public RenderTarget CreateRenderTarget(string name, int width, int height, int outputNum)
        {
            RenderTarget target = new RenderTarget(name, width, height, outputNum);
            renderTargetList.Add(target);
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

        public void Dispose()
        {
            foreach(var frame in renderTargetList)
            {
                frame.Dispose();
            }
        }
    }

}
