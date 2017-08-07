using System.Collections.Generic;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    /// <summary>
    /// レンダリングシステム
    /// </summary>
    public class RenderSystem
    {
        /// <summary>
        /// ポストエフェクト
        /// </summary>
        private PostEffectManager postEffect;

        /// <summary>
        /// 後処理のUtil（選択とか）
        /// </summary>
        private RenderTechnique selectionStage;

        /// <summary>
        /// 最終出力画像
        /// </summary>
        private OutputBuffer outputStage;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RenderSystem()
        {
        }

        /// <summary>
        /// 描画するシーン
        /// </summary>
        public Scene ActiveScene { get; set; }
        
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
        /// GBuffer前
        /// </summary>
        public RenderTechnique PreRenderStage { get; private set; }

        /// <summary>
        /// defferdシェーディング用
        /// </summary>
        public RenderTechnique GBufferStage { get; private set; }

        /// <summary>
        /// ibl用
        /// </summary>
        public RenderTechnique IBLStage { get; private set; }

        /// <summary>
        /// ライティングステージ
        /// </summary>
        public RenderTechnique DeferredStage { get; private set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void Initialize(int width, int height)
        {
            ProcessingTexture = new List<Texture>();
            PostProcessMode = false;
            PreRenderStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Shadow);
            GBufferStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer);
            IBLStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.IBL);
            DeferredStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred);
            selectionStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Selection);
            postEffect = new PostEffectManager();
            outputStage = (OutputBuffer)RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output);
            OutputTexture = ((GBuffer)GBufferStage).GetOutputTexture(GBuffer.GBufferOutputType.Color);

            foreach (var texture in RenderTechniqueFactory.Instance.OutputTextures())
            {
                ProcessingTexture.Add(texture);
            }
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SizeChanged(int width, int height)
        {
            RenderTechniqueFactory.Instance.SizeChanged(width, height);
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
            RenderTechniqueFactory.Instance.Dispose();
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Render()
        {
            //PreRenderStage.Render();
            GBufferStage.Render(ActiveScene);
            IBLStage.Render(ActiveScene);
            DeferredStage.Render(ActiveScene);
            //SelectionStage.ClearBuffer();
            //SelectionStage.Render();

            postEffect.Render(ActiveScene);
            outputStage.uSelectMap = selectionStage.OutputTexture[0];
            outputStage.uTarget = OutputTexture;
            outputStage.Render(ActiveScene);
        }

        /// <summary>
        /// ポストプロセスを行うかのトグル
        /// </summary>
        public void TogglePostProcess()
        {
            PostProcessMode = !PostProcessMode;
        }
    }
}
