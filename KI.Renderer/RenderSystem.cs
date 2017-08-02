using System.Collections.Generic;
using KI.Gfx.KITexture;

namespace KI.Renderer
{
    public class RenderSystem
    {
        /// <summary>
        /// ポストエフェクト
        /// </summary>
        private PostEffectManager PostEffect;

        /// <summary>
        /// 後処理のUtil（選択とか）
        /// </summary>
        private RenderTechnique SelectionStage;

        /// <summary>
        /// 最終出力画像
        /// </summary>
        private OutputBuffer OutputStage;

        /// <summary>
        /// FrameBufferの横
        /// </summary>
        private int Width;

        /// <summary>
        /// FrameBufferの縦
        /// </summary>
        private int Height;

        /// <summary>
        /// 出力テクスチャ
        /// </summary>
        public Texture OutputTexture { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RenderSystem()
        {
        }

        /// <summary>
        /// ポストプロセスモード
        /// </summary>
        public bool PostProcessMode
        {
            get;
            set;
        }

        /// <summary>
        /// レンダリング結果のテクスチャすべて
        /// </summary>
        public List<Texture> ProcessingTexture
        {
            get;
            private set;
        }

        /// <summary>
        /// GBuffer前
        /// </summary>
        public RenderTechnique PreRenderStage
        {
            get;
            private set;
        }

        /// <summary>
        /// defferdシェーディング用
        /// </summary>
        public RenderTechnique GBufferStage
        {
            get;
            private set;
        }

        public RenderTechnique IBLStage
        {
            get;
            private set;
        }

        /// <summary>
        /// ライティングステージ
        /// </summary>
        public RenderTechnique DeferredStage
        {
            get;
            private set;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;
            ProcessingTexture = new List<Texture>();
            PostProcessMode = false;
            PreRenderStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Shadow);
            GBufferStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.GBuffer);
            IBLStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.IBL);
            DeferredStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Deferred);
            SelectionStage = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Selection);
            PostEffect = new PostEffectManager();
            OutputStage = (OutputBuffer)RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Output);
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
            GBufferStage.Render();
            IBLStage.Render();
            DeferredStage.Render();
            //SelectionStage.ClearBuffer();
            //SelectionStage.Render();

            PostEffect.Render();
            OutputStage.uSelectMap = SelectionStage.OutputTexture[0];
            OutputStage.uTarget = OutputTexture;
            OutputStage.Render();
        }

        public void TogglePostProcess()
        {
            PostProcessMode = !PostProcessMode;
        }
    }
}
