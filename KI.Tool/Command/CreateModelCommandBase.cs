using KI.Asset;
using KI.Foundation.Command;
using KI.Foundation.Core;
using KI.Gfx.GLUtil;

namespace KI.Tool.Command
{
    public class CreateModelCommandBase
    {
        protected CommandState CanCreateGeometry(KIObject asset)
        {
            if (!(asset is Geometry))
            {
                return CommandState.Failed;
            }

            Geometry geometry = asset as Geometry;
            if (geometry.GeometryInfo.GeometryType != GeometryType.Triangle)
            {
                return CommandState.Failed;
            }

            return CommandState.Success;
        }
    }
}
