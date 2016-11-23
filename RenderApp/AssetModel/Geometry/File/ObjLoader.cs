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
using RenderApp.Analyzer;
namespace RenderApp.AssetModel
{
    public class OBJMaterial : MaterialFileInfo
    {
        public string name;
        public Vector3 Ka;
        public Vector3 Kd;
        public Vector3 Ks;
        public Vector3 Ke;
        public float Ns;
        public float Ni;
        public float d;
        public float Tr;
        public Vector3 Tf;
        public float illum;
        public string map_Ka;
        public string map_Kd;
        public string map_d;
        public string map_bump;
        public string bump;
    }

    public class CObjFile : GeometryFile
    {
        private Dictionary<string, OBJMaterial> mtlList = new Dictionary<string,OBJMaterial>();
        private List<Vector3> posAllList = new List<Vector3>();
        /// <summary>
        /// 読み込み時のファイルの最初かどうかのフラグ
        /// </summary>
        private bool CreateInfoFlag = true;
        public CObjFile(string name,string filePath)
            :base(filePath)
        {
            try
            {
                geometryInfo = new List<GeometryInfo>();
                string[] parser = File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("Shift_JIS"));

                ReadData(parser);
                SetDrawArrayData();
            }
            catch (Exception)
            {

                Console.WriteLine(filePath + "開けません。現在のフォルダ位置" + System.Environment.CurrentDirectory);
            }
        }

        #region [ファイルのロード処理]
        #region [構造の変更]
        private void SetDrawArrayData()
        {
            //MaterialItem = Material.Default;
            //HalfEdge half = null;
            //if (Normal.Count == 0)
            //{
            //    half = new HalfEdge(m_posStream, m_posIndex);
            //    AddAnalayzer(half);

            //    for (int i = 0; i < m_posIndex.Count / 3; i++)
            //    {
            //        Position.Add(m_posStream[m_posIndex[3 * i]]);
            //        Position.Add(m_posStream[m_posIndex[3 * i + 1]]);
            //        Position.Add(m_posStream[m_posIndex[3 * i + 2]]);

            //        Normal.Add(half.GetNormal(m_posIndex[3 * i]));
            //        Normal.Add(half.GetNormal(m_posIndex[3 * i + 1]));
            //        Normal.Add(half.GetNormal(m_posIndex[3 * i + 2]));

            //        TexCoord.Add(m_texStream[m_texIndex[3 * i]]);
            //        TexCoord.Add(m_texStream[m_texIndex[3 * i + 1]]);
            //        TexCoord.Add(m_texStream[m_texIndex[3 * i + 2]]);
            //    }
            //}
            //MaterialItem.AddTexture(TextureKind.Albedo, new GLUtil.Texture(m_Materials[0].imageFile,DirectoryPath + @"\" + m_Materials[0].imageFile));
        }

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
                GeometryInfo data = null;
                
