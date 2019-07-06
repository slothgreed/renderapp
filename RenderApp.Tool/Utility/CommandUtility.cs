using KI.Gfx;
using KI.Renderer;
using KI.Foundation.Command;

namespace RenderApp.Tool
{
    public static class CommandUtility
    {
        public static CommandResult CanCreatePolygon(SceneNode asset)
        {
            if (!(asset is PolygonNode))
            {
                return CommandResult.Failed;
            }

            PolygonNode polygonNode = asset as PolygonNode;
            if (polygonNode.Type != KIPrimitiveType.Triangles)
            {
                return CommandResult.Failed;
            }

            return CommandResult.Success;
        }
    }
}
