﻿using System.Collections.Generic;
using System.Linq;
using KI.Asset.Loader.Loader;
using KI.Gfx.GLUtil;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// plyファイルデータを独自形式に変換
    /// </summary>
    public class PLYConverter : IGeometry
    {
        /// <summary>
        /// plyファイルデータ
        /// </summary>
        private PLYLoader plyData;

        /// <summary>
        /// plyファイルのローダ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public PLYConverter(string filePath)
        {
            plyData = new PLYLoader(filePath);
            CreateGeometry();
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Geometry[] Geometrys { get; private set; }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateGeometry()
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

            Geometry info = new Geometry(position, null, Vector3.UnitX, null, index, GeometryType.Triangle);
            Geometrys = new Geometry[] { info };
        }
    }
}
