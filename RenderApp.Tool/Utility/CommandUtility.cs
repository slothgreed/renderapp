using KI.Gfx;
using KI.Renderer;
using KI.Tool.Command;

namespace RenderApp.Tool
{
    public static class CommandUtility
    {
        public static CommandResult CanCreatePolygon(SceneNode asset)
        {
            if (!(asset is RenderObject))
            {
                return CommandResult.Failed;
            }

            RenderObject renderObject = asset as RenderObject;
            if (renderObject.Type != PolygonType.Triangles)
            {
                return CommandResult.Failed;
            }

            return CommandResult.Success;
        }
    }
}
