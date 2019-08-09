using System.Collections.Generic;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.KITexture;
using OpenTK;

namespace KI.Asset.Loader.Importer
{
    /// <summary>
    /// objファイルデータを独自形式に変換
    /// </summary>
    public class OBJImporter // : ICreateModel : obj is not single object.
    {
        /// <summary>
        /// objファイルのデータ
        /// </summary>
        private OBJLoader objData;

        /// <summary>
        /// ファイルパス
        /// </summary>
        private string filePath;

        /// <summary>
        /// objファイルのローダ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public OBJImporter(string filePath)
        {
            objData = new OBJLoader(filePath);
            this.filePath = filePath;
            CreateModel();
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Polygon[] Model
        {
            get;
            private set;
        }

        /// <summary>
        /// マテリアルのセット
        /// </summary>
        /// <param name="polygon">形状</param>
        /// <param name="material">マテリアル</param>
        public void SetMaterial(Polygon polygon, OBJMaterial material)
        {
            if (material.map_Kd != null)
            {
                Texture albedo = TextureFactory.Instance.CreateTexture(material.map_Kd);
                polygon.Material.AddTexture(TextureKind.Albedo, albedo);
            }

            if (material.map_bump != null)
            {
                Texture bump = TextureFactory.Instance.CreateTexture(material.map_bump);
                polygon.Material.AddTexture(TextureKind.Normal, bump);
            }

            if (material.map_Ns != null)
            {
                Texture spec = TextureFactory.Instance.CreateTexture(material.map_Ns);
                polygon.Material.AddTexture(TextureKind.Specular, spec);
            }
        }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateModel()
        {
            List<Polygon> polygons = new List<Polygon>();

            foreach (var material in objData.mtlList.Values)
            {
                Polygon polygon = null;
                var vertexs = new List<Vertex>();

                for (int j = 0; j < material.posIndex.Count / 3; j++)
                {
                    Vector3 pos0 = objData.Position[material.posIndex[3 * j]];
                    Vector3 pos1 = objData.Position[material.posIndex[3 * j + 1]];
                    Vector3 pos2 = objData.Position[material.posIndex[3 * j + 2]];

                    Vector3 nor0 = Vector3.Zero;Vector3 nor1 = Vector3.Zero; Vector3 nor2 = Vector3.Zero;
                    Vector2 texCoord0 = Vector2.Zero; Vector2 texCoord1 = Vector2.Zero; Vector2 texCoord2 = Vector2.Zero;
                    if (objData.Normal.Count != 0)
                    {
                        nor0 = objData.Normal[material.norIndex[3 * j]];
                        nor1 = objData.Normal[material.norIndex[3 * j + 1]];
                        nor2 = objData.Normal[material.norIndex[3 * j + 2]];
                    }

                    if (objData.Texcoord.Count != 0)
                    {
                        texCoord0 = objData.Texcoord[material.texIndex[3 * j]];
                        texCoord1 = objData.Texcoord[material.texIndex[3 * j + 1]];
                        texCoord2 = objData.Texcoord[material.texIndex[3 * j + 2]];
                    }


                    vertexs.Add(new Vertex(3 * vertexs.Count + 0, pos0, nor0, texCoord0));
                    vertexs.Add(new Vertex(3 * vertexs.Count + 1, pos1, nor1, texCoord1));
                    vertexs.Add(new Vertex(3 * vertexs.Count + 2, pos2, nor2, texCoord2));
                }

                if (vertexs.Count != 0)
                {
                    polygon = new Polygon(filePath, vertexs, KIPrimitiveType.Triangles);
                    SetMaterial(polygon, material);
                    if (polygon != null)
                    {
                        polygons.Add(polygon);
                    }
                }
            }

            Model = polygons.ToArray();
        }
    }
}
