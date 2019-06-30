using KI.Foundation.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset.Loader.Model
{
    public class IGESLoader
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public IGESLoader(string filePath)
        {
            try
            {
                string[] fileStream = File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("Shift_JIS"));
                ReadData(fileStream);
                ParameterAnalyzer.ParameterDataLog();
            }
            catch (Exception e)
            {
                Logger.Log(Logger.LogLevel.Warning, filePath + "開けません。" + "error : " + e.Message);
            }
        }

        /// <summary>
        /// パラメータセクションの解析用クラス
        /// </summary>
        private IGESParameterAnalyzer ParameterAnalyzer = new IGESParameterAnalyzer(); 
        
        /// <summary>
        /// ディレクトリセクションのデータ
        /// </summary>
        private List<DirectorySectionData> directorySectionDatas;

        /// <summary>
        /// デリミタ
        /// </summary>
        private char Delimiter = ',';

        /// <summary>
        /// 終端デリミタ
        /// </summary>
        private char EndDelimiter = ';';
        /// <summary>
        /// ファイルの読み込み
        /// </summary>
        /// <param name="fileStream">ファイルデータ</param>
        private void ReadData(string[] fileStream)
        {
            try
            {
                List<string> parameterData = new List<string>();
                directorySectionDatas = new List<DirectorySectionData>();
                int lineNumber = 0;
                while (lineNumber < fileStream.Length)
                {
                    char section = fileStream[lineNumber][72];
                    switch (section)
                    {
                        case 'S':
                            StartSection(fileStream[lineNumber]);
                            break;
                        case 'G':
                            GlobalSection(fileStream[lineNumber]);
                            break;
                        case 'D':
                            DirectorySection(fileStream[lineNumber], fileStream[lineNumber + 1]);
                            lineNumber++; // 2行一気に読み取る分
                            break;
                        case 'P':
                            if (fileStream[lineNumber].Contains(EndDelimiter))
                            {
                                // エンドデリミタが出るまで、行情報をスタックし、
                                // エンドデリミタが出たら形状情報を読み取る。
                                parameterData.Add(fileStream[lineNumber]);
                                ParameteDataSection(parameterData);
                                parameterData.Clear();
                            }
                            else
                            {
                                parameterData.Add(fileStream[lineNumber]);
                            }
                            break;
                        case 'T':
                            TerminateSection(fileStream[lineNumber]);
                            break;
                        default:
                            break;
                    }

                    lineNumber++;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// スタートセクション
        /// </summary>
        /// <param name="data">スタートセクションデータ</param>
        private void StartSection(string data)
        {
            // コメント記述位置なのでスキップ
        }

        /// <summary>
        /// グローバルセクション
        /// </summary>
        /// <param name="data">グローバルセクションデータ</param>
        private void GlobalSection(string data)
        {
            // 図面情報記述位置なのでスキップ
        }

        /// <summary>
        /// ディレクトリセクション
        /// ディレクトリセクションは2行で1つ
        /// </summary>
        /// <param name="data1">ディレクトリセクションデータ1行目</param>
        /// <param name="data2">ディレクトリセクションデータ2行目</param>
        private void DirectorySection(string data1, string data2)
        {
            var data = new DirectorySectionData(data1, data2);

            directorySectionDatas.Add(data);
        }

        /// <summary>
        /// パラメータデータセクション
        /// </summary>
        /// <param name="data">パラメータデータセクションデータ</param>
        private void ParameteDataSection(List<string> data)
        {
            string primitiveData = string.Empty;
            int directorySection;

            directorySection = int.Parse(data[0].Substring(8 * 8, 8));
            foreach (var dataLine in data)
            {
                primitiveData += dataLine.Substring(0, 8 * 8).Trim();
            }

            var load = ParameterAnalyzer.Analyze(primitiveData, Delimiter);
            if (load == false)
            {
                throw new FileLoadException();
            }
        }

        /// <summary>
        /// ターミネートセクション
        /// </summary>
        /// <param name="data">ターミネートセクションデータ</param>
        private void TerminateSection(string data)
        {
            // 各セクションの行数は不要なのでスキップ
        }

        /// <summary>
        /// ディレクトリセクションデータ
        /// データの説明
        /// http://afsoft.jp/cad/p08.html
        /// </summary>
        public class DirectorySectionData
        {
            // ディレクトリセクションの番号
            public int Index;

            // 1行目
            public int PrimitiveIndex { get; private set; }
            public int SequeneIndex { get; private set; }
            public int IGESVersion { get; private set; }
            public int LineType { get; private set; }
            public int LayerNumber { get; private set; }
            public int ProjectionGraph { get; private set; }
            public int MatrixType { get; private set; }
            public int Composit { get; private set; }
            public int Status { get; private set; }

            // 2行目
            // PrimitiveIndexは1行目と同じなため不要
            public int LineWidth { get; private set; }
            public int PenNumber { get; private set; }
            public int CardNumber { get; private set; }
            public int GraphElement { get; private set; }
            public int Reserved0 { get; private set; }
            public int Reserved1 { get; private set; }
            public string IndexLabel { get; private set; }
            public int SubScript { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="data1">1行目</param>
            /// <param name="data2">2行目</param>
            public DirectorySectionData(string data1, string data2)
            {
                Load(data1, data2);
            }

            /// <summary>
            /// 読み込み
            /// </summary>
            /// <param name="data1">1行目</param>
            /// <param name="data2">2行目</param>
            private void Load(string data1, string data2)
            {
                PrimitiveIndex  = int.Parse(data1.Substring(0, 8));
                SequeneIndex    = int.Parse(data1.Substring(8 * 1, 8));
                IGESVersion     = int.Parse(data1.Substring(8 * 2, 8));
                LineType        = int.Parse(data1.Substring(8 * 3, 8));
                LayerNumber     = int.Parse(data1.Substring(8 * 4, 8));
                ProjectionGraph = int.Parse(data1.Substring(8 * 5, 8));
                MatrixType      = int.Parse(data1.Substring(8 * 6, 8));
                Composit        = int.Parse(data1.Substring(8 * 7, 8));
                Status          = int.Parse(data1.Substring(8 * 8, 8));
                Index           = int.Parse(data1.Substring(8 * 9 + 1, 7)); // 8*9番目はセクション名、その分1つマイナスした値でSubstring

                LineWidth       = int.Parse(data2.Substring(8 * 1, 8));
                PenNumber       = int.Parse(data2.Substring(8 * 2, 8));
                CardNumber      = int.Parse(data2.Substring(8 * 3, 8));
                GraphElement    = int.Parse(data2.Substring(8 * 4, 8));
                Reserved0       = int.Parse(data2.Substring(8 * 5, 8));
                Reserved1       = int.Parse(data2.Substring(8 * 6, 8));
                IndexLabel      =           data2.Substring(8 * 7, 8).Trim();
                SubScript       = int.Parse(data2.Substring(8 * 8, 8));
            }        
        }
    }
}
