using KI.Gfx;
using KI.Renderer;
using KI.Tool.Command;

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
            if (polygonNode.Type != PolygonType.Triangles)
            {
                return CommandResult.Failed;
            }

            return CommandResult.Success;
        }
    }
}
