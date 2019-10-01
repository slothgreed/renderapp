using System.Collections.Generic;
using KI.Gfx.KITexture;
using KI.Renderer.Technique;
using KI.Gfx.GLUtil;

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
            renderInfo = new RenderInfo();
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
        private HUDTechnique foreGroundTechnique;

        /// <summary>
        /// 前面に書く HUD
        /// </summary>
        public HUDTechnique ForeGroundTechnique
        {
            get
            {
                return foreGroundTechnique;
            }
            set
            {
                foreGroundTechnique = value;
                AddProcessingTexture(foreGroundTechnique);
            }
        }

        /// <summary>
        /// 背景に書く HUD
        /// </summary>
        public List<HUDObject> BackGroundObject { get; private set; }

        /// <summary>
        /// BackGroundBuffer の バッキングフィールド
        /// </summary>
        private HUDTechnique backGroundTechnique;

        /// <summary>
        /// 背景に書く HUD
        /// </summary>
        public HUDTechnique BackGroundTechnique
        {
            get
            {
                return backGroundTechnique;
            }
            set
            {
                backGroundTechnique = value;
                AddProcessingTexture(backGroundTechnique);
            }
        }

        /// <summary>
        /// ZPrepassレンダリング の バッキングフィールド
        /// </summary>
        private ZPrepassRender zprepassTechnique;

        /// <summary>
        /// ZPrepassレンダリング
        /// </summary>
        public ZPrepassRender ZPrepassTechnique
        {
            get
            {
                return zprepassTechnique;
            }
            set
            {
                zprepassTechnique = value;
                AddProcessingTexture(zprepassTechnique);
            }

        }

        /// <summary>
        /// レンダリング情報
        /// </summary>
        private RenderInfo renderInfo;

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SizeChanged(int width, int height)
        {
            RenderQueue.SizeChanged(width, height);
            ZPrepassTechnique.SizeChanged(width, height);
            PostEffect.SizeChanged(width, height);

            if (BackGroundTechnique != null)
            {
                BackGroundTechnique.SizeChanged(width, height);
            }

            if (ForeGroundTechnique != null)
            {
                ForeGroundTechnique.SizeChanged(width, height);
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
        /// 背景レンダリング
        /// </summary>
        private void BackGroundRender()
        {
            if (BackGroundTechnique != null)
            {
                renderInfo.RenderPass = RenderPass.BackGroundPath;
                BackGroundTechnique.Render(BackGroundObject, renderInfo);
            }
        }

        /// <summary>
        /// ZPrepass レンダリング
        /// </summary>
        private void ZPrepassRender()
        {
            renderInfo.RenderPass = RenderPass.ZPrepass;
            DeviceContext.Instance.ColorMask(false,false,false,false);
            ZPrepassTechnique.Render(ActiveScene, renderInfo);
            DeviceContext.Instance.ColorMask(true, true, true, true);
        }

        /// <summary>
        /// ベースレンダリング
        /// </summary>
        private void BaseRender()
        {
            renderInfo.RenderPass = RenderPass.BasePass;
            RenderQueue.Render(ActiveScene, renderInfo);
        }

        private void PostEffectRender()
        {
            if (PostProcessMode)
            {
                renderInfo.RenderPass = RenderPass.PostEffectPass;
                PostEffect.Render(ActiveScene, renderInfo);
            }
        }

        /// <summary>
        /// 前景レンダリング
        /// </summary>
        private void ForeGroundRender()
        {
            if (ForeGroundTechnique != null)
            {
                renderInfo.RenderPass = RenderPass.ForeGroundPass;
                ForeGroundTechnique.Render(ForeGroundObject, renderInfo);
            }
        }

        private void OutputRender()
        {
            OutputBuffer.uTarget = OutputTexture;
            if (BackGroundTechnique != null)
            {
                OutputBuffer.uBackGround = BackGroundTechnique.RenderTarget.RenderTexture[0];
            }
            if (ForeGroundTechnique != null)
            {
                OutputBuffer.uForeGround = ForeGroundTechnique.RenderTarget.RenderTexture[0];
            }

            renderInfo.RenderPass = RenderPass.OutputPath;
            OutputBuffer.Render(ActiveScene, renderInfo);
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

            BackGroundRender();
            ZPrepassRender();
            BaseRender();
            PostEffectRender();
            ForeGroundRender();
            OutputRender();
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

            if (technique.RenderTarget.FrameBuffer.DepthTexture != null)
            {
                ProcessingTexture.Add(technique.RenderTarget.FrameBuffer.DepthTexture);
            }
        }
    }
}
