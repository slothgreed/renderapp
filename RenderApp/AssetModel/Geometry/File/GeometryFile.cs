using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.AssetModel
{
    public abstract class GeometryFile : RAFile
    {
        public GeometryFile(string filePath)
            :base(filePath)
        {

        }
        public abstract List<Geometry> ConvertGeometry();
    }
}
