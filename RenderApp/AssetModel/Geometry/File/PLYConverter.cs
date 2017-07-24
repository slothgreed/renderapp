using KI.Foundation.Utility;
using KI.Gfx.KIAsset;
using KI.Gfx.KIAsset.Loader.Loader;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

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
            var color = new List<Vector3>();
            var texcoord = new List<Vector2>();
            int vertexNum = plyData.Propertys[0].Count;
            for (int i = 0; i < vertexNum; i++)
            {
                var x = plyData.Propertys[0][i]/ 1.732050f + 0.5f;
                var y = plyData.Propertys[1][i] / 1.732050f + 0.5f; 
                var z = plyData.Propertys[2][i] / 1.732050f;
                var angle = plyData.Propertys[3][i];
                var vectorX = plyData.Propertys[4][i];
                var vectorY = plyData.Propertys[5][i];
                var vectorZ = plyData.Propertys[6][i];

                Vector3 tmp = new Vector3(x, y, z) - new Vector3(vectorX, vectorY, vectorZ);

                var vector = tmp;// KICalc.Multiply(SceneManager.Instance.ActiveScene.MainCamera.CameraProjMatrix, tmp);

                position.Add(new Vector3(x, y, z));
                color.Add(new Vector3(vectorX, vectorY, vectorZ));
                texcoord.Add(new Vector2(vector.X, vector.Y));
            }

            List<int> index = plyData.FaceIndex.ToList();

            GeometryInfo info = new GeometryInfo(position, null, color, texcoord, index, GeometryType.Triangle);
            geometry.SetGeometryInfo(info);

            //var line = new List<Vector3>();
            //for (int i = 0; i < color.Count; i++)
            //{
            //    line.Add(position[i]);
            //    line.Add(position[i] + color[i].Normalized() * 0.05f);

            //    line.Add(position[i]);
            //    line.Add(position[i] - color[i].Normalized() * 0.05f);
            //}

            //RenderObject lineGeometry = AssetFactory.Instance.CreateRenderObject(plyData.FileName + ": line");
            //GeometryInfo lineInfo = new GeometryInfo(line, null, Vector3.Zero, null, null, GeometryType.Line);
            //lineGeometry.SetGeometryInfo(lineInfo);            

            g_position = position;
            g_color = color;
            g_texcoord = texcoord;
            g_index = index;

            renderObject = new List<RenderObject> { geometry };
            return renderObject;
        }

        public static List<Vector3> g_position;
        public static List<Vector3> g_color;
        public static List<Vector2> g_texcoord;
        public static List<int> g_index;
    }

}
