using System.Collections.Generic;
using KI.Gfx.KITexture;
using KI.Renderer.Technique;

namespace KI.Renderer
{
    /// <summary>
    /// レンダリングシステム
    /// </summary>
    public class RenderSystem
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RenderSystem()
        {
            ProcessingTexture = new List<Texture>();
            PostProcessMode = true;
            RenderQueue = new RenderQueue();
            PostEffect = new RenderQueue();
            BackGroundObject = new List<HUDObject>();
            ForeGroundObject = new List<HUDObject>();

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
        /// 前面に書く HUD
        /// </summary>
        public List<HUDObject> ForeGroundObject { get; private set; }

        /// <summary>
        /// ForeGroundBuffer の バッキングフィールド
        /// </summary>
        private HUDBuffer foreGroundBuffer;

        /// <summary>
        /// 前面に書く HUD
        /// </summary>
        public HUDBuffer ForeGroundBuffer
        {
            get
            {
                return foreGroundBuffer;
            }
            set
            {
                foreGroundBuffer = value;
                AddProcessingTexture(foreGroundBuffer);
            }
        }

        /// <summary>
        /// 背景に書く HUD
        /// </summary>
        public List<HUDObject> BackGroundObject { get; private set; }

        /// <summary>
        /// BackGroundBuffer の バッキングフィールド
        /// </summary>
        private HUDBuffer backGroundBuffer;

        /// <summary>
        /// 背景に書く HUD
        /// </summary>
        public HUDBuffer BackGroundBuffer
        {
            get
            {
                return backGroundBuffer;
            }
            set
            {
                backGroundBuffer = value;
                AddProcessingTexture(backGroundBuffer);
            }
        }


        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SizeChanged(int width, int height)
        {
            RenderQueue.SizeChanged(width, height);
            PostEffect.SizeChanged(width, height);

            if (BackGroundBuffer != null)
            {
                BackGroundBuffer.SizeChanged(width, height);
            }

            if (ForeGroundBuffer != null)
            {
                ForeGroundBuffer.SizeChanged(width, height);
            }

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

            if (BackGroundBuffer != null)
            {
                BackGroundBuffer.Render(BackGroundObject);
            }

            RenderQueue.Render(ActiveScene);

            if (PostProcessMode)
            {
                PostEffect.Render(ActiveScene);
            }

            if (ForeGroundBuffer != null)
            {
                ForeGroundBuffer.Render(ForeGroundObject);
            }

            OutputBuffer.uTarget = OutputTexture;
            if(BackGroundBuffer != null)
            {
                OutputBuffer.uBackGround = BackGroundBuffer.RenderTarget.RenderTexture[0];
            }
            if(ForeGroundBuffer != null)
            {
                OutputBuffer.uForeGround = ForeGroundBuffer.RenderTarget.RenderTexture[0];
            }

            OutputBuffer.Render(ActiveScene);

        }

        /// <summary>
        /// レンダーテクスチャ追加イベント
        /// </summary>
        /// <param name="sender">レンダーキュー</param>
        /// <param name="e">レンダーキューイベント</param>
        private void RenderQueue_TechniqueAdded(object sender, RenderQueueEventArgs e)
        {
            AddProcessingTexture(e.Technique);
        }

        private void AddProcessingTexture(RenderTechnique technique)
        {
            foreach (var texture in technique.RenderTarget.RenderTexture)
            {
                ProcessingTexture.Add(texture);
            }
        }
    }
}
