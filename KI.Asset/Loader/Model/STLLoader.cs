using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KI.Foundation.Core;
using OpenTK;

namespace KI.Asset.Loader
{
    /// <summary>
    /// STLのローダ現在テキストファイルのみ
    /// </summary>
    public class STLLoader : IModelLoader
    {
        /// <summary>
        /// 頂点
        /// </summary>
        private List<Vector3> position = new List<Vector3>();

        /// <summary>
        /// 法線
        /// </summary>
        private List<Vector3> normal = new List<Vector3>();

        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public STLLoader(string filePath)
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
        /// 頂点
        /// </summary>
        public List<Vector3> Position
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// 法線
        /// </summary>
        public List<Vector3> Normal
        {
            get
            {
                return normal;
            }
        }

        /// <summary>
        /// stlデータの出力
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="pos">頂点</param>
        /// <param name="nor">法線</param>
        public static void Write(string filePath, List<Vector3> pos, List<Vector3> nor)
        {
            StreamWriter write = new StreamWriter(filePath);
            write.WriteLine("solid stl");
            for (int i = 0; i < pos.Count; i += 3)
            {
                write.WriteLine("facet normal " + nor[i].X + " " + nor[i].Y + " " + nor[i].Z);
                write.WriteLine("outer loop");

                write.WriteLine("vertex " + pos[i].X + " " + pos[i].Y + " " + pos[i].Z);
                write.WriteLine("vertex " + pos[i + 1].X + " " + pos[i + 1].Y + " " + pos[i + 1].Z);
                write.WriteLine("vertex " + pos[i + 2].X + " " + pos[i + 2].Y + " " + pos[i + 2].Z);

                write.WriteLine("endloop");
                write.WriteLine("endfacet");
            }

            write.WriteLine("endsolid vcg");
            write.Close();
        }

        /// <summary>
        /// STLデータのロード
        /// </summary>
        /// <param name="parser">STLデータ</param>
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
    }
}
