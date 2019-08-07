using KI.Foundation.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.Loader
{
    /// <summary>
    /// OFF ファイルのローダ
    /// </summary>
    public class OFFLoader : KIFile, IModelLoader
    {
        /// <summary>
        /// 頂点
        /// </summary>
        private List<Vector3> position = new List<Vector3>();

        /// <summary>
        /// 頂点インデックス
        /// </summary>
        private List<Vector4> color = new List<Vector4>();

        /// <summary>
        /// 頂点インデックス
        /// </summary>
        private List<int> index = new List<int>();

        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public OFFLoader(string filePath)
            : base(filePath)
        {
            try
            {
                string[] parser = File.ReadAllLines(filePath, Encoding.GetEncoding("Shift_JIS"));
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
        /// カラー
        /// </summary>
        public List<Vector4> Color
        {
            get
            {
                return color;
            }
        }

        /// <summary>
        /// 要素番号
        /// </summary>
        public List<int> Index
        {
            get
            {
                return index;
            }
        }

        /// <summary>
        /// STLデータのロード
        /// </summary>
        /// <param name="parser">STLデータ</param>
        private void ReadData(string[] parser)
        {
            try
            {
                // 0行目は"OFF"なのでスキップ
                int counter = 1;

                int vertexNum = 0;
                int faceNume = 0;
                int edgeNum = 0;
                string[] line;
                while (parser.Length != counter)
                {
                    line = parser[counter].Split(' ');
                    line = line.Where(p => !(string.IsNullOrWhiteSpace(p) || string.IsNullOrEmpty(p))).ToArray();

                    if (counter == 1)
                    {
                        vertexNum = int.Parse(line[0]);
                        faceNume = int.Parse(line[1]);
                        edgeNum = int.Parse(line[2]);
                    }
                    else if (counter < vertexNum + 2) // 0行目、1行目の分の +2
                    {
                        position.Add(new Vector3(float.Parse(line[0]), float.Parse(line[1]), float.Parse(line[2])));

                        if (line.Length > 3)
                        {
                            color.Add(new Vector4(float.Parse(line[3]), float.Parse(line[4]), float.Parse(line[5]), float.Parse(line[6])));
                        }
                    }
                    else
                    {
                        if (int.Parse(line[0]) == 3)
                        {
                            index.Add(int.Parse(line[1]));
                            index.Add(int.Parse(line[2]));
                            index.Add(int.Parse(line[3]));
                        }
                        else
                        {
                            throw new NotSupportedException("3角形以外はサポート外");
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
