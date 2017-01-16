﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.AssetModel.RA_Geometry
{
    interface IRenderObject
    {
        List<RenderObject> RenderObject { get; }
        List<RenderObject> CreateRenderObject();
    }
}