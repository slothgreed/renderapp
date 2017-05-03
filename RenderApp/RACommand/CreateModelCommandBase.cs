using KI.Foundation.Core;
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
            if (geometry.RenderType != OpenTK.Graphics.OpenGL.PrimitiveType.Triangles)
            {
                return RACommandResource.Failed;
            }
            return RACommandResource.Success;
        }
    }
}
