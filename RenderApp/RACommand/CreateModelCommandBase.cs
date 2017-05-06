using KI.Foundation.Core;
using KI.Gfx.KIAsset;
using RenderApp.AssetModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.RACommand
{
    public class CreateModelCommandBase
    {
        protected string CanCreateGeometry(KIObject asset)
        {
            if (!(asset is Geometry))
            {
                return RACommandResource.Failed;
            }
            Geometry geometry = asset as Geometry;
            if (geometry.geometryInfo.GeometryType != GeometryType.Triangle)
            {
                return RACommandResource.Failed;
            }
            return RACommandResource.Success;
        }
    }
}
