using System.Collections.Generic;
using KI.Asset;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    /// <summary>
    /// 環境プローブ
    /// </summary>
    public class EnvironmentProbe : SceneNode
    {
        /// <summary>
        /// テクスチャパスリスト
        /// </summary>
        private List<string> texturePath;

        /// <summary>
        /// 環境プローブ
        /// </summary>
        /// <param name="name">名前</param>
        public EnvironmentProbe(string name)
            : base(name)
        {
            texturePath = new List<string>();
        }

        /// <summary>
        /// 環境マップ
        /// </summary>
        public Texture Cubemap { get; private set; }

        /// <summary>
        /// Cubemapの作成
        /// </summary>
        /// <param name="px">テクスチャPX</param>
        /// <param name="py">テクスチャPY</param>
        /// <param name="pz">テクスチャPZ</param>
        /// <param name="nx">テクスチャNX</param>
        /// <param name="ny">テクスチャNY</param>
        /// <param name="nz">テクスチャNZ</param>
        public void GenCubemap(string px, string py, string pz, string nx, string ny, string nz)
        {
            Cubemap = TextureFactory.Instance.CreateCubemapTexture(px, py, pz, nx, ny, nz);
            texturePath.Add(px); texturePath.Add(py); texturePath.Add(pz);
            texturePath.Add(nx); texturePath.Add(ny); texturePath.Add(nz);
        }

        /// <summary>
        /// 何も描画しない
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="renderInfo">レンダリング情報</param>
        public override void RenderCore(Scene scene, RenderInfo renderInfo)
        {
            return;
        }
    }
}
