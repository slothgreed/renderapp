using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using RenderApp.Analyzer;
namespace RenderApp.AssetModel
{
    /// <summary>
    /// STLのローダ現在テキストファイルのみ
    /// </summary>
    public class StlFile : GeometryLoader
    {


        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        public StlFile(string name,string filePath)
            :base(name)    
        {
            try
            {
                String[] parser = File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("Shift_JIS"));
                ReadData(parser);
                //HalfEdge half = new HalfEdge(Position);
                ////position;
                //Position.Clear();
                //foreach(var mesh in half.m_Mesh)
                //{
                //    foreach(var vertex in mesh.GetAroundVertex())
                //    {
                //        Position.Add(vertex.Position);
                //    }
                //}
            }
            catch (Exception)
            {

                MessageBox.Show(filePath + "開けません。現在のフォルダ位置" + System.Environment.CurrentDirectory);   
            }

            
        }
        /// <summary>
        /// STLデータのロード
        /// </summary>
        /// <param name="parser">STLデータ</param>
        /// <param name="position">位置情報を格納</param>
        /// <param name="normal">法線情報を格納</param>
        private void ReadData(String[] parser)
        {
            try
            {
                int counter = 0;
                int offset = 0;
                String[] line;
                Vector3 pos;
                while (parser.Length != counter)
                {
                    line = parser[counter].Split(' ');
                    for (int i = 0; i < line.Length; i++)
                    {                        
                        if (line[i] == "outer" && line[i + 1] == "loop") break;
                        if (line[i] == "solid" || line[i] == "endloop" || line[i] == "endfacet") break;
                        if (line[i] == "") continue;
                        if (line[i] == "facet" && line[i + 1] == "normal")
                        {
                            ////同じなものを3つ作って頂点の法線ベクトルにする
                            Normal.Add(new Vector3(float.Parse(line[i + 2]), float.Parse(line[i + 3]), float.Parse(line[i + 4])));
                            Normal.Add(new Vector3(float.Parse(line[i + 2]), float.Parse(line[i + 3]), float.Parse(line[i + 4])));
                            Normal.Add(new Vector3(float.Parse(line[i + 2]), float.Parse(line[i + 3]), float.Parse(line[i + 4])));
                            break;
                        }
                        if (line[i] == "vertex")
                        {
                            while(line[i + offset] != "")
                            {
                                offset++;
                            }

                            pos = new Vector3(float.Parse(line[i + offset + 1]), float.Parse(line[i + offset + 2]), float.Parse(line[i + offset + 3]));
                         
                            Position.Add(pos);                        
                            offset = 0;
                            break;
                        }

                    }
                    counter++;
                }
            }
            catch (Exception)
            {

                throw new FileLoadException("stl file");
            }
        }
        public static void Write(List<Vector3> position,List<Vector3> normal)
        {
            StreamWriter write = new StreamWriter("testfile.stl");
            write.WriteLine("solid stl");
            for (int i = 0; i < position.Count; i += 3)
            {
                write.WriteLine("facet normal " + normal[i].X +" " + normal[i].Y + " " + normal[i].Z);
                write.WriteLine("outer loop");

                write.WriteLine("vertex " + position[i].X + " " + position[i].Y + " " + position[i].Z);
                write.WriteLine("vertex " + position[i + 1].X + " " + position[i + 1].Y + " " + position[i + 1].Z);
                write.WriteLine("vertex " + position[i + 2].X + " " + position[i + 2].Y + " " + position[i + 2].Z);

                write.WriteLine("endloop");
                write.WriteLine("endfacet");
            }
            write.WriteLine("endsolid vcg");
            write.Close();
        }
    }
}
