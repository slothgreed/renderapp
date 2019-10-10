using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.Geometry;
using KI.Asset.Loader.Model;
using System.IO;

namespace KI.Asset.Loader.Importer
{
    /// <summary>
    /// STLのローダ現在テキストファイルのみ
    /// </summary>
    public class XYZImporter : ICreateModel
    {
        /// <summary>
        /// stlファイルのローダ
        /// </summary>
        private XYZLoader xyzData;

        /// <summary>
        /// ファイルパス
        /// </summary>
        private string filePath;

        /// <summary>
        /// STLのローダ。
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public XYZImporter(string filePath)
        {
            xyzData = new XYZLoader(filePath);
            this.filePath = filePath;
            CreateModel();

        }

        public Polygon Model
        {
            get;
            private set;
        }

        public void CreateModel()
        {
            var vertex = new List<Vertex>();
            for(int i = 0; i < xyzData.Position.Count; i++)
            {
                vertex.Add(new Vertex(i, xyzData.Position[i], xyzData.Color[i] / 255.0f));
            }

            Model = new Polygon(filePath, vertex, Gfx.KIPrimitiveType.Points);

            //string binaryPath = Path.GetDirectoryName(filePath) + "\\" + Path.GetFileNameWithoutExtension(filePath) + ".xyz_b";
            //xyzData.WriteBinary(binaryPath, xyzData.Position, xyzData.Intensity, xyzData.Color );
        }
    }
}
