using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using OpenTK;

namespace KI.Asset.Loader.Loader
{
    /// <summary>
    /// plyファイルのローダ
    /// </summary>
    public class PLY2Loader : KIFile
    {
        /// <summary>
        /// 面情報
        /// </summary>
        private List<int> faceIndex = new List<int>();

        /// <summary>
        /// plyファイルのローダ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public PLY2Loader(string filePath, string filePath2)
            : base(filePath)
        {
            try
            {
                Vertexs = new List<Vector3>();
                Propertys = new List<float[]>();
                FaceIndex = new List<int>();
                string[] fileStream = File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("Shift_JIS"));
                ReadData(fileStream);
                string[] fileStream2 = File.ReadAllLines(filePath2, System.Text.Encoding.GetEncoding("Shift_JIS"));
                ReadData2(fileStream2);

            }
            catch (Exception e)
            {
                Logger.Log(Logger.LogLevel.Warning, filePath + "開けません。" + "error : " + e.Message);
            }
        }

        public List<Vector3> Vertexs
        {
            get;
            private set;
        }

        public List<int> FaceIndex
        {
            get;
            private set;
        }

        public List<float[]> Propertys
        {
            get;
            private set;
        }

        /// <summary>
        /// ファイルの読み込み
        /// </summary>
        /// <param name="fileStream">ファイルデータ</param>
        private void ReadData2(string[] fileStream)
        {
            try
            {
                int i;
                for (i = 0; i < fileStream.Length; i++)
                {
                    var lineData = fileStream[i]
                        .Split(' ')
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToArray();
                    var value = new float[11];
                    if(lineData.Length != 11)
                    {
                        continue;
                    }
                    for (int j = 0; j < lineData.Length; j++)
                    {
                        float val = 0;
                        if(float.TryParse(lineData[j], out val))
                        {
                            value[j] = val;
                        }
                    }

                    Propertys.Add(value);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// ファイルの読み込み
        /// </summary>
        /// <param name="fileStream">ファイルデータ</param>
        private void ReadData(string[] fileStream)
        {
            try
            {
                int vertexNum = 0;
                int faceNum = 0;
                int i;
                float x = 0, y = 0, z = 0;
                for (i = 0; i < fileStream.Length; i++)
                {
                    var lineData = fileStream[i]
                        .Split(' ')
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToArray();
                    if(i == 0)
                    {
                        vertexNum = int.Parse(lineData[0]);
                    }
                    if(i == 1)
                    {
                        faceNum = int.Parse(lineData[0]);
                    }
                    if (Vertexs.Count != vertexNum)
                    {
                        if ((i - 2) % 3 == 0)
                        {
                            x = float.Parse(lineData[0]);
                        }
                        if ((i - 2) % 3 == 1)
                        {
                            y = float.Parse(lineData[0]);

                        }
                        if ((i - 2) % 3 == 2)
                        {
                            z = float.Parse(lineData[0]);
                            if (Vertexs.Count != vertexNum)
                            {
                                Vertexs.Add(new Vector3(x, y, z));
                            }
                        }
                    }
                    else
                    {
                        if ((i - 2 - vertexNum * 3) % 4 != 0)
                        {
                            FaceIndex.Add(int.Parse(lineData[0]));
                        }
                        else
                        {
                            int a = 0;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
