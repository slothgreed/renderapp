using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Renderer;
using OpenTK.Graphics.OpenGL;

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
            if (renderObject.Polygon.Type != PrimitiveType.Triangles)
            {
                return CommandResult.Failed;
            }

            return CommandResult.Success;
        }
    }
}
