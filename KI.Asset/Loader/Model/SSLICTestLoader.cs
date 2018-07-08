using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Core;
using OpenTK;

namespace KI.Asset.Loader.Model
{
    class SSLICTestLoader : KIFile
    {
        public List<Vector3> Position = new List<Vector3>();
        public List<Vector2> Texcoord = new List<Vector2>();
        public List<Vector3> Vector = new List<Vector3>();

        public SSLICTestLoader(string filePath) 
            : base(filePath)
        {
        }

        public void Load()
        {
            FilePath = @"E:\MyProgram\KIProject\renderapp\ViewerTest\vectorfield.txt";
            string[] fileStream = File.ReadAllLines(FilePath, System.Text.Encoding.GetEncoding("Shift_JIS"));

            for (int i = 0; i < fileStream.Length; i++)
            {
                string[] lineData = fileStream[i]
                    .Split(',')
                    .Where(p => !string.IsNullOrEmpty(p))
                    .ToArray();

                if (lineData.Length != 8)
                {
                    break;
                }
                float[] floatData = lineData
                    .Select(p => float.Parse(p))
                    .ToArray();

                Position.Add(new Vector3(floatData[0], floatData[1], floatData[2]));
                Texcoord.Add(new Vector2(floatData[3], floatData[4]));
                Vector.Add(new Vector3(floatData[5], floatData[6], floatData[7]));
            }

        }
    }
}
