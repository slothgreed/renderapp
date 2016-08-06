using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using RenderApp;
using RenderApp.GLUtil;
using RenderApp.GLUtil.ShaderModel;
namespace RenderApp.AssetModel
{
    class GeometryFactory
    {
        private static GeometryFactory _instance = new GeometryFactory();
        public static GeometryFactory Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
