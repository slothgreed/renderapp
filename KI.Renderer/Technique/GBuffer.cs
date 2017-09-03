using KI.Asset;
using KI.Gfx.KITexture;
using KI.Gfx.Render;

namespace KI.Renderer
{
    /// <summary>
    /// GBuffer
    /// </summary>
    public class GBuffer : RenderTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GBuffer()
            : base("GBuffer", RenderTechniqueType.GBuffer, RenderType.Original)
        {
        }

        /// <summary>
        /// GBufferの入力値
        /// </summary>
        public enum GBufferOutputType
        {
            Posit = 0,
            Normal,
            Color,
            Light
        }

        /// <summary>
        /// 出力テクスチャの取得
        /// </summary>
        /// <param name="target">GBufferのタイプ</param>
        /// <returns>テクスチャ</returns>
        public Texture GetOutputTexture(GBufferOutputType target)
        {
            return OutputTexture[(int)target];
        }

        /// <summary>
        /// レンダーターゲットの生成
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public override void CreateRenderTarget(int width, int height)
        {
            Texture[] texture = new Texture[4];
            texture[0] = TextureFactory.Instance.CreateTexture("GPosit", width, height);
            texture[1] = TextureFactory.Instance.CreateTexture("GNormal", width, height);
            texture[2] = TextureFactory.Instance.CreateTexture("GColor", width, height);
            texture[3] = TextureFactory.Instance.CreateTexture("GLight", width, height);

            //RenderApp.Globals.Project.ActiveProject.AddChild(texture[0]);
            //RenderApp.Globals.Project.ActiveProject.AddChild(texture[1]);
            //RenderApp.Globals.Project.ActiveProject.AddChild(texture[2]);
            //RenderApp.Globals.Project.ActiveProject.AddChild(texture[3]);

            OutputTexture.Add(texture[0]);
            OutputTexture.Add(texture[1]);
            OutputTexture.Add(texture[2]);
            OutputTexture.Add(texture[3]);

            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget(Name, width, height, OutputTexture.Count);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void Render(IScene scene)
        {
            ClearBuffer();
            RenderTarget.BindRenderTarget(OutputTexture.ToArray());
            foreach (var asset in scene.RootNode.AllChildren())
            {
                if (asset.KIObject is RenderObject)
                {
                    var polygon = asset.KIObject as RenderObject;
                    polygon.Render(scene);
                }
            }

            RenderTarget.UnBindRenderTarget();
        }
    }
}
