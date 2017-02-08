using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using RenderApp.GLUtil;
using KI.Foundation.Utility;
using KI.Foundation.Core;
using KI.Gfx.KIGeometry;
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

        public void SetMaterial(Geometry geometry,OBJMaterial material)
        {
            Material mat = AssetFactory.Instance.CreateMaterial(material.name);
            if(material.map_Kd != null)
            {
                Texture albedo = TextureFactory.Instance.CreateTexture(material.map_Kd);
                mat.AddTexture(TextureKind.Albedo, albedo);
            }

            if(material.map_bump != null)
            {
                Texture bump = TextureFactory.Instance.CreateTexture(material.map_bump);
                mat.AddTexture(TextureKind.Normal, bump);
            }
            
            if(material.map_Ns != null)
            {
                Texture spec = TextureFactory.Instance.CreateTexture(material.map_Ns);
                mat.AddTexture(TextureKind.Specular, spec);
            }
            geometry.MaterialItem = mat;
            string vertex = Utility.ShaderCreater.Instance.GetVertexShader(geometry);
            string frag = Utility.ShaderCreater.Instance.GetOBJFragShader(geometry);
            mat.SetShader(ShaderFactory.Instance.CreateShaderVF(vertex,frag));
        }
        public List<RenderObject> CreateRenderObject()
        {
            List<RenderObject> geometrys = new List<RenderObject>();

            foreach(var material in objData.mtlList.Values)
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

                geometry = new RenderObject(material.name);
                geometry.CreatePNT(Position, Normal, TexCoord, PrimitiveType.Triangles);
                SetMaterial(geometry,material);
                if (geometry != null)
                {
                    geometrys.Add(geometry);
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
