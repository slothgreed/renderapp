using System.Collections.Generic;
using KI.Asset;
using KI.Renderer;

namespace RenderApp.AssetModel.RA_Geometry
{
    interface IRenderObjectConverter
    {
        List<RenderObject> RenderObject { get; }
        List<RenderObject> CreateRenderObject();
    }
}
