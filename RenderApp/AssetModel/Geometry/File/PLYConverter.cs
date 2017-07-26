using OpenTK;
using System.Collections.Generic;
using System.Linq;
using KI.Asset.Loader.Loader;
using KI.Asset;
using KI.Gfx.GLUtil;

namespace RenderApp.AssetModel.RA_Geometry
{
    class PLYConverter : IRenderObjectConverter
    {
        private PLYLoader plyData;

        public PLYConverter(string name, string filePath)
        {
            plyData = new PLYLoader(filePath);
        }

        private List<RenderObject> renderObject;
        public List<RenderObject> RenderObject
        {
            get
            {
                return renderObject;
            }
        }


        public List<RenderObject> CreateRenderObject()
        {
            RenderObject geometry = AssetFactory.Instance.CreateRenderObject(plyData.FileName);

            var position = new List<Vector3>();

            int vertexNum = plyData.Propertys[0].Count;
            for (int i = 0; i < vertexNum; i++)
            {
                var x = plyData.Propertys[0][i];
                var y = plyData.Propertys[1][i];
                var z = plyData.Propertys[2][i];
                var angle = plyData.Propertys[3][i];
                var vectorX = plyData.Propertys[4][i];
                var vectorY = plyData.Propertys[5][i];
                var vectorZ = plyData.Propertys[6][i];

                position.Add(new Vector3(x, y, z));
            }

            List<int> index = plyData.FaceIndex.ToList();

            GeometryInfo info = new GeometryInfo(position, null, Vector3.UnitX, null, index, GeometryType.Triangle);

            geometry.SetGeometryInfo(info);
            renderObject = new List<RenderObject> { geometry };
            return renderObject;
        }
    }

}
