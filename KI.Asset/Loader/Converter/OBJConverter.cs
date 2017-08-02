using System.Collections.Generic;
using KI.Asset.Loader;
using KI.Gfx.GLUtil;
using KI.Gfx.KITexture;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// objファイルデータを独自形式に変換
    /// </summary>
    public class OBJConverter : IGeometry
    {
        /// <summary>
        /// objファイルのデータ
        /// </summary>
        private OBJLoader objData;

        /// <summary>
        /// objファイルのローダ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public OBJConverter(string filePath)
        {
            objData = new OBJLoader(filePath);
        }

        /// <summary>
        /// 形状情報
        /// </summary>
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
                var position = new List<Vector3>();
                var normal = new List<Vector3>();
                var texCoord = new List<Vector2>();
                for (int j = 0; j < material.posIndex.Count / 3; j++)
                {
                    position.Add(objData.Position[material.posIndex[3 * j]]);
                    position.Add(objData.Position[material.posIndex[3 * j + 1]]);
                    position.Add(objData.Position[material.posIndex[3 * j + 2]]);
                    if (objData.Normal.Count != 0)
                    {
                        normal.Add(objData.Normal[material.norIndex[3 * j]]);
                        normal.Add(objData.Normal[material.norIndex[3 * j + 1]]);
                        normal.Add(objData.Normal[material.norIndex[3 * j + 2]]);
                    }

                    if (objData.Texcoord.Count != 0)
                    {
                        texCoord.Add(objData.Texcoord[material.texIndex[3 * j]]);
                        texCoord.Add(objData.Texcoord[material.texIndex[3 * j + 1]]);
                        texCoord.Add(objData.Texcoord[material.texIndex[3 * j + 2]]);
                    }
                }

                if (position.Count != 0)
                {
                    GeometryInfo info = new GeometryInfo(position, normal, null, texCoord, null, GeometryType.Triangle);
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
