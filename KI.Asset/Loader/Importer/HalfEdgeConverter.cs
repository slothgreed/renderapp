using System.Collections.Generic;
using System.Linq;
using KI.Analyzer;
using KI.Gfx.Geometry;

namespace KI.Asset.Loader.Importer
{
    /// <summary>
    /// ハーフエッジを独自形式に変換
    /// </summary>
    public class HalfEdgeImporter : ICreateModel
    {
        /// <summary>
        /// ハーフエッジ
        /// </summary>
        private HalfEdgeDS halfEdge;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public HalfEdgeImporter(string filePath)
        {
            halfEdge = new HalfEdgeDS(filePath);
            HalfEdgeIO.ReadFile(filePath, halfEdge);
            CreateModel();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="half">ハーフエッジ</param>
        public HalfEdgeImporter(HalfEdgeDS half)
        {
            halfEdge = half;
            CreateModel();
        }

        /// <summary>
        /// 形状
        /// </summary>
        public Polygon Model { get; private set; }

        /// <summary>
        /// 形状の作成
        /// </summary>
        public void CreateModel()
        {
            halfEdge.Index = new List<int>();
            foreach (var mesh in halfEdge.HalfEdgeMeshs)
            {
                halfEdge.Index.AddRange(mesh.AroundVertex.Select(p => p.Index));
            }

            ImporterUtility.NormalizeObject(halfEdge.Vertexs);

            foreach (var vertex in halfEdge.Vertexs)
            {
                vertex.TexCoord = vertex.Position.Xz * 10;
            }

            var uvTexture = TextureFactory.Instance.CreateUVTexture(128);
            halfEdge.Textures.Add(Gfx.KITexture.TextureKind.Albedo, uvTexture);
            Model = halfEdge;
        }
    }
}
