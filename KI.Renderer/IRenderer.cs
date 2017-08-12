using System.Collections.Generic;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    /// <summary>
    /// レンダリングシステム
    /// </summary>
    public abstract class IRenderer
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IRenderer()
        {
            ProcessingTexture = new List<Texture>();
            PostProcessMode = false;
            RenderQueue = new RenderQueue();
            PostEffect = new RenderQueue();

            RenderQueue.TechniqueAdded += RenderQueue_TechniqueAdded;
            PostEffect.TechniqueAdded += RenderQueue_TechniqueAdded;
        }

        /// <summary>
        /// 描画するシーン
        /// </summary>
        public IScene ActiveScene { get; set; }

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
        public bool PostProcessMode { get; private set; }

        /// <summary>
        /// レンダリング結果のテクスチャすべて
        /// </summary>
        public List<Texture> ProcessingTexture { get; private set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public abstract void Initialize(int width, int height);

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
            RenderQueue.Render(ActiveScene);

            if (PostProcessMode)
            {
                PostEffect.Render(ActiveScene);
            }

            OutputBuffer.uSelectMap = OutputBuffer.OutputTexture[0];
            OutputBuffer.uTarget = OutputTexture;
            OutputBuffer.Render(ActiveScene);
        }

        /// <summary>
        /// ポストプロセスを行うかのトグル
        /// </summary>
        public void TogglePostProcess()
        {
            PostProcessMode = !PostProcessMode;
        }

        /// <summary>
        /// レンダーテクスチャ追加イベント
        /// </summary>
        /// <param name="sender">レンダーキュー</param>
        /// <param name="e">レンダーキューイベント</param>
        private void RenderQueue_TechniqueAdded(object sender, RenderQueueEventArgs e)
        {
            foreach (var texture in e.Technique.OutputTexture)
            {
                ProcessingTexture.Add(texture);
            }
        }
    }
}
