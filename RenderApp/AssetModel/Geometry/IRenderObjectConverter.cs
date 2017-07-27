using System.Collections.Generic;
using KI.Asset;

namespace RenderApp.AssetModel.RA_Geometry
{
    interface IRenderObjectConverter
    {
        List<RenderObject> RenderObject { get; }
        List<RenderObject> CreateRenderObject();
    }
}
