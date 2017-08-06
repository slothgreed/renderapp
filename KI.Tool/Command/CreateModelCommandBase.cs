using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;
using KI.Renderer;

namespace KI.Tool.Command
{
    public class CreateModelCommandBase
    {
        protected CommandResult CanCreateGeometry(KIObject asset)
        {
            if (!(asset is RenderObject))
            {
                return CommandResult.Failed;
            }

            RenderObject renderObject = asset as RenderObject;
            if (renderObject.Geometry.GeometryType != GeometryType.Triangle)
            {
                return CommandResult.Failed;
            }

            return CommandResult.Success;
        }
    }
}
