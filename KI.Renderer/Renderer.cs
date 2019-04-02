using System.Collections.Generic;
using KI.Asset.Technique;
using KI.Gfx.KITexture;

namespace KI.Asset
{
    /// <summary>
    /// レンダリングシステム
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Renderer()
        {
            ProcessingTexture = new List<Texture>();
            PostProcessMode = true;
            RenderQueue = new RenderQueue();
            PostEffect = new RenderQueue();

            RenderQueue.TechniqueAdded += RenderQueue_TechniqueAdded;
            PostEffect.TechniqueAdded += RenderQueue_TechniqueAdded;
        }

        /// <summary>
        /// 描画するシーン
        /// </summary>
        public Scene ActiveScene { get; set; }

        /// <summary>
        /// レンダーキュー
        /// </summary>
        public RenderQueue RenderQueue { get; set; }

        /// <summary>
        /// ポストエフェクト
        /// </summary>
        public RenderQueue PostEffect { get; set; }

        /// <summary>
        /// 出力用バッファ
        /// </summary>
        public OutputBuffer OutputBuffer { get; set; }
        
        /// <summary>
        /// 出力テクスチャ
        /// </summary>
        public Texture OutputTexture { get; set; }

        /// <summary>
        /// ポストプロセスモード
        /// </summary>
        public bool PostProcessMode { get; set; }

        /// <summary>
        /// レンダリング結果のテクスチャすべて
        /// </summary>
        public List<Texture> ProcessingTexture { get; private set; }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SizeChanged(int width, int height)
        {
            RenderQueue.SizeChanged(width, height);
            PostEffect.SizeChanged(width, height);
            //OutputBuffer.SizeChanged(width, height);
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            RenderQueue.TechniqueAdded -= RenderQueue_TechniqueAdded;
            PostEffect.TechniqueAdded -= RenderQueue_TechniqueAdded;

            RenderQueue.Dispose();
            PostEffect.Dispose();
            RenderTechniqueFactory.Instance.Dispose();
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Render()
        {
            if (ActiveScene == null)
            {
                return;
            }

            RenderQueue.Render(ActiveScene);

            if (PostProcessMode)
            {
                PostEffect.Render(ActiveScene);
            }

            OutputBuffer.uTarget = OutputTexture;
            OutputBuffer.Render(ActiveScene);
        }

        /// <summary>
        /// レンダーテクスチャ追加イベント
        /// </summary>
        /// <param name="sender">レンダーキュー</param>
        /// <param name="e">レンダーキューイベント</param>
        private void RenderQueue_TechniqueAdded(object sender, RenderQueueEventArgs e)
        {
            foreach (var texture in e.Technique.RenderTarget.RenderTexture)
            {
                ProcessingTexture.Add(texture);
            }
        }
    }
}
