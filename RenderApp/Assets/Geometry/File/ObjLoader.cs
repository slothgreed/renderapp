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

namespace RenderApp.Assets
{
    public class CObjFile : GeometryLoader
    {
        
        
        public CObjFile(string filePath)
        {
            try
            {
                FilePath = filePath;
                m_posStream.Clear();
                m_norStream.Clear();
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
            Position = m_posStream;
            Normal = m_norStream;
            Color = m_colStream;
            TexCoord = m_texStream;
            Index = m_posIndex;

            MaterialItem = new Assets.Material(FileName);
            if (Normal.Count == 0)
            {
                HalfEdge half = new HalfEdge(m_posStream, m_posIndex);
                MaterialItem.AddAnalayzer(half);
                for (int i = 0; i < m_posStream.Count; i++)
                {
                    Normal.Add(half.GetNormal(i));
                }
            }
            MaterialItem.AddTexture(TextureKind.Albedo, new GLUtil.Texture(DirectoryPath + @"\" + m_Materials[0].imageFile));
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
                while (parser.Length != counter)
                {
                    line = parser[counter].Split(' ');

                    ReadLineData(line);

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
        private void ReadLineData(string[] line)
        {
            try
            {
                Vector3 pos;
                for (int i = 0; i < line.Length; i++)
                {
                    //コメント領域
                    if (line[i] == "#" || line[i] == "") break;

                    if (line[i] == "mtllib") OpenMTL(line[i + 1]);
                    if (line[i] == "v")
                    {   
                        float X = float.MaxValue;
                        float Y = float.MaxValue;
                        float Z = float.MaxValue;
                        int offset = 1;
                        while(true)
                        {
                            if(line[i + offset]=="")
                            {
                                offset++;
                            }
                            else if(X == float.MaxValue)
                            {
                                X = float.Parse(line[i + offset]);
                                offset++;
                            }else if(Y == float.MaxValue)
                            {
                                Y = float.Parse(line[i + offset]);
                                offset++;
                            }
                            else
                            {
                                Z = float.Parse(line[i + offset]);
                                break;
                            }
                        }

                        pos = new Vector3(X, Y, Z);
                        m_posStream.Add(pos);

                        break;
                    }
                    if (line[i] == "vt")
                    {
                        m_texStream.Add(new Vector2(float.Parse(line[i + 1]), float.Parse(line[i + 2])));
                        break;
                    }
                    if (line[i] == "vn")
                    {
                        m_norStream.Add(new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3])));
                        break;
                    }
                    if (line[i] == "f")
                    {
                        SetIndexBuffer(line);
                        break;
                    }
                    if (line[i] == "usemtl")
                    {

                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void SetIndexBuffer(string[] line)
        {
            try
            {
                
                string[] part1;
                string[] part2;
                string[] part3;
                if (line.Length > 4)
                {
                    throw new Exception();
                }
                part1 = line[1].Split('/');
                part2 = line[2].Split('/');
                part3 = line[3].Split('/');

                //Index番号となるようにー1
                //objファイルのインデックスは1から、リストの順を基準にしたいため0からに
                m_posIndex.Add(Math.Abs(int.Parse(part1[0])) - 1);
                m_posIndex.Add(Math.Abs(int.Parse(part2[0])) - 1);
                m_posIndex.Add(Math.Abs(int.Parse(part3[0])) - 1);
                if (part1.Length > 1)
                {
                    if (part1[1] != "")
                    {
                        m_texIndex.Add(Math.Abs(int.Parse(part1[1])) - 1);
                        m_texIndex.Add(Math.Abs(int.Parse(part2[1])) - 1);
                        m_texIndex.Add(Math.Abs(int.Parse(part3[1])) - 1);

                    }
                }
                if (part1.Length > 2)
                {
                    if (part1[2] != "")
                    {
                        m_norIndex.Add(Math.Abs(int.Parse(part1[2])) - 1);
                        m_norIndex.Add(Math.Abs(int.Parse(part2[2])) - 1);
                        m_norIndex.Add(Math.Abs(int.Parse(part3[2])) - 1);

                    }
                }
            }
            catch (Exception)
            {

                Console.WriteLine("3角形以外の形状を含みます。");
                throw;
            }
        }
        private bool OpenMTL(string file_name)
        {
            string directory = System.IO.Path.GetDirectoryName(FilePath);
            string mtlFile = directory + @"\" + file_name;

            if (!File.Exists(mtlFile))
            {
                return false;
            }
            string[] parser = File.ReadAllLines(mtlFile, System.Text.Encoding.GetEncoding("Shift_JIS"));

            try
            {
                int counter = 0;

                String[] line;
                CMaterial mat = new CMaterial();
                while (parser.Length != counter)
                {
                    line = parser[counter].Split(' ');

                    for (int i = 0; i < line.Length; i++)
                    {
                        //コメント領域
                        if (line[i] == "#" || line[i] == "") break;
                        if (line[i] == "newmtl")
                        {
                            mat = new CMaterial();
                            mat.name = line[i];
                        }
                        if (line[i] == "Ka")
                            mat.Ka = new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3]));
                        if (line[i] == "Kd")
                            mat.Kd = new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3]));
                        if (line[i] == "Ks")
                            mat.Ks = new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3]));
                        if (line[i] == "map_Kd")
                        {

                            mat.imageFile = line[i + 1];
                            m_Materials.Add(mat);
                        }
                    }

                    counter++;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("mtlの読み込みに失敗しました。");
                return false;
            }
            return true;
        }
        #endregion
        #endregion
    }
}
