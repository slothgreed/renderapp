using KI.Foundation.Core;

namespace KI.Gfx.Render
{
    public class RenderTargetFactory : KIFactoryBase<RenderTarget>
    {
        public static RenderTargetFactory Instance { get; } = new RenderTargetFactory();

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
