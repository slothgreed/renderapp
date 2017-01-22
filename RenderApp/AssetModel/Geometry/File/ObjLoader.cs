﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using RenderApp.Analyzer;
using RenderApp.GLUtil;
namespace RenderApp.AssetModel.RA_Geometry
{
    public class OBJMaterial
    {
        public string name;
        public Vector3 Ka;//ambient
        public Vector3 Kd;//diffuse
        public Vector3 Ks;//specular
        public Vector3 Ke;//emissive
        public float Ns;//specular指数
        public float Ni;//屈折率
        public string map_Ka;//ambientMap
        public string map_Kd;//diffuseMap(sponzaはambientと一緒
        public string map_Ns;//specular
        public string map_bump;//bumpMap
        
        public Vector3 Tf;//atmosphereの値（無視rgbで指定されてるがrと同一っぽい）
        public string map_d;//透過度テクスチャ（無視）
        public float d;//透過（無視）
        public float Tr;//透過（無視）
        public float illum;//0~10のパラメータで異なる（無視）sponza全部2（Color on and Ambient on）
        public VertexArrayInfo vertexArrayInfo = new VertexArrayInfo();//ポリゴンのIndex情報を保持
    }

    public class CObjFile : RAFile , IRenderObject
    {
        /// <summary>
        /// 頂点・色・テクスチャ座標を保持。
        /// </summary>
        private VertexInfo vertexInfo;
        private Dictionary<string, OBJMaterial> mtlList = new Dictionary<string,OBJMaterial>();
        /// <summary>
        /// objファイルのローダ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filePath"></param>
        public CObjFile(string name,string filePath)
            :base(filePath)
        {
            try
            {
                vertexInfo = new VertexInfo();
                string[] parser = File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("Shift_JIS"));
                ReadData(parser);
            }
            catch (Exception)
            {
                Utility.Output.Log(Utility.Output.LogLevel.Warning, filePath + "開けません。");
            }
        }

        #region [ファイルのロード処理]
        #region [構造の変更]

