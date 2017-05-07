using System.Collections.Generic;
using OpenTK;
using KI.Gfx.KIAsset;
namespace RenderApp.AssetModel.RA_Geometry
{
    public class OBJConverter : IRenderObjectConverter
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

        public void SetMaterial(Geometry geometry, OBJMaterial material)
        {
            if (material.map_Kd != null)
            {
                Texture albedo = TextureFactory.Instance.CreateTexture(material.map_Kd);
                geometry.AddTexture(TextureKind.Albedo, albedo);
                Globals.Project.ActiveProject.AddChild(albedo);
            }

            if (material.map_bump != null)
            {
                Texture bump = TextureFactory.Instance.CreateTexture(material.map_bump);
                geometry.AddTexture(TextureKind.Normal, bump);
                Globals.Project.ActiveProject.AddChild(bump);
            }

            if (material.map_Ns != null)
            {
                Texture spec = TextureFactory.Instance.CreateTexture(material.map_Ns);
                geometry.AddTexture(TextureKind.Specular, spec);
                Globals.Project.ActiveProject.AddChild(spec);
            }
        }

        public List<RenderObject> CreateRenderObject()
        {
            List<RenderObject> geometrys = new List<RenderObject>();

            foreach (var material in objData.mtlList.Values)
            {
                RenderObject geometry = null;
                var Position = new List<Vector3>();
                var Normal = new List<Vector3>();
                var TexCoord = new List<Vector2>();
                for (int j = 0; j < material.posIndex.Count / 3; j++)
                {
                    Position.Add(objData.vertexInfo.Position[material.posIndex[3 * j]]);
                    Position.Add(objData.vertexInfo.Position[material.posIndex[3 * j + 1]]);
                    Position.Add(objData.vertexInfo.Position[material.posIndex[3 * j + 2]]);
                    if (objData.vertexInfo.Normal.Count != 0)
                    {
                        Normal.Add(objData.vertexInfo.Normal[material.norIndex[3 * j]]);
                        Normal.Add(objData.vertexInfo.Normal[material.norIndex[3 * j + 1]]);
                        Normal.Add(objData.vertexInfo.Normal[material.norIndex[3 * j + 2]]);
                    }
                    if (objData.vertexInfo.TexCoord.Count != 0)
                    {
                        TexCoord.Add(objData.vertexInfo.TexCoord[material.texIndex[3 * j]]);
                        TexCoord.Add(objData.vertexInfo.TexCoord[material.texIndex[3 * j + 1]]);
                        TexCoord.Add(objData.vertexInfo.TexCoord[material.texIndex[3 * j + 2]]);
                    }
                }
                if (Position.Count != 0)
                {
                    geometry = AssetFactory.Instance.CreateRenderObject(material.name);
                    GeometryInfo info = new GeometryInfo(Position, Normal, null, TexCoord, null,GeometryType.Triangle);
                    //geometry.CreateGeometryInfo(objData.vertexInfo, PrimitiveType.Triangles);
                    SetMaterial(geometry, material);
                    if (geometry != null)
                    {
                        geometrys.Add(geometry);
                    }
                }
            }
            _renderObject = geometrys;
            return geometrys;
        }

        private List<RenderObject> _renderObject;
        public List<RenderObject> RenderObject
        {
            get
            {
                return _renderObject;
            }
        }
    }
}
