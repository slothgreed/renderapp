using KI.Asset;
using KI.Gfx.KITexture;

namespace KI.Asset.Technique
{
    /// <summary>
    /// 形状選択ようのレンダリング
    /// </summary>
    public partial class Selection : RenderTechnique
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Selection(string vertexShader, string fragShader)
            : base("SelectionBuffer", vertexShader, fragShader, RenderTechniqueType.Selection, RenderType.OffScreen)
        {
            var textures = Global.Renderer.RenderQueue.OutputTexture(RenderTechniqueType.GBuffer);
            Rectanle.Polygon.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Color]);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uID = -1;
        }

        /// <summary>
        /// 形状の選択
        /// </summary>
        public void SelectObject()
        {
            if (Global.Renderer.ActiveScene.SelectNode != null)
            {
                if (Global.Renderer.ActiveScene.SelectNode is RenderObject)
                {
                    var renderObject = Global.Renderer.ActiveScene.SelectNode as RenderObject;
                    uID = renderObject.ID;
                }
                else
                {
                    uID = 0;
                }
            }
            else
            {
                uID = 0;
            }
        }
    }
}
