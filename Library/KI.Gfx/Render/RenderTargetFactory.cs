using KI.Foundation.Core;

namespace KI.Gfx.Render
{
    /// <summary>
    /// レンダーターゲットのファクトリ
    /// </summary>
    public class RenderTargetFactory : KIFactoryBase<RenderTarget>
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        public static RenderTargetFactory Instance { get; } = new RenderTargetFactory();

        /// <summary>
        /// レンダーターゲットの作成
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        /// <param name="useDepthTexture">デプステクスチャを使うかどうか</param>
        /// <returns>レンダーターゲット</returns>
        public RenderTarget CreateRenderTarget(string name, int width, int height, bool useDepthTexture)
        {
            RenderTarget target = new RenderTarget(name, width, height, useDepthTexture);
            AddItem(target);
            return target;
        }
    }
}
