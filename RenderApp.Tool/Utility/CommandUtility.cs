using KI.Asset;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Tool.Command;

namespace RenderApp.Tool
{
    public static class CommandUtility
    {
        public static CommandResult CanCreatePolygon(KIObject asset)
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
