using System.Collections.Generic;

namespace KI.Renderer
{
    /// <summary>
    /// ポストエフェクトマネージャクラス
    /// </summary>
    public class PostEffectManager
    {
        /// <summary>
        /// ポストエフェクトリスト
        /// </summary>
        private List<RenderTechnique> postEffects = new List<RenderTechnique>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PostEffectManager()
        {
            postEffects = new List<RenderTechnique>();
            Initialize();
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public void SizeChanged(int width, int height)
        {
            foreach (var post in postEffects)
            {
                post.SizeChanged(width, height);
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public void Render(Scene scene)
        {
            foreach (var post in postEffects)
            {
                post.Render(scene);
            }
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 初期ポストエフェクト
        /// </summary>
        private void Initialize()
        {
            //Bloom bloom = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Bloom) as Bloom;
            //bloom.uTarget = Workspace.SceneManager.RenderSystem.GBufferStage.OutputTexture[3];

            //Sobel sobel = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.Sobel) as Sobel;
            //sobel.uTarget = Workspace.SceneManager.RenderSystem.GBufferStage.OutputTexture[3];

            //SSAO ssao = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSAO) as SSAO;
            //ssao.uPosition = Workspace.SceneManager.RenderSystem.GBufferStage.OutputTexture[0];
            //ssao.uTarget = Workspace.SceneManager.RenderSystem.LightingStage.OutputTexture[0];

            SSLIC sslic = RenderTechniqueFactory.Instance.CreateRenderTechnique(RenderTechniqueType.SSLIC) as SSLIC;
            sslic.uVector = Global.RenderSystem.GBufferStage.OutputTexture[2];

            //PostEffects.Add(bloom);
            //PostEffects.Add(sobel);
            //PostEffects.Add(ssao);
            postEffects.Add(sslic);
        }

        /// <summary>
        /// バッファのクリア
        /// </summary>
        private void ClearBuffer()
        {
            foreach (var post in postEffects)
            {
                post.ClearBuffer();
            }
        }
    }
}
