using System.Collections.Generic;

namespace RenderApp.AssetModel.RA_Geometry
{
    interface IRenderObjectConverter
    {
        List<RenderObject> RenderObject { get; }
        List<RenderObject> CreateRenderObject();
    }
}
