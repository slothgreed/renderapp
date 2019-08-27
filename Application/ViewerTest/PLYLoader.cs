using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ViewerTest
{
    public class PLYLoader
    {
        private List<string> propertyNames;

        public IReadOnlyList<string> PropertyName
        {
            get
            {
                return propertyNames.AsReadOnly();
            }
        }

        private List<List<float>> propertys = new List<List<float>>();

        public IReadOnlyList<List<float>> Propertys
        {
            get
            {
                return propertys.AsReadOnly();
            }
        }

        private List<int> faceIndex = new List<int>();
        public IReadOnlyList<int> FaceIndex
        {
            get
            {
                return faceIndex.AsReadOnly();
            }
        }

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
                Console.WriteLine(e.Message);
            }
        }

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
