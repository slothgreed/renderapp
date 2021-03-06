﻿using System.Collections.Generic;
using System.Linq;
using KI.Gfx;
using KI.Gfx.Geometry;
using OpenTK;


namespace KI.Asset.Loader.Importer
{
    /// <summary>
    /// plyファイルデータを独自形式に変換
    /// </summary>
    public class PLYImporter : ICreateModel
    {
        /// <summary>
        /// plyファイルデータ
        /// </summary>
        private PLYLoader plyData;

        /// <summary>
        /// ファイルパス
        /// </summary>
        private string filePath;

        /// <summary>
        /// plyファイルのローダ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public PLYImporter(string filePath)
        {
            plyData = new PLYLoader(filePath);
            this.filePath = filePath;
            CreateModel();
        }

        /// <summary>
        /// 形状情報
        /// </summary>
        public Polygon Model { get; private set; }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateModel()
        {
            var vertexs = new List<Vertex>();

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

                vertexs.Add(new Vertex(i, new Vector3(x, y, z), new Vector3(vectorX, vectorY, vectorZ)));
            }

            List<int> index = plyData.FaceIndex.ToList();
            Model = new Polygon(filePath, vertexs, index, KIPrimitiveType.Triangles);
        }
    }
}