        #endregion
        #region [データのロード]
        /// <summary>
        /// OBJデータのロード
        /// </summary>
        /// <param name="parser">STLデータ</param>
        /// <param name="position">位置情報を格納</param>
        /// <param name="normal">法線情報を格納</param>
        private void ReadData(String[] parser)
        {
            try
            {
                int counter = 0;

                String[] line;
                OBJMaterial currentMat = null;
                while (parser.Length != counter)
                {
                    parser[counter] = parser[counter].Trim();
                    line = parser[counter].Split(' ');
                    ReadLineData(ref currentMat,line);
                    counter++;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 1行ずつの読み込み
        /// </summary>
        /// <param name="line"></param>
        private void ReadLineData(ref OBJMaterial currentMat, string[] line)
        {
            line = line.Where(p => p != "").ToArray();
            for (int i = 0; i < line.Length; i++)
            {
                //コメント領域
                if (line[i] == "#" || line[i] == "") break;

                if (line[i] == "mtllib")
                {
                    OpenMTL(line[i + 1]);
                    break;
                }
                if (line[i] == "v")
                {
                    vertexInfo.Position.Add(new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3])));
                    break;
                }
                if (line[i] == "vt")
                {
                    vertexInfo.TexCoord.Add(new Vector2(float.Parse(line[i + 1]), float.Parse(line[i + 2])));
                    break;
                }
                if (line[i] == "vn")
                {
                    vertexInfo.Normal.Add(new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3])));
                    break;
                }
                if (line[i] == "f")
                {
                    if(currentMat != null)
                    {
                        SetIndexBuffer(currentMat,line);
                    }
                    break;
                }
                if (line[i] == "g")
                {
                    //vertexInfo.group = line[i + 1];
                }
                if (line[i] == "usemtl")
                {
                    if (mtlList.ContainsKey(line[i + 1]))
                    {
                        currentMat = mtlList[line[i + 1]];
                    }
                    else
                    {
                        Utility.Output.Log(Utility.Output.LogLevel.Warning, "not found material on load obj file" + line[i + 1]);
                    }
                    break;
                }
                if (line[i] == "s")
                {
                    //TODO;
                    break;
                }
            }
        }

        /// <summary>
        /// IndexBufferの設定
        /// </summary>
        /// <param name="line"></param>
        private void SetIndexBuffer(OBJMaterial currentMat,string[] line)
        {
            if(line.Length == 4)
            {
                string[] part1;
                string[] part2;
                string[] part3;

                part1 = line[1].Split('/');
                part2 = line[2].Split('/');
                part3 = line[3].Split('/');

                //Index番号となるようにー1
                //objファイルのインデックスは1から、リストの順を基準にしたいため0からに
                currentMat.vertexArrayInfo.posIndex.Add(int.Parse(part1[0]) - 1);
                currentMat.vertexArrayInfo.posIndex.Add(int.Parse(part2[0]) - 1);
                currentMat.vertexArrayInfo.posIndex.Add(int.Parse(part3[0]) - 1);
                if (part1.Length > 1)
                {
                    if (part1[1] != "")
                    {
                        currentMat.vertexArrayInfo.texIndex.Add(int.Parse(part1[1]) - 1);
                        currentMat.vertexArrayInfo.texIndex.Add(int.Parse(part2[1]) - 1);
                        currentMat.vertexArrayInfo.texIndex.Add(int.Parse(part3[1]) - 1);

                    }
                }
                if (part1.Length > 2)
                {
                    if (part1[2] != "")
                    {
                        currentMat.vertexArrayInfo.norIndex.Add(int.Parse(part1[2]) - 1);
                        currentMat.vertexArrayInfo.norIndex.Add(int.Parse(part2[2]) - 1);
                        currentMat.vertexArrayInfo.norIndex.Add(int.Parse(part3[2]) - 1);

                    }
                }
            }
            else if(line.Length == 5)
            {
                //4角形は3角系に変換
                string[] part1;
                string[] part2;
                string[] part3;
                string[] part4;
                part1 = line[1].Split('/');
                part2 = line[2].Split('/');
                part3 = line[3].Split('/');
                part4 = line[4].Split('/');

                //Index番号となるようにー1
                //objファイルのインデックスは1から、リストの順を基準にしたいため0からに
                currentMat.vertexArrayInfo.posIndex.Add(int.Parse(part1[0]) - 1);
                currentMat.vertexArrayInfo.posIndex.Add(int.Parse(part2[0]) - 1);
                currentMat.vertexArrayInfo.posIndex.Add(int.Parse(part3[0]) - 1);

                currentMat.vertexArrayInfo.posIndex.Add(int.Parse(part1[0]) - 1);
                currentMat.vertexArrayInfo.posIndex.Add(int.Parse(part3[0]) - 1);
                currentMat.vertexArrayInfo.posIndex.Add(int.Parse(part4[0]) - 1);
                if (part1.Length > 1)
                {
                    if (part1[1] != "")
                    {
                        currentMat.vertexArrayInfo.texIndex.Add(int.Parse(part1[1]) - 1);
                        currentMat.vertexArrayInfo.texIndex.Add(int.Parse(part2[1]) - 1);
                        currentMat.vertexArrayInfo.texIndex.Add(int.Parse(part3[1]) - 1);

                        currentMat.vertexArrayInfo.texIndex.Add(int.Parse(part1[1]) - 1);
                        currentMat.vertexArrayInfo.texIndex.Add(int.Parse(part3[1]) - 1);
                        currentMat.vertexArrayInfo.texIndex.Add(int.Parse(part4[1]) - 1);
                    }
                }
                if (part1.Length > 2)
                {
                    if (part1[2] != "")
                    {
                        currentMat.vertexArrayInfo.norIndex.Add(int.Parse(part1[2]) - 1);
                        currentMat.vertexArrayInfo.norIndex.Add(int.Parse(part2[2]) - 1);
                        currentMat.vertexArrayInfo.norIndex.Add(int.Parse(part3[2]) - 1);

                        currentMat.vertexArrayInfo.norIndex.Add(int.Parse(part1[2]) - 1);
                        currentMat.vertexArrayInfo.norIndex.Add(int.Parse(part3[2]) - 1);
                        currentMat.vertexArrayInfo.norIndex.Add(int.Parse(part4[2]) - 1);
                    }
                }
            }else
            {
                Utility.Output.Log(Utility.Output.LogLevel.Warning,"we dont support more than 4 polygons");
            }
        }
        #region [read mtl file]
        private bool OpenMTL(string file_name)
        {
            string directory = DirectoryPath + @"\";
            string mtlFile = directory + @"\" + file_name;

            if (!File.Exists(mtlFile))
            {
                return false;
            }
            string[] parser = File.ReadAllLines(mtlFile, System.Text.Encoding.GetEncoding("Shift_JIS"));

            int counter = 0;

            String[] line;
            OBJMaterial mat = null;
            while (parser.Length != counter)
            {
                line = parser[counter].Split(' ');

                for (int i = 0; i < line.Length; i++)
                {
                    line[i] = line[i].Replace("\t", "");

                    //コメント領域
                    if (line[i] == "#" || line[i] == "") break;


                    if (line[i] == "newmtl")
                    {
                        mat = new OBJMaterial();
                        mat.name = line[i + 1];
                        mtlList.Add(mat.name, mat);

                        break;
                    }
                    if (mat != null)
                    {
                        switch (line[i])
                        {
                            case "Ns":
                                mat.Ns = float.Parse(line[i + 1]);
                                break;
                            case "Ni":
                                mat.Ni = float.Parse(line[i + 1]);
                                break;
                            case "d":
                                mat.d = float.Parse(line[i + 1]);
                                break;
                            case "Tr":
                                mat.Tr = float.Parse(line[i + 1]);
                                break;
                            case "Tf":
                                mat.Ka = new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3]));
                                break;
                            case "illum":
                                mat.illum = float.Parse(line[i + 1]);
                                break;
                            case "Ka":
                                mat.Ka = new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3]));
                                break;
                            case "Kd":
                                mat.Kd = new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3]));
                                break;
                            case "Ks":
                                mat.Ks = new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3]));
                                break;
                            case "Ke":
                                mat.Ke = new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3]));
                                break;
                            case "map_Ka":
                                mat.map_Ka = directory + line[i + 1];
                                break;
                            case "map_Kd":
                                mat.map_Kd = directory + line[i + 1];
                                break;
                            case "map_Ns":
                                mat.map_Ns = directory + line[i + 1];
                                break;
                            case "map_d":
                                mat.map_d = directory + line[i + 1];
                                break;
                            case "map_bump":
                            case "bump":
                                mat.map_bump = directory + line[i + 1];
                                break;
                        }
                    }
                }

                counter++;
            }
            return true;
        }
        #endregion
        
        #endregion
        #endregion
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

            foreach(var material in mtlList.Values)
            {
                RenderObject geometry = null;
                var Position = new List<Vector3>();
                var Normal = new List<Vector3>();
                var TexCoord = new List<Vector2>();
                for (int j = 0; j < material.vertexArrayInfo.posIndex.Count / 3; j++)
                {
                    Position.Add(vertexInfo.Position[material.vertexArrayInfo.posIndex[3 * j]]);
                    Position.Add(vertexInfo.Position[material.vertexArrayInfo.posIndex[3 * j + 1]]);
                    Position.Add(vertexInfo.Position[material.vertexArrayInfo.posIndex[3 * j + 2]]);
                    if (vertexInfo.Normal.Count != 0)
                    {
                        Normal.Add(vertexInfo.Normal[material.vertexArrayInfo.norIndex[3 * j]]);
                        Normal.Add(vertexInfo.Normal[material.vertexArrayInfo.norIndex[3 * j + 1]]);
                        Normal.Add(vertexInfo.Normal[material.vertexArrayInfo.norIndex[3 * j + 2]]);
                    }
                    if (vertexInfo.TexCoord.Count != 0)
                    {
                        TexCoord.Add(vertexInfo.TexCoord[material.vertexArrayInfo.texIndex[3 * j]]);
                        TexCoord.Add(vertexInfo.TexCoord[material.vertexArrayInfo.texIndex[3 * j + 1]]);
                        TexCoord.Add(vertexInfo.TexCoord[material.vertexArrayInfo.texIndex[3 * j + 2]]);
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
