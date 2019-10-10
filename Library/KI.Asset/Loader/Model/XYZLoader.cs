using KI.Foundation.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.Loader.Model
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    public class XYZLoader : IModelLoader
    {
        /// <summary>
        /// 頂点
        /// </summary>
        private List<Vector3> position = new List<Vector3>();

        /// <summary>
        /// 頂点のゲッタ
        /// </summary>
        public List<Vector3> Position { get { return position; } }

        /// <summary>
        /// 色
        /// </summary>
        private List<Vector3> color = new List<Vector3>();

        /// <summary>
        /// カラーのゲッタ
        /// </summary>
        public List<Vector3> Color { get { return color; } }

        /// <summary>
        /// 輝度
        /// </summary>
        private List<int> intensity = new List<int>();

        /// <summary>
        /// 輝度のゲッタ
        /// </summary>
        public List<int> Intensity { get { return intensity; } }

        /// <summary>
        /// XYZのローダ
        /// </summary>
        /// <param name="filePath"></param>
        public XYZLoader(string filePath)
        {
            try
            {
                var extension = Path.GetExtension(filePath);
                if (extension.Contains("_b"))
                {
                    ReadBinary(filePath);
                }
                else
                {
                    string[] fileStream = File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("Shift_JIS"));
                    ReadData(fileStream);

                }
            }
            catch (Exception e)
            {
                Logger.Log(Logger.LogLevel.Warning, filePath + "開けません。" + "error : " + e.Message);
            }
        }

        /// <summary>
        /// ファイルの読み込みXYZIRGBを想定
        /// </summary>
        /// <param name="fileStream"></param>
        private void ReadData(string[] fileStream)
        {
            foreach (var file in fileStream)
            {
                var data = file.Split(' ');

                position.Add(new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2])));

                intensity.Add(int.Parse(data[3]));

                color.Add(new Vector3(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6])));
            }
        }

        /// <summary>
        /// オリジナルフォーマットのバイナリ読み込み
        /// </summary>
        public void ReadBinary(string inputPath)
        {
            var reader = new BinaryReader(new FileStream(inputPath, FileMode.Open));
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                position.Add(new Vector3(reader.ReadSingle(),reader.ReadSingle(),reader.ReadSingle()));
                intensity.Add(reader.ReadInt32());
                color.Add(new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            }

            reader.Close();
        }

        /// <summary>
        /// オリジナルフォーマットのバイナリ書き込み
        /// </summary>
        /// <param name="outputPath">出力パス</param>
        /// <param name="position">位置</param>
        /// <param name="intensity">輝度</param>
        /// <param name="color">色</param>
        public void WriteBinary(string outputPath, List<Vector3> position, List<int> intensity, List<Vector3> color)
        {
            var writer = new BinaryWriter(new FileStream(outputPath, FileMode.CreateNew));
            writer.Write(position.Count);
            for (int i = 0; i < position.Count; i++)
            {
                writer.Write(position[i].X); writer.Write(position[i].Y); writer.Write(position[i].Z);
                writer.Write(intensity[i]);
                writer.Write(color[i].X); writer.Write(color[i].Y); writer.Write(color[i].Z);
            }
            writer.Close();
        }
    }
}
