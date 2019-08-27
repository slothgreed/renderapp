using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KI.Foundation.Core;

namespace KI.Asset.Loader
{
    /// <summary>
    /// plyファイルのローダ
    /// </summary>
    public class PLYLoader : IModelLoader
    {
        /// <summary>
        /// プロパティ名
        /// </summary>
        private List<string> propertyNames;

        /// <summary>
        /// プロパティ
        /// </summary>
        private List<List<float>> propertys = new List<List<float>>();

        /// <summary>
        /// 面情報
        /// </summary>
        private List<int> faceIndex = new List<int>();

        /// <summary>
        /// plyファイルのローダ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public PLYLoader(string filePath)
        {
            try
            {
                propertyNames = new List<string>();
                propertys = new List<List<float>>();

                string[] fileStream = File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("Shift_JIS"));
                ReadData(fileStream);
            }
            catch (Exception e)
            {
                Logger.Log(Logger.LogLevel.Warning, filePath + "開けません。" + "error : " + e.Message);
            }
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public List<string> PropertyName
        {
            get
            {
                return propertyNames;
            }
        }

        /// <summary>
        /// プロパティ値
        /// </summary>
        public List<List<float>> Propertys
        {
            get
            {
                return propertys;
            }
        }

        /// <summary>
        /// 面情報
        /// </summary>
        public List<int> FaceIndex
        {
            get
            {
                return faceIndex;
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
                for (i = 0; i < fileStream.Length; i++)
                {
                    var lineData = fileStream[i]
                        .Split(' ')
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToArray();

                    if (lineData[0] == "element")
                    {
                        if (lineData[1] == "vertex")
                        {
                            if (!int.TryParse(lineData[2], out vertexNum))
                            {
                                throw new Exception("can't parse vertexNum");
                            }
                        }

                        if (lineData[1] == "face")
                        {
                            if (!int.TryParse(lineData[2], out faceNum))
                            {
                                throw new Exception("can't parse faceNum");
                            }
                        }
                    }

                    if (lineData[0] == "property")
                    {
                        if (!lineData.Contains("list"))
                        {
                            propertyNames.Add(lineData[2]);
                            propertys.Add(new List<float>());
                        }
                    }

                    if (lineData[0] == "end_header")
                    {
                        break;
                    }
                }

                ReadVertexData(fileStream, i + 1, vertexNum);
                ReadFaceData(fileStream, i + 1 + vertexNum, faceNum);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 頂点データの読み込み
        /// </summary>
        /// <param name="fileStream">ファイルデータ</param>
        /// <param name="startIndex">読み込み開始位置</param>
        /// <param name="vertexNum">頂点数</param>
        private void ReadVertexData(string[] fileStream, int startIndex, int vertexNum)
        {
            for (int i = startIndex; i < startIndex + vertexNum; i++)
            {
                string[] vertexData = fileStream[i]
                    .Split(' ')
                    .Where(p => !string.IsNullOrEmpty(p))
                    .ToArray();

                for (int j = 0; j < propertys.Count; j++)
                {
                    float data = 0;
                    if (float.TryParse(vertexData[j], out data))
                    {
                        propertys[j].Add(data);
                    }
                    else
                    {
                        throw new Exception("can't parse vertex data");
                    }
                }
            }
        }

        /// <summary>
        /// 面データの読み込み
        /// </summary>
        /// <param name="fileStream">ファイルデータ</param>
        /// <param name="startIndex">読み込み開始位置</param>
        /// <param name="faceNum">面数</param>
        private void ReadFaceData(string[] fileStream, int startIndex, int faceNum)
        {
            for (int i = startIndex; i < startIndex + faceNum; i++)
            {
                string[] faceDataStr = fileStream[i]
                    .Split(' ')
                    .Where(p => !string.IsNullOrEmpty(p))
                    .ToArray();

                if (faceDataStr[0] != "3")
                {
                    throw new Exception("not support face data");
                }

                for (int j = 1; j < faceDataStr.Length; j++)
                {
                    int index;
                    if (!int.TryParse(faceDataStr[j], out index))
                    {
                        throw new Exception("can't parse face index");
                    }

                    faceIndex.Add(index);
                }
            }
        }
    }
}
