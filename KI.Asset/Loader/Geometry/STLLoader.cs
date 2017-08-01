using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using KI.Foundation.Utility;
using KI.Foundation.Core;
using System.Collections.ObjectModel;

namespace KI.Asset.Loader
{
    /// <summary>
    /// STLのローダ現在テキストファイルのみ
    /// </summary>
    public class STLLoader : KIFile
    {
        private List<Vector3> position = new List<Vector3>();

        private List<Vector3> normal = new List<Vector3>();

        public GeometryInfo[] GeometryInfos
        {
            get;
            private set;
        }

        public List<Vector3> Position
        {
            get
            {
                return position;
            }
        }

        public List<Vector3> Normal
        {
            get
            {
                return normal;
            }
        }

        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public STLLoader(string filePath)
            : base(filePath)
        {
            try
            {
                string[] parser = File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("Shift_JIS"));
                ReadData(parser);
            }
            catch (Exception)
            {

                Logger.Log(Logger.LogLevel.Error, filePath + "開けません。現在のフォルダ位置" + System.Environment.CurrentDirectory);
            }
        }

        /// <summary>
        /// STLデータのロード
        /// </summary>
        /// <param name="parser">STLデータ</param>
        /// <param name="position">位置情報を格納</param>
        /// <param name="normal">法線情報を格納</param>
        private void ReadData(string[] parser)
        {
            try
            {
                int counter = 0;
                string[] line;
                Vector3 pos;
                while (parser.Length != counter)
                {
                    line = parser[counter].Split(' ');
                    line = line.Where(p => !(string.IsNullOrWhiteSpace(p) || string.IsNullOrEmpty(p))).ToArray();
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == "outer" && line[i + 1] == "loop") break;
                        if (line[i] == "solid" || line[i] == "endloop" || line[i] == "endfacet") break;
                        if (line[i] == string.Empty) continue;
                        if (line[i] == "facet" && line[i + 1] == "normal")
                        {
                            ////同じなものを3つ作って頂点の法線ベクトルにする
                            normal.Add(new Vector3(float.Parse(line[i + 2]), float.Parse(line[i + 3]), float.Parse(line[i + 4])));
                            normal.Add(new Vector3(float.Parse(line[i + 2]), float.Parse(line[i + 3]), float.Parse(line[i + 4])));
                            normal.Add(new Vector3(float.Parse(line[i + 2]), float.Parse(line[i + 3]), float.Parse(line[i + 4])));
                            break;
                        }
                        if (line[i] == "vertex")
                        {
                            pos = new Vector3(float.Parse(line[i + 1]), float.Parse(line[i + 2]), float.Parse(line[i + 3]));

                            position.Add(pos);
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

        public static void Write(List<Vector3> position, List<Vector3> normal)
        {
            StreamWriter write = new StreamWriter("testfile.stl");
            write.WriteLine("solid stl");
            for (int i = 0; i < position.Count; i += 3)
            {
                write.WriteLine("facet normal " + normal[i].X + " " + normal[i].Y + " " + normal[i].Z);
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
