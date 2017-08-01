﻿using System;
using System.Text;
using System.IO;
using KI.Gfx.KITexture;

namespace KI.Asset
{
    public class HDRImage : KIImageInfo
    {
        public HDRImage(string path)
            : base(path)
        {

        }
        public string HDRFormat
        {
            get;
            private set;
        }
        private float _exposure;
        public float Expropsure
        {
            get
            {
                return _exposure;
            }
            private set
            {
                _exposure = value;
            }
        }
        private bool LoadHDRImageData()
        {
            StreamReader reader = new StreamReader(FilePath);
            if (reader == null)
                return false;

            string str;
            while (true)
            {
                str = reader.ReadLine();

                if (str.Contains("#"))
                    continue;

                //空白行の1行後はBinaryになるため
                if (String.IsNullOrWhiteSpace(str))
                {
                    break;
                }

                //空白を削除
                str = str.Replace(" ", "");
                if (str.Contains("FORMAT"))
                {
                    //不要文字を削除
                    str = str.Replace("FORMAT=", "");
                    HDRFormat = str;
                }

                if (str.Contains("EXPOSURE="))
                {
                    str = str.Replace("EXPOSURE=", "");
                    float.TryParse(str, out _exposure);
                }
            }
            str = reader.ReadLine();

            string[] xyData = str.Split(' ');

            for (int i = 0; i < xyData.Length; i++)
            {
                if (xyData[i].Contains("X"))
                {
                    i++;
                    Width = int.Parse(xyData[i]);
                }
                if (xyData[i].Contains("Y"))
                {
                    i++;
                    Height = int.Parse(xyData[i]);
                }
            }

            BinaryReader stream = new BinaryReader(new MemoryStream(Encoding.Unicode.GetBytes(reader.ReadToEnd())));

            //TODO: 実装

            reader.Close();

            return true;
        }



        public override bool LoadImageData()
        {
            return LoadHDRImageData();
        }

    }
}
