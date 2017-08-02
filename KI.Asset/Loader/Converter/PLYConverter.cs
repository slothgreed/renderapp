using OpenTK;
using System.Collections.Generic;
using System.Linq;
using KI.Asset.Loader.Loader;
using KI.Gfx.GLUtil;

namespace KI.Asset
{
    /// <summary>
    /// plyファイルデータを独自形式に変換
    /// </summary>
    class PLYConverter : IGeometry
    {
        /// <summary>
        /// plyファイルのローダ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public PLYConverter(string filePath)
        {
            var plyData = new PLYLoader(filePath);
            CreateGeometryInfo(plyData);
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public GeometryInfo[] GeometryInfos { get; private set; }

        /// <summary>
        /// 形状情報の作成
        /// </summary>
        /// <param name="plyData">plyデータ</param>
        public void CreateGeometryInfo(PLYLoader plyData)
        {
            var position = new List<Vector3>();

            int vertexNum = plyData.Propertys[0].Count;
            for (int i = 0; i < vertexNum; i++)
            {
                var x = plyData.Propertys[0][i];
                var y = plyData.Propertys[1][i];
                var z = plyData.Propertys[2][i];
                var angle = plyData.Propertys[3][i];
                var vectorX = plyData.Propertys[4][i];
                var vectorY = plyData.Propertys[5][i];
                var vectorZ = plyData.Propertys[6][i];

                position.Add(new Vector3(x, y, z));
            }

            List<int> index = plyData.FaceIndex.ToList();

            GeometryInfo info = new GeometryInfo(position, null, Vector3.UnitX, null, index, GeometryType.Triangle);
            GeometryInfos = new GeometryInfo[] { info };
        }
    }
}
