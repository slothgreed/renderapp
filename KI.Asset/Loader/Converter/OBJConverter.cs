using System.Collections.Generic;
using KI.Asset.Loader;
using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;
using OpenTK;

namespace KI.Asset
{
    public class OBJConverter : IGeometry
    {
        private OBJLoader objData;
        /// <summary>
        /// objファイルのローダ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filePath"></param>
        public OBJConverter(string name,string filePath)
        {
            objData = new OBJLoader(name, filePath);
        }

        public GeometryInfo[] GeometryInfos
        {
            get;
            private set;
        }


        public void SetMaterial(Geometry geometry, OBJMaterial material)
        {
            if (material.map_Kd != null)
            {
                Texture albedo = TextureFactory.Instance.CreateTexture(material.map_Kd);
                geometry.AddTexture(TextureKind.Albedo, albedo);
            }

            if (material.map_bump != null)
            {
                Texture bump = TextureFactory.Instance.CreateTexture(material.map_bump);
                geometry.AddTexture(TextureKind.Normal, bump);
            }

            if (material.map_Ns != null)
            {
                Texture spec = TextureFactory.Instance.CreateTexture(material.map_Ns);
                geometry.AddTexture(TextureKind.Specular, spec);
            }
        }

        public void CreateRenderObject()
        {
            List<GeometryInfo> geometrys = new List<GeometryInfo>();

            foreach (var material in objData.mtlList.Values)
            {
                GeometryInfo geometry = null;
                var Position = new List<Vector3>();
                var Normal = new List<Vector3>();
                var TexCoord = new List<Vector2>();
                for (int j = 0; j < material.posIndex.Count / 3; j++)
                {
                    Position.Add(objData.Position[material.posIndex[3 * j]]);
                    Position.Add(objData.Position[material.posIndex[3 * j + 1]]);
                    Position.Add(objData.Position[material.posIndex[3 * j + 2]]);
                    if (objData.Normal.Count != 0)
                    {
                        Normal.Add(objData.Normal[material.norIndex[3 * j]]);
                        Normal.Add(objData.Normal[material.norIndex[3 * j + 1]]);
                        Normal.Add(objData.Normal[material.norIndex[3 * j + 2]]);
                    }
                    if (objData.Texcoord.Count != 0)
                    {
                        TexCoord.Add(objData.Texcoord[material.texIndex[3 * j]]);
                        TexCoord.Add(objData.Texcoord[material.texIndex[3 * j + 1]]);
                        TexCoord.Add(objData.Texcoord[material.texIndex[3 * j + 2]]);
                    }
                }
                if (Position.Count != 0)
                {
                    GeometryInfo info = new GeometryInfo(Position, Normal, null, TexCoord, null, GeometryType.Triangle);
                    //geometry.CreateGeometryInfo(objData.vertexInfo, PrimitiveType.Triangles);
                    //SetMaterial(geometry, material);
                    if (geometry != null)
                    {
                        geometrys.Add(geometry);
                    }
                }
            }

            GeometryInfos = geometrys.ToArray();
        }
    }
}
