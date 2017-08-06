using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Renderer;

namespace KI.Tool.Command
{
    public class CreateModelCommandBase
    {
        protected CommandState CanCreateGeometry(KIObject asset)
        {
            if (!(asset is RenderObject))
            {
                return CommandState.Failed;
            }

            RenderObject renderObject = asset as RenderObject;
            if (renderObject.Geometry.GeometryInfo.GeometryType != GeometryType.Triangle)
            {
                return CommandState.Failed;
            }

            return CommandState.Success;
        }
    }
}
