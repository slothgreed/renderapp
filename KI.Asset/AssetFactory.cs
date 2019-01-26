﻿using System.Collections.Generic;
using KI.Analyzer;
using KI.Asset.Loader.Converter;
using KI.Foundation.Core;
using KI.Gfx.Geometry;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// アセットファクトリ
    /// </summary>
    public class AssetFactory : KIFactoryBase<Polygon>
    {
        /// <summary>
        /// シングルトン
        /// </summary>
        public static AssetFactory Instance { get; } = new AssetFactory();

        /// <summary>
        /// 環境プローブの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>環境プローブ</returns>
        public EnvironmentProbe CreateEnvironmentMap(string name)
        {
            return new EnvironmentProbe(name);
        }

        /// <summary>
        /// カメラの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>カメラ</returns>
        public Camera CreateCamera(string name)
        {
            return new Camera(name);
        }

        /// <summary>
        /// 軸の作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>軸</returns>
        public IPolygon CreateAxis(string name, Vector3 min, Vector3 max)
        {
            return new Axis(name, min, max);
        }

        /// <summary>
        /// 球の作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="radial">半径</param>
        /// <param name="hpartition">縦分割数</param>
        /// <param name="wpartition">横分割数</param>
        /// <param name="orient">内外</param>
        /// <returns>球</returns>
        public IPolygon CreateSphere(string name, float radial, int hpartition, int wpartition, bool orient)
        {
            return new Sphere(name, radial, hpartition, wpartition, orient);
        }

        /// <summary>
        /// 平面の作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>平面</returns>
        public IPolygon CreateRectangle(string name)
        {
            return new Rectangle(name);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="radial">半径</param>
        /// <param name="smoothNum">スムージング回数</param>
        public IPolygon CreateIcosahedron(string name, float radial, int smoothNum)
        {
            return new Icosahedron(name, radial, smoothNum);
        }

        /// <summary>
        /// 3dモデルのロード
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>3dモデル</returns>
        public IPolygon CreateLoad3DModel(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath);
            string fileName = System.IO.Path.GetFileName(filePath);
            switch (extension)
            {
                case ".obj":
                    return new OBJConverter(filePath);
                case ".stl":
                    return new STLConverter(filePath);
                case ".half":
                    return new HalfEdgeConverter(filePath);
                case ".ply":
                    return new PLYConverter(filePath);
            }

            return null;
        }

        /// <summary>
        /// ハーフエッジの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="position">頂点座標</param>
        /// <param name="index">頂点番号</param>
        /// <returns>ハーフエッジ</returns>
        public IPolygon CreateHalfEdge(string name, List<Vector3> position, List<int> index)
        {
            return new HalfEdgeConverter(new HalfEdgeDS(name, position, index));
        }
    }
}
