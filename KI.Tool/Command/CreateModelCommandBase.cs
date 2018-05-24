using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Asset;
using KI.Gfx;

namespace KI.Tool.Command
{
    public class CreateModelCommandBase
    {
        protected CommandResult CanCreatePolygon(KIObject asset)
        {
            if (!(asset is RenderObject))
            {
                return CommandResult.Failed;
            }

            RenderObject renderObject = asset as RenderObject;
            if (renderObject.Polygon.Type != PolygonType.Triangles)
            {
                return CommandResult.Failed;
            }

            return CommandResult.Success;
        }
    }
}