                while (parser.Length != counter)
                {
                    parser[counter] = parser[counter].Trim();
                    line = parser[counter].Split(' ');
                    if(line[0] == "v")
                    {
                        if (CreateInfoFlag)
                        {
                            data = new GeometryInfo();
                            geometryInfo.Add(data);
                            CreateInfoFlag = false;
                        }
                    }
                    if (line[0] == "f")
                    {
                        CreateInfoFlag = true;
                    }
                    ReadLineData(data, line);
                    

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
        private void ReadLineData(GeometryInfo data, string[] line)
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
                    data.posStream.Add(new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3])));
                    posAllList.Add(data.posStream[data.posStream.Count - 1]);
                    break;
                }
                if (line[i] == "vt")
                {
                    data.texStream.Add(new Vector2(float.Parse(line[i + 1]), float.Parse(line[i + 2])));
                    break;
                }
                if (line[i] == "vn")
                {
                    data.norStream.Add(new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3])));
                    break;
                }
                if (line[i] == "f")
                {
                    SetIndexBuffer(data, line);
                    break;
                }
                if (line[i] == "g")
                {
                    data.group = line[i + 1];
                }
                if (line[i] == "usemtl")
                {
                    if (mtlList.ContainsKey(line[i + 1]))
                    {
                        data.Material = mtlList[line[i + 1]];
                    }
                    else
                    {
                        Utility.Output.Error("not found material on load obje file");
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
        private void SetIndexBuffer(GeometryInfo data, string[] line)
        {
            if(line.Length == 4)
            {
                if(data.geometryType == GeometryType.None)
                {
                    data.geometryType = GeometryType.Triangle;
                }
                else if(data.geometryType == GeometryType.Quad)
                {
                    data.geometryType = GeometryType.Mix;
                }
                string[] part1;
                string[] part2;
                string[] part3;

                part1 = line[1].Split('/');
                part2 = line[2].Split('/');
                part3 = line[3].Split('/');

                //Index番号となるようにー1
                //objファイルのインデックスは1から、リストの順を基準にしたいため0からに
                data.posIndex.Add(Math.Abs(int.Parse(part1[0])) - 1);
                data.posIndex.Add(Math.Abs(int.Parse(part2[0])) - 1);
                data.posIndex.Add(Math.Abs(int.Parse(part3[0])) - 1);
                if (part1.Length > 1)
                {
                    if (part1[1] != "")
                    {
                        data.texIndex.Add(Math.Abs(int.Parse(part1[1])) - 1);
                        data.texIndex.Add(Math.Abs(int.Parse(part2[1])) - 1);
                        data.texIndex.Add(Math.Abs(int.Parse(part3[1])) - 1);

                    }
                }
                if (part1.Length > 2)
                {
                    if (part1[2] != "")
                    {
                        data.norIndex.Add(Math.Abs(int.Parse(part1[2])) - 1);
                        data.norIndex.Add(Math.Abs(int.Parse(part2[2])) - 1);
                        data.norIndex.Add(Math.Abs(int.Parse(part3[2])) - 1);

                    }
                }
            }
            else if(line.Length == 5)
            {
                if (data.geometryType == GeometryType.None)
                {
                    data.geometryType = GeometryType.Quad;
                }
                else if (data.geometryType == GeometryType.Quad)
                {
                    data.geometryType = GeometryType.Triangle;
                }
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
                data.posIndex.Add(Math.Abs(int.Parse(part1[0])) - 1);
                data.posIndex.Add(Math.Abs(int.Parse(part2[0])) - 1);
                data.posIndex.Add(Math.Abs(int.Parse(part3[0])) - 1);
                data.posIndex.Add(Math.Abs(int.Parse(part4[0])) - 1);
                if (part1.Length > 1)
                {
                    if (part1[1] != "")
                    {
                        data.texIndex.Add(Math.Abs(int.Parse(part1[1])) - 1);
                        data.texIndex.Add(Math.Abs(int.Parse(part2[1])) - 1);
                        data.texIndex.Add(Math.Abs(int.Parse(part3[1])) - 1);
                        data.texIndex.Add(Math.Abs(int.Parse(part3[1])) - 1);
                    }
                }
                if (part1.Length > 2)
                {
                    if (part1[2] != "")
                    {
                        data.norIndex.Add(Math.Abs(int.Parse(part1[2])) - 1);
                        data.norIndex.Add(Math.Abs(int.Parse(part2[2])) - 1);
                        data.norIndex.Add(Math.Abs(int.Parse(part3[2])) - 1);
                        data.norIndex.Add(Math.Abs(int.Parse(part4[2])) - 1);
                    }
                }
            }else
            {
                Utility.Output.Error("we dont support more than 4 polygons");
            }
        }
        private bool OpenMTL(string file_name)
        {
            string directory = DirectoryPath;
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
                    line[i] = line[i].Replace("\t","");
                    
                    //コメント領域
                    if (line[i] == "#" || line[i] == "") break;
                    

                    if (line[i] == "newmtl")
                    {
                        if(mat !=null)
                        {
                            mtlList.Add(mat.name,mat);
                        }
                        mat = new OBJMaterial();
                        mat.name = line[i + 1];
                        break;
                    }
                    if(mat != null)
                    {
                        switch(line[i])
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
                                mat.map_Ka = DirectoryPath + line[i + 1];
                                break;
                            case "map_Kd":
                                mat.map_Kd = DirectoryPath + line[i + 1];
                                break;
                            case "map_d":
                                mat.map_d = DirectoryPath + line[i + 1];
                                break;
                            case "map_bump":
                                mat.map_bump = DirectoryPath + line[i + 1];
                                break;
                            case "bump":
                                mat.bump = DirectoryPath + line[i + 1];
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

        public override List<Geometry> ConvertGeometry()
        {
            List<Geometry> geometrys = new List<Geometry>();
            for(int i = 0; i < geometryInfo.Count; i++)
            {
                Geometry geometry = null;
                if(geometryInfo[i].geometryType == GeometryType.Triangle)
                {
                    var Position = new List<Vector3>();

                    for (int j = 0; j < geometryInfo[i].posStream.Count / 3; j++)
                    {
                        Position.Add(posAllList[geometryInfo[i].posIndex[3 * j]]);
                        Position.Add(posAllList[geometryInfo[i].posIndex[3 * j + 1]]);
                        Position.Add(posAllList[geometryInfo[i].posIndex[3 * j + 2]]);
                    }

                    geometry = new Primitive(
                        FileName + i.ToString(),
                        geometryInfo[i].posStream,
                        PrimitiveType.Triangles
                        );
                }
                else
                {
                    var Position = new List<Vector3>();

                    for (int j = 0; j < geometryInfo[i].posStream.Count / 4; j++)
                    {
                        Position.Add(posAllList[geometryInfo[i].posIndex[4 * j]]);
                        Position.Add(posAllList[geometryInfo[i].posIndex[4 * j + 1]]);
                        Position.Add(posAllList[geometryInfo[i].posIndex[4 * j + 2]]);
                        Position.Add(posAllList[geometryInfo[i].posIndex[4 * j + 3]]);

                    }

                    geometry = new Primitive(
                        FileName + i.ToString(),
                        geometryInfo[i].posStream,
                        PrimitiveType.Quads
                        );
                }
                if(geometry != null)
                {
                    geometrys.Add(geometry);

                }
            }
            return geometrys;
        }
    }
}
